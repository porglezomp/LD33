using UnityEngine;
using System.Collections;

public class VampireLook : MonoBehaviour {
    
    // Update is called once per frame
    Vector3 mousePos;
    void Update () {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        var distance = (0 - mousePos.z) / Camera.main.transform.forward.z;
        mousePos += Camera.main.transform.forward * distance;
        transform.LookAt(mousePos, Vector3.back);
        // Compensate for the fact that "forward" means out of the 2d plane
        transform.Rotate(transform.right, 90, Space.World);
    }

    void OnDrawGizmos () {
        Gizmos.DrawWireSphere(mousePos, 0.2f);
    }
}
