using System;
using UI.Tutorial;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TypingEffeect))]
    public class ScoreDisplay : MonoBehaviour
    {

        private TypingEffeect _typingEffect;
        private int _score = 0;
        
        private void Start()
        {
            this._typingEffect = this.GetComponent<TypingEffeect>();
            this._score = PlayerPrefs.GetInt("LastScore"); 
            this._typingEffect.StartTyping( );
        }

        private void Update()
        {
            this._typingEffect.textToType = "Score: " + this._score;
        }
    }
}
