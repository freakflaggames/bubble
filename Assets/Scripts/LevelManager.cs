using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> Levels = new List<GameObject>();
    public static LevelManager Instance;
    public Vector2 DifficultyRange;
    public Vector2 DifficultyModifier;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetRandomLevel()
    {
        //get a random level
        int randomLevelIndex = Random.Range(0, Levels.Count);
        GameObject Level = Levels[randomLevelIndex];
        DifficultyRange += DifficultyModifier;
        return Level;
    }
}
