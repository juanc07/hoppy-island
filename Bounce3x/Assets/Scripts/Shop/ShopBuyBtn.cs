using UnityEngine;
using System.Collections;

public class ShopBuyBtn : MonoBehaviour {
	
	//private GameDataManagerController gdc;
	private ShopManagerController shopMc;
	private ShopPopUpController shopPopUpController;
	private bool isVerbose = false;
	
	// Use this for initialization
	void Start () {
		//gdc = GameDataManagerController.GetInstance();
		shopMc = ShopManagerController.GetInstance();
		shopPopUpController = GameObject.FindObjectOfType<ShopPopUpController>();
		shopPopUpController.ShowHideShopPopUpPanel(false);
		Invoke( "InitShopItem" , 0.01f);
		//InitShopItem();
	}
	
	private void InitShopItem(){		
		Item item = shopMc.GetItemByAvatarType( shopMc.CurrentAvatar.avatarType );		
		shopMc.CurrentItem = item;		
		Messenger.Broadcast<Item>( GameEvent.SelectShopItem,item);
		ShowLog( " buybutton  InitShopItem!!" );
	}
	
	private void OnClick(){
		Item item = shopMc.CurrentItem;
		if(item != null){
			ShowLog( " buy item name " + item.name );	
			//if(!gdc.SearchBoughtItemById(item.id) ){
			if(!shopMc.SearchBoughtItemById(item.id)){
				shopPopUpController.ShowHideShopPopUpPanel(true);				
			}else{
				ShowLog( " buy item failed you already bought this name " + item.name );	
			}
						
		}
	}

	private void ShowLog(string val){
		if(isVerbose){
			Debug.Log(val);
		}
	}
}
