using UnityEngine;
using System.Collections;

public class LifeChecker : MonoBehaviour {	
	
	private GameDataManagerController gdc;
	private GameObject lifeHolder;
	
	private GameObject  animalGenerator;	
	private SoundEffectController sec;

	private GameManagerController gameManagerController;

	// Use this for initialization
	void Start () {
		GetGameManagerController();
		gdc = GameDataManagerController.GetInstance();		
		lifeHolder = GameObject.Find("AnchorRight/InGameRightPanel");

		sec = GameObject.Find("SFXManager").GetComponent<SoundEffectController>();		
		Messenger.AddListener(GameEvent.ANIMAL_FELL, OnAnimalFell);
		//Messenger.AddListener(GameEvent.Level_Restart, OnLevelRestart);
		Messenger.AddListener(GameEvent.Level_Start, OnLevelStart);
		Messenger.AddListener(GameEvent.GOT_EXTRA_LIFE, OnGotExtraLife);
		CheckLife();

		AddEventListener();
	}

	private void OnEnable(){
		GetGameManagerController();
	}
	
	private void OnDestroy(){
		RemoveEventListener();
	}
	
	private void AddEventListener(){
		
	}
	
	private void RemoveEventListener(){
		if(gameManagerController!=null){
			gameManagerController.OnLevelRestart-=OnLevelRestart;
		}
	}
	
	private void GetGameManagerController(){
		if(gameManagerController==null){
			gameManagerController = GameManagerController.GetInstance();
			gameManagerController.OnLevelRestart+=OnLevelRestart;
		}
	}

	private void OnLevelRestart(){
		if(this == null)return;
		CheckLife();
	}

	private void OnGotExtraLife(){
		if(this == null)return;
		CheckLife();
	}

	private void OnAnimalFell(){
		if(this == null)return;
		CheckLife();
	}
	
	private void OnLevelStart(){
		if(this == null)return;
		CheckLife();
	}
	
	// Update is called once per frame
	/*void Update (){
		CheckLife();
	}*/
	
	private void CheckLife(){
		
		int childCount = lifeHolder.transform.childCount;
		int life = gdc.GetLife();
		for(int index = 1; index <= childCount; index++){
			if( index <= life ){
				lifeHolder.transform.Find("heart"+index).gameObject.SetActive(true);
			}else{
				lifeHolder.transform.Find("heart"+index).gameObject.SetActive(false);
			}
		}
		
		if( life == 0 ){
			ShowGameOver();			
		}	
	}
	
	private void ShowGameOver(){
		if(!gdc.IsGameOver){
			gdc.IsGameOver = true;
			
			//GameObject inGameGUI = GameObject.Find("InGameGUI/Camera/AnchorCenter");
			//Transform gameOverPanel = inGameGUI.gameObject.transform.Find("GameOverPanel");
			//gameOverPanel.gameObject.SetActive(true);
			
			//heyzapWrapper.SendHeyzapScore();
			//heyzapWrapper.SendHeyzapScoreById(heyzapWrapper.leaderboardId, gdc.GetScore());
			Messenger.Broadcast(GameEvent.CheckAchievement);
			
			//ResetLevel();
			sec.PlaySfx(SoundEffectController.Effects.Gameover);
			Invoke("BroadCastLevelFailed", 0.1f);			
		}
	}
	
	private void BroadCastLevelFailed(){
		Messenger.Broadcast( GameEvent.Level_Failed);

		gameManagerController.GameFailed();
		gameManagerController.ShowHideGameOver(true);
	}
}
