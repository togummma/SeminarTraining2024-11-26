using UnityEngine;

public class BallLaunchManager
{
    private Rigidbody rb;

    public BallLaunchManager(Rigidbody rigidbody)
    {
        rb = rigidbody;
    }

    public void Launch(Vector3 direction, float force, float mass)
    {
        float speed = force / mass;
        rb.velocity = direction.normalized * speed;
    }
}
