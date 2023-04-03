using UnityEngine;
using System.Collections;
using System;

public class GameManagerController : MonoBehaviour {

	private static GameObject container;
	private static GameManagerController instance;

	private Action LevelRestart;
	public event Action OnLevelRestart{
		add{LevelRestart+=value;}
		remove{LevelRestart-=value;}
	}

	private Action LeveFailed;
	public event Action OnLeveFailed{
		add{LeveFailed+=value;}
		remove{LeveFailed-=value;}
	}

	private Action PausedGame;
	public event Action OnPausedGame{
		add{PausedGame+=value;}
		remove{PausedGame-=value;}
	}

	private Action UnPausedGame;
	public event Action OnUnPausedGame{
		add{UnPausedGame+=value;}
		remove{UnPausedGame-=value;}
	}

	private Action ResumeGame;
	public event Action OnResumeGame{
		add{ResumeGame+=value;}
		remove{ResumeGame-=value;}
	}

	private Action PostGameRestart;
	public event Action OnPostGameRestart{
		add{PostGameRestart+=value;}
		remove{PostGameRestart-=value;}
	}

	private Action PreGameRestart;
	public event Action OnPreGameRestart{
		add{PreGameRestart+=value;}
		remove{PreGameRestart-=value;}
	}

	public static GameManagerController GetInstance(){
		if(instance == null){
			container = new GameObject();
			container.name = "GameManagerController";
			instance = container.AddComponent(typeof(GameManagerController)) as GameManagerController;
			DontDestroyOnLoad(instance.gameObject);
		}

		return instance;
	}

	//managers
	private GameDataManagerController gdc;
	private SoundManager soundManager;

	void Start () {
		gdc = GameDataManagerController.GetInstance();
		soundManager = SoundManager.GetInstance();
		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener(){
		soundManager.OnSoundManagerReady+=OnSoundManagerReady;
	}

	private void RemoveEventListener(){
		soundManager.OnSoundManagerReady-=OnSoundManagerReady;
	}

	private void OnSoundManagerReady(){
		if(gdc.IsMusicOn){
			soundManager.PlayBGM(BGM.InGameBGM,1f,true);
		}else{
			soundManager.MuteBGM();
		}
	}

	public void PauseGame(){
		if(Time.timeScale ==1 ){
			Time.timeScale = 0;
			gdc.IsPaused = true;
			if(null!=PausedGame){
				PausedGame();
			}
		}		
	}
	
	public void UnPauseGame(){
		if(Time.timeScale ==0 ){
			gdc.IsOption =false;
			Time.timeScale =1;
			gdc.IsPaused = false;

			if(null!=UnPausedGame){
				UnPausedGame();
			}
		}
	}
	
	public void GameRestart(){
		if(null!=LevelRestart){
			LevelRestart();
		}
	}

	public void GameFailed(){
		if(null!=LeveFailed){
			LeveFailed();
		}
	}

	public void GamePreRestart(){
		if(null!=PreGameRestart){
			PreGameRestart();
		}

	}

	public void GamePostRestart(){
		if(null!=PostGameRestart){
			PostGameRestart();
		}		
	}

	public void GameResume(){
		if(null!=ResumeGame){
			ResumeGame();
		}		
	}

	public void ShowHideGameOver(bool val){
		GameObject inGameGUI = GameObject.Find("InGameGUI/Camera/AnchorCenter");
		Transform gameOverPanel = inGameGUI.gameObject.transform.Find("GameOverPanel");
		gameOverPanel.gameObject.SetActive(val);
	}
}
