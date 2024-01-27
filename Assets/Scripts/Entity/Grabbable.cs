using System;
using entity;
using physic;
using Unity.VisualScripting;
using UnityEngine;

namespace entity
{
    /**
     * Une classe abstraite desquels les éléments pouvant être attrapés avec la longue du·de la joueur·se
     * doivent étendre.
     */
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Attractor))]
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class Grabbable : MonoBehaviour
    {

        /**
         * Ces champs contiennent les quelques components utilisés ici.
         */
        protected Rigidbody2D RigidBody;
        private Attractor _attractor;

        // La scale initiale lorsque la langue attrape l'objet. (utilisé pour rescale ensuite)
        private Vector3 _scaleOnGrab;
        // Distance initiale lorsque la langue attrape l'objet. (utilisé pour le rescale ensuite)
        private float _startDist = 0;

        /**
         * Définition des variables components lors du départ.
         */
        public virtual void Start()
        {
            this.RigidBody = this.GetComponent<Rigidbody2D>();
            this._attractor = this.GetComponent<Attractor>();
        }

        public Vector3 GetScaleOnGrab()
        {
            return this._scaleOnGrab;
        }
        
        /**
         * Méthode qui est appelée quand l'objet est attrapé par la langue du·de la joueur·se
         * <param name="tongue">L'instance de la langue en question</param>
         * <param name="player">L'instance du·de la joueur·se à qui elle appartient.</param>
         */
        public virtual void TongueGrab(Tongue tongue, Player player)
        {
            this._scaleOnGrab = this.transform.localScale;
            Vector2 playerPos = player.GetRigidbody().position;
            Vector2 myPos = this.RigidBody.position;
            this._startDist = Vector2.Distance(playerPos, myPos);
        }

        /**
         * A chaque frame où l'objet est tenu par la langue d'un·e joueur·se, cette fonction va être
         * appelée.
         * Elle permet de définir la taille de l'objet, et de le faire peu à peu rétrécir/agrandir,
         * en fonction de GetScaleFactor.
         * <param name="player">L'instance du·de la joueur·se qui attrape l'objet</param>
         */
        public void GrabUpdate(Player player)
        {
            float distance = Mathf.Max(0, Mathf.Min(Vector2.Distance(player.GetRigidbody().position, this.RigidBody.position), this._startDist));
            float progress = 1 - Mathf.Max(0, Mathf.Min(distance * 1 / this._startDist, 1));
            Vector2 vec2Scale = Vector2.Lerp(this._scaleOnGrab, this.GrabScaleFactor(), progress);
            this.transform.localScale = new Vector3(vec2Scale.x, vec2Scale.y, 1);
        }
        
        /**
         * Méthode abstraite qui permet aux classes filles de définir la taille à laquelle termine
         * l'objet juste avant d'être attrapé par le·la joueur·se.
         */
        public abstract Vector2 GrabScaleFactor();

        public abstract void ResetScale();

        /**
         * Méthode abstraite qui permet aux classes filles de définir ce qui se passe quand l'objet
         * est attrapé par le·la joueur·se.
         * <param name="player">L'instance du·de la joueur·se qui l'attrape.</param>
         */
        public abstract void PlayerGrab(Player player);
        
    }
}