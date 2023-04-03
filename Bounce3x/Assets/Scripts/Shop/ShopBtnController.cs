using UnityEngine;
using System.Collections;

public class ShopBtnController : MonoBehaviour {

	public UISprite buttonIcon;
	private bool isVerbose = false;

	[SerializeField]
	private Item itemData;
	//public Item.AvatarList avatarType = Item.AvatarList.Zombie;
	private ShopManagerController shopMc;
	
	// Use this for initialization
	void Start (){
		shopMc = ShopManagerController.GetInstance();
	}

	private void SetImageIcon(){
		buttonIcon.spriteName = itemData.avatarType.ToString();
	}
	
	void OnClick(){
		//Item item = shopMc.GetItemByAvatarType( avatarType );
		Item item = shopMc.GetItemByAvatarType( itemData.avatarType );
		ShowLog("Avatar id " + item.id);
		bool isOwn = shopMc.SearchBoughtItemById( item.id );
		
		if(isOwn){
			shopMc.CurrentAvatar = item;
		}		
		shopMc.CurrentItem = item;
		Messenger.Broadcast<Item>( GameEvent.SelectShopItem,item);
	}

	public Item ItemData{
		set{
			itemData=value;
			SetImageIcon();
		}
		get{return itemData;}
	}

	private void ShowLog(string val){
		if(isVerbose){
			Debug.Log(val);
		}
	}
}
