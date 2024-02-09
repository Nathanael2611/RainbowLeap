using System.Collections;
using System.Collections.Generic;
using entity;
using Entity.Player;
using input;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerCamera : MonoBehaviour
{

    public Frog playerToFollow;
    private Camera _camera;

    private float _zoomFactor = 1;
    
    void Start()
    {
        this._camera = this.GetComponent<Camera>();
        if (this.playerToFollow)
        {
            this.transform.SetParent(this.playerToFollow.transform);
        }
    }

    void Update()
    {
        if(!this.playerToFollow)
            return;
        if (this.playerToFollow.OnGround() || PressManager.Instance().IsHolding() || this.playerToFollow.colorSelection)
        {
            this._zoomFactor = Mathf.Min(1, Mathf.Max(0, this._zoomFactor + (PressManager.Instance().IsHolding() ? 1 : -1) * Time.unscaledDeltaTime));
        }
        this._camera.orthographicSize = 5.5F + 4 * this._zoomFactor;
        
    }

}
