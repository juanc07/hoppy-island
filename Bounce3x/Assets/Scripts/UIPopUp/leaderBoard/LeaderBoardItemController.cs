using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LeaderBoardItemController : MonoBehaviour {

	private ScoreoidRestApiManager scoreoidRestApiManager;
	public GameObject leaderboardItem;
	private UIGrid uigrid;
	private Bounds mBounds;
	public GameObject panelHolder;
	public UILabel leaderBoardStatusLabel;

	public List<GameObject> leaderboardItems;

	//kii
	private KiiSocialAPI kiiSocialApi;
	private bool isLoadComplete =false;
	private bool isLeaderboardDataLoaded =false;

	private bool isOffline =false;
	private bool isOfflineMessageShown =false;

	// Use this for initialization
	void Start (){
		//DontDestroyOnLoad(this.transform.gameObject);
		uigrid =  this.gameObject.GetComponent<UIGrid>();
		//kii
		kiiSocialApi = KiiSocialAPI.GetInstance();
		//kii


		scoreoidRestApiManager = ScoreoidRestApiManager.GetInstance();
		scoreoidRestApiManager = GameObject.FindObjectOfType(typeof(ScoreoidRestApiManager)) as ScoreoidRestApiManager;
		AddEventListener();

		ShowHideStatus(true,"Loading Please wait...");
	}

	private void ShowHideStatus(bool isShow, string msg=""){
		if(isShow){
			leaderBoardStatusLabel.alpha = 1f;
			leaderBoardStatusLabel.text=msg;
		}else{
			leaderBoardStatusLabel.alpha = 0;
		}
	}

	public void AddEventListener(){
		//scoreoidRestApiManager.OnGetBestScoreComplete+=ShowBestScores;
		//scoreoidRestApiManager.OnGetBestScoreFailed+=ShowBestScoresFailed;
		//scoreoidRestApiManager.OnGetBestScoreInProgress+=BestScoresInProgress;
		//scoreoidRestApiManager.OnRequestFailed+=RequestFailed;

		//kii implementation
		kiiSocialApi.OnOffline += OnOffline;

		kiiSocialApi.OnGetUsersInProgress+=OnGetKiiUsersInProgress;
		kiiSocialApi.OnGetUsersComplete+= OnGetKiiUsersComplete;
		kiiSocialApi.OnGetUsersFailed+= OnGetKiiUsersFailed;

	}
	
	public void RemoveEventListener(){
		//scoreoidRestApiManager.OnGetBestScoreComplete-=ShowBestScores;
		//scoreoidRestApiManager.OnGetBestScoreFailed-=ShowBestScoresFailed;
		//scoreoidRestApiManager.OnGetBestScoreInProgress-=BestScoresInProgress;
		//scoreoidRestApiManager.OnRequestFailed-=RequestFailed;

		kiiSocialApi.OnOffline -= OnOffline;
		kiiSocialApi.OnGetUsersInProgress-=OnGetKiiUsersInProgress;
		kiiSocialApi.OnGetUsersComplete-= OnGetKiiUsersComplete;
		kiiSocialApi.OnGetUsersFailed-= OnGetKiiUsersFailed;
	}

	private void OnDestroy(){
		if(scoreoidRestApiManager!=null){
			RemoveEventListener();
		}
	}

	private void OnOffline(){
		isOffline = true;
	}

	private void OnGetKiiUsersInProgress(){
		isOffline =false;
		isOfflineMessageShown =false;

		isLoadComplete =false;
		isLeaderboardDataLoaded =false;
		ClearChild();
		ShowHideStatus(true,"Loading Please wait...");
	}

	private void OnGetKiiUsersFailed(){
		//Debug.Log("leader boardItem Controller: get kii users failed ");
	}

	private void OnGetKiiUsersComplete(){
		//Debug.Log("leader boardItem Controller: getting users from kii is complete!");
		List<ISocialUser> users = kiiSocialApi.Users;
		foreach(ISocialUser user in users){
			//Debug.Log("leader boardItem Controller: username " + user.username + " score " + user.score);
		}
		isLoadComplete =true;
	}

	private void PopulateLeaderBoard(){
		//Debug.Log("kii event popuplate leaderboard now!!");
		List<ISocialUser> users = kiiSocialApi.Users;
		ClearChild();
		leaderboardItems= new List<GameObject>();
		
		for(int index =0; index<users.Count;index++){
			//Debug.Log(" player name " + players[index].username + " score " + players[index].best_score);
			if(leaderboardItem!=null && this != null){
				GameObject item = NGUITools.AddChild(this.gameObject,leaderboardItem);
				item.gameObject.transform.Find("UsernameLabel").GetComponent<UILabel>().text=users[index].username;
				item.gameObject.transform.Find("ScoreLabel").GetComponent<UILabel>().text=users[index].score.ToString();
				leaderboardItems.Add(item);
			}
		}

		uigrid.Reposition();
		AddSpringPanel();
		if(users.Count==0){
			ShowHideStatus(true,"no score submitted for this category...");
		}else{
			ShowHideStatus(false,"");
		}
	}

	/*
	private void BestScoresInProgress(){
		ClearChild();
		ShowHideStatus(true,"Loading Please wait...");
	}*/


	/*
	private void ShowBestScores(int count){
		//Debug.Log("ShowBestScores here!");
		ClearChild();
		List<IPlayer> players = new List<IPlayer>();
		players = scoreoidRestApiManager.players;

		leaderboardItems= new List<GameObject>();
		
		for(int index =0; index<players.Count;index++){
			//Debug.Log(" player name " + players[index].username + " score " + players[index].best_score);
			if(leaderboardItem!=null && this != null){
				GameObject item = NGUITools.AddChild(this.gameObject,leaderboardItem);
				item.gameObject.transform.Find("UsernameLabel").GetComponent<UILabel>().text=players[index].username;
				item.gameObject.transform.Find("ScoreLabel").GetComponent<UILabel>().text=players[index].best_score;
				leaderboardItems.Add(item);
			}
		}

		uigrid.Reposition();
		AddSpringPanel();
		if(count==0){
			ShowHideStatus(true,"no score submitted for this category...");
		}else{
			ShowHideStatus(false,"");
		}
	}
	*/

	private void ItemChecker(){
		foreach( GameObject obj in leaderboardItems ){
			if(obj==null)break;
			UIPanel uipanel = UIPanel.Find(obj.transform);

			if(uipanel!=null){
				mBounds = NGUIMath.CalculateRelativeWidgetBounds(uipanel.cachedTransform, obj.transform);
				
				if(uipanel.clipping != UIDrawCall.Clipping.None && !ConstrainTargetToBounds(uipanel,obj.transform, ref mBounds,150)){
					obj.gameObject.GetComponent<BoxCollider>().enabled =true;
					int childCnt = obj.gameObject.transform.childCount;
					for(int index = 0; index<childCnt;index++){
						Transform currentChild = obj.gameObject.transform.GetChild(index);
						if(currentChild.name.Equals("LeaderBoardNameBG",StringComparison.Ordinal) || currentChild.name.Equals("ScoreLabel",StringComparison.Ordinal)){
							currentChild.gameObject.SetActive(true);	
						}
					}
					//Debug.Log("with in panel");
				}else{
					obj.gameObject.GetComponent<BoxCollider>().enabled =false;
					int childCnt = obj.gameObject.transform.childCount;
					for(int index = 0; index<childCnt;index++){
						Transform currentChild = obj.gameObject.transform.GetChild(index);
						if(currentChild.name.Equals("LeaderBoardNameBG",StringComparison.Ordinal) || currentChild.name.Equals("ScoreLabel",StringComparison.Ordinal)){
							currentChild.gameObject.SetActive(false);
						}
					}
					//Debug.Log("outside in panel");
				}
			}
		}
	}

	public virtual Vector3 CalculateConstrainOffset (UIPanel uipanel,Vector2 min, Vector2 max, int offsetRectY)
	{
		float offsetX = uipanel.clipRange.z * 0.5f;
		float offsetY = uipanel.clipRange.w * 0.5f;
		
		Vector2 minRect = new Vector2(min.x, min.y + offsetRectY);
		Vector2 maxRect = new Vector2(max.x, max.y - offsetRectY);
		Vector2 minArea = new Vector2( uipanel.clipRange.x - offsetX, uipanel.clipRange.y - offsetY);
		Vector2 maxArea = new Vector2(uipanel.clipRange.x + offsetX, uipanel.clipRange.y + offsetY);
		
		if (uipanel.clipping == UIDrawCall.Clipping.SoftClip)
		{
			
			minArea.x += uipanel.clipSoftness.x;
			minArea.y += uipanel.clipSoftness.y;
			maxArea.x -= uipanel.clipSoftness.x;
			maxArea.y -= uipanel.clipSoftness.y;
		}
		return NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea);
	}
	
	public bool ConstrainTargetToBounds (UIPanel uipanel,Transform target, ref Bounds targetBounds, int offsetRectY )
	{
		Vector3 offset = CalculateConstrainOffset(uipanel,targetBounds.min, targetBounds.max, offsetRectY );		
		if (offset.magnitude > 0f){
			return true;
		}
		return false;
	}

	private void AddSpringPanel(){
		SpringPanel springPanel = panelHolder.gameObject.GetComponent<SpringPanel>();
		if(springPanel==null){
			panelHolder.AddComponent<SpringPanel>();
			springPanel = panelHolder.gameObject.GetComponent<SpringPanel>();
		}
		
		springPanel.target.y = 5;
		springPanel.enabled =true;
	}

	private void ClearChild(){
		int childCnt = this.gameObject.transform.childCount-1;
		for( int index = childCnt; index >= 0;index--){
			Transform child =  this.gameObject.transform.GetChild(index);
			Destroy(child.gameObject);
		}

		uigrid.Reposition();
		AddSpringPanel();
		Invoke("ArrangeGrid", 0.1f);

		/*if(uigrid.gameObject.activeSelf && this.gameObject != null){
			StartCoroutine(DelayReposition(0.1f));
			Debug.Log("show me 2nd");
		}*/
	}

	private void Update(){
		ItemChecker();
		if(isLoadComplete && !isLeaderboardDataLoaded){
			isLeaderboardDataLoaded =true;
			PopulateLeaderBoard();
		}

		if(isOffline && !isOfflineMessageShown){
			isOfflineMessageShown =true;
			RequestFailed();
		}
	}

	private void ShowBestScoresFailed(){
		//scoreoidRestApiManager.OnGetBestScoreFailed-=ShowBestScoresFailed;
		//Debug.Log("ShowBestScoresFailed!");
		ShowHideStatus(true,"loading failed please try again.");
	}

	private void RequestFailed(){
		ShowHideStatus(true,"error: please check your internet connection.");
	}

	private void ArrangeGrid(){
		uigrid.Reposition();
		AddSpringPanel();
	}

	public void StopArrangeGrid(){
		CancelInvoke("ArrangeGrid");
	}

	/*
	IEnumerator DelayReposition(float sec){
		Debug.Log("show me 1st");
		yield return new WaitForSeconds(sec);
		if(uigrid.gameObject.activeSelf){
			uigrid.Reposition();
		}else{
			yield break;
		}
		Debug.Log("show me 3rd");
	}*/
}
