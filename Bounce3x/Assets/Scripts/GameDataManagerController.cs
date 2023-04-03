using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameDataManagerController : MonoBehaviour{
	
	private const float zPosition = -160f;	

	[HideInInspector]
	public Vector3 point1;

	[HideInInspector]
	public Vector3 instantPoint1;

	[HideInInspector]
	public Vector3 instantSlowPoint1;

	[HideInInspector]
	public Vector3 point1b;

	[HideInInspector]
	public Vector3 point2;

	[HideInInspector]
	public Vector3 instantPoint2;

	[HideInInspector]
	public Vector3 instantSlowPoint2;

	[HideInInspector]
	public Vector3 point3;

	[HideInInspector]
	public Vector3 instantPoint3;

	[HideInInspector]
	public Vector3 instantSlowPoint3;

	[HideInInspector]
	public Vector3 point4;

	[HideInInspector]
	public Vector3 point5;

	[HideInInspector]
	public Vector3 point1Slow;

	[HideInInspector]
	public Vector3 point2Slow;

	[HideInInspector]
	public Vector3 point3Slow;

	[HideInInspector]
	public Vector3 point4Slow;

	[HideInInspector]
	public Vector3 pointClearAllPowerUp= new Vector3(-3000f, 4500f, zPosition);

	[HideInInspector]
	public Vector3 pointSlowClearAllPowerUp = new Vector3(-3000f, 4500f, zPosition);

	[HideInInspector]
	public Vector3 pointClearAll= new Vector3(-500f, 480f, zPosition);

	[HideInInspector]
	public Vector3 pointSlowClearAll = new Vector3(-600f, 550f, zPosition);

	[HideInInspector]
	public Vector3 point1OverGrowth;

	[HideInInspector]
	public Vector3 point2OverGrowth;

	[HideInInspector]
	public Vector3 PaddlePoint1 = new Vector3(75f, -116.2f, -181.7f);

	[HideInInspector]
	public Vector3 PaddlePoint2 = new Vector3 (-40f, -116.2f, -181.7f);

	[HideInInspector]
	public Vector3 PaddlePoint3 = new Vector3(-160f, -116.2f, -181.7f);

	//reverse
	[HideInInspector]
	public Vector3 reverseOverGrowthPoint1 = new Vector3 (-160f,250f, zPosition);

	[HideInInspector]
	public Vector3 reverseOverGrowthPoint2 = new Vector3 (300f,225f, zPosition);

	[HideInInspector]
	public Vector3 pointLeftClearAll = new Vector3(1000f, 1500f, zPosition);

	[HideInInspector]
	public Vector3 pointLeftClearAllPowerUp = new Vector3(3000f, 4500f, zPosition);

	[HideInInspector]
	public Vector3 pointSlowLeftClearAll = new Vector3 (50f, -116.2f, zPosition);

	[HideInInspector]
	public Vector3 reverseSlowPoint1 = new Vector3 (75f, -116.2f, zPosition);

	[HideInInspector]
	public Vector3 reverseSlowPoint2 = new Vector3 (-25f, -116.2f, zPosition);

	[HideInInspector]
	public Vector3 instantReverseSlowPoint1 = new Vector3 (90f, -116.2f, zPosition);

	[HideInInspector]
	public Vector3 instantReverseSlowPoint2 = new Vector3 (-20f, -116.2f, zPosition);

	[HideInInspector]
	public Vector3 instantReverseSlowPoint3 = new Vector3 (-150f, -116.2f, zPosition);
	
	[HideInInspector]
	public Vector3 instantReversePoint1 = new Vector3 (210f, 250f, zPosition);
	
	[HideInInspector]
	public Vector3 instantReversePoint2 = new Vector3 (60f, 250f, zPosition);
	
	[HideInInspector]
	public Vector3 reversePoint3= new Vector3(-80f, 200f, zPosition);
	
	[HideInInspector]
	public Vector3 reversePoint2 = new Vector3 (0f, 350f, zPosition);
	
	[HideInInspector]
	public Vector3 reversePoint1 = new Vector3(120f, 250f, zPosition);

	
	private static GameDataManagerController instance;
	private static GameObject container;
	
	public int cloneLocationIndex = -1;
	public int locationIndex = 0;

	public PowerUpChecker.Powerups currentPowerup = PowerUpChecker.Powerups.none;
	public PowerUpChecker.Powerups prevPowerup = PowerUpChecker.Powerups.none;
	public bool hasPowerUp =false;
	
	public 	int currentLevel = 1;
	public int currentCombo = 0;
	private int currentScore = 0;

	public float initLob = 3f;
	public float minLob = 3f;
	public float maxLob = 3.5f;
	public float midLob = 4.5f;
	
	private int missCount = 0;
	private int maxLife =6;
	private int life =3;
	
	private int scoreInterval = 500;	
	private bool isGameOver = false;	
	
	private bool isMusicOn = true;
	private bool isSFXOn = true;
	private bool isOption =false;
	
	private bool isTap =true;
	private bool isSwipe =false;
	
	private SoundEffectController sec;
	
	//warming up vars
	private bool isFirstLaunch =false;
	private bool isPowerLabelWarmUp =false;
	private bool isComboLabelWarmUp =false;
	
	//new
	private int currentGold=0;
	private int totalGold=0;
	private int hasTutorial = 0;
	private int tapCount=0;
	private bool hasFirstAnimal;
	
	private List<int> moveQeue = new List<int>();
	
	private ShopManagerController shopMc;
	private int saveAnimal = 0;
	private bool isKid = false;
	public bool isGetSetGoDone{get;set;}

	private bool isPaused;

	private int hasSave;
	public int HasSave{
		set{hasSave = value;}
		get{return hasSave;}
	}

	private bool isOptionEnable;
	public bool IsOptionEnable{
		set{
			isOptionEnable = value;
			if(isOptionEnable){
				if(null!=ShowOption){
					ShowOption();
				}
			}else{
				if(null!=HideOption){
					HideOption();
				}
			}
		}
		get{return isOptionEnable;}
	}

	private bool isCreditEnable;
	public bool IsCreditEnable{
		set{
			isCreditEnable = value;
			if(isCreditEnable){
				if(null!=ShowCredit){
					ShowCredit();
				}
			}else{
				if(null!=HideCredit){
					HideCredit();
				}
			}
		}
		get{return isCreditEnable;}
	}

	private int warmpUpAnimalThreshold = 1;
	public int WarmpUpAnimalThreshold{
		get{ return warmpUpAnimalThreshold;}
	}

	//75
	private int reverseAnimalThreshold = 150;
	public int ReverseAnimalThreshold{
		get{ return reverseAnimalThreshold;}
	}


	private int powerUpThreshold = 30;
	public int PowerUpThreshold{
		get{ return powerUpThreshold;}
	}


	private int sunlightThreshold = 75;
	public int SunlightThreshold{
		get{ return sunlightThreshold;}
	}


	private int maxActiveAnimal = 5;
	public int MaxActiveAnimal{
		get{ return maxActiveAnimal;}
	}

	private float minPowerUpDelay = 7;
	public float MinPowerUpDelay{
		get{ return minPowerUpDelay;}
	}


	private float maxPowerUpDelay = 20;
	public float MaxPowerUpDelay{
		get{ return maxPowerUpDelay;}
	}

	//new events
	private Action TutorialComplete;
	public event Action OnTutorialComplete{
		add{TutorialComplete+=value;}
		remove{TutorialComplete-=value;}
	}

	private Action ShowOption;
	public event Action OnShowOption{
		add{ShowOption+=value;}
		remove{ShowOption-=value;}
	}

	private Action HideOption;
	public event Action OnHideOption{
		add{HideOption+=value;}
		remove{HideOption-=value;}
	}

	private Action ShowCredit;
	public event Action OnShowCredit{
		add{ShowCredit+=value;}
		remove{ShowCredit-=value;}
	}
	
	private Action HideCredit;
	public event Action OnHideCredit{
		add{HideCredit+=value;}
		remove{HideCredit-=value;}
	}
	
	public static GameDataManagerController GetInstance(){
		if( instance == null ){
			container = new GameObject();
			container.name = "GameDataManager";
			instance =  container.AddComponent( typeof(GameDataManagerController))  as GameDataManagerController ;			
			DontDestroyOnLoad(instance);
		}
		
		return instance;
	}
	
	public void AddMoveQeue(int move){
		int maxMove = 3;
		if(moveQeue.Count < maxMove){
			moveQeue.Add(move);	
		}		
	}

	public void ClearMoveQeue(){
		moveQeue.Clear();
	}
	
	public int GetMoveCount(){
		int moveCount = moveQeue.Count;
		return moveCount;
	}
	
	//1 - left
	//2 -right
	
	public int GetMove(){
		int mov = moveQeue[0];
		RemoveMoveQeue();
		return mov;
	}
	
	public void RemoveMoveQeue(){
		moveQeue.RemoveAt(0);
	}
	
	
	public int CurrentCombo{
		get{ return currentCombo; }
		set{ currentCombo = value; }
	}
	
	public void UpdateScore(int score){
		currentScore +=score;
		ScoreChecker();
	}
	
	public int GetScore(){
		return currentScore;
	}
	
	public void SetScore(int score){
		currentScore = score;
	}
	
	public void UpdateMissCount(int val){
		missCount += val;
	}
	
	public int GetMissCount(){
		return missCount;
	}
	
	public void SetMissCount(int val){
		missCount = val;
	}
	
	private void ScoreChecker(){
		if(saveAnimal > 1 && currentLevel == 1 ){
			IncreaseLevel();
		}else if(saveAnimal > 2 && currentLevel == 2 ){
			IncreaseLevel();
		}else if(saveAnimal > 3 && currentLevel == 3 ){
			IncreaseLevel();
		}else if(saveAnimal > 4 && currentLevel == 4 ){
			IncreaseLevel();
		}else if(saveAnimal > 5 && currentLevel == 5 ){
			IncreaseLevel();
		}else if(saveAnimal > 6 && currentLevel == 6 ){
			IncreaseLevel();
		}else if(saveAnimal > 7 && currentLevel == 7 ){
			IncreaseLevel();
		}else if(saveAnimal > 8 && currentLevel == 8 ){
			IncreaseLevel();
		}else if(saveAnimal > 9 && currentLevel == 9 ){
			IncreaseLevel();
		}else if(saveAnimal > 10 && currentLevel == 10 ){
			IncreaseLevel();
		}else if(saveAnimal > 11 && currentLevel == 11 ){
			IncreaseLevel();
		}else if(saveAnimal > 12 && currentLevel == 12 ){
			IncreaseLevel();
		}else if(saveAnimal > 13 && currentLevel == 13 ){
			IncreaseLevel();
		}else if(saveAnimal > 17 && currentLevel == 14 ){
			IncreaseLevel();
		}else if(saveAnimal > 25 && currentLevel == 15 ){
			IncreaseLevel();
		}else if(saveAnimal > 38 && currentLevel == 16 ){
			IncreaseLevel();
		}else if(saveAnimal > 50 && currentLevel == 17 ){
			IncreaseLevel();
		}else if(saveAnimal > 70 && currentLevel == 18 ){
			IncreaseLevel();
		}else if(saveAnimal > 90 && currentLevel == 19 ){
			IncreaseLevel();
		}else if(saveAnimal > 110 && currentLevel == 20 ){
			IncreaseLevel();
		}else if(saveAnimal > 130 && currentLevel == 21 ){
			IncreaseLevel();
		}else if(saveAnimal > 150 && currentLevel == 22 ){
			IncreaseLevel();
		}else if(saveAnimal > 170 && currentLevel == 23 ){
			IncreaseLevel();
		}else if(saveAnimal > 200 && currentLevel == 24 ){
			IncreaseLevel();
		}else if(saveAnimal > 230 && currentLevel == 25 ){
			IncreaseLevel();
		}else if(saveAnimal > 260 && currentLevel == 26 ){
			IncreaseLevel();
		}else if(saveAnimal > 290 && currentLevel == 27 ){
			IncreaseLevel();
		}else if(saveAnimal > 320 && currentLevel == 28 ){
			IncreaseLevel();
		}else if(saveAnimal > 350 && currentLevel == 29 ){
			IncreaseLevel();
		}else if(saveAnimal > 380 && currentLevel == 30 ){
			IncreaseLevel();
		}else if(saveAnimal > 410 && currentLevel == 31 ){
			IncreaseLevel();
		}else if(saveAnimal > 440 && currentLevel == 32 ){
			IncreaseLevel();
		}else if(saveAnimal > 470 && currentLevel == 33 ){
			IncreaseLevel();
		}else if(saveAnimal > 500 && currentLevel == 34 ){
			IncreaseLevel();
		}
	}	
	
	public int GetLife(){
		return life;
	}
	
	public void SetLife(int val){
		life = val;
	}
	
	public void UpdateLife(int val){
		life+=val;
		//Debug.Log("current life " + life);
	}
	
	private void IncreaseLevel(){
		currentLevel++;
		Messenger.Broadcast( GameEvent.Levelup);		
	}
	
	
	public bool IsGameOver{
		get{  return isGameOver;}
		set{ isGameOver = value; }
	}
	
	public int MaxLife{
		get{ return maxLife;}
		set{ maxLife = value;}
	}
	
	public bool IsMusicOn{
		get{ return isMusicOn; }
		set{ isMusicOn = value; }		
	}
	
	public bool IsSFXOn{
		get{ return isSFXOn; }
		set{ isSFXOn = value; }
	}
	
	public bool IsOption{
		get{ return isOption; }
		set{ isOption = value; }
	}
	
	public bool IsTap{
		get{ return isTap; }
		set{ isTap = value; }
	}
	
	public bool IsSwipe{
		get{ return isSwipe; }
		set{ isSwipe = value; }
	}
	
	public bool IsFirstLaunch{
		set{ isFirstLaunch = value; }
		get{ return isFirstLaunch; }
	}

	public bool IsComboLabelWarmUp{
		set{ isComboLabelWarmUp = value; }
		get{ return isComboLabelWarmUp; }
	}
	
	public bool IsPowerLabelWarmUp{
		set{ isPowerLabelWarmUp = value; }
		get{ return isPowerLabelWarmUp; }
	}
	
	public int TotalGold{
		set{ totalGold = value; }
		get{ return totalGold; }
	}
	
	public int CurrentGold{
		set{ currentGold = value; }
		get{ return currentGold; }
	}
	
	public int HasTutorial{
		set{ hasTutorial = value;}
		get{ return hasTutorial;}
	}
	
	public int TapCount{
		set{ tapCount = value;}
		get{ return tapCount;}
	}
	
	public bool HasFirstAnimal{
		set{ hasFirstAnimal =value; }
		get{ return hasFirstAnimal; }
	}	
	
	public int SaveAnimal{
		set{saveAnimal= value;}
		get{ return saveAnimal;}
	}
	
	public bool IsKid{
		set{isKid = value;}
		get{return isKid;}
	}
	
	// Use this for initialization
	void Start (){
		shopMc = ShopManagerController.GetInstance();

		//DeletePlayerData();
		LoadPlayerData();
		
		//point1 = new Vector3(60f, -116.2f, zPosition);
		point1 = new Vector3(-110f, -116.2f, zPosition);
		point1Slow = new Vector3(-70f,-116.2f, zPosition);
		instantSlowPoint1 = new Vector3(65f,-116.2f, zPosition);
		point1OverGrowth = new Vector3 (-300f, 50f, zPosition);


		point1b = new Vector3(-160f, 250f, zPosition);
		point2 = new Vector3 (-90f, 250f, zPosition);
		instantPoint2 = new Vector3 (-145f, 250f, zPosition);

		point2Slow = new Vector3(-45f, -116.2f, zPosition);
		instantSlowPoint2 = new Vector3(-40f, -116.2f, zPosition);

		point2OverGrowth =new Vector3(-310f, 180f, zPosition);

		//point 3rd
		point3 = new Vector3(-180f, 400f, zPosition);
		//reversePoint3 = new Vector3(-140f, 400f, zPosition);
		instantPoint3 = new Vector3(-240f, 400f, zPosition);

		point3Slow = new Vector3(-160f, -116.2f, zPosition);
		instantSlowPoint3 = new Vector3(-160f, -116.2f, zPosition);

		//last point
		point4 = new Vector3(-3000f, 500f, zPosition);
		point4Slow = new Vector3(-3000f, 1500f, zPosition);

		//pointClearAll = new Vector3(-3000f, 4500f, zPosition);
		//pointLeftClearAll = new Vector3(900f, 900f, zPosition);

		//pointSlowClearAll= new Vector3(-1500f, 1300f, zPosition);
		//pointSlowLeftClearAll= new Vector3(1100f, 1000f, zPosition);

		//slow
		point5 = new Vector3(-2000f, 1500f, zPosition);
		//sec = GameObject.Find("SFXManager").GetComponent<SoundEffectController>();		
		
		string sceneName = Application.loadedLevelName;		
		if( sceneName == GameScreen.Shop ){
			Messenger.Broadcast( GameEvent.GameDataLoaded);	
		}
		
		//DeletePlayerData();
		//StartMoney(100000);
		
		Messenger.AddListener(GameEvent.ShopItemLoaded, onShopItemLoaded);
	}
	
	private void onShopItemLoaded(){		
		shopMc.LoadBoughtItems();
		//LoadPlayerData();		
		//Debug.Log( "gdc check TotalGold " + TotalGold + " HasTutorial " + HasTutorial);
		
		//Item item = shopMc.GetItemById("6");
		//shopMc.SaveBoughtItem(item);
	}
	
	public void StartMoney(int val){		
		TotalGold = val;
		SavePlayerCoin();
	}
	
	public void ResetGameData(){
		locationIndex = 1;
		cloneLocationIndex = -1;
		currentPowerup= PowerUpChecker.Powerups.none;
		hasPowerUp = false;		
		currentLevel = 1;
		CurrentCombo = 0;		
		SetMissCount(0);
		SetLife(3);
		SetScore(0);
		IsGameOver = false;
		SaveAnimal = 0;
		isGetSetGoDone = false;
	}
	
	public void ResetWarmUpData(){
		isPowerLabelWarmUp =false;
		IsComboLabelWarmUp =false;
	}
	
	public void SavePlayerCoin(){
		SaveDataManager.SaveData(PlayerDataKey.TOTAL_GOLD.ToString(),TotalGold);
	}
	
	public void DeletePlayerCoin(){
		SaveDataManager.DeleteSaveData(PlayerDataKey.TOTAL_GOLD.ToString());
	}
	
	public void SavePlayerTutorial(){
		SaveDataManager.SaveData(PlayerDataKey.TUTORIAL.ToString(),HasTutorial);
		if(null!=TutorialComplete){
			TutorialComplete();
		}
	}

	public bool SavePlayerScore(){
		int playerScore = SaveDataManager.LoadIntSaveData(PlayerDataKey.HIGH_SCORE.ToString());
		if(currentScore > playerScore ){
			//show new highscore
			SaveDataManager.SaveData(PlayerDataKey.HIGH_SCORE.ToString(),currentScore);

			return true;
		}else{
			return false;
		}
	}
	
	public void DeletePlayerTutorial(){
		SaveDataManager.DeleteSaveData(PlayerDataKey.TUTORIAL.ToString());
	}
	
	public void SavePlayerData(){
		SavePlayerCoin();
		SavePlayerTutorial();
	}	
	
	public void LoadPlayerData(){
		hasSave = SaveDataManager.LoadIntSaveData(PlayerDataKey.HAS_SAVE.ToString());
		if(hasSave == 0){
			isSFXOn = true;
			isMusicOn = true;
			SaveDataManager.SaveData(PlayerDataKey.BGM.ToString(),1);
			SaveDataManager.SaveData(PlayerDataKey.SFX.ToString(),1);
			SaveDataManager.SaveData(PlayerDataKey.HAS_SAVE.ToString(),1);
		}else{
			if(SaveDataManager.LoadIntSaveData(PlayerDataKey.SFX.ToString()) == 1){
				isSFXOn = true;
			}else{
				isSFXOn = false;
			}

			if(SaveDataManager.LoadIntSaveData(PlayerDataKey.BGM.ToString()) == 1){
				isMusicOn = true;
			}else{
				isMusicOn = false;
			}

			hasTutorial = SaveDataManager.LoadIntSaveData(PlayerDataKey.TUTORIAL.ToString());
			TotalGold =	SaveDataManager.LoadIntSaveData(PlayerDataKey.TOTAL_GOLD.ToString());
		}
	}
	
	public void DeletePlayerData(){
		SaveDataManager.DeleteSaveData(PlayerDataKey.HAS_SAVE.ToString());
		SaveDataManager.DeleteSaveData(PlayerDataKey.BGM.ToString());
		SaveDataManager.DeleteSaveData(PlayerDataKey.CURRENT_AVATAR.ToString());
		SaveDataManager.DeleteSaveData(PlayerDataKey.HIGH_SCORE.ToString());
		SaveDataManager.DeleteSaveData(PlayerDataKey.ITEM.ToString());
		SaveDataManager.DeleteSaveData(PlayerDataKey.SFX.ToString());
		SaveDataManager.DeleteSaveData(PlayerDataKey.TOTAL_GOLD.ToString());
		SaveDataManager.DeleteSaveData(PlayerDataKey.TUTORIAL.ToString());

		shopMc.DeleteBoughtItems();	
		SaveDataManager.DeleteAll();
	}	

	public bool IsPaused{
		set{isPaused = value;}
		get{return isPaused;}
	}
}
