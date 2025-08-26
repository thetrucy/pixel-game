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

            // ❌ Không cần IgnoreCollision nữa
        }

        protected override void Update()
        {
            if (player == null || GetComponent<AlibabaHealth>().isDead) return;

            // Đếm ngược hồi chiêu
            if (headbuttCooldownTimer > 0)
                headbuttCooldownTimer -= Time.deltaTime;

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Kích hoạt headbutt
            if (headbuttCooldownTimer <= 0 && distanceToPlayer <= headbuttRange && !isHeadbutting)
            {
                headbuttCooldownTimer = headbuttCooldown;
                isHeadbutting = true;

                // Dash ngang theo vị trí Player
                dashDirection = new Vector2(Mathf.Sign(player.position.x - transform.position.x), 0f);

                if (animator != null)
                {
                    var attackClip = animator.runtimeAnimatorController.animationClips
                        .FirstOrDefault(c => c.name == "Attack");
                    if (attackClip != null)
                    {
                        animator.speed = attackClip.length / dashDuration;
                    }

                    animator.SetTrigger(headbuttTrigger);
                    Debug.Log("Alibaba kích hoạt húc đầu!");
                }

                StartCoroutine(DashRoutine());
            }

            // Nếu không dash thì chase bình thường
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
            canDamage = true; // Cho phép gây damage

            rb.linearVelocity = dashDirection * dashSpeed;

            yield return new WaitForSeconds(dashDuration);

            // Kết thúc dash
            isHeadbutting = false;
            canDamage = false;

            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            if (animator != null)
            {
                animator.speed = 1f; // Reset speed
                animator.ResetTrigger(headbuttTrigger);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (canDamage && other.CompareTag("Player"))
            {
                HealthManager playerHealth = other.GetComponent<HealthManager>()
                                            ?? other.GetComponentInParent<HealthManager>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage((int)headbuttDamage);
                    Debug.Log($"Alibaba húc đầu gây {headbuttDamage} sát thương!");
                    canDamage = false;
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
                animator.speed = 1f;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, headbuttRange);
        }
    }
}
