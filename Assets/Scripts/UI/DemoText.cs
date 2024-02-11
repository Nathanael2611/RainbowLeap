using System;
using input;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DemoText : MonoBehaviour
    {

        private TextMeshProUGUI _text;

        private void Start()
        {
            this._text = this.GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            this._text.SetText(PressManager.demo ? "Map de démo: ON" : "");
        }
    }
}