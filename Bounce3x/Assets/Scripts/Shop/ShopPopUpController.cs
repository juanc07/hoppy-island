using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class ShopPopUpController : MonoBehaviour {
	
	private Transform shopPopUpPanel;
	private bool isAnimating =false;
	
	// Use this for initialization
	void Start () {
		shopPopUpPanel = gameObject.transform.Find("ShopPopUpPanel");
		shopPopUpPanel.gameObject.transform.localScale =  new Vector3(0.01f,0.01f,0.01f);
		shopPopUpPanel.gameObject.SetActive(false);
		//UISprite bg = shopPopUpPanel.gameObject.transform.Find("ShopPopUpBg").GetComponent<UISprite>();		
	}
	
	public void ShowHideShopPopUpPanel(bool val){
		if(val){
			TweenIn();			
		}else{
			TweenOut();
		}		
	}
	
	private void TweenOut(){
		if(!isAnimating){
			isAnimating =true;
			Messenger.Broadcast(GameEvent.DisableBuying);
			TweenParms parms = new TweenParms();
			parms.Prop("localScale", new Vector3(0.01f,0.01f,0.01f));			
			parms.Ease(EaseType.EaseInElastic); 
			parms.UpdateType(UpdateType.TimeScaleIndependentUpdate);
			//parms.Delay(0.4f);
			parms.AutoKill(true);
			parms.OnComplete(onCompleteTweenOut);		
			HOTween.To(shopPopUpPanel.gameObject.transform, 0.5f, parms );
		}
		
		//alpha
		//Color color = bg.color;
		//color.a = 0f;
		//bg.color =color;
	}
	
	private void TweenIn(){
		if(!isAnimating){
			isAnimating =true;
			Messenger.Broadcast(GameEvent.DisableBuying);
			shopPopUpPanel.gameObject.SetActive(true);
			TweenParms parms = new TweenParms();
			parms.Prop("localScale", new Vector3(1.746366f,1.746366f,1.746366f));
			parms.Ease(EaseType.EaseOutElastic); 
			parms.Delay(0.4f);
			parms.UpdateType(UpdateType.TimeScaleIndependentUpdate);
			//parms.AutoKill(true);
			parms.OnComplete(onCompleteTweenIn);		
			HOTween.To(shopPopUpPanel.gameObject.transform, 0.75f, parms );
		}
	}
	
	private void onCompleteTweenIn(){
		HOTween.Kill();
		isAnimating =false;
		Messenger.Broadcast(GameEvent.EnableBuying);
	}
	
	private void onCompleteTweenOut(){
		shopPopUpPanel.gameObject.SetActive(false);
		HOTween.Kill();
		isAnimating =false;		
	}
}
