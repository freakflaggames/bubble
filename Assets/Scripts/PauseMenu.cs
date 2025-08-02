using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public GameObject menu;
    public GameObject centerAimCheckmark;
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
        Time.timeScale = 1; 
        SceneManager.LoadScene("Title");
    }

    public void CenterAim()
    {
        PlayerController.Instance.centerAim = !PlayerController.Instance.centerAim;
        centerAimCheckmark.SetActive(PlayerController.Instance.centerAim);
    }
}
