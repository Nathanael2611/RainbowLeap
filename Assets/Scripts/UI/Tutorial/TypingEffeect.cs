using System;
using TMPro;
using UnityEngine;

namespace UI.Tutorial
{
    public class TypingEffeect : MonoBehaviour
    {

        private TextMeshProUGUI _text;
        private float _typeStart;
        public float startOffset = 0F;
        public float typeDuration = 1F;
        public string textToType = "";
        
        private void Start()
        {
        }

        private void OnEnable()
        {
            this._text = this.GetComponent<TextMeshProUGUI>();
            this.StartTyping();
        }

        public void StartTyping(float typeDuration)
        {
            this._typeStart = Time.unscaledTime + this.startOffset;
            this.typeDuration = typeDuration;
            if (this._text && this._text.text != null)
            {
                this.textToType = this._text.text;
            }

        }

        public void StartTyping()
        {
            this.StartTyping(this.typeDuration);
        }

        private void Update()
        {
            var length = this.textToType.Length;
            if (length <= 0)
                return;
            float progress = Mathf.Min(this.typeDuration, Mathf.Max(0, Time.unscaledTime - this._typeStart)) / this.typeDuration;
            int lerp = (int) Mathf.Lerp(0, length, progress);
            this._text.SetText(this.textToType.Substring(0, lerp));
        }

        public bool IsTyping()
        {
            float progress = Mathf.Min(this.typeDuration, Mathf.Max(0, Time.unscaledTime - this._typeStart)) / this.typeDuration;
            return progress < 1;
        }
    }
}