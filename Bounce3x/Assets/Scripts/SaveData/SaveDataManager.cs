using UnityEngine;
using System.Collections;

public class SaveDataManager{
	public static void SaveData(string saveDatakey, int val ){
		PlayerPrefs.SetInt(saveDatakey,val);
	}

	public static void SaveData(string saveDatakey, float val ){
		PlayerPrefs.SetFloat(saveDatakey,val);
	}

	public static void SaveData(string saveDatakey, string val ){
		PlayerPrefs.SetString(saveDatakey,val);
	}

	public static int LoadIntSaveData(string saveDatakey){
		return PlayerPrefs.GetInt(saveDatakey);
	}

	public static float LoadFloatSaveData(string saveDatakey){
		return PlayerPrefs.GetFloat(saveDatakey);
	}

	public static string LoadStringSaveData(string saveDatakey){
		return PlayerPrefs.GetString(saveDatakey);
	}

	public static void DeleteSaveData(string saveDatakey){
		PlayerPrefs.DeleteKey(saveDatakey);
	}

	public static void DeleteAll(){
		PlayerPrefs.DeleteAll();
	}
}
