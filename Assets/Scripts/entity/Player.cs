using System;
using UnityEngine;
using System.Drawing;
using Scrtwpns.Mixbox;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms;
using Color = UnityEngine.Color;

[RequireComponent( typeof(Collider2D) )]
[RequireComponent( typeof(Rigidbody2D) )]
[RequireComponent( typeof(Attractor) )]
public class Player : MonoBehaviour
{
    
    public static Player ThePlayer;
    
    private Rigidbody2D     _rigidBody;
    private Collider2D      _collider2D;
    private Attractor       _attractor;
    private SpriteRenderer  _spriteRenderer;
    private float           _aimAngle,
                            _aimStart;
    private Vector2         _aimDirection,
                            _pointHit;
    private GameObject      _objectHit;
    private bool            _nextDirection = true;
    private bool            _onGround = false;
    
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

    private void FixedUpdate()
    {
        Bounds bounds = this._collider2D.bounds;
        Vector2 pos = this._rigidBody.position;
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, -(pos - this._attractor.planet.GetRigidBody().position).normalized, _collider2D.bounds.extents.y + 0.1f);
        Debug.Log(hits.Length);
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
        this._aimAngle = this._rigidBody.rotation -Mathf.Cos(((Time.time - this._aimStart) + 2) * 0.5F) * (this._nextDirection ? -90 : 90);
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this._aimStart = Time.time;
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            this.TryGrab();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void TryGrab()
    {
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
        if(this._onGround) 
            this._rigidBody.AddForce(this._aimDirection * this.jumpStrength);
    }

    private RaycastHit2D? GetCircleHit()
    {
        RaycastHit2D? hitResult = null;
        Vector2 end = Helpers.Rotate(new Vector2(0, 30), -Mathf.Deg2Rad * this._aimAngle);
        var position = this._rigidBody.position;
        this._aimDirection = -(position - end).normalized;
        RaycastHit2D[] castList = Physics2D.RaycastAll(position, this._aimDirection, 300);
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

    public bool IsAiming()
    {
        return Input.GetKey(KeyCode.Space);
    }

    public Color GetColor()
    {
        return this._spriteRenderer.color;
    }

}
