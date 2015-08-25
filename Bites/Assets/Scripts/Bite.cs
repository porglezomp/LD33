﻿using UnityEngine;
using System.Collections;

public class Bite : MonoBehaviour {

    float biteRadius = 0.7f;
    Vector3 biteCenter {
        get { return transform.position + transform.up * 0.2f; }
    }
    Vampire vampire;

    void Start() {
        vampire = GetComponent<Vampire>();
    }
    
    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Bite")) {
            foreach (var collider in Physics.OverlapSphere(biteCenter, biteRadius)) {
                collider.gameObject.SendMessage("Bite", SendMessageOptions.DontRequireReceiver);
                vampire.bitSomeoneThisFrame = true;
            }
        }
    }

    // void OnDrawGizmos () {
    //     if (biting) {
    //         Gizmos.DrawSphere(biteCenter, 0.7f);
    //     }
    // }
}
