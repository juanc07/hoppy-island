using UnityEngine;
using System.Collections;

public class ClearInputBtnClick : MonoBehaviour {

	public UILabel[]  labels;
	public UIInput[]  uiInputs;

	// Use this for initialization
	void Start () {
		
	}
	
	private void OnClick(){
		foreach(UIInput uiInput in uiInputs){
			uiInput.value="";
		}

		foreach(UILabel label in labels){
			label.text = "";
		}
		Debug.Log("clear input");
	}
}
