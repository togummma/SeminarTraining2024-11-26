using UnityEngine;

public class CameraCollisionHandler : MonoBehaviour
{
    public Transform player;
    public float distance = 10f; // カメラとプレイヤーの距離
    public float height = 5f; // カメラの高さ
    public float collisionOffset = 0.2f; // カメラと障害物の間隔
    public string terrainTag = "Terrain"; // 地形タグ

    public float mouseSensitivity = 5f; // マウスの感度
    private float currentYaw = 0f; // 水平回転
    private float currentPitch = 20f; // 垂直回転
    public float minPitch = -60f;
    public float maxPitch = 80f;

    [Header("プレイヤー位置オフセット")]
    public Vector2 screenOffset = new Vector2(0f, 0f); // プレイヤーを画面中央からオフセットする (X: 左右, Y: 上下)

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Playerオブジェクトが設定されていません。");
        }

        Cursor.lockState = CursorLockMode.Locked; // カーソルをロック
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // マウス入力でカメラの回転を更新
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        currentYaw += mouseX;
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

        // カメラの理想的な位置を計算
        Vector3 offset = new Vector3(0, height, -distance);
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 desiredPosition = player.position + rotation * offset;

        // レイキャストで衝突判定
        Vector3 direction = desiredPosition - player.position;
        float targetDistance = direction.magnitude;

        if (Physics.Raycast(player.position, direction.normalized, out RaycastHit hit, targetDistance))
        {
            if (hit.collider.CompareTag(terrainTag))
            {
                desiredPosition = hit.point - direction.normalized * collisionOffset;
            }
        }

        // カメラの位置とプレイヤーをオフセット位置に基づき調整
        transform.position = desiredPosition;

        // プレイヤーをオフセットに基づき視点内で調整
        Vector3 adjustedPlayerPosition = player.position;
        Vector3 screenSpaceOffset = transform.right * screenOffset.x + transform.up * screenOffset.y;
        adjustedPlayerPosition += screenSpaceOffset;

        transform.LookAt(adjustedPlayerPosition);
    }

    // カメラの向きに基づく方向を取得
    public Vector3 GetCameraDirection()
    {
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        return rotation * Vector3.forward; // カメラが向いている方向
    }
}
