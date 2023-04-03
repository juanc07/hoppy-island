using UnityEngine;
using System.Collections;

public class ShopBtn : MonoBehaviour {

	// Use this for initialization
	private GameDataManagerController gdc;
	private ScreenManagerController screenManagerController;
	private GameManagerController gmc;
	
	void Start () {
		gdc = GameDataManagerController.GetInstance();		
		screenManagerController = ScreenManagerController.GetInstance();
		gmc = GameManagerController.GetInstance();
	}
	
	private void OnClick(){
		gmc.UnPauseGame();
		//gmc.HideHeyzapBanner();
		gdc.ResetGameData();
		gdc.ResetWarmUpData();
		screenManagerController.LoadGameByGameScreenName(ScreenType.Shop);
	}
}
