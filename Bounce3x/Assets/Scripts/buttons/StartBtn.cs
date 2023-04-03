using UnityEngine;
using System.Collections;

public class StartBtn : MonoBehaviour {

	// Use this for initialization
	private GameDataManagerController gdc;
	private ScreenManagerController screenManagerController;
	private GameManagerController gmc;
	private bool isClicked =false;

	private BoxCollider myCollider;
	
	void Start () {
		myCollider = this.gameObject.GetComponent<BoxCollider>();
		myCollider.enabled = true;

		gdc = GameDataManagerController.GetInstance();		
		screenManagerController = ScreenManagerController.GetInstance();
		gmc = GameManagerController.GetInstance();
		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
		//Debug.Log("destroy start btn");
	}

	private void AddEventListener(){
		gdc.OnShowOption+=OnShowOption;
		gdc.OnShowCredit+=OnShowOption;

		gdc.OnHideOption+=OnHideOption;
		gdc.OnHideCredit+=OnHideOption;
	}

	private void RemoveEventListener(){
		gdc.OnShowOption-=OnShowOption;
		gdc.OnShowCredit-=OnShowOption;

		gdc.OnHideOption-=OnHideOption;
		gdc.OnHideCredit-=OnHideOption;
	}

	private void OnShowOption(){
		//Debug.Log("start btn on ShowOption");
		myCollider.enabled = false;
	}

	private void OnHideOption(){
		myCollider.enabled = true;
	}
	
	private void OnClick(){
		if(!isClicked && !gdc.IsOptionEnable && !gdc.IsCreditEnable){
			isClicked =true;
			gmc.UnPauseGame();
			gdc.IsGameOver =true;
			//gmc.HideHeyzapBanner();
			gdc.ResetGameData();
			gdc.ResetWarmUpData();
			screenManagerController.LoadGameByGameScreenName(ScreenType.Game);
		}		
	}
}
