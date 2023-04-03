using UnityEngine;
using System.Collections;

public class ScoreChecker : MonoBehaviour {	
	
	private GameDataManagerController gdc;	
	
	private GameObject inGamePanel;
	private Transform scoreHolder;
	private UILabel scoreUILabel;

	private GameManagerController gameManagerController;
	
	void Start (){
		GetGameManagerController();

		gdc = GameDataManagerController.GetInstance();
		inGamePanel = GameObject.Find("InGamePanel");		
		scoreHolder = inGamePanel.transform.Find("ScoreHolder");
		scoreUILabel = scoreHolder.transform.Find("ScoreValue").GetComponent<UILabel>();

		AddEventListener();
	}


	private void OnEnable(){
		GetGameManagerController();
	}
	
	private void OnDestroy(){
		RemoveEventListener();
	}
	
	private void AddEventListener(){
		Messenger.AddListener(GameEvent.GOT_SCORE, OnGotScore);
		Messenger.AddListener(GameEvent.SaveAnimal, OnSaveAnimal);
		//Messenger.AddListener(GameEvent.Level_Restart, OnLevelRestart);
	}
	
	private void RemoveEventListener(){
		if(gameManagerController!=null){
			gameManagerController.OnLevelRestart-=OnLevelRestart;
		}
	}
	
	private void GetGameManagerController(){
		if(gameManagerController==null){
			gameManagerController =  GameManagerController.GetInstance();
			gameManagerController.OnLevelRestart+=OnLevelRestart;
		}
	}

	private void OnLevelRestart(){
		gdc.SetScore(0);
		int score = gdc.GetScore();
		scoreUILabel.text = score.ToString("000000");
	}
	
	private void OnGotScore(){
		if(this == null)return;
		CheckScore();
	}
	
	private void OnSaveAnimal(){
		if(this == null)return;
		CheckScore();
	}

	/*private void OnLevelRestart(){
		gdc.SetScore(0);
		int score = gdc.GetScore();
		scoreUILabel.text = score.ToString("000000");
	}*/
	
	private void CheckScore(){
		int score = gdc.GetScore();
		scoreUILabel.text = score.ToString("000000");
		
		
		/*if(heyzapWrapper.unlockAchievements.Count == 1 && score >= 500){
			heyzapWrapper.UnlockAchievement(heyzapWrapper.achievements[1]);
		}*/
		
		/*if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
				Heyzap.submitScore(score, score.ToString() +" points", "fn9");
			#endif
		}*/
			
		
		/*#if UNITY_ANDROID
			gameCircleChecker.text ="sending score...";
	    	AGSLeaderboardsClient.SubmitScoreSucceededEvent += submitScoreSucceeded;
	    	AGSLeaderboardsClient.SubmitScoreFailedEvent += submitScoreFailed;
	    	AGSLeaderboardsClient.SubmitScore("Bounce3x001",score);    
		#endif*/
	}	
	
	/*
	#if UNITY_ANDROID
		private void submitScoreSucceeded(string leaderboardId){
	  		Debug.Log( " submitScoreSucceeded  leaderboardId " + leaderboardId );
			gameCircleChecker.text =" submitScoreSucceeded  leaderboardId " + leaderboardId;
	    }
	      
	    private void submitScoreFailed(string leaderboardId, string error){
	  		Debug.Log( " submitScoreFailed  leaderboardId " + leaderboardId + " error " + error );
			gameCircleChecker.text =" submitScoreFailed  leaderboardId " + leaderboardId + " error " + error;
	    }
	#endif
	*/
}
