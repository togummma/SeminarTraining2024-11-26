using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("弾の設定")]
    public GameObject bulletPrefab; // 弾のPrefab
    public float spawnDistance = 2f; // プレイヤーからの弾の生成距離
    public float bulletForce = 20f; // 弾の発射時の力（ニュートン）

    [Header("参照設定")]
    public PlayerAttackCrosshairManager crosshairManager; // クロスヘア管理クラス
    public Transform playerTransform; // プレイヤーのTransform（弾の生成基準）

    void Start()
    {
        // bulletPrefabが設定されていない場合の警告
        if (bulletPrefab == null)
        {
            Debug.LogError("弾Prefabが設定されていません！");
        }

        // crosshairManagerを自動で探す
        if (crosshairManager == null)
        {
            crosshairManager = GetComponent<PlayerAttackCrosshairManager>();
            if (crosshairManager == null)
            {
                Debug.LogError("PlayerAttackCrosshairManagerが見つかりません！同じGameObjectにアタッチされていますか？");
            }
        }

        // playerTransformが設定されていない場合の警告
        if (playerTransform == null)
        {
            playerTransform = transform; // 自身のTransformをデフォルトに設定
            Debug.LogWarning("playerTransformが設定されていませんでした。自動的に自身のTransformを設定しました。");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 targetPoint = crosshairManager.GetTargetPoint();
            AlignPlayerToShootDirection(targetPoint);
            Shoot(targetPoint);
        }
    }

    // プレイヤーの向きを射撃方向に合わせる
    void AlignPlayerToShootDirection(Vector3 targetPoint)
    {
        Vector3 directionToTarget = (targetPoint - playerTransform.position).normalized;

        // 水平方向のみを考慮してプレイヤーを回転
        directionToTarget.y = 0f; // Y軸成分を無視して水平方向の回転のみ適用
        if (directionToTarget.sqrMagnitude > 0.01f) // 無効な方向を防ぐ
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            playerTransform.rotation = targetRotation;
        }
    }

    // 弾を発射
    void Shoot(Vector3 targetPoint)
    {
        Vector3 directionToTarget = (targetPoint - playerTransform.position).normalized;

        // 発射位置を計算
        Vector3 spawnPosition = playerTransform.position + directionToTarget * spawnDistance;

        // 弾を生成して発射
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(directionToTarget * bulletForce, ForceMode.Impulse);
            }
        }
    }
}
