using System.Collections;
using System.Collections.Generic;
using entity;
using UnityEngine;

public class DirectionPivot : MonoBehaviour
{

    private Player _player;
    
    void Start()
    {
        Player player = this.transform.parent.GetComponent<Player>();
        if (player)
            this._player = player;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.GetChild(0).gameObject.SetActive(this._player.IsAiming());
        if (this._player.IsAiming())
        {
            this.transform.localRotation = (Quaternion.Euler(0, 0, this._player.GetAimAngle()));
        }
    }
}
