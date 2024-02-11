using System;
using UnityEngine;

namespace Util
{
    public class DestroyAfter : MonoBehaviour
    {

        private float _time;
        public float destroyAfter;

        private void Start()
        {
            this._time = Time.time;
        }

        private void Update()
        {
            if (Time.time - this._time > this.destroyAfter)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}