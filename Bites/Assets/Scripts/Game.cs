using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
    static bool _running;
    public static Game instance;
    GameObject hud;
    GameObject menu;
    GameObject finishScreen;

    public static bool running {
        get { return _running; }
    }
    
    public void StartGame(string level) {
        Debug.Log("load " + level);
        Pints.Reset();
        var tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        menu.SetActive(false);
        hud.SetActive(true);
        finishScreen.SetActive(false);
        tilemap.InitWithMap(level);
        _running = true;
    }

    public void PauseGame() {
        _running = false;
    }

    public void ResumeGame() {
        _running = true;
    }

    public void EndGame() {
        _running = false;
        menu.SetActive(false);
        finishScreen.SetActive(true);
        hud.SetActive(false);
        GameObject.Find("Tilemap").GetComponent<Tilemap>().DestroyMap();
        
        var endScreen = finishScreen.GetComponent<EndScreen>();
        endScreen.score = (float) Pints.numberOfPints;
        endScreen.SubmitScore();
    }

    public void Awake() {
        Game.instance = this;
    }

    public void Start() {
        hud = GameObject.FindWithTag("HUD");
        finishScreen = GameObject.FindWithTag("Finish Screen");
        menu = GameObject.FindWithTag("Menu");
        ReturnToMenu();
    }

    public void ReturnToMenu() {
        finishScreen.SetActive(false);
        hud.SetActive(false);
        menu.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
