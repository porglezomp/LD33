using UnityEngine;
using System.Collections;
using System.IO;

public class Tilemap : MonoBehaviour {
    public string filename;
    public GameObject floorObject;
    public GameObject wallObject;
    public GameObject playerObject;
    
    void Awake () {
        var path = Path.Combine(Application.dataPath, filename);
        using (var file = new StreamReader(path)) {
            string line;
            int x = 0, y = 0;
            while ((line = file.ReadLine()) != null) {
                x = 0;
                foreach (var character in line) {
                    switch (character) {
                    case '#':
                        CreateWall(x, y);
                        break;
                    case 'P':
                        CreatePlayer(x, y);
                        CreateFloor(x, y);
                        break;
                    default:
                        CreateFloor(x, y);
                        break;
                    }
                    x++;
                }
                y--;
            }
        }
    }

    void CreateWall(int x, int y) {
        Vector3 position = new Vector3(x, y, 0);
        GameObject.Instantiate(wallObject, position, Quaternion.identity);
    }

    void CreateFloor(int x, int y) {
        Vector3 position = new Vector3(x, y, 1);
        GameObject.Instantiate(floorObject, position, Quaternion.identity);
    }

    void CreatePlayer(int x, int y) {
        Vector3 position = new Vector3(x, y, 0);
        GameObject.Instantiate(playerObject, position, Quaternion.identity);
    }
}
