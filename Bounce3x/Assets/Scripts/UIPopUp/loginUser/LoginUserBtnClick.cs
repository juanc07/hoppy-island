using UnityEngine;
using System.Collections;
using System;

public class LoginUserBtnClick : MonoBehaviour {

	private ScoreoidRestApiManager scoreoidRestApiManager;
	public UILabel userInputLabel;
	public UILabel passwordLabel;
	public GameObject loginPanel;
	public GameObject loginPopupMessage;

	private IPlayer player;

	private UILeaderBoard leaderboardPanel;
	private GameDataManagerController gdc;

	private string username = "";
	private string defaultInput = "";
	private string blankInput = "";
	private string password = "";

	private bool isBusy =false;

	//new kii
	private KiiSocialAPI kiiSocialApi;
	private bool isLogin=false;

	private bool isLoginFailed = false;
	private bool isLoginComplete =false;
	private bool isShownLoginFailMessage =false;
	private bool isShownLoginCompleteMessage =false;
	private bool isOffline =false;
	private bool isOfflineMessageShown =false;

	// Use this for initialization
	void Start (){
		gdc = GameDataManagerController.GetInstance();
		leaderboardPanel = GameObject.FindObjectOfType(typeof(UILeaderBoard)) as UILeaderBoard;

		scoreoidRestApiManager = ScoreoidRestApiManager.GetInstance();
		scoreoidRestApiManager = GameObject.FindObjectOfType(typeof(ScoreoidRestApiManager)) as ScoreoidRestApiManager;

		kiiSocialApi = KiiSocialAPI.GetInstance();

		AddEventListener();
	}

	private void OnDestroy(){
		if(scoreoidRestApiManager != null){
			RemoveEventListener();
		}
	}
	
	public void AddEventListener(){
		//scoreoidRestApiManager.OnGetPlayerComplete+=GetPlayerComplete;
		//scoreoidRestApiManager.OnGetPlayerFailed+=GetPlayerFailed;
		//scoreoidRestApiManager.OnRequestFailed+=RequestFailed;

		//scoreoidRestApiManager.OnCreateScoreComplete += OnCreateScoreComplete;
		//scoreoidRestApiManager.OnCreateScoreFailed += OnCreateScoreComplete;

		kiiSocialApi.OnLoginUserComplete += OnLoginKiiUserComplete;
		kiiSocialApi.OnLoginUserFailed += OnLoginKiiUserFailed;
		kiiSocialApi.OnOffline += OnOffline;
	}
	
	public void RemoveEventListener(){
		//scoreoidRestApiManager.OnGetPlayerComplete-=GetPlayerComplete;
		//scoreoidRestApiManager.OnGetPlayerFailed-=GetPlayerFailed;
		//scoreoidRestApiManager.OnRequestFailed-=RequestFailed;

		//scoreoidRestApiManager.OnCreateScoreComplete -= OnCreateScoreComplete;
		//scoreoidRestApiManager.OnCreateScoreFailed -= OnCreateScoreComplete;

		kiiSocialApi.OnLoginUserComplete -= OnLoginKiiUserComplete;
		kiiSocialApi.OnLoginUserFailed -= OnLoginKiiUserFailed;
		kiiSocialApi.OnOffline -= OnOffline;
	}

	private void OnOffline(){
		isOffline =true;
	}

	private void OnLoginKiiUserComplete(){
		isLoginComplete =true;
		isBusy =false;
		isLogin =true;
		Debug.Log("OnLoginKiiUserComplete");
	}

	private void OnLoginKiiUserFailed(){
		isLoginFailed = true;
		isBusy =false;
		isLogin =false;
		Debug.Log("OnLoginKiiUserFailed");
	}

	/*
	private void ScoreoidLoginComplete(){
		Debug.Log("GetPlayerComplete");
		ShowHideLoginPanel(false);
		
		IPlayer player = scoreoidRestApiManager.player;
		Debug.Log(" check player " + player.username + " player id " + player.unique_id);
		scoreoidRestApiManager.CreateScore(player.username,player.unique_id,gdc.GetScore().ToString());
		isBusy = false;
	}*/

	private void ShowHideLoginPanel(bool val){
		loginPanel.gameObject.SetActive(val);
	}

	private void LoginFailed(){
		ShowPopupMessage("login failed: please check username and password!");
		Debug.Log("GetPlayerFailed");
		isBusy = false;
	}

	/*
	private void GetPlayerComplete(){
		ScoreoidLoginComplete();
	}*/

	/*
	private void GetPlayerFailed(){
		LoginFailed();
	}*/

	private void RequestFailed(){
		ShowPopupMessage("error: please check your internet connection.");
	}

	private void OnClick(){
		if(!isBusy){
			isOffline =false;
			isOfflineMessageShown= false;

			isShownLoginFailMessage =false;
			isLoginFailed =false;
			isShownLoginCompleteMessage =false;
			isLoginComplete =false;

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
				ShowPopupMessage("checking username and password please wait...");
				//scoreoidRestApiManager.GetPlayer(username,password);
				kiiSocialApi.LoginUser(username,password);
				//Debug.Log("login user successful username "  + userInputLabel.text + " password " + passwordLabel.text );
			}else{
				Debug.Log("login user failed please input username and password");
				ShowPopupMessage("login failed: please input username and password");
				isOffline =false;
				isOfflineMessageShown= false;
				
				isShownLoginFailMessage =false;
				isLoginFailed =false;
				isShownLoginCompleteMessage =false;
				isLoginComplete =false;
				
				isBusy =false;
			}	
		}
	}

	private void Update(){
		if(isOffline && !isOfflineMessageShown){
			isOfflineMessageShown =true;
			RequestFailed();
		}

		if(isLoginFailed && !isShownLoginFailMessage){
			isShownLoginFailMessage =true;
			LoginFailed();
		}

		if(isLoginComplete && !isShownLoginCompleteMessage){
			isShownLoginCompleteMessage =true;
			ShowHideLoginPanel(false);
		}
	}

	private void ShowPopupMessage(string message){
		loginPopupMessage.gameObject.SetActive(true);
		Transform labelHolder = loginPopupMessage.gameObject.transform.Find("Label");
		labelHolder.gameObject.GetComponent<UILabel>().text =message;
	}

	private void HidePopupMessage(){
		loginPopupMessage.gameObject.SetActive(false);
	}

	/*
	private void OnCreateScoreComplete(){
		leaderboardPanel.gameObject.SetActive(true);
		leaderboardPanel.gameObject.transform.Find("Camera/Anchor/LeaderBoardPanelHolder").gameObject.SetActive(true);
		scoreoidRestApiManager.GetBestScores(25,ScoreoidRestApiManager.OrderBy.score.ToString(),ScoreoidRestApiManager.Order.desc.ToString(),"2013-01-28","2013-12-31","",0);
		Debug.Log("[ on submit login Button ]: submit score complete");
	}*/
	
	/*private void OnCreateScoreFailed(){
		Debug.Log("[ on submit login Button ]: submit score failed");
	}*/
}
