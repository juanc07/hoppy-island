using UnityEngine;
using System.Collections;

public class TitleClickBlockerController : MonoBehaviour {

	public GameObject titleClickBlocker;

	// Use this for initialization
	void Start () {
	
	}
	
	public void EnableDisableTitleClickBlocker(bool val){
		titleClickBlocker.SetActive(val);
	}
}
