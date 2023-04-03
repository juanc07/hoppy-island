using UnityEngine;
using System.Collections;

public class GoldController : MonoBehaviour {
	
	private GameDataManagerController gdc;	
	
	// Use this for initialization
	void Start () {
		gdc = GameDataManagerController.GetInstance();
		UpdateGold();
		Messenger.AddListener<Item>( GameEvent.BuyShopItem,OnBuyShopItem );
		Messenger.AddListener( GameEvent.GameDataLoaded,OnGameDataLoaded );
	}
	
	private void OnBuyShopItem(Item item){
		UpdateGold();
	}
	
	private void OnGameDataLoaded(){
		UpdateGold();
	}
	
	private void UpdateGold(){
		if(this == null)return;
		UILabel label = this.gameObject.GetComponent<UILabel>();
		//Debug.Log( " check total gold " + gdc.TotalGold );
		label.text = gdc.TotalGold.ToString();		
	}
}
