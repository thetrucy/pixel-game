using UnityEngine;
using System.Collections;

public class LaserBeam : MonoBehaviour
{
    public int damage = 10;
    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false; // tắt collider lúc spawn
    }

    void OnEnable()
    {
        StartCoroutine(DamageWindow());
    }

    IEnumerator DamageWindow()
    {
        // Chờ 0.2s trước khi bắt đầu gây damage
        yield return new WaitForSeconds(0.2f);

        if (col != null) col.enabled = true;  // bật collider

        // Giữ collider mở trong 0.1s (0.2 -> 0.3s)
        yield return new WaitForSeconds(0.3f);

        if (col != null) col.enabled = false; // tắt collider
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthManager playerHP = other.GetComponent<HealthManager>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(damage);
                col.enabled = false;
            }
        }
    }
}
