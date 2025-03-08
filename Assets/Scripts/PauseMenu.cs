using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    bool active;
    public void TogglePauseMenu()
    {
        active = !active;
        menu.SetActive(active);
        Time.timeScale = active ? 0 : 1;
    }
}
