﻿using UnityEngine;

public class CharacterController : IMovementController
{
    /// This class is responsible for handling all aspects of motion including slope handling & ground detection.

    private CharacterControllerSettings _settings;

    private float _acceleration;
    private Vector3 _bufferedAcceleration;
    private bool _isGrounded;
    private Rigidbody _rb;

    public CharacterController(Rigidbody rb, CharacterControllerSettings settings)
    {
        _rb = rb;
        _settings = settings;

        DebugWindow.AddPrintTask(() => { return "Golem Surface Normal: " + _groundNormal.ToString(); });
    }

    public void FixedUpdate()
    {
        // Workflow optimization.
        _acceleration = _settings.maxSpeed / _settings.timeToMaxSpeed + _settings.friction;

        Vector3 velocity = _rb.velocity;
        Vector3 acceleration = _bufferedAcceleration * Time.fixedDeltaTime;

        // This raycast is responsible for detecting a slope in front of the character.
        if (_isGrounded)
        {
            // Transform the direction we're accelerating to be parallel to the slope.
            Vector3 slope = Vector3.Cross(_rb.transform.right, _groundNormal).normalized;
            Vector3 accelerationDir = Vector3.ProjectOnPlane(acceleration.normalized, _groundNormal).normalized;
            float accelerationMag = acceleration.magnitude;
            velocity += accelerationDir * accelerationMag;

            // Transform the direction we're moving to be parallel to the slope.
            Vector3 velocityDir = Vector3.ProjectOnPlane(velocity.normalized, _groundNormal).normalized;
            float velocityMag = velocity.magnitude;
            velocity = velocityDir * velocityMag;

            // for angle constraints later on.
            Vector3 flat = slope;
            flat.y = 0f;
        }
        else
        {
            velocity += acceleration;
        }

        // Apply gravity only when grounded.
        if (!_isGrounded)
            velocity += Vector3.down * _settings.gravity * Time.fixedDeltaTime;

        if (_isGrounded)
        {
            // Apply friction. INTERFERES WITH GRAVITY.FIX THIS!
            // Solution: Apply friction to only X & Z axes when falling.
            Vector3 postFrictionVelocity = velocity + -velocity.normalized * _settings.friction * Time.fixedDeltaTime;
            if (Vector3.Dot(velocity.normalized, postFrictionVelocity.normalized) > 0f)
                velocity += -velocity.normalized * _settings.friction * Time.fixedDeltaTime;
            else
                velocity = Vector3.zero;
        }
        else
        {
            Vector3 preFrictionVelocityXZ = velocity;
            preFrictionVelocityXZ.y = 0f;

            Vector3 postFrictionVelocity = preFrictionVelocityXZ + -preFrictionVelocityXZ.normalized * _settings.friction * Time.fixedDeltaTime;
            if (Vector3.Dot(preFrictionVelocityXZ.normalized, postFrictionVelocity.normalized) > 0f)
            {
                velocity += -preFrictionVelocityXZ.normalized * _settings.friction * Time.fixedDeltaTime;
            }
            else
            {
                velocity.x = 0f;
                velocity.z = 0f;
            }
        }

        // Clamp velocity to prevent exceeding max speed.
        if (velocity.magnitude > _settings.maxSpeed)
        {
            if (_isGrounded)
            {
                velocity = Vector3.ClampMagnitude(velocity, _settings.maxSpeed);
            }
            else
            {
                // Ensure velocity is only clamped on the X & Z axes when falling.
                Vector3 velocityXZ = velocity;
                velocityXZ.y = 0f;

                velocityXZ = Vector3.ClampMagnitude(velocityXZ, _settings.maxSpeed);
                velocity.x = velocityXZ.x;
                velocity.z = velocityXZ.z;
            }
        }

        // Reset buffered values.
        //Debug.Log(_isGrounded); 
        _isGrounded = false;
        _groundNormal = Vector3.zero;
        _bufferedAcceleration = Vector3.zero;

        _rb.velocity = velocity;
    }

    private Vector3 _groundNormal;
    private ContactPoint[] _contacts;
    public void OnCollisionEnter(Collision collision)
    {
        int amountOfGroundNormals = 0;
        _groundNormal = Vector3.zero;
        // Grounded if at least 1 surface is facing upwards.
        _contacts = collision.contacts; // collision.GetContacts() seems to be bugged. Sometimes it doesn't reset the array when collisions change.

        for (int i = 0; i < _contacts.Length; i++)
        {
            if (_contacts[i].normal.y > 0f)
            {
                _isGrounded = true;
                _groundNormal += _contacts[i].normal;
                amountOfGroundNormals++;
            }
        }

        _groundNormal = (_groundNormal / amountOfGroundNormals).normalized;
    }

    public void AddExternalForce(Vector3 force)
    {

    }

    public void AddForce(Vector3 force)
    {

    }

    public void Move(Vector3 dir)
    {
        _bufferedAcceleration = dir * _acceleration;
    }

    public bool IsGrounded() => _isGrounded;
}