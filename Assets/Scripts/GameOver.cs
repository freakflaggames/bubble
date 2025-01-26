using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI worlds, keys;
    private void Start()
    {
        worlds.text = "You travelled to " + PlayerPrefs.GetInt("worlds") + " pocket worlds!";
        keys.text = "You collected " + PlayerPrefs.GetInt("keys") + " star cannon keys!";
    }
}
