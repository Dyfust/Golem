//using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class CharacterController : IMovementController
{
    /// This class is responsible for handling all aspects of motion including slope handling & ground detection.

    private CharacterControllerSettings _settings;

    private float _acceleration;
    private Vector3 _bufferedAcceleration;
    private Vector3 _accelerationThisFrame;
    private Vector3 _velocity; 
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

        _velocity = _rb.velocity;
        Vector3 previousVelocity = _velocity; 
        Vector3 acceleration = _bufferedAcceleration * Time.fixedDeltaTime;

        // This raycast is responsible for detecting a slope in front of the character.
        if (_isGrounded)
        {
            // Transform the direction we're accelerating to be parallel to the slope.
            Vector3 slope = Vector3.Cross(_rb.transform.right, _groundNormal).normalized;
            Vector3 accelerationDir = Vector3.ProjectOnPlane(acceleration.normalized, _groundNormal).normalized;
            float accelerationMag = acceleration.magnitude;
            _velocity += accelerationDir * accelerationMag;

            // Transform the direction we're moving to be parallel to the slope.
            Vector3 velocityDir = Vector3.ProjectOnPlane(_velocity.normalized, _groundNormal).normalized;
            float velocityMag = _velocity.magnitude;
            _velocity = velocityDir * velocityMag;

            // for angle constraints later on.
            Vector3 flat = slope;
            flat.y = 0f;
        }
        else
        {
            _velocity += acceleration;
        }

        // Apply gravity only when grounded.
        if (!_isGrounded)
            _velocity += Vector3.down * _settings.gravity * Time.fixedDeltaTime;

        if (_isGrounded)
        {
            // Apply friction. INTERFERES WITH GRAVITY.FIX THIS!
            // Solution: Apply friction to only X & Z axes when falling.
            Vector3 postFrictionVelocity = _velocity + -_velocity.normalized * _settings.friction * Time.fixedDeltaTime;
            if (Vector3.Dot(_velocity.normalized, postFrictionVelocity.normalized) > 0f)
                _velocity += -_velocity.normalized * _settings.friction * Time.fixedDeltaTime;
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

        // Clamp velocity to prevent exceeding max speed.
        if (_velocity.magnitude > _settings.maxSpeed)
        {
            if (_isGrounded)
            {
                _velocity = Vector3.ClampMagnitude(_velocity, _settings.maxSpeed);
            }
            else
            {
                // Ensure velocity is only clamped on the X & Z axes when falling.
                Vector3 velocityXZ = _velocity;
                velocityXZ.y = 0f;

                velocityXZ = Vector3.ClampMagnitude(velocityXZ, _settings.maxSpeed);
                _velocity.x = velocityXZ.x;
                _velocity.z = velocityXZ.z;
            }
        }

        // Reset buffered values.
        //Debug.Log(_isGrounded); 
        _isGrounded = false;
        _groundNormal = Vector3.zero;
        _bufferedAcceleration = Vector3.zero;

        _rb.velocity = _velocity;
        _accelerationThisFrame = _velocity - previousVelocity; 
    }

    public Vector3 GetVelocity() => _velocity; 


    private Vector3 _groundNormal;
    private ContactPoint[] _contacts;
    public void OnCollisionStay(Collision collision)
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