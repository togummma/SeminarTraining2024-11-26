using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public int maxHealth = 100; // 最大体力
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth; // 初期体力設定
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth); // 体力が0未満にならないようにする

        Debug.Log($"ダメージを受けました。現在の体力: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(maxHealth, currentHealth); // 体力が最大を超えないようにする

        Debug.Log($"回復しました。現在の体力: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log("プレイヤーが死亡しました！");
        // 死亡処理を記述（例: ゲームオーバー画面の表示）
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
