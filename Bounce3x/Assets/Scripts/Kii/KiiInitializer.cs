using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;

public class KiiInitializer : MonoBehaviour {

	private bool isCalled =false;

	private void Start(){
		if(!isCalled){
			isCalled =true;
			Kii.Initialize("a3049c95", "5beff601e9be1606900469966cc6d0f6", Kii.Site.JP);
			Debug.Log("init kii  kill initilizer ");
		}
	}

	private void Awake(){
		if(!isCalled){
			isCalled =true;
			Kii.Initialize("a3049c95", "5beff601e9be1606900469966cc6d0f6", Kii.Site.JP);
			//Debug.Log("init kii  kill initilizer ");
		}
	}
}
