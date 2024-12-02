using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Playing, Paused, GameOver }
    public GameState CurrentState { get; private set; }

    public int totalEnemies = 0; // 残りの敵数
    public int score = 0;        // プレイヤーのスコア

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        CurrentState = GameState.Playing;
    }

    public void UpdateScore(int points)
    {
        score += points;
        Debug.Log($"スコア: {score}");
    }

    public void GameOver()
    {
        CurrentState = GameState.GameOver;
        Debug.Log("ゲームオーバー！");
        // 必要に応じてUIを表示
    }
}
