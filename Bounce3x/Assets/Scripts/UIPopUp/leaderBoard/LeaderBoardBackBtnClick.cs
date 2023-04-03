using UnityEngine;
using System.Collections;

public class LeaderBoardBackBtnClick : MonoBehaviour {

	public GameObject uiLeaderBoardPanel;
	public LeaderBoardItemController leaderboarditemController;

	// Use this for initialization
	void Start () {
	
	}
	
	private void OnClick(){
		leaderboarditemController.StopArrangeGrid();
		uiLeaderBoardPanel.gameObject.SetActive(false);	
	}
}
