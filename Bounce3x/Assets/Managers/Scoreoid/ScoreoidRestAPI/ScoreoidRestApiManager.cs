using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net;
using System.IO;

public class ScoreoidRestApiManager : MonoBehaviour {
	
	private string apiKey="";
	private string gameId="";

	public enum reponseType{json};
	private reponseType response= reponseType.json;

	public IPlayer player{set;get;}
	public List<IPlayer> players = new List<IPlayer>();

	public enum Platform{web,android,ios,All}
	public enum OrderBy{date,score}
	public enum Order{asc,desc}

	public Action <int>OnGetBestScoreComplete;
	public Action OnGetBestScoreFailed;
	public Action OnGetBestScoreInProgress;

	public Action OnCreatePlayerComplete;
	public Action OnCreatePlayerFailed;

	public Action OnGetPlayerComplete;
	public Action OnGetPlayerFailed;

	public Action OnCreateScoreComplete;
	public Action OnCreateScoreFailed;

	public Action OnRequestFailed;
	public Action OnGetGameGameComplete;
	public Action OnGetGameGameFailed;


	private static ScoreoidRestApiManager instance;
	private static GameObject container;

	private DateTime date;
	private string daily;
	private string weekly;
	public enum ScoreTypes{Daily,Weekly,Monthly,AllTime}

	public static ScoreoidRestApiManager GetInstance(){
		if(instance == null){
			container = new GameObject();
			container.name = "ScoreoidRestApiManager";
			instance =  container.AddComponent( typeof(ScoreoidRestApiManager))  as ScoreoidRestApiManager ;
			ScoreoidConfigHolder scoreoidConfig =(ScoreoidConfigHolder) Resources.Load("Config/ScoreiodConfig",typeof(ScoreoidConfigHolder));
			if(scoreoidConfig!=null){
				instance.apiKey=scoreoidConfig.config.apiKey;
				instance.gameId= scoreoidConfig.config.gameId;
			}else{
				Debug.Log("please Generate ScoreoidConfig");
			}
			DontDestroyOnLoad(instance);
		}
		return instance;
	}
	
	// Use this for initialization
	private void Start (){
	}
	
	public void GetGame(){
		if(apiKey.Equals("",StringComparison.Ordinal) || gameId.Equals("",StringComparison.Ordinal)){
			Debug.Log("please Generate scoreoid config 1st using scoreoid menu");
			return;
		}

		/* API  method */
        string url = "https://api.scoreoid.com/v1/getGame";

        /* Unity WWW Class used for HTTP Request */
        WWWForm form = new WWWForm();
       
        form.AddField( "api_key", apiKey );
        form.AddField( "game_id", gameId);
        form.AddField( "response", response.ToString());
       
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForGetGameRequest(www));
	}
	
	
    IEnumerator WaitForGetGameRequest(WWW www)
    {
        yield return www;

        /* Check for errors */
        if (www.error == null)
        {
			Debug.Log("WWW Ok!: " + www.text);
			bool hasError =false;
			string status = www.text;
			if(status.IndexOf("error")!=-1){
				hasError = true;
				if(null!=OnGetGameGameFailed){
					OnGetGameGameFailed();
				}
			}

			if(!hasError){
				bool ok =false;
				string response = www.text;
				object obj =JSON.JsonDecode(response,ref ok);
				ArrayList objArray =(ArrayList)obj;

				if(ok){
					Hashtable ht =  objArray[0] as Hashtable;
					Hashtable data =(Hashtable)ht["Game"];
					Debug.Log("parse successful");
					Debug.Log("parse data " + data);
					Debug.Log("parse game user_id " + data["user_id"]);
					Debug.Log("parse game name " + data["name"]);
					Debug.Log("parse game short_description " + data["short_description"]);
					if(null!=OnGetGameGameComplete){
						OnGetGameGameComplete();
					}
				}else{
					Debug.Log("parse failed! ");
				}
			}
        } else {
			string HtmlText = GetHtmlFromUri("http://google.com");
			if(HtmlText == "")
			{
				if(null!=OnRequestFailed){
					OnRequestFailed();
				}
			}else{
				if(null!=OnGetGameGameFailed){
					OnGetGameGameFailed();
				}
			}
            Debug.Log("WWW Error: "+ www.error);
        }    
    }

	public void CreatePlayer(string username, string password, string email =""){
		if(apiKey.Equals("",StringComparison.Ordinal) || gameId.Equals("",StringComparison.Ordinal)){
			Debug.Log("please Generate scoreoid config 1st using scoreoid menu");
			return;
		}

		/* API  method */
		string url = "https://api.scoreoid.com/v1/createPlayer";
		
		/* Unity WWW Class used for HTTP Request */
		WWWForm form = new WWWForm();


		Guid guid = Guid.NewGuid();
		String[] parts = guid.ToString().Split('-');
		string timeNow = DateTime.Now.Ticks.ToString();

		StringBuilder sb = new StringBuilder();
		sb.Append(parts[0]);
		sb.Append(timeNow);
		string uniqueId = sb.ToString();

		form.AddField( "api_key", apiKey );
		form.AddField( "game_id", gameId);
		form.AddField( "username", username);
		form.AddField( "unique_id", uniqueId);
		form.AddField( "password", password);
		form.AddField( "email", email);
		form.AddField( "response", response.ToString());
		
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForCreatePlayerRequest(www));
	}

	IEnumerator WaitForCreatePlayerRequest(WWW www)
	{
		yield return www;
		
		/* Check for errors */
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.text);
			bool hasError =false;
			string status = www.text;
			if(status.IndexOf("error")!=-1){
				hasError = true;
				if(null!=OnCreatePlayerFailed){
					OnCreatePlayerFailed();
				}
			}

			if(!hasError){
				bool ok =false;
				string response = www.text;
				object obj =JSON.JsonDecode(response,ref ok);
				Hashtable data =(Hashtable)obj;


				bool checker =false;
				
				try{
					Hashtable errorData =(Hashtable)obj;
					string errorMessage = (string)errorData["error"];
					List<string> errorMassages = new List<string>(new string[]{"A player with that username already exists"});
					foreach( string err in errorMassages ){
						checker = errorMessage.Equals(err, StringComparison.Ordinal);
						if(checker){
							Debug.Log("found error! "+ err);
							break;
						}
					}
					
				}catch(Exception e){
					Debug.Log("got create player exception values " + e);
				}

				
				if(ok){
					if(!checker){
						Debug.Log("createplayer successful");
						Debug.Log("parse data " + data["success"]);

						if(null!=OnCreatePlayerComplete){
							OnCreatePlayerComplete();
						}
					}else{
						Debug.Log("createplayer failed! ");
						if(null!=OnCreatePlayerFailed){
							OnCreatePlayerFailed();
						}
					}
				}else{
					Debug.Log("createplayer failed! ");
					if(null!=OnCreatePlayerFailed){
						OnCreatePlayerFailed();
					}
				}
			}
		} else{
			string HtmlText = GetHtmlFromUri("http://google.com");
			if(HtmlText == "")
			{
				if(null!=OnRequestFailed){
					OnRequestFailed();
				}
			}else{
				if(null!=OnCreatePlayerFailed){
					OnCreatePlayerFailed();
				}
			}
			Debug.Log("createplayer Error: "+ www.error);
		}    
	}

	public void GetPlayer(string username, string password, string email =""){
		if(apiKey.Equals("",StringComparison.Ordinal) || gameId.Equals("",StringComparison.Ordinal)){
			Debug.Log("please Generate scoreoid config 1st using scoreoid menu");
			return;
		}


		/* API  method */
		string url = "https://api.scoreoid.com/v1/getPlayer";
		
		/* Unity WWW Class used for HTTP Request */
		WWWForm form = new WWWForm();
		
		form.AddField( "api_key", apiKey );
		form.AddField( "game_id", gameId);
		form.AddField( "username", username);
		form.AddField( "password", password);
		form.AddField( "email", email);
		form.AddField( "response", response.ToString());
		
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForGetPlayerRequest(www));
	}

	IEnumerator WaitForGetPlayerRequest(WWW www)
	{
		yield return www;
		
		/* Check for errors */
		if (www.error == null)
		{
			Debug.Log("WaitForGetPlayerRequest Ok!: " + www.text);
			bool hasError =false;
			string status = www.text;
			if(status.IndexOf("error")!=-1){
				hasError = true;
				if(null!=OnGetPlayerFailed){
					OnGetPlayerFailed();
				}
			}

			if(!hasError){
				bool ok =false;
				string response = www.text;
				object obj =JSON.JsonDecode(response,ref ok);
				Debug.Log( "check ok " + ok + " obj " + obj);
				bool checker =false;

				try{
					Hashtable errorData =(Hashtable)obj;
					string errorMessage = (string)errorData["error"];
					List<string> errorMassages = new List<string>(new string[]{"Player not found","Invalid player password"});
					foreach( string err in errorMassages ){
						checker = errorMessage.Equals(err, StringComparison.Ordinal);
						if(checker){
							Debug.Log("found error! "+ err);
							break;
						}
					}

				}catch(Exception e){
					Debug.Log("got player values catch from exceptions! exception " + e);
				}

				if(ok){
					if(!checker){
						ArrayList objArray =(ArrayList)obj;
						Hashtable ht =  objArray[0] as Hashtable;
						Hashtable data =(Hashtable)ht["Player"];
						Debug.Log("GetPlayer successful");
						
						player = new Player();
						player.username = (string)data["username"];
						player.password = (string)data["password"];
						player.unique_id = (string)data["unique_id"];
						player.email = (string)data["email"];
						
						player.best_score = data["best_score"].ToString();
						player.gold = data["gold"].ToString();
						player.money = data["money"].ToString();
						player.achievements= (string)data["achievements"];
						player.platform = (string)data["platform"];
						player.rank = data["rank"].ToString();
						
						Debug.Log("parse player unique_id " + player.unique_id);
						Debug.Log("parse player username " + player.username);
						Debug.Log("parse player password " + player.password);
						Debug.Log("parse player gold " + player.gold);
						Debug.Log("parse player money " + player.money);
						Debug.Log("parse player best_score " + player.best_score);
						Debug.Log("parse player platform " + player.platform);
						Debug.Log("parse player rank " + player.rank);

						if(null!=OnGetPlayerComplete){
							OnGetPlayerComplete();
						}
					}else{
						if(null!=OnGetPlayerFailed){
							OnGetPlayerFailed();
						}
						Debug.Log("GetPlayer failed yeah!");
					}
				}else{
					if(null!=OnGetPlayerFailed){
						OnGetPlayerFailed();
					}
					Debug.Log("GetPlayer failed! ");
				}
			}
		} else {
			string HtmlText = GetHtmlFromUri("http://google.com");
			if(HtmlText == "")
			{
				if(null!=OnRequestFailed){
					OnRequestFailed();
				}
			}else{
				if(null!=OnGetPlayerFailed){
					OnGetPlayerFailed();
				}
			}
			Debug.Log("GetPlayer Error: "+ www.error);
		}    
	}

	public void CreateScore(string username,string unique_id,string score,string platform="web,android,ios", int difficulty=0){
		if(apiKey.Equals("",StringComparison.Ordinal) || gameId.Equals("",StringComparison.Ordinal)){
			Debug.Log("please Generate scoreoid config 1st using scoreoid menu");
			return;
		}

		/* API  method */
		string url = "https://api.scoreoid.com/v1/createScore";
		
		/* Unity WWW Class used for HTTP Request */
		WWWForm form = new WWWForm();
		
		form.AddField( "api_key", apiKey );
		form.AddField( "game_id", gameId);
		form.AddField( "response", response.ToString());
		form.AddField( "username", username);
		form.AddField( "score",score);
		form.AddField( "platform",platform);
		form.AddField( "unique_id", unique_id);
		form.AddField( "difficulty", difficulty);

		
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForCreateScoreRequest(www));
	}

	IEnumerator WaitForCreateScoreRequest(WWW www)
	{
		yield return www;
		
		/* Check for errors */
		if (www.error == null)
		{
			Debug.Log("CreateScore Ok!: " + www.text);
			bool hasError =false;
			string status = www.text;
			if(status.IndexOf("error")!=-1){
				hasError = true;
				if(null!=OnCreateScoreFailed){
					OnCreateScoreFailed();
				}
			}

			if(!hasError){
				bool ok =false;
				string response = www.text;
				object obj =JSON.JsonDecode(response,ref ok);
				
				if(ok){
					Hashtable data =(Hashtable)obj;
					if(null!=OnCreateScoreComplete){
						OnCreateScoreComplete();
					}
					Debug.Log("CreateScore successful");
					Debug.Log("parse data " + data["success"]);
				}else{
					if(null!=OnCreateScoreFailed){
						OnCreateScoreFailed();
					}
					Debug.Log("CreateScore failed! ");
				}
			}
		} else{

			string HtmlText = GetHtmlFromUri("http://google.com");
			if(HtmlText == "")
			{
				if(null!=OnRequestFailed){
					OnRequestFailed();
				}
			}else{
				if(null!=OnCreateScoreFailed){
					OnCreateScoreFailed();
				}
			}

			Debug.Log("CreateScore Error: "+ www.error);
		}    
	}

	public void GetBestScores(int limit,string orderBy,string order,string startDate,string endDate,string platform="web,android,ios",int difficulty =0){
		if(apiKey.Equals("",StringComparison.Ordinal) || gameId.Equals("",StringComparison.Ordinal)){
			Debug.Log("please Generate scoreoid config 1st using scoreoid menu");
			return;
		}


		/* API  method */
		string url = "https://api.scoreoid.com/v1/getBestScores";
		
		/* Unity WWW Class used for HTTP Request */
		WWWForm form = new WWWForm();
		
		form.AddField( "api_key", apiKey );
		form.AddField( "game_id", gameId);
		form.AddField( "limit", limit );
		form.AddField( "order_by", orderBy );
		form.AddField( "order", order );
		form.AddField( "start_date", startDate );
		form.AddField( "end_date", endDate );
		form.AddField( "platform", platform );
		form.AddField( "difficulty", difficulty );
		form.AddField( "response", response.ToString());
		
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForGetBestScoreRequest(www));
		Debug.Log("scoreoid api manager getting best scores...");
		if(null!= OnGetBestScoreInProgress){
			OnGetBestScoreInProgress();
		}
	}

	public void GetBestScoresByScoreType(int limit,string orderBy,string order,ScoreTypes scoreTypes,string platform="web,android,ios",int difficulty =0){
		if(apiKey.Equals("",StringComparison.Ordinal) || gameId.Equals("",StringComparison.Ordinal)){
			Debug.Log("please Generate scoreoid config 1st using scoreoid menu");
			return;
		}


		date = DateTime.Today;
		string startDate;
		string endDate;
		
		if(scoreTypes==ScoreTypes.Daily){
			daily = date.Year + "-" + date.Month + "-" + date.Day;
			date = date.AddDays(-1);
			weekly = date.Year + "-" + date.Month + "-" + date.Day;

			startDate = weekly;
			endDate = daily;
		}else if(scoreTypes==ScoreTypes.Weekly){
			daily = date.Year + "-" + date.Month + "-" + date.Day;
			date = date.AddDays(-7);
			weekly = date.Year + "-" + date.Month + "-" + date.Day;

			startDate = weekly;
			endDate = daily;
		}else if(scoreTypes==ScoreTypes.Monthly){
			daily = date.Year + "-" + date.Month + "-" + date.Day;
			date = date.AddDays(-30);
			weekly = date.Year + "-" + date.Month + "-" + date.Day;

			startDate = weekly;
			endDate = daily;
		}else{
			startDate = "2013-01-01";
			endDate = date.Year + "-12-31";
		}


		/* API  method */
		string url = "https://api.scoreoid.com/v1/getBestScores";
		
		/* Unity WWW Class used for HTTP Request */
		WWWForm form = new WWWForm();
		
		form.AddField( "api_key", apiKey );
		form.AddField( "game_id", gameId);
		form.AddField( "limit", limit );
		form.AddField( "order_by", orderBy );
		form.AddField( "order", order );
		form.AddField( "start_date", startDate );
		form.AddField( "end_date", endDate );
		form.AddField( "platform", platform );
		form.AddField( "difficulty", difficulty );
		form.AddField( "response", response.ToString());
		
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForGetBestScoreRequest(www));
		Debug.Log("scoreoid api manager getting best scores...");
		if(null!= OnGetBestScoreInProgress){
			OnGetBestScoreInProgress();
		}
	}

	IEnumerator WaitForGetBestScoreRequest(WWW www)
	{
		yield return www;
		
		/* Check for errors */
		if (www.error == null)
		{
			Debug.Log("WaitForGetBestScoreRequest Ok!: " + www.text);
			bool hasError =false;
			string status = www.text;
			if(status.IndexOf("error")!=-1){
				hasError = true;
				if(null!=OnGetBestScoreFailed){
					OnGetBestScoreFailed();
				}
			}

			if(!hasError){
				bool ok =false;
				string response = www.text;
				object obj =JSON.JsonDecode(response,ref ok);
				
				if(ok){
					ArrayList objArray =(ArrayList)obj;
					players = new List<IPlayer>();
					int playerCnt = objArray.Count;
					Debug.Log("getBestScore count: " + playerCnt);

					for(int index=0;index<playerCnt;index++){
						Hashtable ht =  objArray[index] as Hashtable;
						Hashtable data =(Hashtable)ht["Player"];
						Hashtable scoreData =(Hashtable)ht["Score"];
						IPlayer tempPlayer = new Player();
						tempPlayer.username = (string)data["username"];
						tempPlayer.email = (string)data["email"];
						tempPlayer.best_score = scoreData["score"].ToString();

						//Debug.Log("parse player username " + player.username);
						//Debug.Log("parse player best_score " + player.best_score);
						players.Add(tempPlayer);
					}

					if(null!=OnGetBestScoreComplete){
						OnGetBestScoreComplete(players.Count);
					}
				}else{
					Debug.Log("get best score failed! ");
					if(null!=OnGetBestScoreFailed){
						OnGetBestScoreFailed();
					}
				}
			}
		} else{
			string HtmlText =  NetConnection.GetHtmlFromUri("http://google.com");
			if(HtmlText == "")
			{
				if(null!=OnRequestFailed){
					OnRequestFailed();
				}
			}else{
				if(null!=OnGetBestScoreFailed){
					OnGetBestScoreFailed();
				}
			}
			Debug.Log("get best score Error: "+ www.error);
		}    
	}


	public string GetHtmlFromUri(string resource)
	{
		string html = string.Empty;
		HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
		try
		{
			using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
			{
				bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
				if (isSuccess)
				{
					using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
					{
						//We are limiting the array to 80 so we don't have
						//to parse the entire html document feel free to
						//adjust (probably stay under 300)
						char[] cs = new char[80];
						reader.Read(cs, 0, cs.Length);
						foreach(char ch in cs)
						{
							html +=ch;
						}
						return html;
					}
				}else{
					return "";					
				}
			}
		}
		catch{
			return "";
		}
	}
}
