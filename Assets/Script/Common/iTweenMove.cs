using UnityEngine;

public class iTweenMove : MonoBehaviour
{
    public float x = 0;
    public float time = 0;

    // Use this for initialization
    private void Start()
    {
        iTween.MoveBy(gameObject, iTween.Hash("x", x, "time", time, "easeType", "linear", "loopType", "pingPong", "delay", 0));
    }
}