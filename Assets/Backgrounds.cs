using System;
using System.Collections;
using System.Collections.Generic;
using entity;
using UnityEngine;

public class Backgrounds : MonoBehaviour
{

    public GameObject tileBase;

    private GameObject centerTile;
    
    void Start()
    {
        this.centerTile = GameObject.Instantiate(this.tileBase);
        
    }

    // Update is called once per frame
    void Update()
    {
        Player thePlayer = Player.ThePlayer;
        Vector2 position = thePlayer.GetRigidbody().position;
        float tileX = Mathf.Floor((position.x + 25) / 50) * 50;
        float tileY = Mathf.Floor((position.y + 25) / 50) * 50;
        this.centerTile.transform.position = new Vector3(tileX, tileY, 200);
    }
}
