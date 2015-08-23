using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectRegistry : MonoBehaviour {
    public static ObjectRegistry instance;
    Dictionary<string, List<GameObject>> objects = new Dictionary<string, List<GameObject>>();

    void Awake () {
        ObjectRegistry.instance = this;
    }

    public List<GameObject> ObjectsForKey(string key) {
        if (objects.ContainsKey(key)) {
            return objects[key];
        }
        return null;
    }

}
