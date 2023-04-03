using UnityEngine;
using UnityEditor;
using System.Collections;

public class ScoreoidConfigEditor:EditorWindow{

	private string apiKey="";
	private string gameId="";

	[MenuItem("Custom/Scoreoid/Setup...")]
	public static void MenuItemSetup(){
		EditorWindow.GetWindow(typeof(ScoreoidConfigEditor));
	}

	private void CreateConfig(){
		ScoreoidConfig config = new ScoreoidConfig();
		config.apiKey = apiKey;
		config.gameId = gameId;
		ScoreoidConfigHolder holder = new ScoreoidConfigHolder();
		holder.config = config;

		AssetDatabase.CreateAsset(holder,"Assets/Resources/Config/ScoreiodConfig.asset");
		AssetDatabase.SaveAssets();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = holder;

		EditorUtility.DisplayDialog("Success: ", "Scoreoid Config Created!","ok");
	}

	private void CreateFolder(string path, string folderName){
		string resourcesPath= "Assets/Resources";
		if (!System.IO.Directory.Exists(resourcesPath)){
			AssetDatabase.CreateFolder("Assets", "Resources");
		}
		
		if (!System.IO.Directory.Exists(path + "/" + folderName)){
			AssetDatabase.CreateFolder(path, folderName);
		}
	}

	private void OnGUI(){
		GUILayout.BeginArea(new Rect(20, 20, position.width - 40, position.height-40));
		GUILayout.Label("Scoreoid Setup", EditorStyles.boldLabel);
		GUILayout.Label("App Key", EditorStyles.boldLabel);
		apiKey = EditorGUILayout.TextField("Enter your App Key", apiKey);
		GUILayout.Space(10);

		GUILayout.Label("App GameId", EditorStyles.boldLabel);
		gameId = EditorGUILayout.TextField("Enter your Game Id", gameId);
		GUILayout.Space(10);
		
		// Setup button
		if (GUI.Button(new Rect(0, 120, 100, 30), "Create Config")){
			CreateFolder("Assets/Resources","Config");
			CreateConfig();
		}
		GUILayout.EndArea();
	}
}
