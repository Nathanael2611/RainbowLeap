using System;
using input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuReturnClick : MonoBehaviour, IInputListener
    {

        public float timeBeforeCanReturn = 0F;
        private float _startTime = 0F;

        private void Start()
        {
            this._startTime = Time.unscaledTime;
            PressManager.Instance().RegisterListener(this.gameObject);
        }


        public void SimpleClick()
        {
            if (Time.unscaledTime - this._startTime > this.timeBeforeCanReturn)
            {
                SceneManager.LoadScene("Scenes/MainMenu");
            }
        }

        public void DoubleClick()
        {
        }

        public void HoldStart()
        {
        }

        public void HoldEnd()
        {
        }
    }
}