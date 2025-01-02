using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    public static Managers Instance { get { Init(); return s_instance; } }

    PoolManager pool = new PoolManager();
    ResourceManager resource = new ResourceManager();

    public static PoolManager Pool { get { return Instance.pool; } }
    public static ResourceManager Resource { get { return Instance.resource; } }

    private void Awake()
    {
        s_instance = transform.GetComponent<Managers>();
    }

    static void Init()
    {
        s_instance.pool.Init();
    }

    public static void Clear()
    {
        Pool.Clear();
    }
}
