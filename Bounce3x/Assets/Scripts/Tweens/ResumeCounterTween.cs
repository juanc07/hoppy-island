using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class ResumeCounterTween : MonoBehaviour {

	public UILabel resumeCounterLabel;
	private ResumeCounter resumeCounter;
	private GameManagerController gameManagerController;

	private Vector3 originalScale;
	private Vector3 smallScale = new Vector3(0.000001f,0.000001f,0.000001f);
	private Vector3 bigScale = new Vector3(1.3f,1.3f,1.3f);

	private Sequence tweenSequence;
	private bool isStarted = false;

	// Use this for initialization
	void Start () {
		resumeCounter = new ResumeCounter();
		resumeCounter.counter = 0;
		originalScale = resumeCounterLabel.gameObject.transform.localScale;

		gameManagerController =  GameManagerController.GetInstance();
		ShowHideResumeCounterLabel(false);
		AddEventListener();
		//StartTween();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener(){
		gameManagerController.OnResumeGame+=OnResumeGame;
	}

	private void RemoveEventListener(){
		gameManagerController.OnResumeGame-=OnResumeGame;
	}

	private void OnResumeGame(){
		StartTweenCount1();
	}

	private void StartTweenCount1(){
		if(isStarted) return;

		isStarted = true;
		ShowHideResumeCounterLabel(true);
		resumeCounter.counter = 3;
		resumeCounterLabel.text = resumeCounter.counter.ToString();

		TweenParms tweenBigScale = new TweenParms();
		tweenBigScale.Delay(1f);
		tweenBigScale.Prop("localScale", bigScale);
		tweenBigScale.Ease(EaseType.EaseOutElastic);
		tweenBigScale.UpdateType(UpdateType.TimeScaleIndependentUpdate);
		tweenBigScale.AutoKill(true);
		tweenBigScale.OnComplete(OnDoneCount1);
		//HOTween.To(resumeCounterLabel.gameObject.transform, 1f, tweenBigScale );

		TweenParms tweenSmallScale = new TweenParms();
		tweenSmallScale.Delay(1f);
		tweenSmallScale.Prop("localScale", smallScale);
		tweenSmallScale.Ease(EaseType.EaseOutExpo);
		tweenSmallScale.UpdateType(UpdateType.TimeScaleIndependentUpdate);
		tweenSmallScale.AutoKill(true);


		tweenSequence = new Sequence( new SequenceParms().UpdateType(UpdateType.TimeScaleIndependentUpdate).OnComplete(OnUpdateCounterComplete));
		tweenSequence.autoKillOnComplete = true;
		tweenSequence.Append(HOTween.To(resumeCounterLabel.gameObject.transform, 0.75f, tweenBigScale ));
		tweenSequence.Append(HOTween.To(resumeCounterLabel.gameObject.transform, 0.75f, tweenBigScale ));
		tweenSequence.Append(HOTween.To(resumeCounterLabel.gameObject.transform, 0.75f, tweenBigScale ));
		tweenSequence.Append(HOTween.To(resumeCounterLabel.gameObject.transform, 0.75f, tweenSmallScale ));
		tweenSequence.Play();

		//Debug.Log("start tween count 1");
	}

	private void OnDoneCount1(){
		if(resumeCounter.counter > 0){
			resumeCounter.counter--;
			UpdateResumeCounterLabel();
		}

		resumeCounterLabel.gameObject.transform.localScale = originalScale;
	}


	private void OnUpdateCounterComplete(){
		resumeCounterLabel.gameObject.transform.localScale = originalScale;
		ShowHideResumeCounterLabel(false);
		gameManagerController.UnPauseGame();
		isStarted = false;
	}

	private void UpdateResumeCounterLabel(){
		resumeCounterLabel.text = resumeCounter.counter.ToString();
	}

	private void ShowHideResumeCounterLabel(bool val){
		resumeCounterLabel.gameObject.SetActive(val);
	}
}

public class ResumeCounter{
	public int counter{set ;get;}
}
