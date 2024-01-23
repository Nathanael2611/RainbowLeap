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

    //private Image _playerColor;
    //private Image _planetColor;
    private TextMeshProUGUI _textMeshPro;
    
    void Start()
    {
        //this._playerColor = this.GetComponent<Image>();
        //this._planetColor = this.transform.GetChild(1).GetComponent<Image>();
        this._textMeshPro = this.GetComponent<TextMeshProUGUI>();
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
                Vector2 direction = -(planet.GetRigidBody().position - player.GetRigidbody().position).normalized;
                Transform planetTransform = planet.transform;
                this._textMeshPro.rectTransform.localScale = new Vector3(6f * 0.005F, 6f * 0.005F, 6f * 0.005F);
                float s = (planetTransform.localScale.y * planet.GetCircleCollide().radius) - this._textMeshPro.rectTransform.localScale.y * this._textMeshPro.rectTransform.rect.height;

                this._textMeshPro.rectTransform.position = planetTransform.position + new Vector3(direction.x * s, direction.y * s);
                this._textMeshPro.rectTransform.rotation = player.transform.rotation;
                this._textMeshPro.SetText(Mathf.Floor(player.GetSimilitude()) + "%");
                
                //this._planetColor.color = planet.GetPlanetColor();
                //this._playerColor.color = player.GetColor();
            }
        }
        //this._coloredImage = player.GetAttractor().planet ? player.GetAttractor().planet : Color.white;
        //this._textMeshPro.SetText("Salut");
    }
}
