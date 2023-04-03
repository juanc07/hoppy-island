using UnityEngine;
using System.Collections;

public class ShopYesBtn : MonoBehaviour {

	// Use this for initialization
	private GameDataManagerController gdc;
	private ShopManagerController shopMc;
	private ShopPopUpController shopPopUpController;
	private bool isBuyEnable =false;
	
	void Start () {
		gdc = GameDataManagerController.GetInstance();
		shopMc = ShopManagerController.GetInstance();
		shopPopUpController = GameObject.Find("ShopPopUpAnchor").GetComponent<ShopPopUpController>();
		Messenger.AddListener( GameEvent.EnableBuying,OnEnableBuying  );
		Messenger.AddListener( GameEvent.DisableBuying,OnDisableBuying );
	}
	
	private void OnEnableBuying(){
		isBuyEnable = true;
	}
	
	private void OnDisableBuying(){
		isBuyEnable = false;
	}
	
	private void OnClick(){
		if(isBuyEnable){
			Item item = shopMc.CurrentItem;
			if(item != null){			
				int itemPrice = int.Parse(item.price);
				if( gdc.TotalGold >= itemPrice){
					gdc.TotalGold -= itemPrice;
					gdc.SavePlayerData();
					shopMc.BuyItem(item);				
					Messenger.Broadcast<Item>( GameEvent.BuyShopItem, item );
					shopPopUpController.ShowHideShopPopUpPanel(false);
					Debug.Log( " buy item successfully name " + item.name );	
				}else{
					Debug.Log( " buy item failed insuficient gold name " + item.name );	
				}					
			}
		}
	}
}
