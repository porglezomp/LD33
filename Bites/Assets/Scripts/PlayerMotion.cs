using UnityEngine;
using System.Collections;

public class PlayerMotion : MonoBehaviour {

    public float speed = 1.0f;

    new Rigidbody rigidbody;

    // Use this for initialization
    void Start () {
		rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    Vector3 motion;
    void Update () {
        motion = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    }

    void FixedUpdate () {
        rigidbody.velocity = motion * speed;
    }
}
