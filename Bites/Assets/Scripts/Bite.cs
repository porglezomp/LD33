using UnityEngine;
using System.Collections;

public class Bite : MonoBehaviour {

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Bite")) {
            Debug.Log("Bite!");
        }
    }
}
