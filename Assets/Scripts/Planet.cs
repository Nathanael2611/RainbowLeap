using System;
using System.Collections;
using System.Collections.Generic;
using entity;
using UnityEngine;
using util;

public class Planet : MonoBehaviour
{

    private SpriteRenderer _spriteRenderer;
    
    void Start()
    {
        this._spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var differenceColor = this._spriteRenderer.color - Player.ThePlayer.GetColor();

        Color a = this._spriteRenderer.color;
        Color b = Player.ThePlayer.GetColor();

        Vector3 labA = Helpers.ConvertRGBToLab(a);
        Vector3 labB = Helpers.ConvertRGBToLab(b);

        var deltaE = Helpers.CompareLabs(labA, labB);
        
        //Debug.Log(deltaE);

        if(true)
             return;

        Vector3 hsvA = new Vector3();
        Color.RGBToHSV(a, out hsvA.x, out hsvA.y, out hsvA.z);
        Vector3 hsvB = new Vector3();
        Color.RGBToHSV(b, out hsvB.x, out hsvB.y, out hsvB.z);

        Vector3 difference = hsvA - hsvB;

        float percent = ((difference.x + difference.y + difference.z) / 3) * 100;
        
        Debug.Log(percent);
    }
}
