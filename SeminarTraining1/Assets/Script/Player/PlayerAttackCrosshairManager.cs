using UnityEngine;

public class PlayerAttackCrosshairManager : MonoBehaviour
{
    [Header("カメラ設定")]
    public Camera playerCamera; // TPSカメラ

    [Header("レイキャスト設定")]
    [SerializeField] private bool visualizeRay = true; // レイキャストの可視化フラグ（エディタから設定可能）

    private LineRenderer lineRenderer;

    void Start()
    {
        if (playerCamera == null)
        {
            Debug.LogError("TPSカメラが設定されていません！");
            return;
        }

        // LineRendererの設定
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f; // 線の開始幅
        lineRenderer.endWidth = 0.05f;   // 線の終了幅
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = Color.green; // 緑色の線

        // 初期状態を反映
        lineRenderer.enabled = visualizeRay;
    }

    void Update()
    {
        // フラグに基づいて可視化を切り替える
        lineRenderer.enabled = visualizeRay;

        if (visualizeRay)
        {
            UpdateVisualizedRay();
        }
    }

    // カメラからレイキャストを使ってターゲットポイントを取得
    public Vector3 GetTargetPoint()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("TPSカメラが設定されていません！");
            return Vector3.zero;
        }

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            return hit.point; // ヒットした位置を返す
        }

        // ヒットしなかった場合は最大距離の仮想地点
        return ray.GetPoint(100f);
    }

    // レイキャストの表示を更新
    private void UpdateVisualizedRay()
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
