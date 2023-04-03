using UnityEngine;
using System.Collections;

public class NewHighScoreTweener : MonoBehaviour {

	private GoTween scaleUpTween;
	private GoTween scaleDownTween;

	private GoTweenChain scaleTweenChain;
	private bool isLoop = true;

	// Use this for initialization
	void Start () {
	
	}

	private void OnEnable(){
		isLoop = true;
		StartTween();
	}

	private void OnDisable(){
		isLoop = false;
		scaleTweenChain.complete();
		Go.killAllTweensWithTarget(this.gameObject);
	}
	
	private void StartTween(){
		scaleUpTween = new GoTween(this.gameObject.transform,0.25f,new GoTweenConfig().scale(1.5f,false).setEaseType(GoEaseType.Linear));
		scaleDownTween = new GoTween(this.gameObject.transform,0.25f,new GoTweenConfig().scale(1f,false).onComplete(OnScaleTweenComplete));

		scaleTweenChain = new GoTweenChain();
		scaleTweenChain.append(scaleUpTween).append(scaleDownTween);
		scaleTweenChain.autoRemoveOnComplete = true;
		scaleTweenChain.play();
	}

	private void OnScaleTweenComplete(AbstractGoTween abstractGoTween){
		if(isLoop){
			StartTween();
		}
	}
}
