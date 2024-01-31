using entity;
using Entity.Player;
using UnityEngine;

namespace Entity
{
    
    /**
     * Ce component va diriger l'objet de flèche directionnelle lorsqu'il est nécessaire d'afficher la direction
     * visée.
     */
    public class DirectionPivot : MonoBehaviour
    {

        // Instance du joueur parent.
        private Frog _player;
        // Instance du component SpriteRenderer utilisé.
        private SpriteRenderer _sprite;
    
        /**
         * Définition des valeurs ci-haut lors dès le début.
         */
        void Start()
        {
            Frog player = this.transform.parent.GetComponent<Frog>();
            this._sprite = this.GetComponent<SpriteRenderer>();
            if (player)
                this._player = player;
        }
    
        /**
         * Mise à jour de l'orientation en fonction de la direction de visée du·de la joueur·se.
         */
        void Update()
        {
            float scale = this._player.IsAiming() ? 1 : 0;
            this.transform.localScale = new Vector3(scale, scale, scale);
            if (this._player.IsAiming())
            {
                this.transform.localRotation = (Quaternion.Euler(0, 0, this._player.GetAimAngle()));
            }
        }

    }
}
