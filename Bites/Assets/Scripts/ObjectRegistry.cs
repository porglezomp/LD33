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
            objects[key].RemoveAll(item => item == null);
            return objects[key];
        }
        return null;
    }

    public void RegisterObjectForKey(GameObject obj, string key) {
        var objectList = ObjectsForKey(key);
        if (objectList == null) {
            objectList = new List<GameObject>();
            objects.Add(key, objectList);
        }
        objectList.Add(obj);
    }

}
