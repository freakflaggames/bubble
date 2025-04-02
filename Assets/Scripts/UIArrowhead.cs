using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIArrowhead : MonoBehaviour
{
    LineRenderer line;
    public Transform position1, position2;
    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        line.SetPosition(0, position1.position);
        line.SetPosition(1, position2.position);
    }
}
