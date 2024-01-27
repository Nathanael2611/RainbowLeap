using System;
using UnityEngine;

namespace entity
{
    /**
     * Component annexe au Player pour gérer son sprite en fonction des situations.
     */
    public class PlayerSpriteManager : MonoBehaviour
    {
        
        /**
         * Définition des sprites depuis l'éditeur.
         */
        public Sprite jumpSprite, standingSprite, frontSprite;

        // Instance du·de la joueur·se en question.
        private Player _player;
        // Instance du SpriteRenderer du·de la joueur·se.
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            this._player = this.GetComponent<Player>();
            this._spriteRenderer = this.GetComponent<SpriteRenderer>();
        }
        
        /**
         * Définition du sprite en fonction des conditions ici bas.
         */
        void Update()
        {
            float relVelX = this._player.transform.InverseTransformDirection(this._player.GetRigidbody().velocity).x;
            int motX = this._player.OnGround()
                ? (this._player.GetAimAngle() < 0 ? -1 : 1)
                : (Mathf.Abs(relVelX) < 0.4f ? 0 : (relVelX > 0 ? -1 : 1));
            this._spriteRenderer.flipX = motX < 0;

            if (motX == 0)
            {
                this._spriteRenderer.sprite = this.frontSprite;
            }
            else
            {
                if (this._player.OnGround())
                {
                    this._spriteRenderer.sprite = this.standingSprite;
                }
                else
                {
                    this._spriteRenderer.sprite = this.jumpSprite;
                }
            }
            
        }
    }
}
