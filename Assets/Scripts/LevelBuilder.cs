using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public Sprite BGOverlay;
    public GameObject cannonPrefab, keyPrefab, enemyPrefab, gemPrefab, enemyManager;
    public List<GameObject> enemies, keys, gems, obstacles, elementsToPlace;
    public Vector2 enemyRange, keyPerEnemyRange;
    public float radius;
    public float offset;

    private void Start()
    {
        PlaceElements();
    }

    public void PlaceElements()
    {
        GameObject manager = Instantiate(enemyManager, transform);
        manager.transform.position = transform.position;
        GameObject cannon = Instantiate(cannonPrefab, transform);
        cannon.transform.position = transform.position;

        cannon.GetComponent<CannonManager>().bgOverlay = BGOverlay;

        //difficulty goes up on each world completed
        float minDifficulty = LevelManager.Instance.DifficultyRange.x;
        float maxDifficulty = LevelManager.Instance.DifficultyRange.y;

        //more enemies/keys will spawn based on how difficult, but will not exceed or go under set difficulty range
        int minEnemyDifficulty = Mathf.RoundToInt(Mathf.Clamp(enemyRange.x + minDifficulty, enemyRange.x, enemyRange.y));
        int minKeyDifficulty = Mathf.RoundToInt(Mathf.Clamp(keyPerEnemyRange.x + maxDifficulty, keyPerEnemyRange.x, keyPerEnemyRange.y));
        int maxEnemyDifficulty = Mathf.RoundToInt(Mathf.Clamp(enemyRange.x + maxDifficulty, enemyRange.x, enemyRange.y));
        int maxKeyDifficulty = Mathf.RoundToInt(Mathf.Clamp(keyPerEnemyRange.x + maxDifficulty, keyPerEnemyRange.x, keyPerEnemyRange.y));

        //spawn a random amount of enemies/keys based on difficulty
        int enemyCount = Random.Range(minEnemyDifficulty, maxEnemyDifficulty);
        int gemCount = Random.Range(Mathf.RoundToInt(LevelManager.Instance.DifficultyRange.x), Mathf.RoundToInt(LevelManager.Instance.DifficultyRange.y));
        int obstacleCount = Random.Range(Mathf.RoundToInt(LevelManager.Instance.DifficultyRange.x), Mathf.RoundToInt(LevelManager.Instance.DifficultyRange.y));
        int keyCount = Mathf.Clamp(enemyCount * Random.Range(minKeyDifficulty, maxKeyDifficulty) - gemCount - obstacleCount,1,100);

        //print(minEnemyDifficulty + " " + maxEnemyDifficulty);

        for (int i = 0; i < keyCount; i++)
        {
            GameObject key = Instantiate(keyPrefab, cannon.transform);
            keys.Add(key);
            cannon.GetComponent<CannonManager>().Keys.Add(key);
        }
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, manager.transform);
            enemies.Add(enemy);
            manager.GetComponent<EnemyCycleManager>().Enemies.Add(enemy);
        }
        for (int i = 0; i < gemCount; i++)
        {
            GameObject gem = Instantiate(gemPrefab, manager.transform.parent);
            enemies.Add(gem);
        }
        for (int i = 0; i < obstacleCount; i++)
        {
            GameObject obstacle = Instantiate(LevelManager.Instance.GetRandomObstacle(), manager.transform.parent);
            obstacles.Add(obstacle);
        }

        int totalCount = enemies.Count + keys.Count + gems.Count + obstacles.Count;

        offset = 360 / (totalCount*2) * Random.Range(0, totalCount*2);

        //determine even spacing between enemies, keys, and gems
        int enemyPeriodicity = 0;
        if (keys.Count > 0 && enemies.Count > 0)
        {
            enemyPeriodicity = (keys.Count / enemies.Count) + 1;
        }
        for (int i = 0; i < totalCount; i++)
        {
            if (i % enemyPeriodicity == 0 && enemies.Count > 0)
            {
                elementsToPlace.Add(enemies[0]);
                enemies.RemoveAt(0);
            }
            else if (obstacles.Count > 0)
            {
                elementsToPlace.Add(obstacles[0]);
                obstacles.RemoveAt(0);
            }
            else if (gems.Count > 0)
            {
                elementsToPlace.Add(gems[0]);
                gems.RemoveAt(0);
            }
            else if (keys.Count > 0)
            {
                elementsToPlace.Add(keys[0]);
                keys.RemoveAt(0);
            }
        }

        //place level elements evenly along the edge of a circle
        float angleStep = 360f / totalCount;

        for (int i = 0; i < totalCount; i++)
        {
            float angle = ((i * angleStep) + offset) * Mathf.Deg2Rad;

            float elementRadius = radius;

            if (!elementsToPlace[i].GetComponent<Enemy>())
            {
                elementRadius = 7;
            }

            Vector3 position = new Vector3(
                Mathf.Cos(angle) * elementRadius,
                Mathf.Sin(angle) * elementRadius,
                0f
            );

            elementsToPlace[i].transform.localPosition = position;
        }
    }
}
