using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelDifficulties
{
    [SerializeField]
    public List<GameObject> Difficulties = new List<GameObject>();
    public int CurrentDifficulty;
}
public class LevelManager : MonoBehaviour
{
    public List<LevelDifficulties> Levels = new List<LevelDifficulties>();
    public static LevelManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetRandomLevel()
    {
        //get a random level
        int randomLevelIndex = Random.Range(0, Levels.Count);
        LevelDifficulties Level = Levels[randomLevelIndex];
        //get a random difficulty, but only include difficulties player has completed + the next one
        int randomDifficultyIndex = Random.Range(0, Level.CurrentDifficulty);
        GameObject Difficulty = Level.Difficulties[randomDifficultyIndex];
        //raise difficulty level
        Level.CurrentDifficulty++;
        Level.CurrentDifficulty = Mathf.Clamp(Level.CurrentDifficulty, 0, Level.Difficulties.Count);
        return Difficulty;
    }
}
