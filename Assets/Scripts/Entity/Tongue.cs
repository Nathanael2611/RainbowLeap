using System;
using physic;
using Unity.VisualScripting;
using UnityEngine;

namespace entity
{
    public class Tongue : MonoBehaviour
    {

        private Player _player;

        private LineRenderer _renderer = new();
        private Rigidbody2D _rigidbody;
        private CircleCollider2D _collider2D;
        private Attractor _attractor;

        public float tongueMaxDistance, tongueStrength;

        private bool _comeBack = false;
        private float _launchTime = 0;

        private Vector2 _lockPoint = Vector2.zero;

        private void Awake()
        {
            this._renderer = this.AddComponent<LineRenderer>();
            this._rigidbody = this.AddComponent<Rigidbody2D>();
            this._collider2D = this.AddComponent<CircleCollider2D>();
            this._attractor = this.AddComponent<Attractor>();
            RotationLock rotationLock = this.AddComponent<RotationLock>();
            rotationLock.x = true;
            rotationLock.y = true;
            rotationLock.z = true;
        }

        public void Initialize(Player player, Vector2 aimDirection, float tongueStrength, float tongueMaxDistance)
        {
            this.tongueStrength = tongueStrength;
            this.tongueMaxDistance = tongueMaxDistance;
            this._renderer.positionCount = 2;
            this._renderer.startWidth = 0.22F;

            Attractor playerAttractor = player.GetComponent<Attractor>();
            this._attractor.DontAttract(playerAttractor);
            playerAttractor.DontAttract(this._attractor);
            
            this._renderer.material = Resources.Load<Material>("Materials/TongueMaterial") as Material;

            this._player = player;

            this._renderer.sortingOrder = this._player.GetComponent<SpriteRenderer>().sortingOrder - 1;

            
            this.transform.position = this._player.transform.position + (new Vector3(aimDirection.x, aimDirection.y).normalized * 1);
            Physics2D.IgnoreCollision(this._collider2D, player.GetComponent<Collider2D>(), true);

            this._collider2D.radius = 0.2f;
            this._rigidbody.mass = 1;

            this._rigidbody.AddForce(aimDirection.normalized * tongueStrength);

            this._launchTime = Time.time;

        }

        private void Update()
        {
            if (!this._renderer)
                return;

            if (this._lockPoint != Vector2.zero)
            {
                this.transform.position = new Vector3(this._lockPoint.x, this._lockPoint.y);
            }
            Vector3[] positions = new Vector3[2];
            this._renderer.startColor =  Color.red;
            this._renderer.endColor =  Color.red;
            positions[0] = this._player.transform.position;
            positions[1] = this.transform.position;
            this._renderer.SetPositions(positions);

            var distance = Vector3.Distance(this.transform.position, this._player.transform.position);
            if (distance > this.tongueMaxDistance || Time.time - this._launchTime > 2 || this._lockPoint != Vector2.zero)
            {
                this._comeBack = true;
            }

            this._rigidbody.isKinematic = this._comeBack;
            
            if (this._comeBack)
            {
                this._rigidbody.velocity = Vector2.zero;
                if (this._lockPoint == Vector2.zero)
                {
                    this.transform.position = Vector3.Lerp(this.transform.position, this._player.transform.position,
                        3F * Time.deltaTime);
                }
                else
                {
                    this._player.GetRigidbody().velocity = (-(this._player.GetRigidbody().position - this._rigidbody.position).normalized * 10);
                }

                if(distance < 1)
                    GameObject.Destroy(this.gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            ContactPoint2D contactPoint2D = other.contacts[0];
            this._lockPoint = contactPoint2D.point;

        }
    }
    
}