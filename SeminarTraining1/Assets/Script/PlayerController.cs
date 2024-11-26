using UnityEngine;

public class PlayerRoller : MonoBehaviour
{
    public float moveForce = 10f; // 移動の力
    public float jumpForce = 5f; // ジャンプの力
    public float maxSpeed = 10f; // 速度の上限
    private Rigidbody rb; // Rigidbodyコンポーネントへの参照

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
    }

    void Update()
    {
        // 入力を取得
        float moveX = Input.GetAxis("Horizontal"); // 左右移動
        float moveZ = Input.GetAxis("Vertical");   // 前後移動

        // 力を加えて転がす
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ);
        rb.AddForce(moveDirection * moveForce);

        // ジャンプ処理
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // 速度制限
        LimitSpeed();
    }

    void Jump()
    {
        // 常にジャンプ可能にする
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void LimitSpeed()
    {
        // 現在の速度
        Vector3 currentVelocity = rb.linearVelocity;

        // 水平方向の速度を計算
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);

        // 水平方向の速度が上限を超えている場合
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            // 最大速度に制限
            Vector3 limitedVelocity = horizontalVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, currentVelocity.y, limitedVelocity.z);
        }
    }
}
