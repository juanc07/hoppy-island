//using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShopManagerController : MonoBehaviour {
	
	private static ShopManagerController instance;
	private static GameObject container;	

	private Item currentItem;	
	public List<Item> ShopItems = new List<Item>();		
	private List<string> boughtItemIds = new List<string>();
	private Item currentAvatar;
	private bool isShopItemLoaded=false;
	private bool hasStarted=false;
	
	private ShopDB shopDatabase;
	private List<Item> shopItemDB;

	private Action ShopItemLoadComplete;
	public event Action OnShopItemLoadComplete{
		add{ShopItemLoadComplete+=value;}
		remove{ShopItemLoadComplete-=value;}
	}

	public static ShopManagerController GetInstance(){
		if(instance ==null){
			container = new GameObject();
			container.name = "ShopManager";
			instance =  container.AddComponent( typeof(ShopManagerController)) as ShopManagerController;
			DontDestroyOnLoad(instance);
		}
		
		return instance;
	}
	
	// Use this for initialization
	void Start (){
		if(!hasStarted){
			hasStarted =true;
			shopDatabase = new ShopDB();
			shopDatabase.OnLoadComplete+=OnLoadComplete;
			shopDatabase.Load();					
		}			
	}

	private void OnDestroy(){
		if(shopDatabase!=null){
			shopDatabase.OnLoadComplete-=OnLoadComplete;
		}
	}

	private void OnLoadComplete(){
		//Debug.Log("OnShopLoadComplete!");
		LoadShopData();
	}
	
	private void LoadShopData(){
		if(!isShopItemLoaded){
			isShopItemLoaded =true;		
			
			if(shopItemDB ==null){
				shopItemDB = new List<Item>();
			}

			shopItemDB = shopDatabase.db.content;			
			//Debug.Log( "checkloaded shopDB 0 ===> name " + shopItemDB[0].name + " itemid " + shopItemDB[0].id + " price " + shopItemDB[0].price + " AvatarType " + shopItemDB[0].avatarType );
			//Debug.Log( "checkloaded shopDB 1 ===> name " + shopItemDB[1].name + " itemid " + shopItemDB[1].id + " price " + shopItemDB[1].price + " AvatarType " + shopItemDB[1].avatarType );
			
			ShopItems = shopDatabase.db.content;
			//Debug.Log( "check last item " +  ShopItems[ 1 ].avatarType );
			
			string sceneName = Application.loadedLevelName;		
			if( sceneName == GameScreen.TITLE || sceneName == GameScreen.Shop ){
				Messenger.Broadcast(GameEvent.ShopItemLoaded);	
			}

			if(null!=ShopItemLoadComplete){
				ShopItemLoadComplete();
			}

			string currentAvatarId = SaveDataManager.LoadStringSaveData(PlayerDataKey.CURRENT_AVATAR.ToString());
			Item item;

			if(!currentAvatarId.Equals("",StringComparison.Ordinal)){
				Item saveAvatar = SearchItemById(currentAvatarId);
				CurrentAvatar = saveAvatar;
				CurrentItem = saveAvatar;
			}else{
				item = GetItemByAvatarType(Item.AvatarList.Whale);
				SaveBoughtItem(item);
				CurrentAvatar = ShopItems[6];
				CurrentItem = ShopItems[6];
			}
		}
	}
	
	public Item GetItemByAvatarType( Item.AvatarList avatarType ){
		int len = ShopItems.Count;
		Item item = null;
		for(int index = 0;index<len;index++){
			if(ShopItems[index].avatarType == avatarType){
				item = ShopItems[index];
				break;
			}			
		}		
		return item;
	}

	public Item GetRandomItem(){
		int len = ShopItems.Count;
		int rnd = UnityEngine.Random.Range(0,len);
		Item item = ShopItems[rnd];

		while(item.avatarType == CurrentAvatar.avatarType){
			rnd = UnityEngine.Random.Range(0,len);
			item = ShopItems[rnd];
		}

		return item;
	}

	
	public Item GetItemById(string id ){
		int len = ShopItems.Count;
		Item item = null;
		for(int index = 0;index<len;index++){
			if(ShopItems[index].id == id){
				item = ShopItems[index];
				break;
			}			
		}		
		return item;
	}
	
	
	public Item CurrentItem{
		get{ return currentItem;}
		set{ 
			currentItem = value;
		}
	}
	
	
	public void BuyItem(Item item){
		SaveBoughtItem(item);
		CurrentAvatar = item;
		CurrentItem = item;
	}
	
	public bool SearchBoughtItemById(string id){
		int len = boughtItemIds.Count;	
		bool found =false;
		for(int index = 0;index<len;index++){
			if( boughtItemIds[index].Equals(id,StringComparison.Ordinal)){
				found = true;
				break;
			}
		}
		
		return found;		
	}
	
	public void SaveBoughtItem(Item item){		
		if( item == null ) return;
		//Debug.Log( "check item " + item.name );
		bool found =SearchBoughtItemById(item.id);
		if(!found){
			boughtItemIds.Add(item.id);
			//PlayerPrefs.SetString( PlayerData.ITEM+item.id,item.id );
			//Debug.Log("check bought item key: " + PlayerDataKey.ITEM.ToString() + item.id + " item id: " +  item.id);
			SaveDataManager.SaveData(PlayerDataKey.ITEM.ToString() + item.id,item.id);
			//Debug.Log("loaded item id " + item.id);
			//CurrentAvatar = item;
		}
	}
	
	public void LoadBoughtItems(){
		//Debug.Log("loading bought items...");

		int len = 13;
		//int len =  ShopItems.Count;
		for(int index = 0;index<len;index++){
			//string itemId = PlayerPrefs.GetString(PlayerData.ITEM+index);
			string itemId = SaveDataManager.LoadStringSaveData(PlayerDataKey.ITEM.ToString()+index);
			//Debug.Log("loading bought check itemKey: " +  PlayerDataKey.ITEM.ToString()+index +  " item id: " + itemId);

			if(!itemId.Equals("",StringComparison.Ordinal)){
				Item item = GetItemById(itemId);
				if(item!=null){
					SaveBoughtItem(item);
					//Debug.Log("loading bought item id " + item.id);
				}
			}
		}
	}
	
	public void DeleteBoughtItems(){
		int len = 13;
		//int len =  ShopItems.Count;
		for(int index = 0;index<len;index++){			
			//PlayerPrefs.DeleteKey(PlayerData.ITEM+index);
			SaveDataManager.DeleteSaveData(PlayerDataKey.ITEM.ToString()+index);
		}
		boughtItemIds.Clear();
		SaveDataManager.DeleteSaveData(PlayerDataKey.CURRENT_AVATAR.ToString());
	}
	
	public Item CurrentAvatar{
		set{ 
			currentAvatar = value;
			SaveDataManager.SaveData(PlayerDataKey.CURRENT_AVATAR.ToString(),currentAvatar.id);
		}
		get{ return currentAvatar; }
	}

	public List<Item> ShopItemDB{
		get{return shopItemDB;}
	}

	public Item SearchItemById( string itemId ){
		int itemCount = shopItemDB.Count;
		Item foundItem = null;

		for(int index=0;index<itemCount;index++){
			if(shopItemDB[index]!=null){
				if(shopItemDB[index].id.Equals(itemId,StringComparison.Ordinal)){
					foundItem = shopItemDB[index];
					break;
				}
			}
		}
		return foundItem;
	}

	public Item SearchItemByAvatarType( Item.AvatarList avatarType ){
		int itemCount = shopItemDB.Count;
		Item foundItem = null;
		
		for(int index=0;index<itemCount;index++){
			if(shopItemDB[index]!=null){
				if(shopItemDB[index].avatarType == avatarType){
					foundItem = shopItemDB[index];
					break;
				}
			}
		}
		return foundItem;
	}

	public bool IsShopItemLoaded{
		get{return isShopItemLoaded;}
	}
}
