using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // プレイヤーのTransform
    public Vector3 offset = new Vector3(0, 5, -10); // カメラの位置オフセット
    public float smoothSpeed = 0.125f; // 滑らかさの速度

    void LateUpdate()
    {
        // 目標の位置を計算
        Vector3 desiredPosition = target.position + offset;
        // 滑らかに移動
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // カメラ位置を更新
        transform.position = smoothedPosition;

        // 常にプレイヤーを注視
        transform.LookAt(target);
    }
}
