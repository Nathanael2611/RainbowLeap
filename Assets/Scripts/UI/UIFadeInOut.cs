using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class UIFadeInOut : MonoBehaviour
    {

        public float fadeInStart;
        public float fadeInStop;
        public float fadeOutStart, fadeOutStop;
        
        private float _startTime;

        private Image image;
        
        private void Start()
        {
            this._startTime = Time.unscaledTime;
            this.image = this.GetComponent<Image>();
        }


        private void Update()
        {
            float progress = Time.unscaledTime - this._startTime;

            float alpha = 0;
            if (progress < this.fadeInStart)
                alpha = 0;
            else if (progress < this.fadeInStop)
            {
                float total = this.fadeInStop - this.fadeInStart;
                alpha = Math.Min(total,
                    Math.Max(progress - this.fadeInStart, 0)) / total;
            }
            else if (progress < this.fadeOutStart)
                alpha = 1;
            else if (progress < this.fadeOutStop)
            {
                float total = this.fadeOutStop - this.fadeOutStart;
                alpha = 1 - Math.Min(total,
                    Math.Max(progress - this.fadeOutStart, 0)) / total;

            }
            else
            {
                alpha = 0;
            }
            this.image.color = new Color(1, 1, 1,
                alpha);
            this.transform.localScale = new Vector3(alpha, alpha, alpha);
        }
    }
}