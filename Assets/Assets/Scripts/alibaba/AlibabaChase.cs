using UnityEngine;
using Enemies;
using System.Collections;
using System.Linq;

namespace Enemies.Alibaba
{
    public class AlibabaChase : EnemyChaseBase
    {
        [Header("Alibaba Settings")]
        public float customStopDistance = 1f;

        [Header("Headbutt Settings")]
        public float headbuttRange = 1.5f;
        public float headbuttDamage = 10f;
        public float headbuttCooldown = 2f;
        public string headbuttTrigger = "Headbutt";
        public float dashSpeed = 20f;
        public float dashDuration = 0.5f;

        private float headbuttCooldownTimer = 0f;
        private bool isHeadbutting = false;
        private Animator animator;
        private Vector2 dashDirection;
        private bool canDamage = false;

        protected override void Start()
        {
            base.Start();
            stopDistance = customStopDistance;
            animator = GetComponent<Animator>();

            player = GameObject.FindGameObjectWithTag("Player")?.transform;

            // Ignore collision với player
            if (player != null)
            {
                Collider2D enemyCol = GetComponent<Collider2D>();
                Collider2D playerCol = player.GetComponent<Collider2D>();
                if (enemyCol != null && playerCol != null)
                    Physics2D.IgnoreCollision(enemyCol, playerCol, true);
            }
        }

        protected override void Update()
        {
            if (player == null || GetComponent<AlibabaHealth>().isDead) return;

            // cooldown headbutt
            if (headbuttCooldownTimer > 0)
                headbuttCooldownTimer -= Time.deltaTime;

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // trigger dash + anim
            if (!isHeadbutting && headbuttCooldownTimer <= 0 && distanceToPlayer <= headbuttRange)
            {
                isHeadbutting = true;
                headbuttCooldownTimer = headbuttCooldown;

                dashDirection = new Vector2(Mathf.Sign(player.position.x - transform.position.x), 0f);

                if (animator != null)
                    animator.SetTrigger(headbuttTrigger);

                StartCoroutine(DashRoutine());
            }

            // chase bình thường nếu không dash
            if (!isHeadbutting)
            {
                Vector2 moveDir = GetChaseDirection();
                rb.linearVelocity = new Vector2(moveDir.x * moveSpeed, rb.linearVelocity.y);

                if (sr != null)
                    sr.flipX = (player.position.x < transform.position.x);
            }
        }

        private IEnumerator DashRoutine()
        {
            canDamage = true;
            rb.linearVelocity = dashDirection * dashSpeed;

            yield return new WaitForSeconds(dashDuration);

            // kết thúc dash
            isHeadbutting = false;
            canDamage = false;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (canDamage && other.CompareTag("Player"))
            {
                HealthManager playerHealth = other.GetComponent<HealthManager>() ?? other.GetComponentInParent<HealthManager>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage((int)headbuttDamage);
                    canDamage = false; // chỉ damage 1 lần trên collider
                }
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            OnTriggerEnter2D(other);
        }

        private void OnDisable()
        {
            if (animator != null)
                animator.ResetTrigger(headbuttTrigger);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, headbuttRange);
        }
    }
}
