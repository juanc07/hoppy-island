using UnityEngine;
using System.Collections;

public class GameOverScoreChecker : MonoBehaviour {
	
	private GameDataManagerController gdc;
	private UILabel gameScoreLabel;
	private GameObject inGameGUI;
	private GameCenterManager gameCenterManager;

	public GameObject yourScoreLabel;
	public GameObject newHighScoreLabel;

	// Use this for initialization
	void Start () {
		gdc =  GameDataManagerController.GetInstance();
		inGameGUI = GameObject.Find("InGameGUI/Camera/AnchorCenter");		
		gameScoreLabel = inGameGUI.gameObject.transform.Find("GameOverPanel/GameOverScoreLabel").GetComponent<UILabel>();

		gameCenterManager = GameCenterManager.GetInstance();

		UpdateScore();
		Messenger.AddListener( GameEvent.Level_Failed, onLevelFailed);
	}
	
	
	private void onLevelFailed(){
		UpdateScore();
	}
	
	private void UpdateScore(){
		if(this==null){
			return;
		}

		bool hasNewHighScore = gdc.SavePlayerScore();

		if(hasNewHighScore){
			//show new high Score!!
			ShowHideYourScoreLabel(false);
			ShowHideNewHighScoreLabel(true);
		}else{
			ShowHideYourScoreLabel(true);
			ShowHideNewHighScoreLabel(false);
		}

		if(gameScoreLabel!=null && gdc!=null){
			gameScoreLabel.text = gdc.GetScore().ToString();
			if(gameCenterManager!=null){
				if(gameCenterManager.isAuthenticated){
					gameCenterManager.SubmitScore(gdc.GetScore(),"slappybird3d_Leaderboard_gigadrillgames01");
				}
			}
		}
	}
	
	private void ShowHideNewHighScoreLabel(bool val){
		newHighScoreLabel.gameObject.SetActive(val);
	}

	private void ShowHideYourScoreLabel(bool val){
		yourScoreLabel.gameObject.SetActive(val);
	}
}
