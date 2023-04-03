using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditItemWindow : ScriptableWizard {	
	
	private Item currentItem;	
	static private ShopContentHolder db;	
	
	private string itemId;
	private string itemName;
	private string itemPrice;
	private Item.ItemType itemType;
	private Item.AvatarList avatarType;

	private Transform itemTransform;
	private Transform itemShopTransform;
	
	private ShopDBLoader shopDB;	
	
	static private string currentItemId;
	
	
	public static void CreateWizard (string currItemId){
		currentItemId = currItemId;
		ScriptableWizard.DisplayWizard<EditItemWindow>("Edit Item", "Done", "Modify");
    }
	
	void OnWizardCreate (){
		
	}
	
	
	
	void OnGUI(){		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "id",GUILayout.Width(50) );
        //itemId = EditorGUILayout.TextField(itemId, GUILayout.Width(200));
		EditorGUILayout.LabelField(itemId, GUILayout.Width(200));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "name",GUILayout.Width(50) );
		itemName = EditorGUILayout.TextField(itemName);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "price",GUILayout.Width(50) );
		itemPrice = EditorGUILayout.TextField(itemPrice);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();		
		avatarType = (Item.AvatarList)EditorGUILayout.EnumPopup("avatarType",avatarType);
		EditorGUILayout.EndHorizontal();		
		
		EditorGUILayout.BeginHorizontal();		
		itemType = (Item.ItemType)EditorGUILayout.EnumPopup("ItemType",itemType);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "Prefab",GUILayout.Width(100) );
		itemTransform = (Transform)EditorGUILayout.ObjectField(itemTransform,typeof(Transform), true);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "ShopPrefab",GUILayout.Width(100) );
		itemShopTransform = (Transform)EditorGUILayout.ObjectField(itemShopTransform,typeof(Transform), true);
		EditorGUILayout.EndHorizontal();
		
		 //BeginWindows();		 
		 //windowRect = GUI.Window (0, windowRect, DoMyWindow, "My Window");
		
		if (GUILayout.Button("update", GUILayout.Height(25) )){
			currentItem.name = itemName;
			currentItem.price = itemPrice;
			currentItem.avatarType = avatarType;
			currentItem.type = itemType;
			currentItem.itemTransform = itemTransform;
			currentItem.itemShopTransform = itemShopTransform;
			ModifyItem(currentItem);
			Close();
        }		
		
		//EndWindows ();
	}
	
	void OnWizardUpdate (){
		UpdateMyAssetLocation();
		currentItem= GetItemById(currentItemId);
		
		itemId = currentItem.id;
		itemName = currentItem.name;
		itemPrice = currentItem.price;
		avatarType = currentItem.avatarType;
		itemType = currentItem.type;
		itemTransform = currentItem.itemTransform;
		itemShopTransform = currentItem.itemShopTransform;
		Debug.Log(" currentItemId " + currentItemId);
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
	
	private void ModifyItem( Item item ){
		List<Item> itemList = new List<Item>();
		itemList = db.content;		
		
		for(int index = 0; index<itemList.Count;index++){
			if(itemList[index].id == item.id ){
				itemList[index] = item;
				break;
			}
		}
		EditorUtility.SetDirty(shopDB.dbaseAsset);
	}	
}
