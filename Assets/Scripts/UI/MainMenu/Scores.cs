using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.MainMenu
{
    public class Scores : MonoBehaviour
    {

        private TextMeshProUGUI _text;
        
        private void Start()
        {
            this._text = this.GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            this._text.SetText("Dernier score: " + PlayerPrefs.GetInt("LastScore") + "\nMeilleur score: " + PlayerPrefs.GetInt("BestScore"));
        }
    }
}