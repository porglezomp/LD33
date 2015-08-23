using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    new Rigidbody rigidbody;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    bool shouldWalk = true;
    void Update () {
        if (shouldWalk) {
            shouldWalk = false;
            var randomPoint = new Vector3(Random.value * 24, Random.value * 16);
            StartCoroutine(WalkPathToPoint(randomPoint));
        }
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
            rigidbody.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, 2 * Time.deltaTime));
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
