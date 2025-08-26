using UnityEngine;

namespace Enemies.Aladin
{
    public class AladinAttack : MonoBehaviour
    {
        private Animator animator;
        private Transform player;
        private float attackTimer = 0f;
        private Vector3 lockedTargetPos;

        [Header("Attack Settings")]
        public float attackCooldown = 5f;
        public GameObject laserPrefab;
        public float beamDuration = 0.5f;
        public Transform firePoint;

        private void Start()
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogWarning("Animator not found for AladinAttack!");
            }

            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogWarning("'Player' not found for AladinAttack!");
            }

            if (laserPrefab == null)
            {
                Debug.LogWarning("LaserPrefab not assigned for AladinAttack!");
            }

            if (firePoint == null)
            {
                Debug.LogWarning("FirePoint not assigned for AladinAttack!");
            }
        }

        private void Update()
        {
            if (player == null || GetComponent<AladinHealth>().isDead1()) return;

            attackTimer += Time.deltaTime;

            if (attackTimer >= attackCooldown)
            {
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                    LockTargetPosition();
                }
                attackTimer = 0f;
            }
        }

        public void LockTargetPosition()
        {
            if (player != null)
            {
                lockedTargetPos = player.position;
                Debug.Log($"Aladin locked player position: {lockedTargetPos}");
            }
            else
            {
                Debug.LogWarning("Cannot lock target position: Player is null!");
            }
        }

        public void SpawnBeam()
        {
            if (laserPrefab == null)
            {
                Debug.LogWarning("LaserPrefab not assigned for AladinAttack!");
                return;
            }

            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
            if (firePoint == null)
            {
                Debug.LogWarning("FirePoint not assigned, using transform.position for AladinAttack!");
            }

            GameObject beam = Instantiate(laserPrefab, spawnPos, Quaternion.identity);

            Vector2 dir = (lockedTargetPos - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y - 0.1f, dir.x) * Mathf.Rad2Deg;
            beam.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Destroy(beam, beamDuration);
        }
    }
}