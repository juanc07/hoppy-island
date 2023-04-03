using UnityEngine;
using System.Collections;

public class LoginCreateBtnBtnClick : MonoBehaviour {

	public GameObject createUserPanel;
	public GameObject loginUserPanel;

	// Use this for initialization
	void Start () {
	
	}
	
	private void OnClick(){
		loginUserPanel.gameObject.SetActive(false);
		createUserPanel.gameObject.SetActive(true);
	}
}
