using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float maintainForceTime = 3f; // 力を維持する時間（秒）
    public float dragCoefficient = 0.1f; // 空気抵抗の強さ（調整可能）
    private float initialForce; // 初期力
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbodyがアタッチされていません！");
            return;
        }

        rb.useGravity = false; // 重力を無効化

        // 初速度を力として適用
        ApplyInitialForce();
    }

    void Update()
    {
        // 空気抵抗に基づいて減速させる
        ApplyDragForce();

        // 玉が速度が十分に小さくなるまで減速を続ける
        if (rb.velocity.magnitude < 0.1f)
        {
            rb.velocity = Vector3.zero; // 完全に停止させる
        }
    }

    private void ApplyInitialForce()
    {
        // 初期力を直接設定し、力を適用
        rb.velocity = transform.forward * initialForce; // 発射方向に初期速度を設定
    }

    private void ApplyDragForce()
    {
        // 空気抵抗を速度の2乗に比例して減少させる
        Vector3 dragForce = -rb.velocity.normalized * dragCoefficient * rb.velocity.sqrMagnitude;
        rb.AddForce(dragForce, ForceMode.Force); // 継続的に空気抵抗を加える
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 他のBallと衝突した場合
        if (collision.gameObject.CompareTag("Ball"))
        {
            BallMovement otherBall = collision.gameObject.GetComponent<BallMovement>();
            if (otherBall != null)
            {
                // 衝突したボールの力を統一
                initialForce = Mathf.Min(initialForce, otherBall.initialForce);
                ApplyInitialForce();
            }
        }
    }

    /// <summary>
    /// 呼び出し側から初期力の大きさを設定
    /// </summary>
    /// <param name="force">初期力</param>
    public void SetInitialForce(float force)
    {
        initialForce = force;
    }
}
