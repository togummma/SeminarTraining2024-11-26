using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    public float maxDistance = 100f; // レイキャストの最大距離
    public LayerMask hitLayers;      // レイキャストが当たるレイヤー
    private RaycastHit hitInfo;      // レイキャストのヒット情報

    public Vector3 HitPoint { get; private set; } // ヒット地点
    public bool HasHit { get; private set; }      // ヒットしたかどうか

    void Update()
    {
        // カメラ中心からのレイキャストを計算
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hitInfo, maxDistance, hitLayers))
        {
            HitPoint = hitInfo.point;
            HasHit = true;
        }
        else
        {
            HitPoint = ray.GetPoint(maxDistance); // ヒットしなかった場合は最大距離地点
            HasHit = false;
        }

        // デバッグ用にレイキャストを視覚化
        Debug.DrawLine(ray.origin, HitPoint, HasHit ? Color.green : Color.red);
    }
}
