using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float maintainForceTime = 3f; // 力を維持する時間（秒）
    public float decelerationRate = 2f; // 減速率
    private float currentForce; // 現在の力
    private bool isDecelerating = false; // 減速中かどうか
    private Rigidbody rb;

    private static float sharedForce = 0f; // すべての玉で共有する力
    private static bool sharedForceUpdated = false; // 共有力が更新されたかどうか

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbodyがアタッチされていません！");
            return;
        }

        rb.useGravity = false; // 重力を無効化

        // 初期力が設定されていない場合、警告を表示
        if (currentForce <= 0)
        {
            Debug.LogWarning("初期力が設定されていません。SetInitialForceメソッドを呼び出してください。");
        }

        // 初速度を力として適用
        ApplyForce();

        // 力維持後に減速を開始
        Invoke(nameof(StartDeceleration), maintainForceTime);
    }

    void Update()
    {
        // 減速中の力計算
        if (isDecelerating && currentForce > 0)
        {
            currentForce -= decelerationRate * Time.deltaTime;
            currentForce = Mathf.Max(0, currentForce); // 力が0以下にならないようにする
            ApplyForce();
        }

        // 共有力が更新されている場合、全てのボールの力を統一
        if (sharedForceUpdated)
        {
            currentForce = sharedForce;
            ApplyForce();
            if (currentForce <= 0)
            {
                sharedForceUpdated = false; // 更新完了
            }
        }
    }

    private void StartDeceleration()
    {
        isDecelerating = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 他のBallと衝突した場合
        if (collision.gameObject.CompareTag("Ball"))
        {
            BallMovement otherBall = collision.gameObject.GetComponent<BallMovement>();
            if (otherBall != null)
            {
                // 共有力を統一
                sharedForce = Mathf.Min(sharedForce > 0 ? sharedForce : currentForce, currentForce);
                sharedForceUpdated = true;
            }
        }
    }

    /// <summary>
    /// 呼び出し側から力の大きさを設定
    /// </summary>
    /// <param name="force">初期力</param>
    public void SetInitialForce(float force)
    {
        currentForce = force;
        sharedForce = force; // 共有力も更新
    }

    /// <summary>
    /// Rigidbodyに力を適用
    /// </summary>
    private void ApplyForce()
    {
        if (rb != null)
        {
            // AddForceを使用して力を加える（減速を自然に反映）
            Vector3 forceDirection = transform.forward * currentForce;
            rb.AddForce(forceDirection, ForceMode.Force); // 継続的に力を加える
        }
    }
}
