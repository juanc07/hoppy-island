//using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
 
public class CreateCameraLocationAsset
{
    [MenuItem("Custom/Cameras/Create camera location holder")]
    public static void CreateMyAsset()
    {
        List<CameraLocation> content = new List<CameraLocation>();
        CameraLocationHolder asset = new CameraLocationHolder(content);  //scriptable object 
        AssetDatabase.CreateAsset(asset, "Assets/CameraLocationDatabase.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}