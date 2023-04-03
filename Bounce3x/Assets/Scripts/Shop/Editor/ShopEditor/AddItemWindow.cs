using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddItemWindow : ScriptableWizard {	
	
	private Item currentItem;	
	static private ShopContentHolder db;	
	
	private string itemId;
	private string itemName;
	private string itemPrice;
	private Item.ItemType type;
	private Item.AvatarList avatarType;
	
	private ShopDBLoader shopDB;
	
	
	public static void CreateWizard (){
		ScriptableWizard.DisplayWizard<AddItemWindow>("Add Item", "Done", "Add");
    }
	
	void OnWizardCreate (){
		
	}
	
	
	
	void OnGUI(){		
		EditorGUILayout.BeginHorizontal();		
        //itemId = EditorGUILayout.TextField(itemId, GUILayout.Width(200));
		EditorGUILayout.LabelField( "id",GUILayout.Width(50));
		EditorGUILayout.LabelField(itemId, GUILayout.Width(200));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "name",GUILayout.Width(50) );
		itemName = EditorGUILayout.TextField(itemName, GUILayout.Width(200));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "price",GUILayout.Width(50) );
		itemPrice = EditorGUILayout.TextField(itemPrice, GUILayout.Width(200));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		avatarType = (Item.AvatarList)EditorGUILayout.EnumPopup("avatarType",avatarType);
		EditorGUILayout.EndHorizontal();		
		
		EditorGUILayout.BeginHorizontal();
		type = (Item.ItemType)EditorGUILayout.EnumPopup("ItemType",type);
		EditorGUILayout.EndHorizontal();
		
		 //BeginWindows();		 
		 //windowRect = GUI.Window (0, windowRect, DoMyWindow, "My Window");
		
		//if (GUILayout.Button("ADD", GUILayout.Width(50) )){
		if (GUILayout.Button("ADD",GUILayout.Height(25))){
			currentItem = new Item();
			currentItem.name =itemName;
			currentItem.id = itemId;
			currentItem.price = itemPrice;
			//currentItem.type = Item.ItemType.Avatar;
			currentItem.avatarType = avatarType;
			AddItem(currentItem);			
			Close();
        }		
		
		//EndWindows ();
	}
	
	void OnWizardUpdate (){
		UpdateMyAssetLocation();		
		itemId = shopDB.GetNextId();
		//itemId = db.content.Count.ToString();	
		itemName = "itemName"+shopDB.GetNextId();
		itemPrice = "0";		
	}
	
	void OnWizardOtherButton (){
	}
	
	
	public void UpdateMyAssetLocation(){
		if(shopDB == null){
			shopDB = new ShopDBLoader();	
		}		
		shopDB.Load();
		db = shopDB.db;
    }
	
	private Item GetItemById(string currId){
		List<Item> itemList = new List<Item>();
		itemList = db.content;
		Item item = null;
		
		for(int index = 0; index<itemList.Count;index++){
			if(itemList[index].id == currId ){
				item = itemList[index];
				break;
			}
		}
		
		return item;
	}
	
	void AddItem(Item newItem){
        db.content.Add(newItem);
        //EditorUtility.SetDirty(dbaseAsset);
		EditorUtility.SetDirty(shopDB.dbaseAsset);
    }
}
