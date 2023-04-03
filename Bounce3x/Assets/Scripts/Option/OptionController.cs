using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System;

public class OptionController : MonoBehaviour {
	
	private GameDataManagerController gdc;	
	private Transform optionPanel;	
	private GameObject opttionGUIAnchor;
	
	private float optionOffsetY = 0.05f;
	private float optionOffsetZ = -1f;
	private bool isOptionAnimating =false;

	private GameManagerController gameManagerController;

	private Action ShowOptionComplete;
	public event Action OnShowOptionComplete{
		add{ShowOptionComplete+=value;}
		remove{ShowOptionComplete-=value;}
	}

	private Action HideOptionComplete;
	public event Action OnHideOptionComplete{
		add{HideOptionComplete+=value;}
		remove{HideOptionComplete-=value;}
	}

	// Use this for initialization
	void Start () {
		GetGameManagerController();
		gdc = GameDataManagerController.GetInstance();
		
		opttionGUIAnchor = GameObject.Find("OptionGUI/Camera/Anchor");
		optionPanel = opttionGUIAnchor.transform.Find("OptionPanel");
		optionPanel.gameObject.transform.position =  new Vector3(-5,optionOffsetY,optionOffsetZ);		
		
		//Messenger.AddListener(GameEvent.Level_Restart, OnRestartLevel);
		AddEventListener();
	}

	private void OnEnable(){
		GetGameManagerController();
	}
	
	private void OnDestroy(){
		RemoveEventListener();
	}
	
	private void AddEventListener(){
		
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

	private void OnLevelRestart(){
		if(this == null)return;
		HideOptionWindow();
	}
	
	/*private void OnRestartLevel(){
		if(this == null)return;
		HideOptionWindow();
	}*/
	
	public bool ShowOptionWindow(){
		if(!gdc.IsOption && !isOptionAnimating){
			isOptionAnimating =true;
			gdc.IsFirstLaunch =true;
			gdc.IsOption =true;
			gdc.IsOptionEnable = true;
			optionPanel.gameObject.SetActive(true);
			TweenParms parms = new TweenParms(); 		
			parms.Prop("position", new Vector3(0,optionOffsetY,optionOffsetZ));		
			parms.Ease(EaseType.EaseOutElastic);
			parms.UpdateType(UpdateType.TimeScaleIndependentUpdate);
			//parms.Delay(0.4f);
			parms.OnComplete(OnDoneAnimatingShopWindow);
			//HOTween.To(optionPanel.gameObject.transform, 1.5f, parms );
			HOTween.To(optionPanel.gameObject.transform, 0.75f, parms );
			//PauseGame();
			
			string sceneName = Application.loadedLevelName;
			
			if(sceneName ==  GameScreen.TITLE  ){
				Transform backToMainBtn = optionPanel.Find("BackToMainBtn");
				backToMainBtn.gameObject.SetActive(false);
				
				Transform restartBtn = optionPanel.Find("RestartBtn");
				restartBtn.gameObject.SetActive(false);
				
				Transform optionHeader = optionPanel.Find("OptionHeader");
				optionHeader.gameObject.SetActive(true);
				
				Transform pauseHeader = optionPanel.Find("PauseHeader");
				pauseHeader.gameObject.SetActive(false);
				
				Transform closeBtn = optionPanel.Find("CloseBtn");
				closeBtn.gameObject.SetActive(true);
				
				Transform resumetBtn = optionPanel.Find("ResumetBtn");
				resumetBtn.gameObject.SetActive(false);
			}else if(sceneName ==  GameScreen.GAME){
				gameManagerController.PauseGame();

				Transform optionHeader = optionPanel.Find("OptionHeader");
				optionHeader.gameObject.SetActive(false);
				
				Transform pauseHeader = optionPanel.Find("PauseHeader");
				pauseHeader.gameObject.SetActive(true);
				
				Transform closeBtn = optionPanel.Find("CloseBtn");
				closeBtn.gameObject.SetActive(false);
				
				Transform resumetBtn = optionPanel.Find("ResumetBtn");
				resumetBtn.gameObject.SetActive(true);
			}


			return true;
			//Debug.Log( "show option!! gdc.IsOption " +gdc.IsOption  );
		}else{
			return false;
			//HideOptionWindow();
		}
	}
	
	
	public void HideOptionWindow(){
		if(gdc!=null){
			if(gdc.IsOption && !isOptionAnimating){
				isOptionAnimating =true;
				//gameManagerController.UnPauseGame();
				//gameManagerController.GameResume();
				TweenParms parms = new TweenParms(); 		
				parms.Prop("position", new Vector3(-3,0,0));
				parms.Ease(EaseType.EaseInBack);
				parms.UpdateType(UpdateType.TimeScaleIndependentUpdate);
				parms.OnComplete(HideOptionTweenComplete);
				HOTween.To(optionPanel.gameObject.transform, 0.25f, parms );
			}
		}
	}
	
	private void OnDoneAnimatingShopWindow(){
		isOptionAnimating = false;
		if(null!= ShowOptionComplete){
			ShowOptionComplete();
		}
	}
	
	private void HideOptionTweenComplete(){
		gdc.IsOptionEnable = false;
		gdc.IsOption =false;
		isOptionAnimating = false;

		if(null!= HideOptionComplete){
			HideOptionComplete();
		}
	}
}
