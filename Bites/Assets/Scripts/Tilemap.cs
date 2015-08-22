using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Tilemap : MonoBehaviour {
    public string filename;
    public GameObject floorObject;
    public GameObject wallObject;
    public GameObject playerObject;
    public GameObject enemyObject;
    
    void Awake () {
        var path = Path.Combine(Application.dataPath, filename);
        var grid = new List<List<Tile>>();
        using (var file = new StreamReader(path)) {
            string line;
            int x = 0, y = 0;
            while ((line = file.ReadLine()) != null) {
                x = 0;
                var row = new List<Tile>();
                grid.Add(row);
                foreach (var character in line) {
                    switch (character) {
                    case '#':
                        CreateObject(wallObject, x, y);
                        row.Add(Tile.WallTile);
                        break;
                    case 'P':
                        CreateObject(playerObject, x, y);
                        CreateFloor(x, y);
                        row.Add(Tile.EmptyTile);
                        break;
                    case 'E':
                        CreateObject(enemyObject, x, y);
                        CreateFloor(x, y);
                        row.Add(Tile.EmptyTile);
                        break;
                    default:
                        CreateFloor(x, y);
                        row.Add(Tile.EmptyTile);
                        break;
                    }
                    x++;
                }
                y--;
            }
        }
        var output = new Tile[grid[0].Count, grid.Count];

        for(int i = 0; i < grid.Count; i++) {
            for(int j = 0; j < output.GetLength(1); j++)
                output[i, j] = grid[j][i];
        }
        PathFinder.Init(output);
    }

    void CreateObject(GameObject obj, int x, int y) {
        Vector3 position = new Vector3(x, y, 0);
        GameObject.Instantiate(obj, position, obj.transform.rotation);
    }

    void CreateFloor(int x, int y) {
        Vector3 position = new Vector3(x, y, 1);
        GameObject.Instantiate(floorObject, position, Quaternion.identity);
    }
}
