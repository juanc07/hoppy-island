using UnityEngine;
using System.Collections;
using System;

public class CreateUserBtnClick : MonoBehaviour {

	private ScoreoidRestApiManager scoreoidRestApiManager;
	public UILabel userInputLabel;
	public UILabel passwordLabel;

	public GameObject uiPopupMassage;
	public GameObject createUserPanel;
	public GameObject loginUserPanel;

	private UILeaderBoard leaderboardPanel;
	private GameDataManagerController gdc;

	private string username = "";
	private string defaultInput = "";
	private string blankInput = "";
	private string password = "";

	private bool isBusy =false;

	//new kii
	private KiiSocialAPI kiiSocialApi;
	private bool isOffline =false;
	private bool isOfflineMessageShown =false;

	private bool isCreateUserComplete =false;
	private bool isCreateUserCompleteMessageShown =false;

	private bool isCreateUserFail =false;
	private bool isCreateUserFailMessageShown =false;


	// Use this for initialization
	void Start () {
		gdc = GameDataManagerController.GetInstance();

		scoreoidRestApiManager = ScoreoidRestApiManager.GetInstance();
		scoreoidRestApiManager = GameObject.FindObjectOfType(typeof(ScoreoidRestApiManager)) as ScoreoidRestApiManager;
		leaderboardPanel = GameObject.FindObjectOfType(typeof(UILeaderBoard)) as UILeaderBoard;

		kiiSocialApi = KiiSocialAPI.GetInstance();

		AddEventListener();
	}

	private void OnDestroy(){
		if(scoreoidRestApiManager != null){
			RemoveEventListener();
		}
	}

	public void AddEventListener(){
		//scoreoidRestApiManager.OnCreatePlayerComplete+=CreatePlayerComplete;
		//scoreoidRestApiManager.OnCreatePlayerFailed+=CreatePlayerFailed;
		//scoreoidRestApiManager.OnRequestFailed+=RequestFailed;

		//scoreoidRestApiManager.OnCreateScoreComplete += OnCreateScoreComplete;
		//scoreoidRestApiManager.OnCreateScoreFailed += OnCreateScoreComplete;

		//scoreoidRestApiManager.OnGetPlayerComplete+=GetPlayerComplete;
		//scoreoidRestApiManager.OnGetPlayerFailed+=GetPlayerFailed;

		kiiSocialApi.OnOffline += OnOffline;
		kiiSocialApi.OnCreateUserComplete+=OnCreateKiiUserComplete;
		kiiSocialApi.OnCreateUserFailed+=OnCreateKiiUserFailed;
	}

	public void RemoveEventListener(){
		//scoreoidRestApiManager.OnCreatePlayerComplete-=CreatePlayerComplete;
		//scoreoidRestApiManager.OnCreatePlayerFailed-=CreatePlayerFailed;
		//scoreoidRestApiManager.OnRequestFailed-=RequestFailed;

		//scoreoidRestApiManager.OnCreateScoreComplete -= OnCreateScoreComplete;
		//scoreoidRestApiManager.OnCreateScoreFailed -= OnCreateScoreComplete;
		
		//scoreoidRestApiManager.OnGetPlayerComplete-=GetPlayerComplete;
		//scoreoidRestApiManager.OnGetPlayerFailed-=GetPlayerFailed;

		kiiSocialApi.OnOffline -= OnOffline;
		kiiSocialApi.OnCreateUserComplete-=OnCreateKiiUserComplete;
		kiiSocialApi.OnCreateUserFailed-=OnCreateKiiUserFailed;
	}

	private void OnOffline(){
		isOffline =true;
	}

	private void OnCreateKiiUserComplete(){
		isCreateUserComplete =true;
		Debug.Log(" OnCreateKiiUserComplete ");
	}

	private void CreateUserCompleteMessage(){
		createUserPanel.gameObject.SetActive(false);
		isBusy = false;
	}

	private void OnCreateKiiUserFailed(){
		isCreateUserFail =true;
		Debug.Log(" OnCreateKiiUserFailed ");
	}
	
	private void OnClick(){
		if(!isBusy){
			isCreateUserComplete =false;
			isCreateUserCompleteMessageShown =false;

			isCreateUserFail =false;
			isCreateUserFailMessageShown =false;

			isOffline =false;
			isOfflineMessageShown =false;
			isBusy =true;

			username = userInputLabel.text;
			defaultInput = "You can type here";
			blankInput = "";
			password = passwordLabel.text;

			bool result1 = username.Equals(defaultInput, StringComparison.Ordinal);
			bool result2 = username.Equals(blankInput, StringComparison.Ordinal);
			bool result3 = password.Equals(defaultInput, StringComparison.Ordinal);
			bool result4 = password.Equals(blankInput, StringComparison.Ordinal);



			//if( !result1 && !result2 && !result3 && !result4 ){
			if( !result1 && !result2){
				//scoreoidRestApiManager.CreatePlayer(username,password);
				kiiSocialApi.CreateUser( username,password );
				Debug.Log("create user successful username "  + userInputLabel.text + " password " + passwordLabel.text );
			}else{
				ShowPopupMessage("creating user failed: please input username and password");
				isCreateUserComplete =false;
				isCreateUserCompleteMessageShown =false;
				
				isCreateUserFail =false;
				isCreateUserFailMessageShown =false;
				
				isOffline =false;
				isOfflineMessageShown =false;
				isBusy =false;
				Debug.Log("create user failed please input username and password");
			}
		}
	}

	/*
	private void CreatePlayerComplete(){
		Debug.Log("Create player complete!");
		//ShowPopupMessage("creating user complete!");
		createUserPanel.gameObject.SetActive(false);
		//loginUserPanel.gameObject.SetActive(true);
		scoreoidRestApiManager.GetPlayer(username,password);
		isBusy = false;
	}*/

	private void CreatePlayerFailed(){
		Debug.Log("creating user failed: Create player failed!");
		ShowPopupMessage("creating user failed: A player with that username already exists!");
		isBusy =false;
	}

	/*
	private void GetPlayerComplete(){
		Debug.Log("[submit and create ]: GetPlayerComplete");

		IPlayer player = scoreoidRestApiManager.player;
		Debug.Log(" check player " + player.username + " player id " + player.unique_id);
		scoreoidRestApiManager.CreateScore(player.username,player.unique_id,gdc.GetScore().ToString());
	}*/
	
	private void GetPlayerFailed(){
		Debug.Log("[submit and create ]: GetPlayerFailed");
	}

	private void RequestFailed(){
		ShowPopupMessage("error: please check your internet connection.");
	}

	private void ShowPopupMessage(string message){
		uiPopupMassage.gameObject.SetActive(true);
		Transform labelHolder = uiPopupMassage.gameObject.transform.Find("Label");
		labelHolder.gameObject.GetComponent<UILabel>().text =message;
	}
	
	private void HidePopupMessage(){
		uiPopupMassage.gameObject.SetActive(false);
	}

	/*
	private void OnCreateScoreComplete(){
		leaderboardPanel.gameObject.SetActive(true);
		leaderboardPanel.gameObject.transform.Find("Camera/Anchor/LeaderBoardPanelHolder").gameObject.SetActive(true);
		scoreoidRestApiManager.GetBestScores(25,ScoreoidRestApiManager.OrderBy.score.ToString(),ScoreoidRestApiManager.Order.desc.ToString(),"2013-01-28","2013-12-31","",0);
		Debug.Log("[ on submit create Button ]: submit score complete");
	}*/
	
	/*private void OnCreateScoreFailed(){
		Debug.Log("[ on submit create Button ]: submit score failed");
	}*/

	private void Update(){		
		if(isOffline && !isOfflineMessageShown){
			isOfflineMessageShown =true;
			RequestFailed();
		}

		if(isCreateUserComplete && !isCreateUserCompleteMessageShown){
			isCreateUserCompleteMessageShown =true;
			CreateUserCompleteMessage();
		}

		if(isCreateUserFail && !isCreateUserFailMessageShown){
			isCreateUserFailMessageShown =true;
			CreatePlayerFailed();
		}
	}
}
