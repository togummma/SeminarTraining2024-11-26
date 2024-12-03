using UnityEngine;

public class BallMovementManager : MonoBehaviour
{
    public enum BallState { Neutral, Static, Dynamic, Decelerating }

    [Header("設定")]
    public float dynamicDuration = 2f; // 動的状態時間（秒）
    public float airResistance = 0.1f; // 減速状態の空気抵抗
    public float minSpeed = 0.1f; // 静的状態になる速度閾値

    private BallState currentState = BallState.Neutral;
    private float remainingDynamicTime;
    private float dynamicSpeed;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbodyがアタッチされていません！");
            return;
        }

        // 初期化で物理演算を有効に設定
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        currentState = BallState.Neutral;
    }

    void Start()
    {
        Debug.Log("Ball initialized, waiting for launch...");
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case BallState.Dynamic:
                HandleDynamicState();
                break;

            case BallState.Decelerating:
                HandleDeceleratingState();
                break;

            case BallState.Static:
                // 静的状態では何もしない
                break;
        }
    }

    public void Launch(Vector3 direction, float force)
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody is null in Launch!");
            return;
        }

        if (currentState != BallState.Neutral)
        {
            Debug.LogError("Cannot launch unless in Neutral state.");
            return;
        }

        dynamicSpeed = force / rb.mass;
        rb.velocity = direction.normalized * dynamicSpeed;

        remainingDynamicTime = dynamicDuration;

        Debug.Log($"Launch called: Speed={dynamicSpeed}, Direction={direction.normalized}");
        SetState(BallState.Dynamic);
    }

    private void HandleDynamicState()
    {
        if (remainingDynamicTime > 0f)
        {
            remainingDynamicTime -= Time.fixedDeltaTime;

            if (rb.velocity.magnitude < dynamicSpeed)
            {
                rb.velocity = rb.velocity.normalized * dynamicSpeed;
            }
        }
        else
        {
            SetState(BallState.Decelerating);
        }
    }

    private void HandleDeceleratingState()
    {
        if (rb.velocity.sqrMagnitude > minSpeed * minSpeed)
        {
            Vector3 resistanceForce = -rb.velocity.normalized * airResistance * rb.velocity.sqrMagnitude;
            rb.AddForce(resistanceForce, ForceMode.Force);
        }
        else
        {
            rb.velocity = Vector3.zero;
            SetState(BallState.Static);
        }
    }

    private void SetState(BallState newState)
    {
        Debug.Log($"Attempting to change state from {currentState} to {newState}");

        if (!IsValidStateTransition(currentState, newState))
        {
            Debug.LogError($"Invalid state transition from {currentState} to {newState}.");
            return;
        }

        currentState = newState;
        Debug.Log($"State changed to: {currentState}");
    }

    private bool IsValidStateTransition(BallState fromState, BallState toState)
    {
        switch (fromState)
        {
            case BallState.Neutral:
                return toState == BallState.Dynamic;

            case BallState.Dynamic:
                return toState == BallState.Decelerating;

            case BallState.Decelerating:
                return toState == BallState.Static;

            case BallState.Static:
                return false; // 静的状態からは遷移しない
        }

        return false; // どのケースにも該当しない場合は無効
    }
}
