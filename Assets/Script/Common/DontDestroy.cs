using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        GameObject.DontDestroyOnLoad(gameObject);
    }
}