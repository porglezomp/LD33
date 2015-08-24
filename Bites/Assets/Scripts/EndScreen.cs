using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndScreen : MonoBehaviour {
    public Text pintsDisplay;
    float _pints = 0;
    public float score {
        get { return _pints; }
        set {
            _pints = value;
            pintsDisplay.text = string.Format("{0:f2} Pints", _pints);
        }
    }
}
