using System.Collections.Generic;
using UnityEngine;

namespace physic
{
    [RequireComponent (typeof (Rigidbody2D))]
    public class Attractor : MonoBehaviour
    {
    
        private static readonly List<Attractor> Attractors = new List<Attractor>();
        private static float G = 6.64f;

        private Rigidbody2D _rigidBody;
    
        public Vector2 startVelocity;
        public Attractor planet = null;
        public bool autoPlanet = true;

        // These objects will not be attracted
        private List<Attractor> _dontAttract = new();

        private void Awake()
        {
            this._rigidBody = this.GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            this._rigidBody.gravityScale = 0;
            if (!this._rigidBody.isKinematic) 
                this._rigidBody.velocity = startVelocity;
        
        }

        public bool DoAttract(Attractor attractor)
        {
            return !this._dontAttract.Contains(attractor);
        }
        public void DontAttract(Attractor other)
        {
            this._dontAttract.Add(other);
        }

        private void OnEnable()
        {
            Attractors.Add(this);
        }

        private void OnDisable()
        {
            Attractors.Remove(this);
        }
    
        void FixedUpdate()
        {
            if (planet)
            {
                Vector3 targetDir = (this.transform.position - this.planet.transform.position).normalized;
                Quaternion toAttain = Quaternion.FromToRotation(this.transform.up, targetDir) * this.transform.rotation;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, toAttain, 0.1F);
            }

            float mostAttraction = 0F;
            foreach (Attractor attractor in Attractors)
            {
                if (attractor != this)
                    this.Attract(attractor);
                if (attractor.DoAttract(this) && this.autoPlanet)
                {
                    float attraction = attractor.GetAttractionForce(this);
                    if (attraction > mostAttraction)
                    {
                        this.planet = attractor;
                        mostAttraction = attraction;
                    }
                }
                
            }
        }

        private void Attract(Attractor other)
        {
            if (!this.DoAttract(other))
                return;
            Vector3 direction = (this._rigidBody.position - other._rigidBody.position);
            float distance = direction.magnitude;
        
            if(distance == 0)
                return;
        
            float forceMagnitude = G * (this._rigidBody.mass * other._rigidBody.mass) / Mathf.Pow(distance, 2);
            Vector2 force = direction.normalized * forceMagnitude;

            other._rigidBody.AddForce(force);
        }

        public float GetAttractionForce(Attractor other)
        {
            Vector3 direction = (this._rigidBody.position - other._rigidBody.position);
            float distance = direction.magnitude;
            if(distance == 0)
                return 1;
            return G * (this._rigidBody.mass * other._rigidBody.mass) / Mathf.Pow(distance, 2);
        }

        public Rigidbody2D GetRigidBody()
        {
            return this._rigidBody;
        }
    
    }
}
