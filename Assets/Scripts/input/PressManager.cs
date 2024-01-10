using System;
using System.Collections.Generic;
using UnityEngine;

namespace input
{
    public class PressManager : MonoBehaviour
    {
        
        public static PressManager Instance;
    
        public KeyCode keyToUse = KeyCode.Space;
        public float doubleClickTime = 0.3F;

        private List<IInputListener> _listeners = new();
        private float _lastDown, _lastUp;
        private bool _clickPending = false, _wasHolding;

        private void Awake()
        {
            PressManager.Instance = this;
        }

        private void Update()
        {
            PressManager.Instance = this;
        
            if (Input.GetKeyDown(this.keyToUse))
            {
                if(Time.unscaledTime - this._lastDown < this.doubleClickTime)
                    this.DoubleClick();
                else
                    this._clickPending = true;
                this._lastDown = Time.unscaledTime;
            }
            if (Input.GetKeyUp(this.keyToUse))
                this._lastUp = Time.unscaledTime;

            if (this._lastUp > this._lastDown && this._clickPending && (Time.unscaledTime - this._lastDown) > this.doubleClickTime)
            {
                this.SimpleClick();
                if (!this._wasHolding)
                    this.HoldStop();
            }

            
            if (this.IsHolding())
            {
                this._clickPending = false;
                if (!this._wasHolding)
                    this.HoldStart();
            }

        }

        private void HoldStart()
        {
            this._wasHolding = true;
            foreach (IInputListener listener in this._listeners)
            {
                listener.HoldStart();
            }
        }

        private void HoldStop()
        {
            this._wasHolding = false;
            foreach (IInputListener listener in this._listeners)
            {
                listener.HoldEnd();
            }
        }

        public void DoubleClick()
        {
            this._clickPending = false;
            foreach (IInputListener listener in this._listeners)
            {
                listener.DoubleClick();
            }
        }
    
        public void SimpleClick()
        {
            this._clickPending = false;
            foreach (IInputListener listener in this._listeners)
            {
                listener.SimpleClick();
            }
        }

        public float GetHoldTime()
        {
            return this._lastUp > this._lastDown ? 0 : Math.Max(0, Time.unscaledTime - this._lastDown - this.doubleClickTime);
        }
    
        public bool IsHolding()
        {
            return GetHoldTime() > 0;
        }

        public void RegisterListener(IInputListener listener)
        {
            this._listeners.Add(listener);
        }

        public void UnregisterListener(IInputListener listener)
        {
            this._listeners.Remove(listener);
        }
    
    }
}
