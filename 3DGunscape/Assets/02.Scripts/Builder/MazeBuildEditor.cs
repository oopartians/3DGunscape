using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MazeBuildScript))]
public class MazeBuildEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MazeBuildScript myScript = (MazeBuildScript)target;
        if (GUILayout.Button("Build"))
        {
            myScript.BuildMaze();
        }
        if (GUILayout.Button("Clear Maze"))
        {
            myScript.clearMaze();
        }
    }
}
