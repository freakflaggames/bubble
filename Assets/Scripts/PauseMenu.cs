using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public GameObject menu;
    public bool paused;
    private void Awake()
    {
        Instance = this;
    }
    public void TogglePauseMenu()
    {
        paused = !paused;
        menu.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;
        if (paused)
        {
            AudioManager.Instance.StopMusic();
        }
        else
        {
            AudioManager.Instance.PlayMusic();
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Title");
    }
}
