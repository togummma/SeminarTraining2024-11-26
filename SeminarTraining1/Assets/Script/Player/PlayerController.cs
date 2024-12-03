using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 10f; // プレイヤーの移動速度（高速化）
    public float rotationSpeed = 10f; // 回転速度
    public float buoyancy = 2f;  // 水中の浮遊感（上下の慣性）
    public float drag = 0.9f;    // 水中の抵抗

    [Header("参照設定")]
    public Camera playerCamera; // プレイヤーが操作の基準とするカメラ

    private Rigidbody rb;

    void Start()
    {
        Debug.Log("PlayerController.cs: プレイヤーの移動制御スクリプト開始");

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbodyがアタッチされていません。RigidbodyをPlayerオブジェクトに追加してください。");
        }

        if (playerCamera == null)
        {
            Debug.LogError("Cameraが設定されていません。PlayerCameraを割り当ててください。");
        }
    }

void Update()
{
    if (playerCamera == null) return;

    // 入力取得
    float horizontal = Input.GetAxis("Horizontal"); // 左右移動（A/Dキー、または矢印キー）
    float vertical = Input.GetAxis("Vertical");     // 前後移動（W/Sキー、または矢印キー）
    float ascend = Input.GetKey(KeyCode.Space) ? 1f : 0f;  // 上昇（スペースキー）
    float descend = Input.GetKey(KeyCode.LeftShift) ? 1f : 0f; // 下降（左シフトキー）

    // カメラの向きに基づく移動方向を計算
    Vector3 forward = playerCamera.transform.forward; // カメラの前方
    Vector3 right = playerCamera.transform.right;     // カメラの右方向
    forward.y = 0f; // 水平方向のみを考慮
    right.y = 0f;    // 水平方向のみを考慮
    forward.Normalize();
    right.Normalize();

    // 水平方向の移動を計算
    Vector3 moveDirectionHorizontal = (forward * vertical + right * horizontal).normalized * moveSpeed;

    // 上下方向の移動を計算
    float verticalMovement = (ascend - descend) * moveSpeed;

    // 移動方向を合成
    Vector3 moveDirection = moveDirectionHorizontal + new Vector3(0f, verticalMovement, 0f);

    // 移動方向がゼロベクトルでない場合のみ回転処理を実行
    if (moveDirectionHorizontal.sqrMagnitude > 0.01f)
    {
        RotateTowardsMovement(moveDirectionHorizontal);
    }

    // Rigidbodyに力を加える
    rb.AddForce(moveDirection, ForceMode.Acceleration);

    // 水中での抵抗効果
    rb.velocity *= drag;

    // 浮力効果（上下移動時に自然な慣性を追加）
    if (ascend > 0 || descend > 0)
    {
        rb.AddForce(Vector3.up * buoyancy * (ascend - descend), ForceMode.Acceleration);
    }
}

// プレイヤーを進行方向に回転させる
void RotateTowardsMovement(Vector3 moveDirection)
{
    // 移動方向がゼロベクトルでないことを確認
    if (moveDirection.sqrMagnitude > 0.01f)
    {
        // Y軸方向をゼロにして水平成分のみを使用
        Vector3 flatDirection = new Vector3(moveDirection.x, 0f, moveDirection.z).normalized;

        // 水平方向のみを考慮して回転を計算
        Quaternion targetRotation = Quaternion.LookRotation(flatDirection);

        // プレイヤーを滑らかに回転させる
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
}