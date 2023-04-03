using UnityEngine;
using System.Collections;

public class AvatarPriceController : MonoBehaviour {

	// Use this for initialization	
	private ShopManagerController shopMC;
	public GameObject buyButton;
	
	void Start (){
		shopMC =  GameObject.Find("ShopManager").GetComponent<ShopManagerController>();
		Messenger.AddListener<Item>( GameEvent.SelectShopItem, onSelectShopItem);
		Messenger.AddListener( GameEvent.ShopItemLoaded, onLoadedShopItem);
		Messenger.AddListener<Item>( GameEvent.BuyShopItem, OnBuyShopItem);
		ShowHideBuyButton(false);
	}
	
	private void OnBuyShopItem(Item item){
		if(this==null)return;
		//UpdateItem(shopMC.CurrentItem);
		UpdateItem(item);
	}
	
	private void onLoadedShopItem(){
		if(this==null)return;
		UpdateItem(shopMC.CurrentItem);
	}
	
	private void onSelectShopItem(Item item){
		if(this==null)return;
		UpdateItem(item);
	}
	
	private void UpdateItem(Item item){
		UILabel label = this.gameObject.GetComponent<UILabel>();		
		if(!shopMC.SearchBoughtItemById(item.id)){
			label.text =  item.price;
			ShowHideBuyButton(true);
		}else{
			label.text =  "own";
			ShowHideBuyButton(false);
		}
	}

	private void ShowHideBuyButton(bool val){
		buyButton.SetActive(val);
	}
}
