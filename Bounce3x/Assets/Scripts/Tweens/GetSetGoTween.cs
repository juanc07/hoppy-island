using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class GetSetGoTween : MonoBehaviour {
	
	private Transform getSet;
	private Transform go;
	private GameObject helperPanel;
	private GameDataManagerController gdc;
	private GameManagerController gameManagerController;

	private GoTween getSetScaleTween;
	private GoTween goScaleTween;
	private GoTweenChain getSetGoTweenChain;
	
	// Use this for initialization
	void Start () {
		GetGameManagerController();

		gdc = GameDataManagerController.GetInstance();
		helperPanel = GameObject.Find("HelperPanel");
		getSet = helperPanel.transform.FindChild("GetSet");
		go = helperPanel.transform.FindChild("Go");
		ShowHideGetsetAndGo(true);
		
		getSet.transform.localScale = new Vector3(0.01f,0.01f,0.01f);
		go.transform.localScale = new Vector3(0.01f,0.01f,0.01f);


		AnimateGetset();
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
		if(this == null ) return;
		ShowHideGetsetAndGo(true);
		AnimateGetset();
	}
	
	private void AnimateGetset(){
		gameManagerController.GamePostRestart();

		getSetScaleTween = new GoTween(getSet,0.85f,new GoTweenConfig().scale(1f,false).setEaseType(GoEaseType.ElasticInOut).setUpdateType(GoUpdateType.TimeScaleIndependentUpdate).onComplete(OnGetSetTweenComplete));
		goScaleTween = new GoTween(go,0.85f,new GoTweenConfig().scale(1f,false).setEaseType(GoEaseType.ElasticInOut).setUpdateType(GoUpdateType.TimeScaleIndependentUpdate).onComplete(OnGetSetGoTweenComplete));

		getSetGoTweenChain = new GoTweenChain();
		getSetGoTweenChain.append(getSetScaleTween).appendDelay(0.1f).append(goScaleTween);
		getSetGoTweenChain.autoRemoveOnComplete = true;
		getSetGoTweenChain.play();
	}

	private void OnGetSetTweenComplete(AbstractGoTween abstractGoTween){
		getSet.transform.localScale = new Vector3(0.01f,0.01f,0.01f);
	}

	private void OnGetSetGoTweenComplete(AbstractGoTween abstractGoTween){
		ShowHideGetsetAndGo(false);
		StartCoroutine(ShowTutorial(0.75f));
	}

	private void ShowHideGetsetAndGo(bool val){
		getSet.gameObject.SetActive(val);
		go.gameObject.SetActive(val);
	}
	
	private IEnumerator ShowTutorial(float delay){
		yield return new WaitForSeconds(delay);
		go.transform.localScale = new Vector3(0.01f,0.01f,0.01f);		
		gdc.isGetSetGoDone = true;
		Messenger.Broadcast(GameEvent.GetSetGoComplete);
		gameManagerController.GamePreRestart();
	}		
}
