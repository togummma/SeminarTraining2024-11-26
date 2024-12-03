using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    [Header("弾の設定")]
    public GameObject bulletPrefab; // 弾のPrefab
    public float spawnDistance = 2f; // プレイヤーからの弾の生成距離
    public float bulletForce = 20f; // 弾の発射時の力（ニュートン）

    [Header("カメラ設定")]
    public Camera playerCamera; // 攻撃方向を決定するカメラ
    public Transform playerTransform; // プレイヤーのTransform（弾の生成基準）

    private LineRenderer lineRenderer;

    void Start()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("弾Prefabが設定されていません！");
        }

        if (playerCamera == null)
        {
            Debug.LogError("カメラが設定されていません！");
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
        // 常にレイキャストの可視化を更新
        UpdateVisualizedRay();

        if (Input.GetMouseButtonDown(0))
        {
            AlignPlayerToShootDirection();
            Shoot();
        }
    }

    // 射撃処理
    void Shoot()
    {
        Ray ray = new Ray(playerTransform.position, playerTransform.forward);
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
        }

        Vector3 shootDirection = (targetPoint - playerTransform.position).normalized;
        Vector3 spawnPosition = playerTransform.position + shootDirection * spawnDistance;

        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(shootDirection * bulletForce, ForceMode.Impulse);
            }
        }
    }

    // プレイヤーの向きを射撃方向に合わせる
    void AlignPlayerToShootDirection()
    {
        Ray ray = new Ray(playerTransform.position, playerTransform.forward);
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
        }

        Vector3 shootDirection = (targetPoint - playerTransform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(shootDirection.x, 0, shootDirection.z));
        playerTransform.rotation = targetRotation;
    }

    // レイキャストの可視化を常に更新
    void UpdateVisualizedRay()
    {
        if (lineRenderer == null || playerCamera == null) return;

        // プレイヤーの位置と向きを基準にレイキャスト
        Ray ray = new Ray(playerTransform.position, playerTransform.forward);
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
        lineRenderer.SetPosition(0, ray.origin); // プレイヤー位置
        lineRenderer.SetPosition(1, endPoint);   // レイキャストの終点
    }
}
