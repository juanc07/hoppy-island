using UnityEngine;
using System.Collections;

public class TitleShopBtn : MonoBehaviour {

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
		screenManagerController.LoadGameByGameScreenName(ScreenType.Shop);
	}
}
