using UnityEngine;
using System.Collections;

public class Bite : MonoBehaviour {

    const float cooldown = 0.5f;
    float timer = 0;
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
        if (timer > 0) timer -= Time.deltaTime;

        if (Input.GetButtonDown("Bite") && timer <= 0) {
            foreach (var collider in Physics.OverlapSphere(biteCenter, biteRadius)) {
                if (collider.gameObject.tag == "Enemy") {
                    collider.gameObject.SendMessage("Bite", SendMessageOptions.DontRequireReceiver);
                    vampire.bitSomeoneThisFrame = true;
                    timer = cooldown;
                    break;
                }
            }
        }
    }

    // void OnDrawGizmos () {
    //     if (biting) {
    //         Gizmos.DrawSphere(biteCenter, 0.7f);
    //     }
    // }
}
