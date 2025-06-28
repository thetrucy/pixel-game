using UnityEngine;

public class Follow_Mouse : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Call the method to rotate the player towards the mouse position
        Rotate_Follow_Mouse();
    }
    void Rotate_Follow_Mouse()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Calculate the direction from the player to the mouse position
        Vector3 direction = (mousePosition - transform.position).normalized;
        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Rotate the player towards the mouse position
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
