using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] spawnObjects; // 発生させるオブジェクトの配列
    public float spawnRadius = 10f;  // 発生範囲の半径
    public float spawnInterval = 2f; // 発生間隔（秒）

    private float timer; // 発生間隔を管理するタイマー

    void Update()
    {
        // タイマーを進める
        timer += Time.deltaTime;

        // 指定の間隔に達したらオブジェクトを発生させる
        if (timer >= spawnInterval)
        {
            SpawnObject();
            timer = 0f; // タイマーをリセット
        }
    }

    void SpawnObject()
    {
        if (spawnObjects.Length == 0) return; // 発生させるオブジェクトがない場合は何もしない

        // ランダムな位置を計算（プレイヤーのZ座標より前方に限定）
        Vector3 spawnPosition;
        do
        {
            spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = transform.position.y; // 地面と同じ高さにする
        } while (spawnPosition.z <= transform.position.z); // Z座標がプレイヤーの前方になるまでループ

        // ランダムなオブジェクトを選択
        GameObject randomObject = spawnObjects[Random.Range(0, spawnObjects.Length)];

        // オブジェクトを発生
        GameObject spawnedObject = Instantiate(randomObject, spawnPosition, Quaternion.identity);

        // ランダムな色を適用
        ApplyRandomColor(spawnedObject);
    }

    void ApplyRandomColor(GameObject obj)
    {
        // オブジェクトにRendererがあるか確認
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            // ランダムな色を生成
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            // マテリアルに適用
            objRenderer.material.color = randomColor;
        }
    }
}
