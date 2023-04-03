using UnityEngine;
using System.Collections;
using System;

using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class GameCenterManager : MonoBehaviour {

	private static GameCenterManager instance;
	private static GameObject container;

	public bool isAuthenticated = false;

	private Action <bool>Authenticate;
	public event Action <bool>OnAuthenticate{
		add{ Authenticate+=value;}
		remove{Authenticate-=value;}
	}

	private Action <bool>ScoreSubmitted;
	public event Action <bool>OnScoreSubmitted{
		add{ScoreSubmitted+=value;}
		remove{ScoreSubmitted-=value;}
	}

	public static GameCenterManager GetInstance(){
		if(instance ==null){
			container = new GameObject();
			container.name="GameCenterManager";
			instance =  container.AddComponent(typeof(GameCenterManager)) as GameCenterManager;
			DontDestroyOnLoad(instance.gameObject);
		}

		return instance;
	}

	public void AuthenticateUser(){
		Social.localUser.Authenticate(ProcessAuthentication);
	}

	private void ProcessAuthentication(bool sucess){
		if(sucess){
			isAuthenticated = true;
			//Debug.Log("ProcessAuthentication status sucess" );
		}else{
			isAuthenticated = false;
			//Debug.Log("ProcessAuthentication status failed" );
		}

		if(null!=Authenticate){
			Authenticate(sucess);
		}
	}

	public void ShowLeaderBoard(string leaderboardId,int timeScope=0){
		//Social.ShowLeaderboardUI();
		TimeScope scope;

		if(timeScope == 1){
			scope = TimeScope.Today;
		}else if(timeScope == 2){
			scope = TimeScope.Week;
		}else{
			scope = TimeScope.AllTime;
		}

		GameCenterPlatform.ShowLeaderboardUI(leaderboardId,scope);
	}

	public void SubmitScore(long score, string leaderboardId ){
		#if UNITY_IPHONE
			Social.ReportScore(score,leaderboardId,ProcessSubmittedScore);
		#endif
	}

	private void ProcessSubmittedScore(bool sucess){
		if(sucess){
			//Debug.Log("submit score success");
		}else{
			//Debug.Log("submit score failed");
		}

		if(null!=ScoreSubmitted){
			ScoreSubmitted(sucess);
		}
	}

}
