using UnityEngine;
using System.Collections;

public class SwipeBtn : MonoBehaviour {
	
	private GameDataManagerController gdc;
	
	private Transform swipeBtn;
	private TapBtn tapBtn;
	private Transform optionPanel;	
	private GameObject opttionGUIAnchor;
	
	// Use this for initialization
	void Start () {
		gdc = GameDataManagerController.GetInstance();		
		opttionGUIAnchor = GameObject.Find("OptionGUI/Camera/Anchor");
		optionPanel = opttionGUIAnchor.transform.Find("OptionPanel");
		swipeBtn = optionPanel.transform.Find("SwipeBtn");
		tapBtn = optionPanel.transform.Find("TapBtn").GetComponent<TapBtn>();
		
		if(!gdc.IsFirstLaunch){
			LoadDefaultSettings();
		}else{
			LoadUserSettings();
		}
	}
	
	private void LoadDefaultSettings(){		
		DisableSwipe();
	}
	
	private void LoadUserSettings(){		
		if( gdc.IsSwipe ){
			EnableSwipe();
		}else{
			DisableSwipe();
		}
	}
	
	private void ToggleSwipeControl(){		
		if(!gdc.IsSwipe){
			EnableSwipe();
		}else{
			DisableSwipe();
			tapBtn.EnableTap();
		}
	}
	
	public void DisableSwipe(){	
		if(swipeBtn == null)return;
		gdc.IsSwipe =false;			
		swipeBtn.transform.Find("OffBG").gameObject.SetActive(true);
		swipeBtn.transform.Find("OnBG").gameObject.SetActive(false);	
		//Debug.Log( "swipe control off" );		
	}
	
	public void EnableSwipe(){
		if(swipeBtn == null)return;
		gdc.IsSwipe =true;
		swipeBtn.transform.Find("OffBG").gameObject.SetActive(false);
		swipeBtn.transform.Find("OnBG").gameObject.SetActive(true);		
		//Debug.Log( "swipe control on" );		
		tapBtn.DisableTap();		
	}
	
	private void OnClick(){
		ToggleSwipeControl();
	}
}
