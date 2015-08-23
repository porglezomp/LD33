using UnityEngine;
using System.Collections;

public class Game {
    static bool _running;
    public static bool running {
        get { return _running; }
    }

    public static void StartGame() {
        _running = true;
    }

    public static void PauseGame() {
        _running = false;
    }
}
