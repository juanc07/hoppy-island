using UnityEngine;
using System.Collections;

public class loginOkBtnClick : MonoBehaviour {

	public GameObject loginPopupMessage;

	// Use this for initialization
	void Start () {
	
	}
	
	private void OnClick(){
		loginPopupMessage.gameObject.SetActive(false);
	}
}
