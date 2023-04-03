using UnityEngine;
using System.Collections;

public class TweenChecker : MonoBehaviour {
	
	
	private GameObject inGamePanel;
	private UILabel comboLabel;
	private TweenPosition tweenPosition;
	private TweenAlpha tweenAlpha;
	
	private Vector3 origPosition;
	//private Vector3 targetPosition;
	private Quaternion origRotation;
	
	
	public Camera mainCamera;
	public Camera NGUICamera;
	public GameObject target;	
	
	// Use this for initialization
	void Start (){			
		origPosition = this.transform.localPosition;
		//targetPosition = this.transform.localPosition;
		origRotation = this.transform.localRotation;
		
		inGamePanel = GameObject.Find("InGamePanel");
		comboLabel =inGamePanel.transform.Find("ComboLabel").GetComponent<UILabel>();
		comboLabel.text ="+1 \nCombo x2";
		tweenPosition = comboLabel.GetComponent<TweenPosition>();
		tweenPosition.callWhenFinished = "DoneAnimation";		
		
		tweenAlpha = comboLabel.GetComponent<TweenAlpha>();
		tweenAlpha.callWhenFinished = "DoneAlpha";		
		
		StartAtTarget();
		//PlayTextComboAnimation();
	}
	
	private void StartAtTarget(){
		inGamePanel = GameObject.Find("InGamePanel");
		comboLabel =inGamePanel.transform.Find("ComboLabel").GetComponent<UILabel>();
		comboLabel.text ="see me";      
		
		Vector3 screenPos =mainCamera.ScreenToWorldPoint( target.transform.localPosition );
		Vector3 guiCameraPosition = NGUICamera.ScreenToWorldPoint(screenPos);
		comboLabel.gameObject.transform.localPosition =new Vector3(guiCameraPosition.x + 50, -300f, guiCameraPosition.z);
	}
	
	public void PlayTextComboAnimation(){		
		//tweenPosition.Reset();
		//tweenPosition.Play(true);		
	}
	
	public void PlayAlpha(){
		//tweenAlpha.Reset();
		//tweenAlpha.Play(true);
	}
	
	private void DoneAlpha(){
		PlayAlpha();
		Debug.Log("DoneAnimation");
	}
	
	private void DoneAnimation(){		
		//PlayTextComboAnimation();
		Debug.Log("DoneAnimation");
	}
	
	private void ResetPosition(){
		this.transform.localPosition = origPosition;
		this.transform.localRotation = origRotation;
	}
	
	// Update is called once per frame
	void Update () {		
	}
}
