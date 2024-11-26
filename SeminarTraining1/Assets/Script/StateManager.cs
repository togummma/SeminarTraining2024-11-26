using UnityEngine;

public class StateManager : MonoBehaviour
{
    public float lifetime = 20f; // オブジェクトが消えるまでの時間（秒）

    void Start()
    {
        // lifetime秒後にこのオブジェクトを削除
        Destroy(gameObject, lifetime);
    }
}
