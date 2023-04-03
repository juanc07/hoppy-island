using Holoville.HOTween;
using UnityEngine;
using System.Collections;

public class PowerupLabelTween: MonoBehaviour {
	
	//private Vector3 originalPosition;
	private Vector3 targetPosition;
	private UILabel label;
	
	private Camera mainCamera;
	private Camera NGUICamera;
	private GameObject target;
	private Vector3 pos;
	
	
	private GameObject inGamePanel;
	private UILabel comboLabel;
	
	private GameDataManagerController gdc;
	private TweenAlpha tweenAlpha;	
	// Use this for initialization	
	
	void Awake(){
		mainCamera = GameObject.Find("Main Camera").camera;
		NGUICamera = GameObject.Find("InGameGUI/Camera").camera;	
		target = GameObject.Find("Whale/TextTarget");	
	}
	
	void Start (){
		gdc = GameDataManagerController.GetInstance();
		tweenAlpha = this.GetComponent<TweenAlpha>();
		label = this.GetComponent<UILabel>();
		label.text ="xxxxx";		
		//label.transform.localScale =  new Vector3( 35f,35f,35f );
		label.transform.localScale =  new Vector3( 1f,1f,1f );
		
		ShowPowerupLabel();
		
		//Debug.Log("warmup PowerupLabelTween check !! " + gdc.IsPowerLabelWarmUp);		
		if(!gdc.IsPowerLabelWarmUp){
			gdc.IsPowerLabelWarmUp = true;
			targetPosition = new Vector3( -83.1041f,196.0544f,0f );
			//Debug.Log("warmup!!");
		}else{
			FollowTarget();			
			//Debug.Log("warmup go!!");
		}
		
		tweenAlpha.callWhenFinished = "TweenComplete";
		TweenStart();		
	}
	
	
	private void ShowPowerupLabel(){
		PowerUpChecker.Powerups powerupType = gdc.currentPowerup;
		//Debug.Log( "currentPowerup  " + powerupType);
		
		switch(powerupType){			
			case PowerUpChecker.Powerups.AutoPilot:
				label.text ="AutoPilot";				
			break;
			
			case PowerUpChecker.Powerups.ClearAll:
				label.text ="ClearAll";				
			break;
			
			case PowerUpChecker.Powerups.Duplicate:
				label.text ="Help";				
			break;

			case PowerUpChecker.Powerups.Life:
				label.text ="Life +1";				
			break;
			
			case PowerUpChecker.Powerups.Overgrowth:
				label.text ="Overgrowth";				
			break;			
			
			case PowerUpChecker.Powerups.Slow:
				label.text ="Slow";				
			break;
			
			
			case PowerUpChecker.Powerups.AdvanceLevel:
				label.text ="Level +1";
			break;
			
			default:
				label.text ="Nice Catch!";
			break;
		}	
	}	
	
	private void TweenStart(){		
		Sequence mySequence = new Sequence();
		
		TweenParms parms = new TweenParms();
		//Debug.Log(" originalPosition " + originalPosition + " targetPosition " + targetPosition);
		//parms.Prop("position", targetPosition);
		parms.Prop("localPosition", targetPosition);
		parms.Ease(EaseType.EaseInOutQuad); 
		parms.Delay(0f);	
		
		TweenParms parms2 = new TweenParms(); 				
		parms2.Prop("alpha", 0);
		parms2.Delay(0f);
		parms2.Ease(EaseType.EaseInBounce);
		parms2.AutoKill(true);
		parms2.OnComplete( TweenComplete );		
		
		mySequence.Append(HOTween.To(this.transform, 1f, parms ));
		mySequence.Append(HOTween.To(label,1f, parms2 ));
		mySequence.Play();
	}
	
	
	private void FollowTarget(){	
		mainCamera = NGUITools.FindCameraForLayer(target.layer);
		NGUICamera = NGUITools.FindCameraForLayer(gameObject.layer);
		
		pos = mainCamera.WorldToViewportPoint(target.transform.position);
		pos = NGUICamera.ViewportToWorldPoint(pos);		
		
		pos.z = 0f;		
		transform.position = pos;		
		
		//originalPosition = transform.position;
		targetPosition = transform.position;
		targetPosition.x -= 90;
		targetPosition.y += 100;
	}
	
	private void Reset(){
		label.alpha = 1;
		//this.transform.localPosition = originalPosition;
	}
	
	private void TweenComplete(){
		//Reset();		
		//TweenStart();
		//isTweenStarted = false;
		Destroy(this.gameObject);
		//Debug.Log("Tween Complete!!!");
	}
}
