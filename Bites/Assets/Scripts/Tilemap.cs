using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Tilemap : MonoBehaviour {
    public GameObject floorObject;
    public GameObject wallObject;
    public GameObject playerObject;
    public GameObject enemyObject;
    public GameObject crossObject;
    public GameObject torchObject;
    public GameObject coffinObject;
    public GameObject stakesObject;

    Tile[,] map;
    
    public void InitWithMap(string filename) {
        Pints.Init();
        StartCoroutine(Pints.PintsDecay());
        RenderSettings.ambientLight = Color.black;

        var grid = new List<List<Tile>>();
        int x = 0, y = 0;
        var file = Resources.Load(filename) as TextAsset;
        foreach (var line in file.text.Split('\n')) {
            x = 0;
            var row = new List<Tile>();
            grid.Add(row);
            foreach (var character in line) {
                var tile = Tile.EmptyTile;
                GameObject obj;
                switch (character) {
                case '#':
                    CreateObject(wallObject, x, y).tag = "Map Tile";
                    tile = Tile.WallTile;
                    break;
                case '!':
                    CreateObject(playerObject, x, y);
                    CreateFloor(x, y);
                    break;
                case '?':
                    CreateObject(coffinObject, x, y).tag = "Map Tile";
                    CreateFloor(x, y).tag = "Map Tile";
                    tile = Tile.WallTile;
                    break;
                case 'E':
                    CreateObject(enemyObject, x, y).tag = "Map Tile";
                    CreateFloor(x, y).tag = "Map Tile";
                    break;
                case '+':
                    CreateObject(crossObject, x, y).tag = "Map Tile";
                    CreateFloor(x, y).tag = "Map Tile";
                    tile = Tile.WallTile;
                    break;
                case 'T':
                    obj = CreateObject(torchObject, x, y);
                    obj.tag = "Map Tile";
                    ObjectRegistry.instance.RegisterObjectForKey(obj, "Light");
                    CreateFloor(x, y).tag = "Map Tile";
                    break;
                case 'W':
                    obj = CreateObject(stakesObject, x, y);
                    obj.tag = "Map Tile";
                    ObjectRegistry.instance.RegisterObjectForKey(obj, "Weapon");
                    CreateFloor(x, y).tag = "Map Tile";
                    break;
                case '_':
                    tile = Tile.WallTile;
                    break;
                default:
                    CreateFloor(x, y).tag = "Map Tile";
                    break;
                }
                row.Add(tile);
                x++;
            }
            y++;
        }
        map = new Tile[grid[0].Count, grid.Count];

        for(int i = 0; i < map.GetLength(0); i++) {
            for(int j = 0; j < map.GetLength(1); j++)
                map[i, j] = grid[j][i];
        }
        PathFinder.Init(map);
    }

    public void DestroyMap() {
        foreach (var tile in GameObject.FindGameObjectsWithTag("Map Tile")) {
            Destroy(tile);
        }
        Destroy(GameObject.FindWithTag("Player"));
    }

    GameObject CreateObject(GameObject obj, int x, int y) {
        Vector3 position = new Vector3(x, y, 0);
        return (GameObject) GameObject.Instantiate(obj, position, obj.transform.rotation);
    }

    GameObject CreateFloor(int x, int y) {
        Vector3 position = new Vector3(x, y, 1);
        return (GameObject) GameObject.Instantiate(floorObject, position, Quaternion.identity);
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
