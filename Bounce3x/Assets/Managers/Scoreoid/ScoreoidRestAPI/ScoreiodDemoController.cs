using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScoreiodDemoController : MonoBehaviour {

	private ScoreoidRestApiManager scoreoidRestApiManager;

	private string username="";
	private string password="";
	private string uniqueId="";
	private string score="";

	// Use this for initialization
	void Start () {
		scoreoidRestApiManager = ScoreoidRestApiManager.GetInstance();
	}

	private void OnDestroy(){
		scoreoidRestApiManager.OnGetBestScoreComplete-=ShowBestScores;
		scoreoidRestApiManager.OnGetBestScoreFailed-=ShowBestScoresFailed;

		scoreoidRestApiManager.OnCreatePlayerComplete-=OnCreatePlayerComplete;
		scoreoidRestApiManager.OnCreatePlayerFailed-=OnCreatePlayerFailed;

		scoreoidRestApiManager.OnGetPlayerComplete-=OnGetPlayerComplete;
		scoreoidRestApiManager.OnGetPlayerFailed-=OnGetPlayerFailed;

		scoreoidRestApiManager.OnCreateScoreComplete-=OnCreateScoreComplete;
		scoreoidRestApiManager.OnCreateScoreFailed-=OnCreateScoreFailed;
	}

	private void GetLeaderboardScore(){
		scoreoidRestApiManager.GetBestScoresByScoreType(25,ScoreoidRestApiManager.OrderBy.score.ToString(),ScoreoidRestApiManager.Order.desc.ToString(),ScoreoidRestApiManager.ScoreTypes.AllTime,"",0);
		scoreoidRestApiManager.OnGetBestScoreComplete+=ShowBestScores;
		scoreoidRestApiManager.OnGetBestScoreFailed+=ShowBestScoresFailed;
	}

	private void CreateUser(){
		if(username.Equals("",StringComparison.Ordinal) || password.Equals("",StringComparison.Ordinal)){
			Debug.Log("please enter username and password");
			return;
		}

		scoreoidRestApiManager.CreatePlayer(username,password);
		scoreoidRestApiManager.OnCreatePlayerComplete+=OnCreatePlayerComplete;
		scoreoidRestApiManager.OnCreatePlayerFailed+=OnCreatePlayerFailed;
	}

	private void OnCreatePlayerComplete(){
		Debug.Log("OnCreatePlayerComplete");
	}

	private void OnCreatePlayerFailed(){
		Debug.Log("OnCreatePlayerFailed");
	}

	private void LoginUser(){
		if(username.Equals("",StringComparison.Ordinal) || password.Equals("",StringComparison.Ordinal)){
			Debug.Log("please enter username and password");
			return;
		}

		scoreoidRestApiManager.GetPlayer(username,password);
		scoreoidRestApiManager.OnGetPlayerComplete+=OnGetPlayerComplete;
		scoreoidRestApiManager.OnGetPlayerFailed+=OnGetPlayerFailed;
	}

	private void OnGetPlayerComplete(){
		Debug.Log("OnGetPlayerComplete!");
		uniqueId = scoreoidRestApiManager.player.unique_id;
	}

	private void OnGetPlayerFailed(){
		Debug.Log("OnGetPlayerFailed!");
	}
	
	private void ShowBestScores(int count){
		Debug.Log("ShowBestScores here!");
		scoreoidRestApiManager.OnGetBestScoreComplete-=ShowBestScores;
		List<IPlayer> players = new List<IPlayer>();
		players = scoreoidRestApiManager.players;

		for(int index =0; index<players.Count;index++){
			Debug.Log(" player name " + players[index].username + " score " + players[index].best_score);
		}
	}

	private void ShowBestScoresFailed(){
		Debug.Log("ShowBestScoresFailed!");
	}

	private void CreateScore(){
		/*if(uniqueId.Equals("",StringComparison.Ordinal)){
			Debug.Log("please login 1st");
			return;
		}*/

		scoreoidRestApiManager.CreateScore(username,uniqueId,score);
		scoreoidRestApiManager.OnCreateScoreComplete+=OnCreateScoreComplete;
		scoreoidRestApiManager.OnCreateScoreFailed+=OnCreateScoreFailed;
	}

	private void OnCreateScoreComplete(){
		Debug.Log("OnCreateScoreComplete!");
	}

	private void OnCreateScoreFailed(){
		Debug.Log("OnCreateScoreFailed!");
	}

	private void OnGUI(){
		GUI.Label(new Rect(10, 0, 100, 20), "Create User");
		username = GUI.TextField(new Rect(10, 20, 200, 20), username, 25);
		password = GUI.TextField(new Rect(10, 50, 200, 20), password, 25);

		if (GUI.Button(new Rect(10, 80, 100, 30), "Create User")){
			CreateUser();
		}

		if (GUI.Button(new Rect(10, 120, 100, 30), "Login User")){
			LoginUser();
		}

		GUI.Label(new Rect(10, 180, 100, 20), "Send Score");
		score = GUI.TextField(new Rect(10, 200, 200, 20), score, 25);
		if (GUI.Button(new Rect(10, 225, 100, 30), "Create Score")){
			CreateScore();
		}

		GUI.Label(new Rect(10, 300, 200, 20), "Leaderboard Score");
		if (GUI.Button(new Rect(10, 320, 200, 30), "Get Leaderboard Score")){
			GetLeaderboardScore();
		}
	}
}
