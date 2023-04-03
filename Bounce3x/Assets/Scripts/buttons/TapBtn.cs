using UnityEngine;
using System.Collections;

public class TapBtn : MonoBehaviour {
	
	private GameDataManagerController gdc;	
	
	private Transform tapBtn;
	private SwipeBtn swipeBtn;
	private Transform optionPanel;	
	private GameObject opttionGUIAnchor;
	
	// Use this for initialization
	void Start (){
		gdc = GameDataManagerController.GetInstance();		
		opttionGUIAnchor = GameObject.Find("OptionGUI/Camera/Anchor");
		optionPanel = opttionGUIAnchor.transform.Find("OptionPanel");
		tapBtn = optionPanel.transform.Find("TapBtn");		
		swipeBtn = optionPanel.transform.Find("SwipeBtn").GetComponent<SwipeBtn>();
			
		if(!gdc.IsFirstLaunch){
			LoadDefaultSettings();
		}else{
			LoadUserSettings();
		}
	}
	
	private void OnClick(){
		ToggleTapControl();
	}
	
	private void LoadDefaultSettings(){		
		EnableTap();
	}
	
	private void LoadUserSettings(){		
		if( gdc.IsTap ){
			EnableTap();
		}else{
			DisableTap();
		}
	}
	
	public void DisableTap(){
		if(tapBtn == null)return;
		gdc.IsTap =false;
		tapBtn.transform.Find("OffBG").gameObject.SetActive(true);
		tapBtn.transform.Find("OnBG").gameObject.SetActive(false);		
		//Debug.Log( "tap control off" );		
	}
	
	public void EnableTap(){
		if(tapBtn == null)return;
		gdc.IsTap =true;
		tapBtn.transform.Find("OffBG").gameObject.SetActive(false);
		tapBtn.transform.Find("OnBG").gameObject.SetActive(true);
		//Debug.Log( "tap control on" );
		swipeBtn.DisableSwipe();
	}
	
	private void ToggleTapControl(){	
		if(!gdc.IsTap){
			EnableTap();
		}else{			
			DisableTap();
			swipeBtn.EnableSwipe();			
		}		
	}
}
