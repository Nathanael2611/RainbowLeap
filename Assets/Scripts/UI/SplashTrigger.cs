using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class SplashTrigger : MonoBehaviour
    {

        public float splashEnd;
        private float _startTime;
        private bool _triggered = false;
        
        private void Start()
        {
            this._startTime = Time.unscaledTime;
        }

        private void Update()
        {
            if (!this._triggered && Time.unscaledTime - this._startTime > this.splashEnd)
            {
                SceneManager.LoadScene("MainMenu");
                this._triggered = true;
            }
        }
    }
}