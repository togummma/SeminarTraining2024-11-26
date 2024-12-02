using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    [Header("弾の設定")]
    public GameObject bulletPrefab; // 弾のPrefab
    public Vector3 bulletOffset = new Vector3(0f, 1f, 1f); // 発射位置のオフセット
    public float bulletForce = 20f; // 弾の発射時の力（ニュートン）

    [Header("カメラ設定")]
    public Camera playerCamera; // 攻撃方向を決定するカメラ

    private AttackCooldownManager cooldownManager;

    void Start()
    {
        // 同じオブジェクトにあるクールダウンマネージャーを取得
        cooldownManager = GetComponent<AttackCooldownManager>();

        if (bulletPrefab == null)
        {
            Debug.LogError("弾Prefabが設定されていません！");
        }

        if (playerCamera == null)
        {
            Debug.LogError("プレイヤーのカメラが設定されていません！");
        }
    }

    void Update()
    {
        // 左クリックで攻撃
        if (Input.GetMouseButtonDown(0) && cooldownManager != null && cooldownManager.CanAttack())
        {
            FireProjectile();
            cooldownManager.ResetGauge(); // クールダウンをリセット
        }
    }

    /// <summary>
    /// 弾を発射
    /// </summary>
    private void FireProjectile()
    {
        // カメラの正面方向に基づいて発射位置と方向を計算
        Vector3 spawnPosition = playerCamera.transform.position + playerCamera.transform.forward * 1.5f;
        Vector3 fireDirection = playerCamera.transform.forward;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit))
        {
            // ヒットした位置に向けて発射方向を調整
            fireDirection = (hit.point - spawnPosition).normalized;
        }

        // 弾を生成
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.LookRotation(fireDirection));

        /// BallMovementを使用して初期力を設定
        BallMovement ballMovement = bullet.GetComponent<BallMovement>();
        if (ballMovement != null)
        {
            ballMovement.SetInitialForce(bulletForce); // 初期力を設定
        }
        else
        {
            Debug.LogError("弾PrefabにBallMovementコンポーネントがアタッチされていません！");
        }

        Debug.Log($"弾を発射しました！ 力: {bulletForce}N, 発射位置: {spawnPosition}, 発射方向: {fireDirection}");
    }
}
