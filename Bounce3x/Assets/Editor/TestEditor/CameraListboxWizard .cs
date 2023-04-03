using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
 
public class CameraListboxWizard : ScriptableWizard
{
    public string DBLocation = "Assets/Resources/CameraLocationDatabase.asset";
    private CameraLocation camLocation;
    static private Object dbaseAsset;
    static private CameraLocationHolder db;
 
    [MenuItem("Custom/Cameras/CameraListBox - Database Helper")]
    static void DoSet()
    {
        ScriptableWizard.DisplayWizard("Add view to CameraListBox", typeof(CameraListboxWizard), "DONE");
    }
 
    void OnWizardUpdate()
    {
        helpString = "This will control the Camera Location asset database\n, and allow you to 'save' the Camera.main's position\n while in the player mode\n";
 
        UpdateMyAssetLocation();
    }
 
    void AddExistingCameras()
    {
        int count = Camera.allCameras.Length;
        for (int i = 0; i < count; ++i)
        {
            //except for the main camera, which we'll use to transition between the others
            if (Camera.allCameras[i] != Camera.main)
            {
                camLocation = new CameraLocation();
                camLocation.name = Camera.allCameras[i].name;
                camLocation.fov = Camera.allCameras[i].fov;
                camLocation.position = Camera.allCameras[i].transform.position;
                camLocation.rotation = Camera.allCameras[i].transform.rotation;
                AddCamera(camLocation);
            }
        }
    }
 
    void OnWizardCreate()
    {
        Debug.Log("done");
    }
 
    void AddCamera(CameraLocation cam)
    {
        db.content.Add(cam);
        EditorUtility.SetDirty(dbaseAsset);
    }
 
    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
         EditorGUILayout.BeginHorizontal();
        GUILayout.Label(name, GUILayout.MaxWidth(0));
        DBLocation = EditorGUILayout.TextField(DBLocation);
        if (GUILayout.Button("Browse"))
        {
            DBLocation = EditorUtility.OpenFilePanel(name, DBLocation, "asset");
            UpdateMyAssetLocation();
        }
        EditorGUILayout.EndHorizontal();
 
        if (GUILayout.Button("Update Database Location"))
        {
            UpdateMyAssetLocation();
        }
 
        EditorGUILayout.Space();
        if (GUILayout.Button("Add current Camera.main position", GUILayout.Height(50)))
        {
            camLocation = new CameraLocation();
            AddCamera(camLocation);
        }
        EditorGUILayout.Space();
        DisplayCurrentCamList();
        EditorGUILayout.Space();
 
        if (GUILayout.Button("Add all existing 'Cameras' to the list\n this can 'import' max camera locations", GUILayout.Height(50)))
        {
            AddExistingCameras();
        }
        EditorGUILayout.EndVertical();
    }
 
    void DisplayCurrentCamList()
    {
        for (int i = 0; i < db.content.Count; i++ )
        {
            EditorGUILayout.BeginHorizontal();
            db.content[i].name = GUILayout.TextField(db.content[i].name, GUILayout.Width(200));
            if (GUILayout.Button("UP"))
            {
                if (i < 0)
                {
                    CameraLocation item = db.content[i];
                    db.content.RemoveAt(i);
                    db.content.Insert(i - 1, item);
                }
            }
            if (GUILayout.Button("DN"))
            {
                if (i < db.content.Count)
                {
                    CameraLocation item = db.content[i];
                    db.content.RemoveAt(i);
                    db.content.Insert(i + 1, item);
                }
            }
            if (GUILayout.Button("Remove"))
            {
                db.content.Remove(db.content[i]);
            }
            //db.SetDirty();
            EditorUtility.SetDirty(dbaseAsset);
            EditorGUILayout.EndHorizontal();
        }
    }
 
    public void UpdateMyAssetLocation()
    {
        dbaseAsset = AssetDatabase.LoadAssetAtPath(DBLocation, typeof(CameraLocationHolder));
        CameraLocationHolder CameraLocationDB = (CameraLocationHolder)dbaseAsset;
        db = CameraLocationDB;
    }
    public void CreateMyAsset()
    {
        List<CameraLocation> content = new List<CameraLocation>();
        CameraLocationHolder asset = new CameraLocationHolder(content);  //scriptable object 
        AssetDatabase.CreateAsset(asset, "CameraLocationDatabase");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}