using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class CloudTweenAnimation : MonoBehaviour {
	
	private Vector3 startPosition;
	private Vector3 targetPosition;
	private Quaternion startRotation;
	public float duration = 1;
	private TweenParms parms;
	
	// Use this for initialization
	void Start (){		
		startPosition = this.transform.localPosition;
		startPosition.x = -1077.331f;
		
		targetPosition = this.transform.localPosition;		
		targetPosition.x = 1030.388f;
		startRotation = this.transform.localRotation;		
		
		parms = new TweenParms();
		StartTween();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void StartTween(){		 		
		parms.Prop("position", targetPosition);
		parms.Ease(EaseType.EaseOutCubic); 
		parms.Delay(0f);
		parms.AutoKill(true);
		parms.OnComplete(Reset);
		HOTween.To(transform, duration, parms );		
	}
	
	private void Reset(){
		//HOTween.Restart();
		this.transform.localPosition = startPosition;
		this.transform.localRotation = startRotation;		
		//Debug.Log("ResetCloudPosition");
		StartTween();
	}
}
