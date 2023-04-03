using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopDBLoader:IDatabaseLoader{
	
	public string DBLocation = "Assets/Resources/ShopDatabase.asset";		
	public Object dbaseAsset {get;set;}
	public ShopContentHolder db{get;set;}

	private string backupAssetDir = "Assets/Resources/Backup/ShopDatabase.asset";
	private string backupDir = "Assets/Resources/Backup";
	
	public void Load(){
		dbaseAsset = AssetDatabase.LoadAssetAtPath(DBLocation, typeof(ShopContentHolder));
        ShopContentHolder shopDB = dbaseAsset as ShopContentHolder;
        db = shopDB;
	}
	
	public void Save(){
		
	}
	
	public Item GetItemById(string id){
		List<Item> itemList = new List<Item>();
		itemList = db.content;
		Item item = null;
		
		for(int index = 0; index<itemList.Count;index++){
			if(itemList[index].id == id ){
				item = itemList[index];
				break;
			}
		}
		
		return item;
	}
	
	public string GetNextId(){
		int nextId;
		if(db.content.Count > 0){
			nextId = int.Parse(db.content[db.content.Count-1].id);		
			nextId++;	
		}else{
			nextId = 0;
		}		
		
		return nextId.ToString();
	}
	
	public void Delete(){
		FileUtil.DeleteFileOrDirectory(DBLocation);
		AssetDatabase.Refresh();
	}
	
	private void DeleteBackup(){
		AssetDatabase.DeleteAsset(backupAssetDir);
		//FileUtil.DeleteFileOrDirectory(backupDir);
		AssetDatabase.Refresh();
		Debug.Log("Delete backup!");
	}
	
	public void Backup(){
		DeleteBackup();		
		//string guid = AssetDatabase.CreateFolder("Assets/Resources","Backup");		
		//string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);		
		AssetDatabase.CopyAsset(DBLocation,backupAssetDir);
		AssetDatabase.Refresh();		
		Debug.Log("create backup!!");
	}
	
	public void Restore(){
		AssetDatabase.DeleteAsset(DBLocation);
		AssetDatabase.CopyAsset(backupAssetDir,DBLocation);
		AssetDatabase.Refresh();
		Debug.Log("Restore backup!!");
	}
}
