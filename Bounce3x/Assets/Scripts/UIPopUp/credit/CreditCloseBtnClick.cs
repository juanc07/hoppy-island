using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class CreditCloseBtnClick : MonoBehaviour {

	public GameObject creditPanel;
	private Vector3 originalPosition;
	private bool isClick = false;

	private TitleClickBlockerController titleClickBlokcerController;
	private GameDataManagerController gdc;

	// Use this for initialization
	void Start () {
		gdc = GameDataManagerController.GetInstance();
		titleClickBlokcerController = GameObject.FindObjectOfType<TitleClickBlockerController>();
		originalPosition = creditPanel.gameObject.transform.localPosition;
	}

	/*private void TweenOutCreditPanel(){
		GoTween creditTween = new GoTween(creditPanel.gameObject.transform,0.55f,new GoTweenConfig().localPosition(originalPosition,false).setEaseType(GoEaseType.ElasticInOut).onComplete(TweenOutComplete).setUpdateType(GoUpdateType.TimeScaleIndependentUpdate));
		GoTweenChain creditTweenChain = new GoTweenChain();
		creditTweenChain.append(creditTween);
		creditTweenChain.autoRemoveOnComplete = true; 
		creditTweenChain.play();
	}*/

	private void TweenOutCreditPanel(){
		TweenParms parms = new TweenParms();
		parms.Prop("localPosition",originalPosition);
		parms.Ease(EaseType.EaseInBack);
		parms.UpdateType(UpdateType.TimeScaleIndependentUpdate);
		parms.OnComplete(TweenOutComplete);
		parms.AutoKill(true);
		HOTween.To(creditPanel.gameObject.transform, 0.25f, parms );
	}

	//private void TweenOutComplete(AbstractGoTween abstractGoTween){
	private void TweenOutComplete(){
		gdc.IsCreditEnable = false;
		isClick = false;
		titleClickBlokcerController.EnableDisableTitleClickBlocker(false);
		creditPanel.gameObject.SetActive(false);
	}

	private void OnClick(){
		if(!isClick){
			isClick = true;
			TweenOutCreditPanel();
		}
	}
}
