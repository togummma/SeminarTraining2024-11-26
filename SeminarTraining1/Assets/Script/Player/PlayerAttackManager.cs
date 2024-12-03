using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    [Header("弾の設定")]
    public GameObject bulletPrefab; // 弾のPrefab
    public float spawnDistance = 2f; // プレイヤーからの弾の生成距離
    public float bulletForce = 20f; // 弾の発射時の力（ニュートン）

    [Header("カメラ設定")]
    public Camera playerCamera; // TPSカメラ
    public Transform playerTransform; // プレイヤーのTransform（弾の生成基準）

    private LineRenderer lineRenderer; // レイキャスト表示用

    void Start()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("弾Prefabが設定されていません！");
        }

        if (playerCamera == null)
        {
            Debug.LogError("TPSカメラが設定されていません！");
        }

        if (playerTransform == null)
        {
            Debug.LogError("プレイヤーのTransformが設定されていません！");
        }

        // LineRendererの設定
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f; // 線の開始幅
        lineRenderer.endWidth = 0.05f;   // 線の終了幅
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = Color.green; // 緑色の線
    }

    void Update()
    {
        // レイキャストを表示
        UpdateVisualizedRay();

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 targetPoint = GetTargetPoint();
            AlignPlayerToShootDirection(targetPoint);
            Shoot(targetPoint);
        }
    }

    // カメラからレイキャストを使ってターゲットポイントを取得
    Vector3 GetTargetPoint()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            return hit.point; // ヒットした位置を返す
        }

        // ヒットしなかった場合は最大距離の仮想地点
        return ray.GetPoint(100f);
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

    // カメラからのレイキャストを可視化
    void UpdateVisualizedRay()
    {
        if (lineRenderer == null || playerCamera == null) return;

        // カメラの中心からのレイキャストを計算
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        Vector3 endPoint;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            endPoint = hit.point;
        }
        else
        {
            endPoint = ray.GetPoint(100f);
        }

        // LineRendererを更新
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, ray.origin); // カメラの位置
        lineRenderer.SetPosition(1, endPoint);   // レイキャストの終点
    }
}
