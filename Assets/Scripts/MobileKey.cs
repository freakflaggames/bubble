using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobileKey : MonoBehaviour
{
    public string Key;
    GameOver gameover;
    private void Start()
    {
        var text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Key = text.text;
        gameover = GameOver.Instance;
    }
    private void Update()
    {
        if (Key != "space" && Key != "del" && Key != "caps")
        {
            if (gameover.caps)
            {
                Key = Key.ToUpper();
            }
            else
            {
                Key = Key.ToLower();
            }
        }
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Key;
    }
    public void PressKey()
    {
        if (Key == "space")
        {
            gameover.nameInput.text += " ";
        }
        else if (Key == "del")
        {
            if (gameover.nameInput.text.Length > 0)
            { 
                gameover.nameInput.text = gameover.nameInput.text.Remove(gameover.nameInput.text.Length - 1);
            }
        }
        else if (Key == "caps")
        {
            gameover.caps = !gameover.caps;
        }
        else
        {
            gameover.nameInput.text += Key;
        }
    }
}
