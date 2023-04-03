using UnityEngine;
using System.Collections;

public class CloseBtn : MonoBehaviour {
	
	private OptionController optionController;
	private TitleClickBlockerController titleClickBlokcerController;
	
	// Use this for initialization
	void Start () {
		titleClickBlokcerController = GameObject.FindObjectOfType<TitleClickBlockerController>();
		optionController = GameObject.Find("OptionGUI").GetComponent<OptionController>();
	}
	
	private void OnClick(){
		titleClickBlokcerController.EnableDisableTitleClickBlocker(false);
		optionController.HideOptionWindow();
	}
}
