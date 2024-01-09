using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ RequireComponent( typeof (LineRenderer) ) ]
public class DirectionRenderer : MonoBehaviour
{

    public Player player;
    private LineRenderer _lineRenderer;
    
    void Start()
    {
        if (!this.player)
        {
            GameObject.Destroy(this);
        }
        
        this._lineRenderer = this.GetComponent<LineRenderer>();
        this._lineRenderer.positionCount = 2;
    }
    
    void Update()
    {
        this._lineRenderer.enabled = this.player.IsAiming();
        Vector2 playerPosition = this.player.transform.position;
        Vector2 aimDirection = this.player.GetAimDirection();
        Vector2 end = playerPosition + (aimDirection * 30);
        if (this.player.IsAimingForSomething())
            end = this.player.GetPointHit();
        this._lineRenderer.SetPosition(0, playerPosition);
        this._lineRenderer.SetPosition(1, end);
    }
}
