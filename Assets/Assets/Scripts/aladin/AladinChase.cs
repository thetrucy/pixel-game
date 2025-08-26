using UnityEngine;

namespace Enemies.Aladin
{
    public class AladinChase : EnemyChaseBase
    {
        [Header("Aladin Settings")]
        public float customStopDistance = 7f;

        protected override void Start()
        {
            base.Start();
            stopDistance = customStopDistance;

            player = GameObject.FindGameObjectWithTag("Player")?.transform;

            if (player != null)
            {
                Collider2D enemyCol = GetComponent<Collider2D>();
                Collider2D playerCol = player.GetComponent<Collider2D>();

                if (enemyCol != null && playerCol != null)
                {
                    Physics2D.IgnoreCollision(enemyCol, playerCol, true);
                }
            }
            else
            {
                Debug.LogWarning("'Player' not found!");
            }
        }
    }
}
