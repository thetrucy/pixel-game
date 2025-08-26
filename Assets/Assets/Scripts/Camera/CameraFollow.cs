using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;           // Player
    public float smoothSpeed = 0.125f; // Độ mượt của camera
    public Vector3 offset;             // Khoảng cách so với player

    public float minX = 12.5f;         // Giới hạn trái
    public float maxX = 50.5f;         // Giới hạn phải
    public float minY = -1.5f;         // Giới hạn dưới
    public float maxY = 16.5f;         // Giới hạn trên

    private void LateUpdate()
    {
        if (target == null) return;

        // Vị trí mong muốn dựa trên target + offset
        float desiredX = target.position.x + offset.x;
        float desiredY = target.position.y + offset.y;

        // Giới hạn X và Y
        float clampedX = Mathf.Clamp(desiredX, minX, maxX);
        float clampedY = Mathf.Clamp(desiredY, minY, maxY);

        // Vị trí mới camera
        Vector3 newPos = new Vector3(clampedX, clampedY, transform.position.z);

        // Mượt theo Lerp
        transform.position = Vector3.Lerp(transform.position, newPos, smoothSpeed);
    }
}
