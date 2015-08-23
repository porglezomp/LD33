using UnityEngine;
using System.Collections;

public class CameraMotion : MonoBehaviour {

    GameObject _player;
    GameObject player {
        get {
            if (_player == null) _player = GameObject.FindWithTag("Player");
            return _player;
        }
    }

    Vector3 targetPosition {
        get { return player.transform.position - transform.forward * 5; }
    }

    bool init = false;

    // Use this for initialization
    void Start () {
        if (Game.running) {
            transform.position = targetPosition;
            init = true;
        }
    }
    
    // Update is called once per frame
    void Update () {
        if (Game.running) {
            if (!init) {
                init = true;
                transform.position = targetPosition;
            }
            var offset = 0.8f * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition, offset);
        } else if (init) {
            init = false;
        }
    }
}
