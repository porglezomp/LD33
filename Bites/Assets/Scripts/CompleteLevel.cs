using UnityEngine;
using System.Collections;

public class CompleteLevel : MonoBehaviour {
    void OnTriggerEnter () {
        Game.instance.EndGame();
    }
}
