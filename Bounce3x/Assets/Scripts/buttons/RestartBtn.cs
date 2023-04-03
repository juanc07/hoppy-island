using UnityEngine;
using System.Collections;

public class RestartBtn : MonoBehaviour {

	private GameManagerController gameManagerController;
	private GameDataManagerController gdc;
	private ScreenManagerController screenManagerController;
	private OptionController optionController;
	//private GameManagerController gmc;
	
	// Use this for initialization
	void Start () {
		GetGameManagerController();
		gdc = GameDataManagerController.GetInstance();		
		screenManagerController = ScreenManagerController.GetInstance();
		GetOptionController();
		//gmc = GameObject.Find("GameManager").GetComponent<GameManagerController>();
	}

	private void OnEnable(){
		GetOptionController();
		GetGameManagerController();
	}

	private void GetGameManagerController(){
		if(gameManagerController==null){
			gameManagerController = GameManagerController.GetInstance();
		}
	}

	private void GetOptionController(){
		if(optionController==null){
			optionController = GameObject.Find("OptionGUI").GetComponent<OptionController>();
		}
	}
	
	private void OnClick(){
		optionController.HideOptionWindow();
		gameManagerController.UnPauseGame();
		gameManagerController.ShowHideGameOver(false);
		//screenManagerController.ShowHideGameOver(false);
		gdc.IsGameOver =true;
		gdc.isGetSetGoDone = false;
		gdc.ResetGameData();
		gameManagerController.GameRestart();
	}
}
