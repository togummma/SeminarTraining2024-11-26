using UnityEngine;

public class BallPhysicsManager
{
    private Rigidbody rb;

    public BallPhysicsManager(Rigidbody rigidbody)
    {
        rb = rigidbody;
    }

    public void ApplyDynamicMotion(Vector3 direction, float speed)
    {
        rb.velocity = direction.normalized * speed;
    }

    public void ApplyDeceleration(float airResistance, float minSpeed)
    {
        if (rb.velocity.sqrMagnitude > minSpeed * minSpeed)
        {
            Vector3 resistanceForce = -rb.velocity.normalized * airResistance * rb.velocity.sqrMagnitude;
            rb.AddForce(resistanceForce, ForceMode.Force);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}
