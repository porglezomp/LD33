using UnityEngine;
using System.Collections;

public class Vampire : MonoBehaviour {

    bool transitionedToBatThisFrame;
    bool bitSomeoneThisFrame;

    public float suspiciousness {
        get {
            if (transitionedToBatThisFrame || bitSomeoneThisFrame) {
                return 10000;
            }
            var brightness = LightBrightness();
            if (brightness > 0.5f) { return brightness; }
            return 0.5f;
        }
    }

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
        transitionedToBatThisFrame = false;
        bitSomeoneThisFrame = false;
    }

    float LightBrightness() {
        var lights = ObjectRegistry.instance.ObjectsForKey("Light");
        if (lights == null) return 0.0f;

        var max = 0.0f;
        const float maxIntensity = 2.0f;
        foreach (var light in lights) {
            var distance = Vector3.Distance(light.transform.position, transform.position);
            var lightIntensity = Mathf.Min(1.0f / distance, maxIntensity);
            max = Mathf.Max(max, lightIntensity);
        }
        return max;
    }
}
