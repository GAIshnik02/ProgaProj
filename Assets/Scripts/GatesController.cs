using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public GameObject[] gates; // Массив объектов стен, которые будут использоваться как ворота
    private bool roomCleared = false;
    private List<Enemy> enemiesInRoom = new List<Enemy>();
    public GameObject[] enemyTypes;
    public Transform[] enemySpawners;
    public GameObject shield;
    public GameObject healthPotion;
    public PlayerController player;

    [HideInInspector] public List<GameObject> enemies;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>(); // Ищем объект PlayerController в сцене
        if (player == null)
        {
            Debug.LogError("PlayerController not found in the scene.");
        }

        // Заполняем список врагов в комнате, если есть
        Enemy[] enemiesArray = GetComponentsInChildren<Enemy>();
        foreach (Enemy enemy in enemiesArray)
        {
            enemiesInRoom.Add(enemy);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !roomCleared)
        {
            StartCoroutine(StartRoomSequence());
        }
    }

    private IEnumerator StartRoomSequence()
    {
        yield return new WaitForSeconds(1f);
        CloseDoors();

        foreach (Transform spawner in enemySpawners)
        {
            int rand = Random.Range(0, 11);
            if (rand < 9)
            {
                GameObject enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
                GameObject enemy = Instantiate(enemyType, spawner.position, Quaternion.identity);
                enemy.transform.parent = transform;
                enemies.Add(enemy);
            }
            else if (rand == 9)
            {
                Instantiate(healthPotion, spawner.position, Quaternion.identity);
            }
            else
            {
                Instantiate(shield, spawner.position, Quaternion.identity);
            }
        }

        StartCoroutine(CheckEnemies());
    }

    private IEnumerator CheckEnemies()
    {
        yield return new WaitForSeconds(3f);
        yield return new WaitUntil(() => enemies.Count == 0);

        OpenDoors();
        roomCleared = true;
        if (player != null)
        {
            Debug.Log("Room cleared, updating cleared rooms count.");
            player.UpdateClearRooms();
        }
        else
        {
            Debug.LogError("PlayerController is not assigned.");
        }
    }

    private void CloseDoors()
    {
        foreach (GameObject gate in gates)
        {
            if (gate != null)
            {
                gate.SetActive(true); // Активируйте стены, закрывая комнату
            }
        }
    }

    private void OpenDoors()
    {
        foreach (GameObject wall in gates)
        {
            if (wall != null)
            {
                Destroy(wall); // Уничтожайте стены, открывая комнату
            }
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemiesInRoom.Remove(enemy);
        if (enemiesInRoom.Count == 0)
        {
            OpenDoors();
        }
    }
}
