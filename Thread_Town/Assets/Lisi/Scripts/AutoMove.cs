using UnityEngine;

public class AutoMove : MonoBehaviour
{
    [Header("移动开关")]
    public bool moveX = true;   // 是否在X轴上移动
    public bool moveY = false;  // 是否在Y轴上移动
    public bool moveZ = false;  // 是否在Z轴上移动

    [Header("移动参数")]
    public float speed = 1f;        // 移动速度
    public float distanceX = 2f;    // X轴移动距离
    public float distanceY = 0f;    // Y轴移动距离
    public float distanceZ = 0f;    // Z轴移动距离

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 offset = Vector3.zero;

        if (moveX)
        {
            offset.x = Mathf.PingPong(Time.time * speed, distanceX * 2) - distanceX;
        }

        if (moveY)
        {
            offset.y = Mathf.PingPong(Time.time * speed, distanceY * 2) - distanceY;
        }

        if (moveZ)
        {
            offset.z = Mathf.PingPong(Time.time * speed, distanceZ * 2) - distanceZ;
        }

        transform.position = startPos + offset;
    }
}
