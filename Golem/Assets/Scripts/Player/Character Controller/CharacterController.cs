using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterController : IMovementController
{
    /// This class is responsible for handling all aspects of motion including slope handling & ground detection.
    private CharacterControllerSettings _settings;

    private float _acceleration;
    private Vector3 _bufferedAcceleration;
    private Vector3 _velocity;
    private float _t;

    private bool _isGrounded;
    private Vector3 _groundNormal;
    private List<Vector3> _collisionNormals = new List<Vector3>();

    private Rigidbody _rb;

    public CharacterController(Rigidbody rb, CharacterControllerSettings settings)
    {
        _rb = rb;
        _settings = settings;
        _acceleration = (_settings.maxSpeed / _settings.timeToMaxSpeed) + _settings.friction;
    }

    public void FixedUpdate()
    {
        _velocity = _rb.velocity;
        Vector3 acceleration = _bufferedAcceleration;
        _bufferedAcceleration = Vector3.zero;

        _isGrounded = DetectGround();

        //This raycast is responsible for detecting a slope in front of the character.
        if (_isGrounded)
        {
            _velocity = HandleSlope(_velocity, acceleration, _groundNormal);
        }
        else
        {
            _velocity += acceleration;
        }

        if (_isGrounded)
        {
            //Apply friction. INTERFERES WITH GRAVITY.FIX THIS!
            //Solution: Apply friction to only X & Z axes when falling.
            Vector3 friction = -_velocity.normalized * _settings.friction * Time.fixedDeltaTime;
            Vector3 postFrictionVelocity = _velocity + friction;
            if (Vector3.Dot(_velocity.normalized, postFrictionVelocity.normalized) > 0f)
                _velocity += friction;
            else
                _velocity = Vector3.zero;
        }
        else
        {
            Vector3 preFrictionVelocityXZ = _velocity;
            preFrictionVelocityXZ.y = 0f;

            Vector3 postFrictionVelocity = preFrictionVelocityXZ + -preFrictionVelocityXZ.normalized * _settings.friction * Time.fixedDeltaTime;
            if (Vector3.Dot(preFrictionVelocityXZ.normalized, postFrictionVelocity.normalized) > 0f)
            {
                _velocity += -preFrictionVelocityXZ.normalized * _settings.friction * Time.fixedDeltaTime;
            }
            else
            {
                _velocity.x = 0f;
                _velocity.z = 0f;
            }
        }

        // Apply gravity only when grounded.
        if (_isGrounded == false)
            _velocity += Vector3.down * _settings.gravity * Time.fixedDeltaTime;

        //Clamp velocity to prevent exceeding max speed.
        float cappedSpeed = _settings.maxSpeed * _t;
        if (_rb.velocity.magnitude > _settings.maxSpeed)
        {
            if (_isGrounded)
            {
                _velocity = Vector3.ClampMagnitude(_velocity, _settings.maxSpeed);
            }
            else
            {
                //Ensure velocity is only clamped on the X & Z axes when falling.
                Vector3 velocityXZ = _rb.velocity;
                velocityXZ.y = 0f;

                velocityXZ = Vector3.ClampMagnitude(velocityXZ, _settings.maxSpeed);
                _velocity.x = velocityXZ.x;
                _velocity.z = velocityXZ.z;
            }
        }

        _rb.velocity = _velocity;
    }

    private Vector3 HandleSlope(Vector3 velocity, Vector3 acceleration, Vector3 groundNormal)
    {
        // Transform the direction we're accelerating to be parallel to the slope.
        Vector3 accelerationDir = Vector3.ProjectOnPlane(acceleration.normalized, groundNormal).normalized;
        float accelerationMag = acceleration.magnitude;
        velocity += accelerationDir * accelerationMag;

        // Transform the direction we're moving to be parallel to the slope.
        Vector3 velocityDir = Vector3.ProjectOnPlane(velocity.normalized, groundNormal).normalized;
        float velocityMag = velocity.magnitude;
        velocity = velocityDir * velocityMag;

        return velocity;
    }

    private bool DetectGround()
    {
        bool isGrounded = false;
        int amountOfGroundNormals = 0;

        _groundNormal = Vector3.zero;

        for (int i = 0; i < _collisionNormals.Count; i++)
        {
            if (Vector3.Dot(_collisionNormals[i], Vector3.up) > 0.25f)
            {
                _groundNormal += _collisionNormals[i];
                amountOfGroundNormals++;
                isGrounded = true;
            }
        }

        if (amountOfGroundNormals > 0)
            _groundNormal = (_groundNormal / amountOfGroundNormals).normalized;

        _collisionNormals.Clear();

        return isGrounded;
    }

    public void OnCollisionStay(Collision collision)
    {
        Vector3 normal = Vector3.zero;
        normal = collision.GetContact(0).normal;
        _collisionNormals.Add(normal);
    }

    public void Move(Vector3 dir, float t)
    {
        _bufferedAcceleration += dir * _acceleration * Time.fixedDeltaTime * t;
        _t = t;
    }

    public Vector3 GetVelocity() => _velocity;
    public bool IsGrounded() => _isGrounded;
    public Vector3 GetCollisionNormal() => _groundNormal;
}