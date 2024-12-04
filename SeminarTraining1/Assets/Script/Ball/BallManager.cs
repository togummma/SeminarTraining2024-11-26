using UnityEngine;

public class BallManager : MonoBehaviour
{
    public float dynamicDuration = 2f;
    public float airResistance = 0.1f;
    public float minSpeed = 0.1f;

    private BallStateManager stateManager;
    private BallPhysicsManager physicsManager;
    private BallLaunchManager launchManager;

    private float remainingDynamicTime;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbodyがアタッチされていません！");
            return;
        }

        // 依存性を注入
        stateManager = new BallStateManager();
        physicsManager = new BallPhysicsManager(rb);
        launchManager = new BallLaunchManager(rb);
    }

    void FixedUpdate()
    {
        switch (stateManager.CurrentState)
        {
            case BallStateManager.BallState.Dynamic:
                HandleDynamicState();
                break;

            case BallStateManager.BallState.Decelerating:
                HandleDeceleratingState();
                break;
        }
    }

    public void Launch(Vector3 direction, float force)
    {
        if (!stateManager.SetState(BallStateManager.BallState.Dynamic))
        {
            Debug.LogError("Launch is not allowed in the current state.");
            return;
        }

        launchManager.Launch(direction, force, rb.mass);
        remainingDynamicTime = dynamicDuration;
        BallEventManager.TriggerStateChange("Dynamic");
    }

    private void HandleDynamicState()
    {
        if (remainingDynamicTime > 0f)
        {
            remainingDynamicTime -= Time.fixedDeltaTime;
        }
        else
        {
            if (stateManager.SetState(BallStateManager.BallState.Decelerating))
            {
                BallEventManager.TriggerStateChange("Decelerating");
            }
        }
    }

    private void HandleDeceleratingState()
    {
        physicsManager.ApplyDeceleration(airResistance, minSpeed);
        if (rb.velocity.sqrMagnitude <= minSpeed * minSpeed)
        {
            if (stateManager.SetState(BallStateManager.BallState.Static))
            {
                BallEventManager.TriggerStateChange("Static");
            }
        }
    }
}
