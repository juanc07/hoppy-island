using UnityEngine;
using System.Collections;

public class ResumeBtn : MonoBehaviour {
	
	private OptionController optionController;
	private GameManagerController gameManagerController;
	private GameDataManagerController gdc;
	
	// Use this for initialization
	void Start () {
		gdc = GameDataManagerController.GetInstance();
		gameManagerController =  GameManagerController.GetInstance();
		optionController = GameObject.Find("OptionGUI").GetComponent<OptionController>();
	}
	
	private void OnClick(){
		gdc.IsOptionEnable = false;
		optionController.HideOptionWindow();
		gameManagerController.GameResume();
	}
}
