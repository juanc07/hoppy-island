using UnityEngine;
using System.Collections;

public class AvatarNameController : MonoBehaviour {

	// Use this for initialization
	private ShopManagerController shopMC;
	
	void Start (){
		shopMC =  GameObject.Find("ShopManager").GetComponent<ShopManagerController>();
		Messenger.AddListener<Item>( GameEvent.SelectShopItem, onSelectShopItem);
		Messenger.AddListener( GameEvent.ShopItemLoaded, onLoadedShopItem);
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
		label.text = item.name;
	}
}
