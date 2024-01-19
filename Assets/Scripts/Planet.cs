using System;
using System.Collections;
using System.Collections.Generic;
using entity;
using physic;
using UnityEngine;
using util;

public class Planet : Attractor
{

    private SpriteRenderer _spriteRenderer;
    
     public override void Start()
     {
         this._spriteRenderer = this.GetComponent<SpriteRenderer>(); 
     }

    public Color GetPlanetColor()
    {
        return this._spriteRenderer.color;
    }
    
}
