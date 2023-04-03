using UnityEngine;
using System.Collections;
using System;

public class ScreenManagerController : MonoBehaviour {
	
	private GameObject preloaderPanel;
	private Transform preloaderLabel;
	private UILabel preloaderUILabel;

	private GameManagerController gameManagerController;
	private static GameObject container;
	private static ScreenManagerController instance;

	private string levelToLoad;
	public string LevelToLoad{
		get{return levelToLoad;}
	}
	
	private Action LoadScreen;
	public event Action OnLoadScreen{
		add{LoadScreen+=value;}
		remove{LoadScreen-=value;}
	}

	public static ScreenManagerController GetInstance(){
		if(instance == null){
			container = new GameObject();
			container.name = "ScreenManagerController";
			instance = container.AddComponent(typeof(ScreenManagerController)) as ScreenManagerController;
			DontDestroyOnLoad(instance.gameObject);
		}

		return instance;
	}
	
	// Use this for initialization
	void Start (){
		GetGameManager();
	}

	private void GetGameManager(){
		if(gameManagerController==null){
			gameManagerController = GameManagerController.GetInstance();
		}
	}
	
	public void LoadGameByGameScreenName( ScreenType screenType ){
		levelToLoad = screenType.ToString();
		Application.LoadLevel(ScreenType.Preloader.ToString());
		
		/*preloaderPanel = GameObject.Find("PreloaderPanel");			
		preloaderLabel = preloaderPanel.transform.Find("PreloaderImageLabel");
		preloaderLabel.gameObject.SetActive(true);*/

		if(null!=LoadScreen){
			LoadScreen();
		}
	}
}
