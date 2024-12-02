using UnityEngine;

public class CameraCollisionHandler : MonoBehaviour
{
    // プレイヤーのTransformへの参照
    public Transform player;

    // カメラの距離と高さ
    public float distance = 10f;
    public float height = 5f;

    // レイキャストによるカメラ補正
    public float collisionOffset = 0.2f; // 地形にめり込まないようにするオフセット
    public string terrainTag = "Terrain"; // 地形タグ

    // カメラの回転速度
    public float mouseSensitivity = 5f;

    // カメラの回転角度
    private float currentYaw = 0f;
    private float currentPitch = 20f;

    // 垂直方向の回転制限
    public float minPitch = -60f;
    public float maxPitch = 80f;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Playerオブジェクトが設定されていません。インスペクターで設定してください。");
        }

        // カーソルをロック
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // マウス入力でカメラの回転角度を更新
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        currentYaw += mouseX;
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

        // カメラの理想的な位置を計算
        Vector3 offset = new Vector3(0, height, -distance);
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 desiredPosition = player.position + rotation * offset;

        // レイキャストで地形との衝突を判定
        Vector3 direction = desiredPosition - player.position; // プレイヤーからカメラへの方向
        float targetDistance = direction.magnitude;

        if (Physics.Raycast(player.position, direction.normalized, out RaycastHit hit, targetDistance))
        {
            // 衝突したオブジェクトが地形タグを持つ場合
            if (hit.collider.CompareTag(terrainTag))
            {
                // カメラを障害物の手前に配置
                desiredPosition = hit.point - direction.normalized * collisionOffset;
            }
        }

        // カメラの位置と回転を更新
        transform.position = desiredPosition;
        transform.LookAt(player.position); // プレイヤーを注視
    }
}
