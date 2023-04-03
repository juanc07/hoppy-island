using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;
using System;

public class KiiSocialAPI :MonoBehaviour, ISocialAPI{

	private static KiiSocialAPI instance;
	private static GameObject container;
	public const string BOUNCE3_BUCKET="bounce3x";
	private int cacheScore;
	private List<ISocialUser> users = new List<ISocialUser>();

	public static KiiSocialAPI GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name ="KiiSocialAPI";
			instance = container.AddComponent(typeof(KiiSocialAPI)) as KiiSocialAPI;
			DontDestroyOnLoad(instance);
		}
		return instance;
	}


	private Action CreateUserComplete;
	public event Action OnCreateUserComplete{
		add{CreateUserComplete+=value;}
		remove{CreateUserComplete-=value;}
	}

	private Action CreateUserFailed;
	public event Action OnCreateUserFailed{
		add{CreateUserFailed+=value;}
		remove{CreateUserFailed-=value;}
	}

	private Action LoginUserComplete;
	public event Action OnLoginUserComplete{
		add{LoginUserComplete+=value;}
		remove{LoginUserComplete-=value;}
	}

	private Action LoginUserFailed;
	public event Action OnLoginUserFailed{
		add{LoginUserFailed+=value;}
		remove{LoginUserFailed-=value;}
	}

	private Action SendScoreComplete;
	public event Action OnSendScoreComplete{
		add{SendScoreComplete+=value;}
		remove{SendScoreComplete-=value;}
	}

	private Action SendScoreFailed;
	public event Action OnSendScoreFailed{
		add{SendScoreFailed+=value;}
		remove{SendScoreFailed-=value;}
	}

	private Action GetScoreComplete;
	public event Action OnGetScoreComplete{
		add{GetScoreComplete+=value;}
		remove{GetScoreComplete-=value;}
	}

	private Action GetScoreFailed;
	public event Action OnGetScoreFailed{
		add{GetScoreFailed+=value;}
		remove{GetScoreFailed-=value;}
	}

	private Action GetUsersComplete;
	public event Action OnGetUsersComplete{
		add{GetUsersComplete+=value;}
		remove{GetUsersComplete-=value;}
	}

	private Action GetUsersFailed;
	public event Action OnGetUsersFailed{
		add{GetUsersFailed+=value;}
		remove{GetUsersFailed-=value;}
	}

	private Action GetUsersInProgress;
	public event Action OnGetUsersInProgress{
		add{GetUsersInProgress+=value;}
		remove{GetUsersInProgress-=value;}
	}

	private Action Offline;
	public event Action OnOffline{
		add{Offline+=value;}
		remove{Offline-=value;}
	}

	public void CreateUser(string username,string password){
		KiiUser.Builder builder = KiiUser.BuilderWithName(username);
		//builder.WithEmail(email);
		KiiUser usr = builder.Build();
		usr.Register(password, (KiiUser user, Exception e) =>
		             {
			if (e != null)
			{
				if(null!=CreateUserFailed){
					CreateUserFailed();
				}
				Debug.LogError("Signup failed: " + e.ToString());
				// process error
			}
			else
			{
				if(null!=CreateUserComplete){
					CreateUserComplete();
				}
				Debug.Log("Signup succeeded");
				// do something with user, it's a valid user now
			}
		});

	}

	public void LoginUser(string username,string password){
		string HtmlText =  NetConnection.GetHtmlFromUri("http://google.com");
		if(HtmlText.Equals("",StringComparison.Ordinal)){
			if(null!=Offline){
				Offline();
				return;
			}
		}


		KiiUser.LogIn(username,password, (KiiUser user, Exception e) =>
		              {
			if (e != null)
			{
				if(null!=LoginUserFailed){
					LoginUserFailed();
				}

				Debug.LogError("Login failed: " + e.ToString());
				// process error
			}
			else{
				if(null!=LoginUserComplete){
					LoginUserComplete();
				}
				Debug.Log("Login succeeded");
				// do something with user, it's a valid user now
			}
		});

	}

	public bool CheckUser(){
		if(KiiUser.CurrentUser!=null){
			return true;
		}else{
			return false;
		}
	}

	public void DeleteBucket(){
		KiiBucket bucket = Kii.Bucket(BOUNCE3_BUCKET);
		bucket.Delete(DeleteBucketCallback);
	}

	private void DeleteBucketCallback(KiiBucket k, Exception e){
		Debug.Log("Bucket has been deleted...");
	}

	public void SendScoreWithTimeStamp(int score, ScoreCategory category){
		string HtmlText =  NetConnection.GetHtmlFromUri("http://google.com");
		if(HtmlText.Equals("",StringComparison.Ordinal)){
			if(null!=Offline){
				Offline();
				return;
			}
		}
		
		if(KiiUser.CurrentUser == null) return;
		
		DateTime date = DateTime.Today;
		Int32 timestamp = (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		
		KiiUser user =KiiUser.CurrentUser;
		KiiBucket bucket = Kii.Bucket(BOUNCE3_BUCKET);
		KiiObject kiiobj = bucket.NewKiiObject();
		kiiobj["username"] = user.Username;
		kiiobj["score"] = score;
		kiiobj["timestamp"] = GetTimeStampByScoreCategory(category);
		
		kiiobj.Save( (KiiObject obj,Exception e )=>{
			if(e!=null){
				Debug.LogError(e.ToString());
				if(null!=SendScoreFailed){
					SendScoreFailed();
				}
			}else{
				//Debug.Log("Highscore sent");
				if(null!=SendScoreComplete){
					SendScoreComplete();
				}
			}
		});
		
	}


	public void SendScore(int score){
		string HtmlText =  NetConnection.GetHtmlFromUri("http://google.com");
		if(HtmlText.Equals("",StringComparison.Ordinal)){
			if(null!=Offline){
				Offline();
				return;
			}
		}

		if(KiiUser.CurrentUser == null) return;

		DateTime date = DateTime.Today;
		Int32 timestamp = (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

		KiiUser user =KiiUser.CurrentUser;
		KiiBucket bucket = Kii.Bucket(BOUNCE3_BUCKET);
		KiiObject kiiobj = bucket.NewKiiObject();
		kiiobj["username"] = user.Username;
		kiiobj["score"] = score;
		kiiobj["timestamp"] = timestamp;

		kiiobj.Save( (KiiObject obj,Exception e )=>{
			if(e!=null){
				//Debug.LogError(e.ToString());
				if(null!=SendScoreFailed){
					SendScoreFailed();
				}
			}else{
				//Debug.Log("Highscore sent");
				if(null!=SendScoreComplete){
					SendScoreComplete();
				}
			}
		});

	}

	public int GetScore(){
		string HtmlText =  NetConnection.GetHtmlFromUri("http://google.com");
		if(HtmlText.Equals("",StringComparison.Ordinal)){
			if(null!=Offline){
				Offline();
				return 0;
			}
		}

		//for check
		KiiUser user =KiiUser.CurrentUser;

		KiiBucket bucket = Kii.Bucket(BOUNCE3_BUCKET);
		KiiQuery query = new KiiQuery();
		query.SortByDesc("score");
		query.Limit = 10;
		bucket.Query(query, (KiiQueryResult<KiiObject> list, Exception e) =>
		             {
			if (e != null)
			{
				//Debug.LogError("get score Failed to query " + e.ToString());
				if(null!=GetScoreFailed){
					GetScoreFailed();
				}
			}
			else
			{
				//Debug.Log("get score Query succeeded");
				int i = 0;
				foreach (KiiObject obj in list)
				{
					int score = obj.GetInt("score");
					string username = obj.GetString("username");
					if(username.Equals(user.Username,StringComparison.Ordinal)){
						//Debug.Log( " username " + username + " score " + score );
						cacheScore = score;
						if(null!=GetScoreComplete){
							GetScoreComplete();
						}
						break;
					}
				}
			}
		});


		return cacheScore;
	}


	public void GetUsers(){
		if(null!=GetUsersInProgress){
			GetUsersInProgress();
		}

		users.Clear();
		users =null;
		string HtmlText =  NetConnection.GetHtmlFromUri("http://google.com");
		if(HtmlText.Equals("",StringComparison.Ordinal)){
			if(null!=Offline){
				Offline();
				return;
			}
		}

		KiiBucket bucket = Kii.Bucket(BOUNCE3_BUCKET);
		KiiQuery query = new KiiQuery();
		query.SortByDesc("score");
		query.Limit = 10;
		bucket.Query(query, (KiiQueryResult<KiiObject> list, Exception e) =>
		             {
			if (e != null)
			{
				//Debug.LogError("Failed to get scores query " + e.ToString());
				if(null!=GetUsersFailed){
					GetUsersFailed();
				}
			}
			else
			{
				//Debug.Log("get scores Query succeeded");
				int i = 0;
				users = new List<ISocialUser>();
				foreach (KiiObject obj in list)
				{
					int score = obj.GetInt("score");
					string username = obj.GetString("username");
					KiiSocialUser user = new KiiSocialUser();
					users.Add(user);
					user.username = username;
					user.score = score;
					//Debug.Log( "getScores ==> username " + user.username + " score " + user.score );
				}

				if(null!=GetUsersComplete){
					GetUsersComplete();
				}
			}
		});
	}

	public void GetUsers(ScoreCategory category){
		if(null!=GetUsersInProgress){
			GetUsersInProgress();
		}

		if(users!=null){
			users.Clear();
		}
		users =null;
		string HtmlText =  NetConnection.GetHtmlFromUri("http://google.com");
		if(HtmlText.Equals("",StringComparison.Ordinal)){
			if(null!=Offline){
				Offline();
				return;
			}
		}


		/*DateTime date;
		
		date = DateTime.Today;
		Int32 todayTimestamp = (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		Debug.Log( "check timestamp now daily " +  todayTimestamp + " date1 " +  date);
		
		date = DateTime.Today;
		date = date.AddDays(-7);
		Int32 weeklyTimestamp = (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		Debug.Log( "check timestamp now weekly " +  weeklyTimestamp + " date2 " +  date);
		
		date = DateTime.Today;
		date = date.AddDays(-30);
		Int32 monthlyTimestamp = (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		Debug.Log( "check timestamp now monthly " +  monthlyTimestamp + " date3 " +  date );
		*/


		//condition daily
		//greater than  or equal today
		
		//weekly 
		//lessthan today and greater than or equal weekly
		
		//monthly less than weekly and greater than or monthly

		
		KiiBucket bucket = Kii.Bucket(BOUNCE3_BUCKET);
		KiiQuery query = new KiiQuery();
		query.SortByDesc("score");
		query.Limit = 10;
		bucket.Query(query, (KiiQueryResult<KiiObject> list, Exception e) =>
		             {
			if (e != null)
			{
				Debug.LogError("Failed to get scores query " + e.ToString());
				if(null!=GetUsersFailed){
					GetUsersFailed();
				}
			}
			else
			{
				//Debug.Log("get scores Query succeeded");
				int i = 0;
				users = new List<ISocialUser>();
				foreach (KiiObject obj in list)
				{
					int score = obj.GetInt("score");
					string username = obj.GetString("username");
					Int32 timestamp = obj.GetInt("timestamp");
					//Debug.Log( "getScores ==> username " + username + " score " + score + " timestamp " + timestamp);

					/*
					 * KiiSocialUser user = new KiiSocialUser();
					users.Add(user);
					user.username = username;
					user.score = score;
					*/

					if(category == ScoreCategory.Daily){
						if(timestamp >= GetTimeStampByScoreCategory(ScoreCategory.Daily)){
							AddUser(username,score,timestamp);
						}
					}else if(category == ScoreCategory.Weekly){
						if(timestamp < GetTimeStampByScoreCategory(ScoreCategory.Daily) && timestamp >= GetTimeStampByScoreCategory(ScoreCategory.Weekly) ){
							AddUser(username,score,timestamp);
						}
					}else if(category == ScoreCategory.Monthly){
						if(timestamp < GetTimeStampByScoreCategory(ScoreCategory.Weekly) && timestamp >= GetTimeStampByScoreCategory(ScoreCategory.Monthly) ){
							AddUser(username,score,timestamp);
						}
					}else{
						AddUser(username,score,timestamp);
					}
				}
				
				if(null!=GetUsersComplete){
					GetUsersComplete();
				}
			}
		});
	}


	public int GetTimeStampByScoreCategory(ScoreCategory category){
		DateTime date;
		Int32 timestamp;

		if(category == ScoreCategory.Daily){
			date = DateTime.Today;
			timestamp = (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			//Debug.Log( "check timestamp now daily " +  timestamp + " date1 " +  date);
		}else if(category == ScoreCategory.Weekly){
			date = DateTime.Today;
			date = date.AddDays(-7);
			timestamp = (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			//Debug.Log( "check timestamp now weekly " +  timestamp + " date2 " +  date);
		}else{
			date = DateTime.Today;
			date = date.AddDays(-30);
			timestamp = (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			//Debug.Log( "check timestamp now monthly " +  timestamp + " date3 " +  date );
		}

		return timestamp;
	}

	private void AddUser(string username, int score, int timeStamp){
		KiiSocialUser user = new KiiSocialUser();
		users.Add(user);
		user.username = username;
		user.score = score;
		user.timestamp = timeStamp;
		//Debug.Log( "getScores ==> username " + user.username + " score " + user.score + " timestamp " + user.timestamp );
	}


	public List<ISocialUser> Users{
		get{return users;}
	}
}
