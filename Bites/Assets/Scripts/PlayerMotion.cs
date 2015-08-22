using UnityEngine;
using System.Collections;

public class PlayerMotion : MonoBehaviour {

    public float speed = 1.0f;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        var motion = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.position += (Vector3) motion * Time.deltaTime * speed;
    }
}
