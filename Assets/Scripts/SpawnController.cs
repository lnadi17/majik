using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BoxCollider))]
public class SpawnController : MonoBehaviour
{
    public Transform player;
    public GameObject[] gameObjects;
    [Range(0, 1f)]
    public float[] spawnChance;
    public float minSpawnTime;
    public float maxSpawnTime;

    // maxObjects should not be changed from inspector after entering play mode
    [SerializeField]
    private int maxObjects;

    private Bounds spawnBounds;
    private BoxCollider spawnArea;
    private int currentObjects;
    private List<EnemyController> enemyControllers;

    // Start is called before the first frame update
    void Start()
    {
        currentObjects = 0;
        enemyControllers = new List<EnemyController>();
        spawnArea = GetComponent<BoxCollider>();
        spawnBounds = spawnArea.bounds;
        spawnArea.enabled = false;
        StartCoroutine(SpawnCoroutine());
    }


    private IEnumerator SpawnCoroutine() {
        while (true) {
            for (int i = 0; i < gameObjects.Length; i++) {
                if (Random.Range(0, 1f) > spawnChance[i]) {
                    SpawnObject(gameObjects[i]);
                }
            }
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }

    private void SpawnObject(GameObject obj) {
        if (currentObjects == maxObjects) {
            Debug.Log("Max object count reached");
            return;
        }
        currentObjects++;
        Vector3 pos = RandomPointInBounds(spawnBounds);
        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(pos, out closestHit, 500, NavMesh.AllAreas)) {
            GameObject spawnedObject = Instantiate(obj, closestHit.position, Quaternion.identity);
            EnemyController spawnedController = spawnedObject.GetComponent<EnemyController>();
            spawnedController.DeathEvent += DecreaseObjectCount;
            spawnedController.goal = player.transform;
            enemyControllers.Add(spawnedController);
        } else {
            Debug.Log("Could not spawn");
        }
    }

    private Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    private void DecreaseObjectCount() {
        currentObjects--;
    }

    private void OnDisable() {
        foreach(EnemyController controller in enemyControllers) {
            controller.DeathEvent -= DecreaseObjectCount;
        }
    }
}

