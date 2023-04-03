using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LeaderBoardScoreBtn : MonoBehaviour {
	
	//private ScoreoidRestApiManager scoreoidRestApiManager;
	private bool isBusy=false;
	public enum ScoreTypes{Daily,Weekly,Monthly,AllTime}
	public ScoreTypes scoreTypes = ScoreTypes.AllTime;

	private KiiSocialAPI kiiSocialApi;

	// Use this for initialization
	void Start () {
		kiiSocialApi = KiiSocialAPI.GetInstance();
		AddEventListener();
		//scoreoidRestApiManager = ScoreoidRestApiManager.GetInstance();
		//scoreoidRestApiManager = GameObject.FindObjectOfType(typeof(ScoreoidRestApiManager)) as ScoreoidRestApiManager;
		//scoreoidRestApiManager.OnGetBestScoreComplete+=ShowBestScores;
		//scoreoidRestApiManager.OnGetBestScoreFailed+=ShowBestScoresFailed;
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener(){
		kiiSocialApi.OnGetUsersComplete+=OnGetUsersComplete;
		kiiSocialApi.OnGetUsersFailed+=OnGetUsersFailed;
	}

	private void RemoveEventListener(){
		kiiSocialApi.OnGetUsersComplete-=OnGetUsersComplete;
		kiiSocialApi.OnGetUsersFailed-=OnGetUsersFailed;
	}

	private void OnGetUsersFailed(){
		isBusy =false;
	}

	private void OnGetUsersComplete(){
		isBusy =false;
	}
	
	private void OnClick(){
		if(!isBusy){
			DateTime date = DateTime.Today;
			string daily;
			string weekly;

			if(scoreTypes==ScoreTypes.AllTime){
				kiiSocialApi.GetUsers(ScoreCategory.Alltime);
				//scoreoidRestApiManager.GetBestScoresByScoreType(25,ScoreoidRestApiManager.OrderBy.score.ToString(),ScoreoidRestApiManager.Order.desc.ToString(),ScoreoidRestApiManager.ScoreTypes.AllTime,"",0);
				//scoreoidRestApiManager.GetBestScores(25,ScoreoidRestApiManager.OrderBy.score.ToString(),ScoreoidRestApiManager.Order.desc.ToString(),"2013-01-01",date.Year + "-12-31","",0);
			}else if(scoreTypes==ScoreTypes.Daily){
				kiiSocialApi.GetUsers(ScoreCategory.Daily);
				/*daily = date.Year + "-" + date.Month + "-" + date.Day;
				date = date.AddDays(-1);
				weekly = date.Year + "-" + date.Month + "-" + date.Day;
				scoreoidRestApiManager.GetBestScores(25,ScoreoidRestApiManager.OrderBy.score.ToString(),ScoreoidRestApiManager.Order.desc.ToString(),weekly,daily,"",0);
				Debug.Log("check==> weekly " + weekly + " to daily " + daily );*/
				//scoreoidRestApiManager.GetBestScoresByScoreType(25,ScoreoidRestApiManager.OrderBy.score.ToString(),ScoreoidRestApiManager.Order.desc.ToString(),ScoreoidRestApiManager.ScoreTypes.Daily,"",0);
			}else if(scoreTypes==ScoreTypes.Weekly){
				/*
				daily = date.Year + "-" + date.Month + "-" + date.Day;
				date = date.AddDays(-7);
				weekly = date.Year + "-" + date.Month + "-" + date.Day;
				scoreoidRestApiManager.GetBestScores(25,ScoreoidRestApiManager.OrderBy.score.ToString(),ScoreoidRestApiManager.Order.desc.ToString(),weekly,daily,"",0);
				Debug.Log("check==> weekly " + weekly + " to daily " + daily );
				*/
				//scoreoidRestApiManager.GetBestScoresByScoreType(25,ScoreoidRestApiManager.OrderBy.score.ToString(),ScoreoidRestApiManager.Order.desc.ToString(),ScoreoidRestApiManager.ScoreTypes.Weekly,"",0);
				kiiSocialApi.GetUsers(ScoreCategory.Weekly);
			}else if(scoreTypes==ScoreTypes.Monthly){
				kiiSocialApi.GetUsers(ScoreCategory.Monthly);
				/*
				daily = date.Year + "-" + date.Month + "-" + date.Day;
				date = date.AddDays(-30);
				weekly = date.Year + "-" + date.Month + "-" + date.Day;
				scoreoidRestApiManager.GetBestScores(25,ScoreoidRestApiManager.OrderBy.score.ToString(),ScoreoidRestApiManager.Order.desc.ToString(),weekly,daily,"",0);
				Debug.Log("check==> monthly " + weekly + " to daily " + daily );
				*/
				//scoreoidRestApiManager.GetBestScoresByScoreType(25,ScoreoidRestApiManager.OrderBy.score.ToString(),ScoreoidRestApiManager.Order.desc.ToString(),ScoreoidRestApiManager.ScoreTypes.Monthly,"",0);
			}

			isBusy =true;
			//Debug.Log("get best scores  isBusy " + isBusy );
		}else{
			//Debug.Log("can't get best scores busy!  isBusy " + isBusy );
		}
	}
	
	private void ShowBestScores(int count){
		//Debug.Log("ShowBestScores here!");
		/*scoreoidRestApiManager.OnGetBestScoreComplete-=ShowBestScores;
		List<IPlayer> players = new List<IPlayer>();
		players = scoreoidRestApiManager.players;
		
		for(int index =0; index<players.Count;index++){
			Debug.Log(" player name " + players[index].username + " score " + players[index].best_score);
		}*/
		
		isBusy =false;
	}
	
	private void ShowBestScoresFailed(){
		/*scoreoidRestApiManager.OnGetBestScoreFailed-=ShowBestScoresFailed;*/
		Debug.Log("ShowBestScoresFailed!");
		
		isBusy =false;
	}
}
