using System;
using Entity.Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ActionsText : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshPro;

        private void Awake()
        {
            this._textMeshPro = this.GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            Frog frog = Frog.TheFrog();
            if (frog)
            {
                this._textMeshPro.SetText(frog.actions + "");
            }
        }
    }
}