using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    // Use this for initialization
    void Start () {
        Debug.Log(FindPathTo(new Vector3(0, 0)));
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    public Route FindPathTo(Vector3 position) {
        return PathFinder.Find((int) transform.position.x, (int) -transform.position.y, 1, 1);
    }
}
