using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class CreditBtn : MonoBehaviour {

	public GameObject creditPanel;
	//private Vector3 originalPosition;

	private GoTween creditTween;
	private GoTweenChain creditTweenChain;

	private bool isClick = false;

	private UIButton creditButton;
	private TitleClickBlockerController titleClickBlokcerController;
	private GameDataManagerController gdc;

	// Use this for initialization
	void Start () {
		gdc = GameDataManagerController.GetInstance();
		titleClickBlokcerController = GameObject.FindObjectOfType<TitleClickBlockerController>();
		creditButton = this.gameObject.GetComponent<UIButton>();
		//originalPosition = creditPanel.gameObject.transform.localPosition;
	}

	/*private void TweenCreditPanel(){
		creditTween = new GoTween(creditPanel.gameObject.transform,0.55f,new GoTweenConfig().localPosition(new Vector3(0,0,0),false).setEaseType(GoEaseType.ElasticInOut).setUpdateType(GoUpdateType.TimeScaleIndependentUpdate).onComplete(OnCompleteTween));
		creditTweenChain = new GoTweenChain();
		creditTweenChain.append(creditTween);
		creditTweenChain.autoRemoveOnComplete = true; 
		creditTweenChain.play();
	}*/

	private void TweenCreditPanel(){
		TweenParms parms = new TweenParms(); 		
		//originalPosition.x = 0;
		//originalPosition.y = 0;
		parms.Prop("localPosition",new Vector3(0,0,0));		
		parms.Ease(EaseType.EaseOutElastic);
		parms.UpdateType(UpdateType.TimeScaleIndependentUpdate);
		parms.OnComplete(OnCompleteTween);
		parms.AutoKill(true);
		HOTween.To(creditPanel.gameObject.transform, 0.75f, parms );
	}

	//private void OnCompleteTween(AbstractGoTween abstractGoTween){
	private void OnCompleteTween(){
		isClick = false;
	}

	private void ActivateDeactivateButton(bool val){
		creditButton.isEnabled = val;
	}
	
	private void OnClick(){
		if(!isClick){
			isClick = true;
			gdc.IsCreditEnable = true;
			creditPanel.gameObject.SetActive(true);
			titleClickBlokcerController.EnableDisableTitleClickBlocker(true);
			TweenCreditPanel();
			//Debug.Log("show credit here");
		}
	}
}
