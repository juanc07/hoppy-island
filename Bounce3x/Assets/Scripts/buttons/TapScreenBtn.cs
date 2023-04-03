using UnityEngine;
using System.Collections;

public class TapScreenBtn : MonoBehaviour {

	// Use this for initialization
	private GameDataManagerController gdc;
	private ScreenManagerController screenManagerController;
	private GameManagerController gmc;
	private bool isClicked =false;

	// Use this for initialization
	void Start () {
		gdc = GameDataManagerController.GetInstance();		
		screenManagerController = ScreenManagerController.GetInstance();
		gmc = GameManagerController.GetInstance();
	}
	
	private void OnClick(){
		if(!isClicked){
			isClicked =true;
			gmc.UnPauseGame();
			gdc.IsGameOver =true;
			//gmc.HideHeyzapBanner();
			gdc.ResetGameData();
			gdc.ResetWarmUpData();
			screenManagerController.LoadGameByGameScreenName(ScreenType.Game);
		}
		Debug.Log("on tap screen!!");
	}
}
