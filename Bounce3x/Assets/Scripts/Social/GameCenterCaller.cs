using UnityEngine;
using System.Collections;

public class GameCenterCaller : MonoBehaviour {

	private GameCenterManager gameCenterManager;

	// Use this for initialization
	void Start () {
		gameCenterManager = GameCenterManager.GetInstance();
		AddEventListener();
		gameCenterManager.AuthenticateUser();
	}

	private void AddEventListener(){
		gameCenterManager.OnAuthenticate+=OnAuthenticate;
	}

	private void RemoveEventListener(){
		gameCenterManager.OnAuthenticate-=OnAuthenticate;
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void OnAuthenticate(bool status){
		if(status){
			Debug.Log("[game center caller]: authenticate sucess show leaderboard now!");
			gameCenterManager.ShowLeaderBoard("slappybird3d_Leaderboard_gigadrillgames01");
		}else{
			Debug.Log("[game center caller]: authenticate failed");
		}
	}



}
