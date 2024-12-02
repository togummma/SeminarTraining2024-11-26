using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 移動速度
    public float moveSpeed = 5f; // プレイヤーの移動速度
    public float buoyancy = 2f;  // 水中の浮遊感（上下の慣性）
    public float drag = 0.9f;    // 水中の抵抗

    // Rigidbodyへの参照
    private Rigidbody rb;

    void Start()
    {
        Debug.Log("PlayerController.cs: プレイヤーの移動制御スクリプト開始"); // スクリプトの役割: プレイヤーの移動を管理します
        rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
        if (rb == null)
        {
            Debug.LogError("Rigidbodyがアタッチされていません。RigidbodyをPlayerオブジェクトに追加してください。");
        }
    }

    void Update()
    {
        // 入力取得
        float horizontal = Input.GetAxis("Horizontal"); // 左右移動（A/Dキー、または矢印キー）
        float vertical = Input.GetAxis("Vertical");     // 上下移動（W/Sキー、または矢印キー）
        float ascend = Input.GetKey(KeyCode.Space) ? 1f : 0f;  // 上昇（スペースキー）
        float descend = Input.GetKey(KeyCode.LeftShift) ? 1f : 0f; // 下降（左シフトキー）

        // 移動ベクトルを計算
        Vector3 moveDirection = new Vector3(horizontal, ascend - descend, vertical) * moveSpeed;

        // Rigidbodyに力を加える
        rb.AddForce(moveDirection, ForceMode.Acceleration);

        // 水中での浮遊感（徐々に速度を減少させる）
        rb.linearVelocity = rb.linearVelocity * drag;

        // 浮力効果（上下移動時に自然な慣性を追加）
        if (ascend > 0 || descend > 0)
        {
            rb.AddForce(Vector3.up * buoyancy * (ascend - descend), ForceMode.Acceleration);
        }
    }
}
