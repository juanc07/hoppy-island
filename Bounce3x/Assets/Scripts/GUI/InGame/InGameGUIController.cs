using UnityEngine;
using System.Collections;

public class InGameGUIController : MonoBehaviour {

	private SoundManager soundManager;
	private GameManagerController gameManagerController;
	private ScreenManagerController screenManagerController;

	// Use this for initialization
	void Start () {
		gameManagerController = GameManagerController.GetInstance();
		screenManagerController = ScreenManagerController.GetInstance();
		soundManager = SoundManager.GetInstance();
		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}
	
	private void AddEventListener(){
		gameManagerController.OnPausedGame+=OnPausedGame;
		gameManagerController.OnUnPausedGame+=OnUnPausedGame;
		gameManagerController.OnLevelRestart+=OnLevelRestart;
		gameManagerController.OnLeveFailed+=OnLeveFailed;
		
		screenManagerController.OnLoadScreen+=OnLoadScreen;
	}
	
	private void RemoveEventListener(){
		gameManagerController.OnPausedGame-=OnPausedGame;
		gameManagerController.OnUnPausedGame-=OnUnPausedGame;
		gameManagerController.OnLevelRestart-=OnLevelRestart;
		gameManagerController.OnLeveFailed-=OnLeveFailed;
		
		screenManagerController.OnLoadScreen-=OnLoadScreen;
	}
	
	private void OnLoadScreen(){
		StopBlinkerSfx();
	}
	
	private void OnLeveFailed(){
		StopBlinkerSfx();
	}
	
	private void OnLevelRestart(){
		//Debug.Log("level restart in game gui Controller");
		StopBlinkerSfx();
	}
	
	private void OnPausedGame(){
		StopBlinkerSfx();
	}
	
	private void OnUnPausedGame(){
	}
	
	private void StopBlinkerSfx(){
		if(soundManager != null){
			//Debug.Log("stop blinker in game gui");
			soundManager.StopSfx(SFX.PowerUpTimerEnding);
		}
	}
}
