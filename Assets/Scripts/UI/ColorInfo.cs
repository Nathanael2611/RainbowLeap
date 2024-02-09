using entity;
using Entity.Planets;
using Entity.Player;
using physic;
using TMPro;
using UnityEngine;

namespace UI
{
    
    /**
     * Ce component permet d'afficher le texte du pourcentage de ressemblance sur la planète sur laquelle on se trouve.
     */
    public class ColorInfo : MonoBehaviour
    {
        
        // Le component text à utiliser.
        private TextMeshProUGUI _textMeshPro;

        /**
         * Définition de l'objet texte au départ.
         */
        void Start()
        {
            this._textMeshPro = this.GetComponent<TextMeshProUGUI>();
        }

        /**
         * A chaque frame, on place le texte au niveau de la planète sur laquelle se trouve le·la joueur·se.
         */
        void Update()
        {
            Frog player = Frog.TheFrog();
            if(!player || !this._textMeshPro)
                return;
            Attractor playerAttractor = player.GetAttractor();
            if (playerAttractor.planet && playerAttractor.planet.isActiveAndEnabled)
            {
                Attractor planetAttractor = playerAttractor.planet;
                
                // On s'assure que la planète soit bien du type Planet, sinon ça veut dire qu'elle n'a pas
                // de couleur d'objectif.
                if (planetAttractor.GetType() == typeof(Planet))
                {
                    Planet planet = (Planet)planetAttractor;
                    
                    // On trouve la direction entre le centre de la planète, et le·la joueur·se.
                    Vector2 direction = -(planet.GetRigidBody().position - player.GetRigidbody().position).normalized;
                    Transform planetTransform = planet.transform;
                    // définition de la taille
                    this._textMeshPro.rectTransform.localScale = new Vector3(6f * 0.005F, 6f * 0.005F, 6f * 0.005F);
                    float s = (planetTransform.localScale.y * planet.GetCircleCollide().radius) -
                              this._textMeshPro.rectTransform.localScale.y * this._textMeshPro.rectTransform.rect.height;
                    // On se déplace vers d'en prenant en compte le rayon de la planète.
                    this._textMeshPro.rectTransform.position =
                        planetTransform.position + new Vector3(direction.x * s, direction.y * s);
                    // Rotoation = pareil que le joueur 
                    
                    this._textMeshPro.rectTransform.rotation = player.transform.rotation;
                    // Définition du texte 
                    this._textMeshPro.SetText(Mathf.Floor(player.GetSimilitude()) + "%");
                }
            }
        }
    }
}