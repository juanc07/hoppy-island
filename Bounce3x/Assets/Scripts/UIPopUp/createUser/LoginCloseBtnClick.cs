using UnityEngine;
using System.Collections;

public class LoginCloseBtnClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	private void OnClick(){
		UIPanel uipanel = UIPanel.Find(this.gameObject.transform);
		uipanel.gameObject.SetActive(false);
		Debug.Log("close login");
	}
}
