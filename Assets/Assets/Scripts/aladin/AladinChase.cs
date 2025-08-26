using UnityEngine;

namespace Enemies.Aladin {
    public class AladinChase : EnemyChaseBase
    {
        [Header("Aladin Settings")] public float customStopDistance = 7f;

        protected override void Start()
        {
            base.Start();
            stopDistance = customStopDistance;

            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogWarning("'Player' not found for AladinChase!");
                return;
            }

            Collider2D enemyCol = GetComponent<Collider2D>();
            Collider2D playerCol = player.GetComponent<Collider2D>();
            if (enemyCol != null && playerCol != null)
            {
                Physics2D.IgnoreCollision(enemyCol, playerCol, true);
            }
            else
            {
                Debug.LogWarning("Collider missing for AladinChase or Player!");
            }

            // Đăng ký với EnemyManager
            if (EnemyManager.Instance != null)
            {
                EnemyManager.Instance.RegisterEnemy();
            }
            else
            {
                Debug.LogWarning("EnemyManager instance not found for AladinChase!");
            }
        }

        protected override void Update()
        {
            if (player == null || GetComponent<AladinHealth>().isDead1()) return;

            Vector2 moveDir = GetChaseDirection();
            rb.linearVelocity = moveDir * moveSpeed;

            if (sr != null)
            {
                sr.flipX = player.position.x < transform.position.x;
            }
            else
            {
                Debug.LogWarning("SpriteRenderer not found for AladinChase!");
            }
        }
    }
}