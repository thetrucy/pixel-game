using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject playerPrefab;   // Prefab của Player
    public Transform spawnPoint;      // Vị trí spawn cố định

    private GameObject currentPlayer;

    void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player Prefab chưa được gán!");
            return;
        }

        if (currentPlayer != null)
        {
            return;
        }

        // Nếu có spawnPoint thì spawn ở đó, không thì spawn ngay vị trí PlayerSpawner
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
        currentPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

    }
}
