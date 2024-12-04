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
        if (bulletPrefab == null)
        {
            Debug.LogError("弾Prefabが設定されていません！");
        }

        if (crosshairManager == null)
        {
            crosshairManager = GetComponent<PlayerAttackCrosshairManager>();
            if (crosshairManager == null)
            {
                Debug.LogError("PlayerAttackCrosshairManagerが見つかりません！同じGameObjectにアタッチされていますか？");
            }
        }

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
            if (targetPoint == Vector3.zero)
            {
                Debug.LogError("targetPoint が無効です。クロスヘアが正しく動作しているか確認してください。");
                return;
            }

            AlignPlayerToShootDirection(targetPoint);
            Shoot(targetPoint);
        }
    }

    void AlignPlayerToShootDirection(Vector3 targetPoint)
    {
        Vector3 directionToTarget = (targetPoint - playerTransform.position).normalized;

        directionToTarget.y = 0f; // Y軸成分を無視
        if (directionToTarget.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            playerTransform.rotation = targetRotation;
        }
    }

    void Shoot(Vector3 targetPoint)
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("弾Prefabが設定されていません。");
            return;
        }

        Vector3 directionToTarget = (targetPoint - playerTransform.position).normalized;
        Vector3 spawnPosition = playerTransform.position + directionToTarget * spawnDistance;

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        if (bullet == null)
        {
            Debug.LogError("弾の生成に失敗しました。");
            return;
        }

        BallManager ballManager = bullet.GetComponent<BallManager>();
        if (ballManager == null)
        {
            Debug.LogError("生成された弾に BallManager がアタッチされていません。");
            return;
        }

        Debug.Log("Launching ball...");
        ballManager.Launch(directionToTarget, bulletForce);
    }
}
