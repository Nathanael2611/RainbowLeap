using System;
using Entity.Player;
using TMPro;
using UnityEngine;

namespace UI.Score
{
    public class ScoreAdd : MonoBehaviour
    {

        public float scoreUpDuration = 1.3F;
        
        private TextMeshProUGUI _textMeshPro;
        private Animator _animator;
        private int _lastScore = 0;
        private float _difference = 0;
        private float _scoreChange = -10;
        private static readonly int ScoreUp = Animator.StringToHash("ScoreUp");

        private void Awake()
        {
            this._textMeshPro = this.GetComponent<TextMeshProUGUI>();
            this._animator = this.GetComponent<Animator>();
        }

        private void Update()
        {
            Color color = Color.HSVToRGB((Mathf.Cos(Time.time * 0.2F) + 1F) / 2F, 1, 1);
            this._textMeshPro.color = color;

            Frog frog = Frog.TheFrog();
            if (frog)
            {
                if (frog.score != this._lastScore)
                {
                    this._scoreChange = Time.unscaledTime;
                    this._animator.SetTrigger(ScoreUp);
                    this._difference = frog.score - this._lastScore;
                    //Debug.Log("Difference: " + _difference + " / Before " + this._lastScore + " / After " + frog.score);
                }
                this._lastScore = frog.score;
                float scoreToDisplay = _difference;
                if (Time.unscaledTime - this._scoreChange > 1.5F)
                {
                    this._textMeshPro.SetText("");
                    return;
                }
                this._textMeshPro.SetText(scoreToDisplay + "");
            }
        }
    }
}