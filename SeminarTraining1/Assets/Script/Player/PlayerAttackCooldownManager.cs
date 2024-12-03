using UnityEngine;

public class AttackCooldownManager : MonoBehaviour
{
    [Header("攻撃設定")]
    public float attackCooldown = 2f; // クールタイム（秒）
    [Range(0, 1)] public float initialGaugeRatio = 0.5f; // ゲージの初期割合（0=0%、1=100%）

    private float currentGauge; // 現在の攻撃可能ゲージ値

    void Start()
    {
        // 攻撃可能ゲージを初期化
        currentGauge = initialGaugeRatio * attackCooldown;
    }

    void Update()
    {
        // ゲージを回復
        if (currentGauge < attackCooldown)
        {
            currentGauge += Time.deltaTime;
            currentGauge = Mathf.Min(currentGauge, attackCooldown); // 最大値を超えないようにする
        }
    }

    /// <summary>
    /// 現在の攻撃ゲージ値を取得
    /// </summary>
    /// <returns>攻撃ゲージ値</returns>
    public float GetCurrentGauge()
    {
        return currentGauge;
    }

    /// <summary>
    /// 攻撃可能かどうかを判定
    /// </summary>
    /// <returns>攻撃可能であればtrue</returns>
    public bool CanAttack()
    {
        return currentGauge >= attackCooldown;
    }

    /// <summary>
    /// 攻撃後にゲージをリセット
    /// </summary>
    public void ResetGauge()
    {
        currentGauge = 0;
    }
}
