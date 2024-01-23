using System;
using entity;
using physic;
using Unity.VisualScripting;
using UnityEngine;

namespace entity
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Attractor))]
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class Grabbable : MonoBehaviour
    {

        protected Rigidbody2D RigidBody;
        private Attractor _attractor;

        private Vector3 _scaleOnGrab;
        private float _startDist = 0;

        public virtual void Start()
        {
            this.RigidBody = this.GetComponent<Rigidbody2D>();
            this._attractor = this.GetComponent<Attractor>();
    
        }

        public void TongueGrab(Tongue tongue, Player player)
        {
            //this.GetComponent<Collider2D>().enabled = false;
            //Attractor playerAttractor = player.GetComponent<Attractor>();
            //this._attractor.DontAttract(playerAttractor);
            //playerAttractor.DontAttract(this._attractor);
            this._scaleOnGrab = this.transform.localScale;
            Vector2 playerPos = player.GetRigidbody().position;
            Vector2 myPos = this.RigidBody.position;
            this._startDist = Vector2.Distance(playerPos, myPos);
            //this._rigidBody.constraints = RigidbodyConstraints2D.None;
        }

        public void GrabUpdate(Player player)
        {
            float distance = Mathf.Max(0, Mathf.Min(Vector2.Distance(player.GetRigidbody().position, this.RigidBody.position), this._startDist));
            //this._rigidBody.velocity = ( this._rigidBody.position - player.GetRigidbody().position).normalized * (3 * Time.deltaTime);
            float progress = 1 - Mathf.Max(0, Mathf.Min(distance * 1 / this._startDist, 1));
            Vector2 vec2Scale = Vector2.Lerp(this._scaleOnGrab, this.GrabScaleFactor(), progress);
            this.transform.localScale = new Vector3(vec2Scale.x, vec2Scale.y, 1);
        }
        
        

        public abstract Vector2 GrabScaleFactor();

        public abstract void PlayerGrab(Player player);
        
        
    }
}