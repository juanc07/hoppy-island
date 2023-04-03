using UnityEngine;
using System.Collections;

public class GameEvent : MonoBehaviour {	
	public const string Level_Start = "Level_Start";
	public const string Level_Failed = "Level_Failed";
	//public const string Level_Restart = "Level_Restart";
	public const string SelectShopItem = "SelectShopItem";
	public const string BuyShopItem = "BuyShopItem";
	public const string GameDataLoaded = "GameDataLoaded";	
	public const string ShopItemLoaded = "ShopItemLoaded";	
	
	public const string EnableBuying = "EnableBuying";
	public const string DisableBuying = "DisableBuying";
	
	public const string Levelup = "Levelup";
	
	public const string FirstTap = "FirstTap";	
	public const string EndTutorial = "EndTutorial";
	public const string FirstAnimal = "FirstAnimal";
	
	public const string CheckAchievement = "CheckAchievement";
	
	public const string HitIndex0 = "HitIndex0";
	public const string HitIndex1 = "HitIndex1";
	public const string HitIndex2 = "HitIndex2";	

	public const string GOT_SCORE = "GOT_SCORE";
	public const string SaveAnimal = "SaveAnimal";
	public const string ANIMAL_FELL = "ANIMAL_FELL";
	public const string GetSetGoComplete = "GetSetGoComplete";
	public const string GOT_EXTRA_LIFE = "GOT_EXTRA_LIFE";
}


