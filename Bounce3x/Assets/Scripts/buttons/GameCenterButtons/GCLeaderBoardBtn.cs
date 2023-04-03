using UnityEngine;
using System.Collections;

public class GCLeaderBoardBtn : MonoBehaviour {

	private GameCenterManager gameCenterManager;

	// Use this for initialization
	void Start () {
		gameCenterManager = GameCenterManager.GetInstance();
		AddEventListener();
		gameCenterManager.AuthenticateUser();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener(){

	}

	private void RemoveEventListener(){

	}
	
	private void OnClick(){
		if(gameCenterManager!=null){
			if(gameCenterManager.isAuthenticated){
				gameCenterManager.ShowLeaderBoard("slappybird3d_Leaderboard_gigadrillgames01");
			}
		}
	}
}
