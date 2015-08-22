using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public enum Tile {
    EmptyTile,
    WallTile,    
}

public class Node {
    public float x = 0, y = 0;
    public List<Node> neighbors = new List<Node>();

    public Node(float x, float y) {
        this.x = x;
        this.y = y;
    }

    public void AddNeighbor(Node neighbor) {
        neighbors.Add(neighbor);
    }

    public float Distance(Node other) {
        float dx = other.x - x;
        float dy = other.y - y;
        return Mathf.Sqrt(dx*dx + dy*dy);
    }

    public override string ToString() {
        return "Node @ " + x + ", " + y + " has " + neighbors.Count + " neighbors";
    }
}

public class Route : IComparable<Route> {
    float length = 0;
    public List<Node> nodes = new List<Node>();

    public Node lastNode {
        get { return nodes.Last(); }
    }

    public Route() { }
    public Route(Route r) {
        this.length = r.length;
        this.nodes = new List<Node>(r.nodes);
    }

    public void AddNode(Node node) {
        if (nodes.Count > 0) {
            length += nodes.Last().Distance(node);
        }
        nodes.Add(node);
    }

    public Route AndThen(Node next) {
        var result = new Route(this);
        result.AddNode(next);
        return result;
    }

    public int CompareTo(Route other) {
        return length.CompareTo(other.length);
    }

    public override string ToString() {
        return "Route " + length + " long with " + nodes.Count + " nodes";
    }
}

public class PathFinder {
    static Node[,] world;
    public static void Init(Tile[,] inputGrid) {
        int width = inputGrid.GetLength(0);
        int height = inputGrid.GetLength(1);

        world = new Node[width, height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                world[x, y] = new Node(x, y);
            }
        }

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (inputGrid[x, y] == Tile.EmptyTile) {
                    // top row
                    if (y > 0) {
                        // if (x > 0 && inputGrid[x - 1, y - 1] == Tile.EmptyTile) {
                        //     world[x, y].AddNeighbor(world[x - 1, y - 1]);
                        // }
                        if (inputGrid[x, y - 1] == Tile.EmptyTile) {
                            world[x, y].AddNeighbor(world[x, y - 1]);
                        }
                        // if (x < width - 1 && inputGrid[x + 1, y - 1] == Tile.EmptyTile) {
                        //     world[x, y].AddNeighbor(world[x + 1, y - 1]);
                        // }
                    }
                    // middle row
                    {
                        if (x > 0 && inputGrid[x - 1, y] == Tile.EmptyTile) {
                            world[x, y].AddNeighbor(world[x - 1, y]);
                        }
                        // self
                        if (x < width - 1 && inputGrid[x + 1, y] == Tile.EmptyTile) {
                            world[x, y].AddNeighbor(world[x + 1, y]);
                        }
                    }
                    // bottom row
                    if (y < height - 1) {
                        // if (x > 0 && inputGrid[x - 1, y + 1] == Tile.EmptyTile) {
                        //     world[x, y].AddNeighbor(world[x - 1, y + 1]);
                        // }
                        if (inputGrid[x, y + 1] == Tile.EmptyTile) {
                            world[x, y].AddNeighbor(world[x, y + 1]);
                        }
                        // if (x < width - 1 && inputGrid[x + 1, y + 1] == Tile.EmptyTile) {
                        //     world[x, y].AddNeighbor(world[x + 1, y + 1]);
                        // }
                    }
                }
            }
        }
        // now the grid connections are made
    }

    public static Route Find(int fromX, int fromY, int toX, int toY) {
        var searchSet = new Heap<Route>();
        foreach (var element in world) {
            if (element.x == fromX && element.y == fromY) {
                var first = new Route();
                first.AddNode(element);
                searchSet.Insert(first);
                break;
            }
        }

        if (!searchSet.hasItems) {
            Debug.LogError("Pathfinding from an out of bounds location " + fromX + " " + fromY);
            return null;
        }

        var visited = new Dictionary<Node, bool>();
        visited.Add(searchSet.root.lastNode, false);
        while (searchSet.hasItems) {
            var testRoute = searchSet.PopRoot();
            foreach (var neighbor in testRoute.lastNode.neighbors) {
                if (visited.ContainsKey(neighbor)) continue;
                visited.Add(neighbor, false);

                if (neighbor.x == toX && neighbor.y == toY) {
                    return testRoute.AndThen(neighbor);
                }

                searchSet.Insert(testRoute.AndThen(neighbor));
            }
        }
        return null;
    }
}
