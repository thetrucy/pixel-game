using UnityEngine;

public class AladinAttack : MonoBehaviour
{
    private Animator animator;
    private float attackTimer = 0f;

    [Header("Attack Settings")]
    public float attackCooldown = 5f;
    public GameObject laserPrefab;
    public float beamDuration = 0.5f;

    public Transform firePoint;
    private Transform player;

    private Vector3 lockedTargetPos;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            animator.SetTrigger("Attack");
            attackTimer = 0f;
        }
    }

    public void LockTargetPosition()
    {
        if (player != null)
        {
            lockedTargetPos = player.position;
            Debug.Log("Locked player position: " + lockedTargetPos);
        }
    }

    public void SpawnBeam()
    {
        if (laserPrefab == null) return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        GameObject beam = Instantiate(laserPrefab, spawnPos, Quaternion.identity);

        // dùng vị trí đã lock
        Vector2 dir = (lockedTargetPos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y - 0.1f, dir.x) * Mathf.Rad2Deg;

        beam.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Destroy(beam, beamDuration);
    }
}
