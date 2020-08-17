using UnityEngine;

public interface IMovementController
{
    void FixedUpdate();
    void Move(Vector3 dir);
    void AddForce(Vector3 force);
    void AddExternalForce(Vector3 force);
    void OnCollisionStay(Collision collision);
    bool IsGrounded();
}