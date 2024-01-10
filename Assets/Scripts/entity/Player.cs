using System;
using input;
using physic;
using Scrtwpns.Mixbox;
using UnityEngine;
using util;
using Color = UnityEngine.Color;

namespace entity
{
    [RequireComponent( typeof(Collider2D) )]
    [RequireComponent( typeof(Rigidbody2D) )]
    [RequireComponent( typeof(Attractor) )]
    public class Player : MonoBehaviour, IInputListener
    {
    
        public static Player ThePlayer;
    
        private Rigidbody2D     _rigidBody;
        private Collider2D      _collider2D;
        private Attractor       _attractor;
        private SpriteRenderer  _spriteRenderer;
        private float           _aimValue, _aimAngle, _turnAngle, _aimStop;
        private Vector2         _aimDirection,
            _pointHit;
        private GameObject      _objectHit;
        private bool            _nextDirection = true;
        private bool            _onGround = false;

        private float _turnValue, _lastJump;
    
        public float jumpStrength = 100;
    
        private void Awake()
        {
            Player.ThePlayer = this;
            this._rigidBody = this.GetComponent<Rigidbody2D>();
            this._rigidBody.gravityScale = 0;
            this._spriteRenderer = this.GetComponent<SpriteRenderer>();
            this._collider2D = this.GetComponent<Collider2D>();
            this._attractor = this.GetComponent<Attractor>();
        }

        private void Start()
        {
            PressManager.Instance.RegisterListener(this);
        }

        private void FixedUpdate()
        {
            Bounds bounds = this._collider2D.bounds;
            Vector2 pos = this._rigidBody.position;
        
            RaycastHit2D[] hits = Physics2D.RaycastAll(pos, -(pos - this._attractor.planet.GetRigidBody().position).normalized, _collider2D.bounds.extents.y + 0.1f);
            int i = 0;
            foreach (var hit2D in hits)
            {
                if (hit2D.rigidbody == this._rigidBody) continue;
                i++;
            }

            this._onGround = i > 0;
        }

        private void Update()
        {
            if (PressManager.Instance.IsHolding())
            {
                this._aimValue += Time.unscaledDeltaTime * (this._nextDirection ? 1 : -1);
            }

            if (Time.unscaledTime - this._lastJump > 1)
                this._turnValue += Time.unscaledDeltaTime;
            

            //this._aimAngle = 
            if (this.IsAiming())
            {
                this._aimAngle = -Mathf.Cos(((this._aimValue) + 2) * 0.5F) * 90;
            }
            else
            {
                this._turnAngle = (this.GetNonAimingAngle());
            }

            this._spriteRenderer.flipX = this.IsAiming() ? (this._aimAngle < 0) : (this._turnAngle < 0);

            //this.transform.rotation = Quaternion.Euler(0, 0, rot);

            RaycastHit2D? hitResult = this.GetCircleHit();

        
            if (hitResult.HasValue)
            {
                RaycastHit2D hit2D = hitResult.Value;
                this._pointHit = hit2D.point;
                this._objectHit = hit2D.collider.gameObject;
            }
            else
            {
                this._objectHit = default;
            }
            
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void TryGrab(float angle)
        {
            this._aimDirection = Helpers.Rotate(new Vector2(0, 1), -Mathf.Deg2Rad * this._aimAngle);
            if (this._objectHit)
            {
                ColoredCircle coloredCircle = this._objectHit.GetComponent<ColoredCircle>();
                if (coloredCircle != null)
                {
                    this._spriteRenderer.color = Mixbox.Lerp(this._spriteRenderer.color, coloredCircle.GetColor(), 0.5F);
                    GameObject.Destroy(this._objectHit);
                }
            }

            this._nextDirection = (this._aimAngle - this._rigidBody.rotation) > 0;
            if (this._onGround)
            {
                this._rigidBody.AddRelativeForce(this._aimDirection * this.jumpStrength);
            }
            this._lastJump = Time.unscaledTime;
        }

        private RaycastHit2D? GetCircleHit()
        {
            RaycastHit2D? hitResult = null;
            Vector2 end = Helpers.Rotate(new Vector2(0, 30), -Mathf.Deg2Rad * this._aimAngle);
            var position = this._rigidBody.position;
            this._aimDirection = -(position - end).normalized;
            RaycastHit2D[] castList = Physics2D.RaycastAll(position, this._aimDirection, 4);
            foreach (RaycastHit2D hit2D in castList)
            {
                if (hit2D.rigidbody == this._rigidBody) continue;
                float previous = !hitResult.HasValue ? 0 : Math.Abs(Vector2.Distance(hitResult.Value.point, this._rigidBody.position));
                float actual = Math.Abs(Vector2.Distance(hit2D.point, this._rigidBody.position));
                if (!hitResult.HasValue || previous > actual)
                {
                    //f (hit2D.rigidbody.gameObject.CompareTag("Hittable"))
                    {
                        hitResult = hit2D;
                    }
                }
            }
        
            return hitResult;
        }

        public float GetAimAngle()
        {
            return this._aimAngle;
        }

        public Vector2 GetAimDirection()
        {
            return this._aimDirection;
        }

        public GameObject GetObjectHit()
        {
            return this._objectHit;
        }

        public Vector2 GetPointHit()
        {
            return this._pointHit;
        }

        public bool IsAimingForSomething()
        {
            return this._objectHit != null;
        }

        public float GetNonAimingAngle()
        {
            float cos = Mathf.Cos(this._turnValue * 3F);
            return (int) (cos < 0 ? Mathf.Floor(cos) : Mathf.Ceil(cos)) * 75;
        }
        
        public bool IsAiming()
        {
            return PressManager.Instance.IsHolding();
        }

        public Color GetColor()
        {
            return this._spriteRenderer.color;
        }

        public void SimpleClick()
        {
            this._aimStop = Time.unscaledTime;
            this.TryGrab(this._turnAngle);
        }

        public void DoubleClick()
        {
        }

        public void HoldStart()
        {
        }

        public void HoldEnd()
        {
            this.TryGrab(this._aimAngle);
        }
    }
    
    
}
