using UnityEngine;
using Enemies;

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
            Collider2D enemyCol = GetComponent<Collider2D>();
            Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;

            if (player != null && enemyCol != null)
            {
                Collider2D playerCol = player.GetComponent<Collider2D>();
                if (playerCol != null)
                {
                    Physics2D.IgnoreCollision(enemyCol, playerCol, true);
                }
            }
        }
    }
}
