using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RadialPlacer))]
public class RadialPlacerEditor : Editor
{
    private void OnEnable()
    {
    }
    public override void OnInspectorGUI()
    {
        RadialPlacer radialPlacer = (RadialPlacer)target;
        if (GUILayout.Button("Place Elements"))
        {
            radialPlacer.PlaceElements();
        }

        base.OnInspectorGUI();
    }
}
