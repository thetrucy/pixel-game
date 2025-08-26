using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyChaseBase : MonoBehaviour
    {
        [Header("Chase Settings")]
        public string playerTag = "Player";
        public float moveSpeed = 1.5f;
        public float stopDistance = 3f;
        public float tolerance = 0.1f;

        protected Transform player;
        protected Rigidbody2D rb;
        protected SpriteRenderer sr;

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();

            GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("⚠ Không tìm thấy Player với tag: " + playerTag);
            }
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
            Vector2 direction = (player.position - transform.position);
            float distance = direction.magnitude;

            if (distance > stopDistance + tolerance)
            {
                // Di chuyển về phía player
                return direction.normalized;
            }
            else if (distance < stopDistance - tolerance)
            {
                // Lùi ra nếu quá gần
                return -direction.normalized;
            }
            else
            {
                // Giữ vị trí nếu ở khoảng cách phù hợp
                return Vector2.zero;
            }
        }
    }
}
