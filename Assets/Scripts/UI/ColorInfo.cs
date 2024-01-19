using System.Collections;
using System.Collections.Generic;
using entity;
using physic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using util;

public class ColorInfo : MonoBehaviour
{

    private Image _playerColor;
    private Image _planetColor;
    private TextMeshProUGUI _textMeshPro;
    
    void Start()
    {
        this._playerColor = this.GetComponent<Image>();
        this._planetColor = this.transform.GetChild(1).GetComponent<Image>();
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
                Planet planet = (Planet) planetAttractor;
                this._planetColor.color = planet.GetPlanetColor();
                this._playerColor.color = player.GetColor();
                Color a = planet.GetPlanetColor();
                Color b = Player.ThePlayer.GetColor();

                Vector3 labA = Helpers.ConvertRGBToLab(a);
                Vector3 labB = Helpers.ConvertRGBToLab(b);

                var deltaE = Helpers.CompareLabs(labA, labB);
                this._textMeshPro.SetText(Mathf.Floor(100 - deltaE) + "%");

            }
        }
        //this._coloredImage = player.GetAttractor().planet ? player.GetAttractor().planet : Color.white;
        //this._textMeshPro.SetText("Salut");
    }
}
