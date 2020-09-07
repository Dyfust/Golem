using UnityEngine;

public interface IMovementController
{
    void FixedUpdate();
    void Move(Vector3 dir);
    void OnCollisionStay(Collision collision);
    bool IsGrounded();
}