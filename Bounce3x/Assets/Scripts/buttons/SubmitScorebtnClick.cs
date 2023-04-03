using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubmitScorebtnClick : MonoBehaviour{

	private ScoreoidRestApiManager scoreoidRestApiManager;
	private UILeaderBoard leaderboardPanel;
	private CreateUserPanel createUserPanel;
	private LoginUserPanel loginUserPanel;
	private PopUpPanelHolder popUpPanelHolder;

	private GameDataManagerController gdc;
	private bool isBusy=false;

	private KiiSocialAPI kiiSocialApi;

	private bool isSubmitScoreComplete =false;
	private bool isSubmitScoreCompleteMessageShown=false;

	private bool isSubmitScoreFail =false;
	private bool isSubmitScoreFailMessageShown=false;


	// Use this for initialization
	void Start () {
		gdc = GameDataManagerController.GetInstance();
		kiiSocialApi = KiiSocialAPI.GetInstance();

		scoreoidRestApiManager = ScoreoidRestApiManager.GetInstance();

		scoreoidRestApiManager = GameObject.FindObjectOfType(typeof(ScoreoidRestApiManager)) as ScoreoidRestApiManager;
		AddEventListener();

		popUpPanelHolder = GameObject.FindObjectOfType(typeof(PopUpPanelHolder)) as PopUpPanelHolder;
		popUpPanelHolder.gameObject.transform.Find("CreateUserPanel").gameObject.SetActive(true);
		popUpPanelHolder.gameObject.transform.Find("LoginUserPanel").gameObject.SetActive(true);


		leaderboardPanel = GameObject.FindObjectOfType(typeof(UILeaderBoard)) as UILeaderBoard;
		createUserPanel = GameObject.FindObjectOfType(typeof(CreateUserPanel)) as CreateUserPanel;
		loginUserPanel = GameObject.FindObjectOfType(typeof(LoginUserPanel)) as LoginUserPanel;

		popUpPanelHolder.gameObject.transform.Find("CreateUserPanel").gameObject.SetActive(false);
		popUpPanelHolder.gameObject.transform.Find("LoginUserPanel").gameObject.SetActive(false);
	}

	private void OnDestroy(){
		if(scoreoidRestApiManager != null){
			RemoveEventListener();
		}
	}

	public void AddEventListener(){
		//scoreoidRestApiManager.OnCreateScoreComplete += OnCreateScoreComplete;
		//scoreoidRestApiManager.OnCreateScoreFailed += OnCreateScoreComplete;

		kiiSocialApi.OnSendScoreComplete+= OnKiiSendScoreComplete;
		kiiSocialApi.OnSendScoreFailed+= OnKiiSendScoreFailed;

		kiiSocialApi.OnGetUsersComplete+= OnGetKiiUsersComplete;
		kiiSocialApi.OnGetUsersFailed+= OnGetKiiUsersFailed;
	}
	
	public void RemoveEventListener(){
		//scoreoidRestApiManager.OnCreateScoreComplete -= OnCreateScoreComplete;
		//scoreoidRestApiManager.OnCreateScoreFailed -= OnCreateScoreComplete;

		kiiSocialApi.OnSendScoreComplete-= OnKiiSendScoreComplete;
		kiiSocialApi.OnSendScoreFailed-= OnKiiSendScoreFailed;

		kiiSocialApi.OnGetUsersComplete-= OnGetKiiUsersComplete;
		kiiSocialApi.OnGetUsersFailed-= OnGetKiiUsersFailed;
	}

	private void OnKiiSendScoreFailed(){
		isBusy =false;
		isSubmitScoreFail = true;
		Debug.Log(" submit button send kii score failed ");
	}

	private void OnGetKiiUsersFailed(){
		Debug.Log(" get kii users failed ");
	}
	
	private void OnClick(){
		isSubmitScoreComplete = false;
		isSubmitScoreFail =false;

		isSubmitScoreCompleteMessageShown =false;
		isSubmitScoreFailMessageShown =false;

		//kii implementation
		if(kiiSocialApi.CheckUser()){
			//kiiSocialApi.DeleteBucket();
			kiiSocialApi.SendScore(gdc.GetScore());
			//kiiSocialApi.SendScoreWithTimeStamp(gdc.GetScore(),ScoreCategory.Monthly);
			Debug.Log("sending score to kii");
		}else{
			loginUserPanel.gameObject.SetActive(true);
		}



		/*
		//scoreroid implementation
		if(scoreoidRestApiManager.player==null){
			loginUserPanel.gameObject.SetActive(true);
		}else{
			if(!isBusy){
				isBusy =true;
				IPlayer player = scoreoidRestApiManager.player;
				Debug.Log(" check player " + player.username + " player id " + player.unique_id);
				scoreoidRestApiManager.CreateScore(player.username,player.unique_id,gdc.GetScore().ToString());
			}
		}*/
	}

	private void OnKiiSendScoreComplete(){
		isBusy =false;
		isSubmitScoreComplete = true;
		Debug.Log("sending score to kii is complete!");
		kiiSocialApi.GetUsers(ScoreCategory.Alltime);
		Debug.Log("getting users from kii");
	}

	private void OnGetKiiUsersComplete(){
		isBusy =false;
		Debug.Log("getting users from kii is complete!");
		List<ISocialUser> users = kiiSocialApi.Users;
		foreach(ISocialUser user in users){
			Debug.Log("username " + user.username + " score " + user.score);
		}
	}

	/*private void OnCreateScoreComplete(){
		leaderboardPanel.gameObject.SetActive(true);
		leaderboardPanel.gameObject.transform.Find("Camera/Anchor/LeaderBoardPanelHolder").gameObject.SetActive(true);
		scoreoidRestApiManager.GetBestScores(25,ScoreoidRestApiManager.OrderBy.score.ToString(),ScoreoidRestApiManager.Order.desc.ToString(),"2013-01-28","2013-12-31","",0);
		Debug.Log("[ SubmitScoreBtnClick ]: submit score complete");
		isBusy = false;
	}*/

	/*
	private void OnCreateScoreFailed(){
		Debug.Log("[ SubmitScoreBtnClick ]: submit score failed");
		isBusy = false;
	}*/

	private void PopulateLeaderBoard(){
		leaderboardPanel.gameObject.SetActive(true);
		leaderboardPanel.gameObject.transform.Find("Camera/Anchor/LeaderBoardPanelHolder").gameObject.SetActive(true);
		isBusy = false;
	}

	private void Update(){
		if(isSubmitScoreComplete && !isSubmitScoreCompleteMessageShown){
			isSubmitScoreCompleteMessageShown =true;
			PopulateLeaderBoard();
		}
	}
}
