using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class Enemy : MonoBehaviour {

    new Rigidbody rigidbody;
    public Material fadeMaterial;
    float speed = 2;
    float numberOfPints = 1;
    float awareness = 0;
    float awarenessThreshold = 5;
    float maxAwareness = 7;
    bool aware { get { return awareness > awarenessThreshold; } }
    bool hasWeapon = false;
    Vampire player {
        get { return GameObject.FindWithTag("Player").GetComponent<Vampire>(); }
    }

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Bite() {
        StartCoroutine(FadeOut());
        speed /= 2;
    }

    IEnumerator FadeOut() {
        GetComponent<Collider>().enabled = false;
        var renderer = GetComponent<Renderer>();
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
    bool shouldWalk = true;
    IEnumerator currentAction = null;
    void Update () {
        if (CanSeePlayer()) {
            Debug.Log("Can see player! " + awareness);
            if (aware) {
                awareness = maxAwareness;
            } else {
                awareness += player.suspiciousness * Time.deltaTime;
            }
        }

        if (shouldWalk) {
            shouldWalk = false;
            Interrupt(currentAction);
            var destination = new Vector3(Random.value * 24, Random.value * 16);
            currentAction = WalkPathToPoint(destination);
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
        if (routine == null) return;
        StopCoroutine(routine);
    }

    IEnumerator WalkPathToPoint(Vector3 position) {
        Route path = null;
        for (int i = 0; i < 32 && path == null; i++) {
            position = new Vector3(Random.value * 24, Random.value * 16);
            path = FindPathTo(position);
        }

        Debug.Log("In order to get to " + position + ", " + gameObject.name + " is walking " + path);
        if (path != null) {
            foreach (var node in path.nodes) {
                while (WalkToPoint(node.x, node.y).MoveNext()) {
                    DrawRoute(path);
                    yield return 0;
                }
            }
        }
        shouldWalk = true;
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
