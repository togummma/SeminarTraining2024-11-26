using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    private PlayerHealthManager healthManager; // プレイヤーの体力を管理するスクリプト
    private AttackCooldownManager cooldownManager; // クールダウン管理スクリプト
    private ProgressBar healthBar; // UIの体力バー
    private ProgressBar attackGaugeBar; // UIの攻撃可能ゲージバー

    void Start()
    {
        // UI Documentのルート要素を取得
        var root = GetComponent<UIDocument>().rootVisualElement;

        // HUD要素を取得
        healthBar = root.Q<ProgressBar>("health-bar");
        attackGaugeBar = root.Q<ProgressBar>("attack-gauge-bar");

        // プレイヤーのスクリプトを取得
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            healthManager = player.GetComponent<PlayerHealthManager>();
            cooldownManager = player.GetComponent<AttackCooldownManager>();
        }
        else
        {
            Debug.LogError("タグ 'Player' を持つオブジェクトが見つかりません！");
        }
    }

    void Update()
    {
        if (healthManager != null)
        {
            // 体力バーを更新
            UpdateHealthBar();
        }

        if (cooldownManager != null)
        {
            // 攻撃可能ゲージバーを更新
            UpdateAttackGaugeBar();
        }
    }

    /// <summary>
    /// 体力バーを更新する
    /// </summary>
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = ((float)healthManager.GetCurrentHealth() / healthManager.GetMaxHealth()) * 100;
        }
        else
        {
            Debug.LogWarning("体力バーがUIに設定されていません！");
        }
    }

    /// <summary>
    /// 攻撃可能ゲージバーを更新する
    /// </summary>
    private void UpdateAttackGaugeBar()
    {
        if (attackGaugeBar != null)
        {
            float currentGauge = cooldownManager.GetCurrentGauge();
            float maxGauge = cooldownManager.attackCooldown;
            attackGaugeBar.value = (currentGauge / maxGauge) * 100;
        }
        else
        {
            Debug.LogWarning("攻撃可能ゲージバーがUIに設定されていません！");
        }
    }
}
