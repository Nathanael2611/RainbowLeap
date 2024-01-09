using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
public class Attractor : MonoBehaviour
{
    
    private static readonly List<Attractor> Attractors = new List<Attractor>();
    private static float G = 6.64f;

    private Rigidbody2D _rigidBody;
    
    public Vector2 startVelocity;
    public Attractor planet = null;

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
            this.transform.rotation = Quaternion.FromToRotation(this.transform.up, targetDir) * this.transform.rotation;
        }

        foreach (Attractor attractor in Attractors)
        {
            if (attractor != this)
                this.Attract(attractor);
        }
    }

    private void Attract(Attractor other)
    {
        Vector3 direction = (this._rigidBody.position - other._rigidBody.position);
        float distance = direction.magnitude;
        
        if(distance == 0)
            return;
        
        float forceMagnitude = G * (this._rigidBody.mass * other._rigidBody.mass) / Mathf.Pow(distance, 2);
        Vector2 force = direction.normalized * forceMagnitude;

        other._rigidBody.AddForce(force);
    }

    public Rigidbody2D GetRigidBody()
    {
        return this._rigidBody;
    }
    
}
