using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyChaseBase : MonoBehaviour
    {
        [Header("Chase Settings")]
        public Transform player;
        public float moveSpeed = 1.5f;
        public float stopDistance = 3f;
        public float tolerance = 0.1f;

        protected Rigidbody2D rb;
        protected SpriteRenderer sr;

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
        }

        protected virtual void Update()
        {
            if (player == null) return;

            Vector2 moveDir = GetChaseDirection();

            rb.linearVelocity = moveDir * moveSpeed;

            if (sr != null)
                sr.flipX = (player.position.x < transform.position.x);
        }

        protected virtual Vector2 GetChaseDirection()
        {
            float distanceX = Mathf.Abs(transform.position.x - player.position.x);
            float dirX = Mathf.Sign(player.position.x - transform.position.x);

            if (distanceX > stopDistance + tolerance)
            {
                return new Vector2(dirX, 0f);
            }
            else if (distanceX < stopDistance - tolerance)
            {
                return new Vector2(-dirX, 0f);
            }
            else
            {
                float targetX = player.position.x - dirX * stopDistance;
                Vector2 targetPos = new Vector2(targetX, transform.position.y);
                return (targetPos - (Vector2)transform.position).normalized;
            }
        }
    }
}
