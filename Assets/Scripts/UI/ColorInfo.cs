using System.Collections;
using System.Collections.Generic;
using entity;
using physic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorInfo : MonoBehaviour
{

    private Image _coloredImage;
    private TextMeshProUGUI _textMeshPro;
    
    void Start()
    {
        this._coloredImage = this.GetComponent<Image>();
        this._textMeshPro = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Player player = Player.ThePlayer;
        Attractor playerAttractor = player.GetAttractor();
        if (playerAttractor.planet)
        {
            Attractor planetAttractor = playerAttractor.planet;
            if (planetAttractor.GetType() == typeof(Planet))
            {
                Planet planet = (Planet)planetAttractor;
            }
        }
        this._coloredImage = player.GetAttractor().planet ? player.GetAttractor().planet : Color.white;
        this._textMeshPro.SetText("Salut");
    }
}
