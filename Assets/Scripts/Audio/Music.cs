using UnityEngine;

// Đặt trên object Music, PHẢI ở gốc Hierarchy (DontDestroyOnLoad không hoạt động với object con)
public class Music : MonoBehaviour
{
    static Music instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
