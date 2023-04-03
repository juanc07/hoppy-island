using UnityEngine;
using System.Collections;

public class BackToMainBtn : MonoBehaviour {
	
	private GameDataManagerController gdc;
	private ScreenManagerController screenManagerController;
	private GameManagerController gmc;
	
	// Use this for initialization
	void Start () {
		gdc = GameDataManagerController.GetInstance();		
		screenManagerController = ScreenManagerController.GetInstance();
		gmc = GameManagerController.GetInstance();
	}
	
	private void OnClick(){
		gdc.IsOptionEnable = false;
		gmc.UnPauseGame();		
		gdc.ClearMoveQeue();
		gdc.ResetGameData();
		gdc.ResetWarmUpData();		
		screenManagerController.LoadGameByGameScreenName(ScreenType.Title);
	}
}
