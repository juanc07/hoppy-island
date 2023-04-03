using UnityEngine;
using System.Collections;

public class OptionBtn : MonoBehaviour {
	
	private OptionController optionController;
	public GameObject optionGUI;
	private TitleClickBlockerController titleClickBlokcerController;

	// Use this for initialization
	void Start (){
		titleClickBlokcerController = GameObject.FindObjectOfType<TitleClickBlockerController>();
		if(optionGUI!=null){
			optionGUI.SetActive(true);
			optionController =optionGUI.GetComponent<OptionController>();
		}
	}
	
	private void OnClick(){
		if(optionController.ShowOptionWindow()){
			titleClickBlokcerController.EnableDisableTitleClickBlocker(true);
		}
	}
}
