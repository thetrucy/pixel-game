using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    public Transform target; // Assign your player transform in the inspector
    public Vector3 offset = new Vector3(0, 0, -10); // Camera offset

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.position + offset;
    }
}
