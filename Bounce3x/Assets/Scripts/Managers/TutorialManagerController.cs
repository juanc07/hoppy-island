using UnityEngine;
using System.Collections;

public class TutorialManagerController : MonoBehaviour {
	
	//private GameManagerController gmc;	
	private GameDataManagerController gdc;
	private GameObject tutorialCenterAnchor;
	private Transform centerPanel;
	private Transform hand1;
	private Transform hand2;
	private Transform arrow;
	//private Transform circle;
	private Transform saveAnimal;
	private Transform tap;
	private Transform tapArrowLeft;
	private Transform tapArrowRight;

	private GameManagerController gameManagerController;
	// Use this for initialization
	void Start (){
		GetGameManagerController();
		gdc = GameDataManagerController.GetInstance();
		//Messenger.AddListener(GameEvent.FirstTap, OnFirstTap);
		AddEventListener();
	}

	private void OnEnable(){
		GetGameManagerController();
	}
	
	private void OnDestroy(){
		RemoveEventListener();
	}
	
	private void AddEventListener(){
		Messenger.AddListener(GameEvent.FirstTap, OnFirstTap);
		Messenger.AddListener(GameEvent.FirstAnimal, OnFirstAnimal);
		Messenger.AddListener(GameEvent.EndTutorial, OnEndTutorial);
		Messenger.AddListener(GameEvent.Level_Failed, OnLevelFailed);
		//Messenger.AddListener(GameEvent.Level_Restart, OnLevelRestart);		
		Messenger.AddListener(GameEvent.GetSetGoComplete, OnGetSetGoComplete);	
	}
	
	private void RemoveEventListener(){
		if(gameManagerController!=null){
			gameManagerController.OnLevelRestart-=OnLevelRestart;
		}
	}
	
	private void GetGameManagerController(){
		if(gameManagerController==null){
			gameManagerController = GameManagerController.GetInstance();
			gameManagerController.OnLevelRestart+=OnLevelRestart;
		}
	}
	
	private void OnGetSetGoComplete(){
		InitTutorial();
		CheckTutorial();
	}
	
	private void InitTutorial(){
		tutorialCenterAnchor = GameObject.Find("TutorialGUI/Camera/TutorialCenterAnchor");
		centerPanel = tutorialCenterAnchor.gameObject.transform.Find("CenterPanel");
		
		hand1 = centerPanel.gameObject.transform.Find("TutorialHand1");
		hand2 = centerPanel.gameObject.transform.Find("TutorialHand2");
		arrow = centerPanel.gameObject.transform.Find("TutorialArrow");
		//circle = centerPanel.gameObject.transform.Find("TutorialCircle");
		saveAnimal = centerPanel.gameObject.transform.Find("TutorialSaveAnimal");
		tap = centerPanel.gameObject.transform.Find("TutorialTap");
		tapArrowLeft = centerPanel.gameObject.transform.Find("TapArrowLeft");
		tapArrowRight = centerPanel.gameObject.transform.Find("TapArrowRight");		
		
		hand1.gameObject.SetActive(false);
		hand2.gameObject.SetActive(false);
		tapArrowRight.gameObject.SetActive(false);
		tapArrowLeft.gameObject.SetActive(false);
	}
	
	private void OnLevelRestart(){
		if(this == null || !gdc.isGetSetGoDone )return;
		//Debug.Log("OnLevelRestart tutorial gdc.HasTutorial " + gdc.HasTutorial);
		gdc.TapCount = 0;
		gdc.HasFirstAnimal =false;
		CheckTutorial();
	}
	
	private void OnLevelFailed(){
		if(this == null || !gdc.isGetSetGoDone )return;
		//Debug.Log("level failed tut");
		hand2.gameObject.SetActive(false);
		tapArrowRight.gameObject.SetActive(false);
		centerPanel.gameObject.SetActive(false);
		gdc.TapCount = 0;
		gdc.HasFirstAnimal =false;
		//centerPanel.gameObject.SetActive(true);
	}
	
	private void OnEndTutorial(){
		if(this == null || !gdc.isGetSetGoDone )return;
		hand2.gameObject.SetActive(false);
		tap.gameObject.SetActive(false);
		tapArrowRight.gameObject.SetActive(false);
	}
	
	private void OnFirstAnimal(){
		if(this == null || !gdc.isGetSetGoDone )return;
		gdc.TapCount = 0;
		saveAnimal.gameObject.SetActive(true);
		arrow.gameObject.SetActive(true);
		//circle.gameObject.SetActive(true);
		
		tapArrowLeft.gameObject.SetActive(true);
		hand1.gameObject.SetActive(true);		
		
		hand2.gameObject.SetActive(false);
		tapArrowRight.gameObject.SetActive(false);
		gdc.HasFirstAnimal =true;
	}
	
	private void OnFirstTap(){
		if(this == null || !gdc.isGetSetGoDone )return;
		saveAnimal.gameObject.SetActive(false);
		arrow.gameObject.SetActive(false);
		//circle.gameObject.SetActive(false);
		
		hand1.gameObject.SetActive(false);
		hand2.gameObject.SetActive(true);
		
		tapArrowLeft.gameObject.SetActive(false);
		tapArrowRight.gameObject.SetActive(true);
	}
	
	private void CheckTutorial(){
		if(this == null || !gdc.isGetSetGoDone )return;
		if(gdc.HasTutorial == 0){
			//gmc.PauseGame();
			centerPanel.gameObject.SetActive(true);
			//start tutorial
		}
	}	
}
