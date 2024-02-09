using System;
using Entity.Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreText : MonoBehaviour
    {

        public float scoreUpDuration = 1.3F;
        
        private TextMeshProUGUI _textMeshPro;
        private Animator _animator;
        private int _lastScore = 0;
        private float _scoreToLerp = 0;
        private float _scoreChange = 0;
        private static readonly int ScoreUp = Animator.StringToHash("ScoreUp");

        private void Awake()
        {
            this._textMeshPro = this.GetComponent<TextMeshProUGUI>();
            this._animator = this.GetComponent<Animator>();
        }

        private void Update()
        {
            Frog frog = Frog.TheFrog();
            if (frog)
            {
                if (frog.score != this._lastScore)
                {
                    this._scoreChange = Time.unscaledTime;
                    this._animator.SetTrigger(ScoreUp);
                    this._scoreToLerp = this._lastScore;
                }
                this._lastScore = frog.score;
                float scoreToDisplay = (int) Mathf.Lerp(this._scoreToLerp, frog.score, Mathf.Max(0, Mathf.Min(this.scoreUpDuration, Time.unscaledTime - this._scoreChange)) / this.scoreUpDuration);
                this._textMeshPro.SetText(scoreToDisplay + "");
            }
        }
    }
}