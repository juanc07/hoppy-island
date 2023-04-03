using UnityEngine;
using System.Collections;

public class GooglePlayController : MonoBehaviour {

	/*
	public string []regularAchievementIds;
	public string []hiddenAchievementIds;
	public string []incrementalAchievementIds;
	
	public string []leaderboardIds;
	private bool hasShownFace;
	
	public bool isDebug;
		
	void Start()
	{
		Gpg.Game.OnConnected += () =>
		{
			Debug.Log("Connected to Google Play Games");			
			ShowFace();
		};
		
		Gpg.Game.OnConnectionFailed += () =>
		{
			Debug.LogWarning("Failed to connect to Google Play Games");
		};
		
		Gpg.Game.OnDisconnected += () =>
		{
			Debug.Log("Disconnected from Google Play Games");
		};
		
		Gpg.Game.OnAchievementsLoaded += () =>
		{
			Debug.Log("Achievements loaded:");
			foreach(var achievement in Gpg.Game.Achievements)
				Debug.Log("  " + achievement);
		};
		
		Gpg.Game.OnAchievementsLoadingFailed += () =>
		{
			Debug.LogWarning("Achievements loading failed");
		};
		
		Gpg.Game.OnAchievementChanged += (achievement) =>
		{
			Debug.Log("Achievement changed: " + achievement);
		};
		
		
		Gpg.Game.OnLeaderboardsLoaded += () =>
		{
			Debug.Log("Leaderboards loaded:");
			foreach(var leaderboard in Gpg.Game.Leaderboards)
				Debug.Log("  " + leaderboard);
		};
		
		Gpg.Game.OnLeaderboardsLoadingFailed += () =>
		{
			Debug.LogWarning("Leaderboards loading failed");
		};
		
		Gpg.Game.OnLeaderboardScoresLoaded += (leaderboard) =>
		{
			Debug.Log("Leaderboard Scores loaded for " + leaderboard.DisplayName +
				" [" + leaderboard.Data.Span + ", " + leaderboard.Data.Collection + "]");
			foreach(var score in leaderboard.Data.Scores)
				Debug.Log("  [" + score.Rank + "] " + score.ScoreHolderName + " - " + score.Score);
		};
		
		Gpg.Game.OnLeaderboardScoresLoadingFailed += () =>
		{
			Debug.LogWarning("Leaderboard scores loading failed");
		};
		
		SignIn();
		Connect();		
	}
	
	private void ShowFace(){
		if(!hasShownFace){
			hasShownFace = true;
			SignOut();
			SignIn();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Connect(){
		if(!Gpg.Game.IsConnected){
			Gpg.Game.Connect();
		}
	}
	
	public void SignIn(){
		if(!Gpg.Game.IsConnected){			
			Gpg.Game.SignIn();			
		}
	}
	
	public void Disconnect(){
		Gpg.Game.Disconnect();
	}
	
	public void SignOut(){
		Gpg.Game.SignOut();
	}
	
	public void LoadAchievment(){
		Gpg.Game.LoadAchievements();
	}
	
	public void ShowAchievement(){
		Gpg.Game.ShowAchievements();
	}
	
	public void UnlockAchievement(string achievementId){
		Gpg.Game.UnlockAchievement(achievementId);
	}
	
	public void RevealAchievement(string achievementId){
		Gpg.Game.RevealAchievement(achievementId);
	}
	
	public void IncrementAchievement(string achievementId, int increment=1){
		Gpg.Game.IncrementAchievement(achievementId, increment);
	}
	
	public void LoadLeaderBoard(){
		Gpg.Game.LoadLeaderboards();
	}
	
	public void ShowLeaderBoard(){
		Gpg.Game.ShowLeaderboards();
	}
	
	public void SendScore(string leaderboardId, int score){
		Gpg.Game.SubmitScore(leaderboardId, score);
	}
	
	public void ShowLeaderboardById(string leaderboardId){
		Gpg.Game.ShowLeaderboard(leaderboardId);
	}
	
	public void LoadTop(string leaderboardId){
		Gpg.Game.LoadScores(leaderboardId,
		Gpg.LeaderboardSpan.Weekly,
		Gpg.LeaderboardCollection.Public,
		Gpg.LeaderboardSeed.Top);
	}
	
	
	
	void OnGUI()
	{
		if(!isDebug) return;
		
		GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
		GUI.skin.button.fixedHeight = (Screen.height - 20) / 10;
		
		if (!Gpg.Game.IsConnected)
		{
			if (GUILayout.Button("Connect"))
			{
				Connect();
			}
			
			if (GUILayout.Button("Sign In"))
			{
				SignIn();
			}
		}
		else
		{
			if (GUILayout.Button("Show Achievements"))
			{
				ShowAchievement();
			}
			
			if (GUILayout.Button("Load Achievements"))
			{
				LoadAchievment();
			}			
			
			if (GUILayout.Button("Disconnect"))
			{
				Disconnect();
			}
			
			if (GUILayout.Button("Sign Out"))
			{
				SignOut();
			}
			
			if (GUILayout.Button("Show Leaderboard"))
			{
				ShowLeaderBoard();
			}
		}
		
		if (GUILayout.Button("Exit"))
		{
			Application.Quit();
		}
		
		GUILayout.EndArea();
	}*/
		
}
