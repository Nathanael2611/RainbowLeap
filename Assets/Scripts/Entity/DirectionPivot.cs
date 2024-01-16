using System;
using System.Collections;
using System.Collections.Generic;
using entity;
using UnityEngine;

public class DirectionPivot : MonoBehaviour
{

    private Player _player;
    private SpriteRenderer _sprite;
    
    void Start()
    {
        Player player = this.transform.parent.GetComponent<Player>();
        this._sprite = this.GetComponent<SpriteRenderer>();
        if (player)
            this._player = player;
    }

    // Update is called once per frame
    void Update()
    {

        float scale = this._player.IsAiming() ? 1 : 0;
        this.transform.localScale = new Vector3(scale, scale, scale);
        if (this._player.IsAiming())
        {
            this.transform.localRotation = (Quaternion.Euler(0, 0, this._player.GetAimAngle()));
        }
    }

    private void LateUpdate()
    {
    }
}
