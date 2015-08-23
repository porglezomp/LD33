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
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xPrime = x*Mathf.Sqrt(1 - y*y/2);
        float yPrime = y*Mathf.Sqrt(1 - x*x/2);
        motion = new Vector3(xPrime, yPrime, 0);
    }

    void FixedUpdate () {
        rigidbody.velocity = motion * speed;
    }
}
