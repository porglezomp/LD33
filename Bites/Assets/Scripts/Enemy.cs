using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class Enemy : MonoBehaviour {

    new Rigidbody rigidbody;
    Animator animator;
    public Material fadeMaterial;
    GameObject awarenessBar;
    bool dying = false;
    float speed {
        get {
            if (dying) return 1;
            if (aware || searchingForWeapon) return 4;
            return 2;
        }
    }
    float numberOfPints = 1;
    float _awareness;
    float awareness {
        get { return _awareness; }
        set {
            _awareness = value;
            if (_awareness > maxAwareness) _awareness = maxAwareness;
            var scale = awarenessBar.transform.localScale;
            scale.x = awareness / awarenessThreshold;
            awarenessBar.transform.localScale = scale;
        }
    }
    bool searchingForWeapon = false;

    const float awarenessThreshold = 5;
    const float maxAwareness = 7;
    const float awarenessDecayFactor = 0.99f;
    const float awarenessScaleFactor = 3;
    bool aware { get { return awareness > awarenessThreshold; } }
    bool _hasWeapon = false;
    bool hasWeapon {
        get { return _hasWeapon; }
        set {
            _hasWeapon = value;
            animator.SetBool("HasWeapon", hasWeapon);
        }
    }
    Vampire player {
        get { return GameObject.FindWithTag("Player").GetComponent<Vampire>(); }
    }

    // Use this for initialization
    void Start () {
        awarenessBar = transform.GetChild(0).gameObject;
        rigidbody = GetComponent<Rigidbody>();
        awareness = 0;
        animator = GetComponentInChildren<Animator>();
        name = "Enemy " + Random.value;
    }

    public void Bite() {
        StartCoroutine(FadeOut());
        dying = true;
    }

    IEnumerator FadeOut() {
        GetComponent<Collider>().enabled = false;
        var renderer = transform.Find("enemy/Cube_001").GetComponent<Renderer>();
        renderer.material = fadeMaterial;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        var color = renderer.material.color;

        const float fadeOutDuration = 2;
        float timer = fadeOutDuration;
        while (timer > 0) {
            timer -= Time.deltaTime;
            color.a = timer / fadeOutDuration;
            var fraction = Time.deltaTime / fadeOutDuration;
            Pints.AddPints(numberOfPints * fraction);
            renderer.material.color = color;
            yield return 0;
        }
        Destroy(gameObject);
    }
    
    // Update is called once per frame
    bool interrupt = true;
    IEnumerator currentAction = null;
    void Update () {
        if (CanSeePlayer()) {
            if (aware) {
                awareness = maxAwareness;
            } else {
                awareness += player.suspiciousness * awarenessScaleFactor * Time.deltaTime;
                if (aware) interrupt = true;
            }
        } else {
            awareness *= awarenessDecayFactor;
        }

        if (hasWeapon && aware) {
            animator.SetBool("Attacking", true);
        } else {
            animator.SetBool("Attacking", false);
        }

        if (interrupt) {
            interrupt = false;
            Interrupt(currentAction);
            
            if (aware) {
                if (hasWeapon) {
                    if (CanSeePlayer()) {
                        currentAction = AttackPlayer();
                    }
                } else {
                    currentAction = SearchForWeapon();
                }
            } else { 
                var destination = new Vector3(Random.value * 24, Random.value * 16);
                currentAction = WalkPathToPoint(destination);
            }

            StartCoroutine(currentAction);
        }
    }

    bool CanSeePlayer() {
        RaycastHit hit;
        var direction = player.transform.position - transform.position;
        if (Physics.Raycast(transform.position, direction, out hit, direction.magnitude + 1)) {
            return hit.collider.gameObject.tag == "Player";
        }
        return false;
    }

    void Interrupt(IEnumerator routine) {
        searchingForWeapon = false;
        if (routine == null) return;
        StopCoroutine(routine);
    }

    IEnumerator AttackPlayer() {
        Debug.Log(gameObject + " decides to attack");
        int targetX = -1, targetY = -1;
        IEnumerator path = null;
        while (aware) {
            if (path == null || targetX != Mathf.Round(player.transform.position.x) || targetY != Mathf.Round(player.transform.position.y)) {
                Interrupt(path);
                targetX = (int) Mathf.Round(player.transform.position.x);
                targetY = (int) Mathf.Round(player.transform.position.y);
                path = WalkPathToPoint(player.transform.position);
            }
            if (Vector3.Distance(transform.position, player.transform.position) < 0.75) {
                Debug.Log(Vector3.Distance(transform.position, player.transform.position));
                Pints.AddPints(-2f * Time.deltaTime);
            }
            yield return path.MoveNext();
        }
        interrupt = true;
    }

    IEnumerator SearchForWeapon() {
        Debug.Log(gameObject + " begins a search for a weapon");
        var items = ObjectRegistry.instance.ObjectsForKey("Weapon");
        if (items != null) {
            var index = (int) (Random.value * items.Count);
            var target = items[index].transform.position;
            var path = WalkPathToPoint(target);
            while (path.MoveNext()) {
                searchingForWeapon = true;
                yield return 0;
            }
            Debug.Log(gameObject + " has found a weapon");
            hasWeapon = true;
        } else {
            var destination = new Vector3(Random.value * 24, Random.value * 16);
            var path = WalkPathToPoint(destination);
            while (path.MoveNext()) {
                searchingForWeapon = true;
                yield return 0;
            }
        }
        interrupt = true;
    }

    IEnumerator WalkPathToPoint(Vector3 position) {
        // Debug.Log(gameObject + " is walking to " + position);
        Route path = FindPathTo(position);
        for (int i = 0; i < 32 && path == null; i++) {
            position = new Vector3(Random.value * 24, Random.value * 16);
            path = FindPathTo(position);
        }

        if (path != null) {
            var firstNode = true;
            foreach (var node in path.nodes) {
                if (firstNode) {
                    firstNode = false;
                    continue;
                }

                var walk = WalkToPoint(node.x, node.y);
                while (walk.MoveNext()) {
                    DrawRoute(path);
                    yield return 0;
                }
            }
        }
        Debug.Log("Finally walked to");
        interrupt = true;
    }

    void DrawRoute(Route path) {
        foreach (var node in path.nodes) {
            Debug.DrawRay(new Vector3(node.x, node.y, -1), Vector3.up);
        }
    }

    IEnumerator WalkToPoint(float x, float y) {
        var targetPosition = new Vector3(x, y, 0);
        while (Vector3.Distance(targetPosition, transform.position) > 0.1) {
            rigidbody.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime));
            yield return 0;
        }
    }

    public Route FindPathTo(Vector3 position) {
        return FindPathTo((int) Mathf.Round(position.x), (int) Mathf.Round(position.y));
    }

    Route FindPathTo(int x, int y) {
        return PathFinder.Find((int) Mathf.Round(transform.position.x),
                        (int) Mathf.Round(transform.position.y), x, y);
    }
}
