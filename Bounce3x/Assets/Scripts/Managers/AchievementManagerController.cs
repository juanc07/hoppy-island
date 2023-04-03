using UnityEngine;
using System.Collections;

public class AchievementManagerController : MonoBehaviour {
	
	//private HeyzapWrapper heyzapWrapper;
	private GameDataManagerController gdc;
	// Use this for initialization
	void Start () {
		gdc = GameDataManagerController.GetInstance();
		//heyzapWrapper = GameObject.Find("HeyzapWrapper").GetComponent<HeyzapWrapper>();
		
		Messenger.AddListener<Item>( GameEvent.BuyShopItem, OnBuyShopItem );
		Messenger.AddListener( GameEvent.CheckAchievement, OnCheckAchievement );
	}
	
	private void OnCheckAchievement(){
		CheckAchievements();
	}
	
	private void OnBuyShopItem(Item item){
		Debug.Log("AchievementManagerController OnBuyShopItem  check item " + item.name );
		if( item.avatarType == Item.AvatarList.Nyancat){
			Debug.Log("unlock achievement");
			//heyzapWrapper.UnlockAchievement(heyzapWrapper.achievements[3]);
		}
	}
	
	private void CheckAchievements(){
		int score = gdc.GetScore();
		
		/*if(heyzapWrapper.unlockAchievements.Count == 0){
			heyzapWrapper.UnlockAchievement(heyzapWrapper.achievements[0]);				
		}else if(heyzapWrapper.unlockAchievements.Count == 1 && score >= 500){
			heyzapWrapper.UnlockAchievement(heyzapWrapper.achievements[1]);
		}*/
		
		if(score >= 100000){
			//heyzapWrapper.UnlockAchievement(heyzapWrapper.achievements[2]);
		}
		
		if(gdc.currentLevel >= 12){
			//heyzapWrapper.UnlockAchievement(heyzapWrapper.achievements[4]);
		}
	}
}
