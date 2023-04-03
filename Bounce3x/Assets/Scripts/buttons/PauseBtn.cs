using UnityEngine;
using System.Collections;

public class PauseBtn : MonoBehaviour {
	
	private OptionController optionController;
	private GameDataManagerController gdc;
	public GameObject optionGUI;
	private GameManagerController gameManagerController;

	private UIButton pauseButton;
	private int hasTutorialComplete;

	// Use this for initialization
	void Start () {
		pauseButton = this.gameObject.GetComponent<UIButton>();
		gdc = GameDataManagerController.GetInstance();
		gameManagerController = GameManagerController.GetInstance();

		//hasTutorialComplete = SaveDataManager.LoadIntSaveData(PlayerDataKey.TUTORIAL.ToString());
		hasTutorialComplete = gdc.HasTutorial;


		if(optionGUI!=null){
			optionGUI.SetActive(true);
			optionController = optionGUI.gameObject.GetComponent<OptionController>();
			if(optionController!=null){
				optionController.HideOptionWindow();
			}else{
				Debug.Log("optionController is null!");
			}
		}
		EnableDisablePauseButton(false);
		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener(){
		gdc.OnTutorialComplete+=OnTutorialComplete;

		gameManagerController.OnPausedGame+=OnPausedGame;
		gameManagerController.OnUnPausedGame+=OnUnPausedGame;
		gameManagerController.OnPostGameRestart+=OnPostGameRestart;
		gameManagerController.OnPreGameRestart+=OnPreGameRestart;
		gameManagerController.OnLeveFailed+=OnLeveFailed;
		gameManagerController.OnLevelRestart+=OnLevelRestart;
	}

	private void RemoveEventListener(){
		gdc.OnTutorialComplete-=OnTutorialComplete;

		gameManagerController.OnPausedGame-=OnPausedGame;
		gameManagerController.OnUnPausedGame-=OnUnPausedGame;
		gameManagerController.OnLeveFailed-=OnLeveFailed;
		gameManagerController.OnLevelRestart-=OnLevelRestart;
		gameManagerController.OnPostGameRestart-=OnPostGameRestart;
		gameManagerController.OnPreGameRestart-=OnPreGameRestart;
	}

	private void OnTutorialComplete(){
		hasTutorialComplete = 1;
		EnableDisablePauseButton(true);
	}
	
	private void OnClick(){
		if(!gdc.IsGameOver && !gdc.IsPaused && !gdc.IsGameOver && gdc.isGetSetGoDone && hasTutorialComplete == 1){
			optionController.ShowOptionWindow();	
		}		
	}

	private void EnableDisablePauseButton(bool val){
		pauseButton.isEnabled = val;
	}

	private void OnPostGameRestart(){
		EnableDisablePauseButton(false);
	}

	private void OnPreGameRestart(){
		if(hasTutorialComplete==1){
			EnableDisablePauseButton(true);
			//Debug.Log("pre game restart pause button! enable button hasTutorialComplete " + hasTutorialComplete );
		}else{
			//Debug.Log("pre game restart pause button! disable button hasTutorialComplete " + hasTutorialComplete );
		}
	}

	private void OnPausedGame(){
		EnableDisablePauseButton(false);
	}

	private void OnUnPausedGame(){
		if(hasTutorialComplete==1){
			EnableDisablePauseButton(true);
		}
	}

	private void OnLeveFailed(){
		EnableDisablePauseButton(false);
	}

	private void OnLevelRestart(){
		EnableDisablePauseButton(false);
		//pauseButton.isEnabled = true;
	}
}
