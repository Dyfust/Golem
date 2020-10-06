using UnityEngine;

public interface IMovementController
{
    void FixedUpdate();
    void Move(Vector3 dir, float t);
    void OnCollisionStay(Collision collision);
    bool IsGrounded();
}