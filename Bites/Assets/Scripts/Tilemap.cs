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
    public GameObject crossObject;
    public GameObject torchObject;

    Tile[,] map;
    
    void Awake () {
        Pints.Init();
        StartCoroutine(Pints.PintsDecay());

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
                    var tile = Tile.EmptyTile;
                    switch (character) {
                    case '#':
                        CreateObject(wallObject, x, y);
                        tile = Tile.WallTile;
                        break;
                    case 'P':
                        CreateObject(playerObject, x, y);
                        CreateFloor(x, y);
                        break;
                    case 'E':
                        CreateObject(enemyObject, x, y);
                        CreateFloor(x, y);
                        break;
                    case '+':
                        CreateObject(crossObject, x, y);
                        CreateFloor(x, y);
                        tile = Tile.WallTile;
                        break;
                    case 'T':
                        CreateObject(torchObject, x, y);
                        CreateFloor(x, y);
                        break;
                    case '_':
                        tile = Tile.WallTile;
                        break;
                    default:
                        CreateFloor(x, y);
                        break;
                    }
                    row.Add(tile);
                    x++;
                }
                y++;
            }
        }
        map = new Tile[grid[0].Count, grid.Count];

        for(int i = 0; i < map.GetLength(0); i++) {
            for(int j = 0; j < map.GetLength(1); j++)
                map[i, j] = grid[j][i];
        }
        PathFinder.Init(map);
        Game.StartGame();
    }

    void CreateObject(GameObject obj, int x, int y) {
        Vector3 position = new Vector3(x, y, 0);
        GameObject.Instantiate(obj, position, obj.transform.rotation);
    }

    void CreateFloor(int x, int y) {
        Vector3 position = new Vector3(x, y, 1);
        GameObject.Instantiate(floorObject, position, Quaternion.identity);
    }

    void OnDrawGizmos () {
        if (map != null) {
            for (int i = 0; i < map.GetLength(0); i++) {
                for (int j = 0; j < map.GetLength(1); j++) {
                    if (map[i, j] == Tile.EmptyTile) {
                        var position = new Vector3(PathFinder.world[i, j].x, PathFinder.world[i, j].y, -1);
                        Gizmos.DrawWireSphere(position, 0.25f);
                    }
                }
            }
        }
    }
}
