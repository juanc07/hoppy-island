using UnityEngine;
using System.Collections;
using System;

public class LeaderBoardBtn : MonoBehaviour {

	public GameObject leaderBoardPanel;
	private ScoreoidRestApiManager scoreoidRestApiManager;

	//kii
	private KiiSocialAPI kiiSocialApi;

	void Start (){
		//kii
		kiiSocialApi = KiiSocialAPI.GetInstance();
		//kii

		scoreoidRestApiManager = ScoreoidRestApiManager.GetInstance();
		scoreoidRestApiManager = GameObject.FindObjectOfType(typeof(ScoreoidRestApiManager)) as ScoreoidRestApiManager;
		//scoreoidRestApiManager.GetBestScores(25,ScoreoidRestApiManager.OrderBy.score.ToString(),ScoreoidRestApiManager.Order.desc.ToString(),"2013-01-28","2013-12-31","",0);

		//scoreoidRestApiManager.OnGetBestScoreComplete+=ShowBestScores;
		//scoreoidRestApiManager.OnGetBestScoreFailed+=ShowBestScoresFailed;

		/*DateTime date;

		Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		Debug.Log( "check timestamp now UTC " +  unixTimestamp);

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
	}

	private void OnClick(){
		leaderBoardPanel.gameObject.SetActive(true);
		//kiiSocialApi.GetUsers();
		kiiSocialApi.GetUsers(ScoreCategory.Alltime);
		//scoreoidRestApiManager.GetBestScores(25,ScoreoidRestApiManager.OrderBy.score.ToString(),ScoreoidRestApiManager.Order.desc.ToString(),"2013-01-28","2013-12-31","",0);
		//scoreoidRestApiManager.GetBestScoresByScoreType(25,ScoreoidRestApiManager.OrderBy.score.ToString(),ScoreoidRestApiManager.Order.desc.ToString(),ScoreoidRestApiManager.ScoreTypes.AllTime,"",0);
	}

	/*private void ShowBestScores(){
		Debug.Log("ShowBestScores LeaderBoardBtn!");
	}
	
	private void ShowBestScoresFailed(){
		Debug.Log("ShowBestScoresFailed LeaderBoardBtn!");
	}*/
}
