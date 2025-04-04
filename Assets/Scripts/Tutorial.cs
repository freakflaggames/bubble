using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;
    int index = -1;
    public List<GameObject> tutorialSections;
    private void Awake()
    {
        Instance = this;
    }
    public void NextSection()
    {
        index++;
        Time.timeScale = 0.1f;
        for (int i = 0; i < tutorialSections.Count; i++)
        {
            tutorialSections[i].gameObject.SetActive(false);
            if (i == index)
            {
                print("hi");
                tutorialSections[i].gameObject.SetActive(true);
            }
        }
    }
}
