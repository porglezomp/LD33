using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Pints {
    static Text pintsDisplay;
    const double initalPints = 4;
    public static double numberOfPints = 0;
    static double _pintsDecayRate = 0.02;
    public static double pintsDecayRate {
        get { return _pintsDecayRate; }
        set { _pintsDecayRate = value; }
    }

    public static void Reset() {
        numberOfPints = initalPints;
    }

    public static void Init() {
        Pints.pintsDisplay = GameObject.FindWithTag("Pints Display").GetComponent<Text>();
        displayPints();
    }

    public static void AddPints(double pintsIncrement) {
        numberOfPints += pintsIncrement;
        if (numberOfPints < 0) {
            numberOfPints = 0;
            Game.instance.EndGame();
        }
        displayPints();
    }

    static void displayPints() {
        pintsDisplay.text = String.Format("{0:f2} Pints", numberOfPints);
    }

    public static IEnumerator PintsDecay() {
        while (true) {
            yield return 0;
            if (Game.running) {
                AddPints(-pintsDecayRate * Time.deltaTime);
            }
        }
    }
}
