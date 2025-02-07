using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialPlacer : MonoBehaviour
{
    //evenly spaces out elements in a radius
    public List<GameObject> elementsToPlace;
    public float radius;
    public float offset;

    private void Start()
    {
    }

    public void PlaceElements()
    { 
        int count = elementsToPlace.Count;

        float angleStep = 360f / count; // Angle step between each object

        for (int i = 0; i < count; i++)
        {
            float angle = ((i * angleStep)+offset) * Mathf.Deg2Rad;

            Vector3 position = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f
            );

            elementsToPlace[i].transform.position = position;
        }
    }
}

