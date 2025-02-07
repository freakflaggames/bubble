using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialPlacer : MonoBehaviour
{
    //evenly spaces out elements in a radius
    public float radius;
    public float offset;

    private void Start()
    {
        PlaceElements();
    }

    public void PlaceElements()
    {
        List<GameObject> objectsToPlace = new List<GameObject>();

        foreach(Transform child in transform)
        {
            objectsToPlace.Add(child.gameObject);
        }

        int count = objectsToPlace.Count;

        float angleStep = 360f / count; // Angle step between each object

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            Vector3 position = new Vector3(
                Mathf.Cos(angle+offset) * radius,
                Mathf.Sin(angle+offset) * radius,
                0f
            );

            objectsToPlace[i].transform.position = position;
        }
    }
}

