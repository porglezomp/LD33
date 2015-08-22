using UnityEngine;
using System.Collections;

public class CameraMotion : MonoBehaviour {

    Vector3 targetPosition {
        get { return player.transform.position - transform.forward * 5; }
    }

    GameObject player;
    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player");
        transform.position = targetPosition;
    }
    
    // Update is called once per frame
    void Update () {
        var offset = 0.8f * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetPosition, offset);
    }
}
