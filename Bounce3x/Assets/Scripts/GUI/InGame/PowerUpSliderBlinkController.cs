using UnityEngine;
using System.Collections;

public class PowerUpSliderBlinkController : MonoBehaviour {

	private GoTween alphaTweenGlowOut;
	private GoTween alphaTweenGlowIn;

	private GoTweenChain alphaTweenChain;
	public UISprite target;
	private Color32 originalColor;
	private bool isLoop = false;
	private bool hasStarted = false;


	// Use this for initialization
	void Start () {
		//255,124,124,255
		originalColor = target.color;
	}

	private void OnDisable(){
		StopTween();
	}
	
	public void StartTween(){
		hasStarted = true;
		target.gameObject.SetActive(true);

		isLoop = true;

		alphaTweenGlowOut = new GoTween(target,0.3f,new GoTweenConfig().colorProp("color",new Color32(255,0,0,255),false));
		alphaTweenGlowIn = new GoTween(target,0.3f,new GoTweenConfig().colorProp("color",new Color32(255,124,124,255),false).onComplete(OnTweenComplete));

		alphaTweenChain = new GoTweenChain();
		alphaTweenChain.append(alphaTweenGlowOut).append(alphaTweenGlowIn);
		alphaTweenChain.autoRemoveOnComplete = true;
		alphaTweenChain.play();
	}

	public void StopTween(){
		hasStarted = false;
		isLoop = false;
		target.gameObject.SetActive(false);
		//Debug.Log("stop blink tween!!");
	}

	private void OnTweenComplete(AbstractGoTween abstractGotween){
		if(isLoop){
			StartTween();
		}
	}

	public bool HasStarted{
		set{hasStarted = value;}
		get{return hasStarted;}
	}
}
