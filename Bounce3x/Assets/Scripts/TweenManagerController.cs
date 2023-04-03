using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class TweenManagerController : MonoBehaviour {
	
	//public Transform powerupLabel;
	
	// Use this for initialization
	void Start () {
		HOTween.Init(true, true, false);
		//Messenger.AddListener(GameEvent.FIRST_CATCH, onFirstCatch);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/*private void onFirstCatch(){
		Messenger.RemoveListener(GameEvent.FIRST_CATCH, onFirstCatch);
		GameObject inGamePanel = GameObject.Find("InGamePanel");		
		Transform scLabel  =  Instantiate(powerupLabel, new Vector3(0,0,0), inGamePanel.transform.rotation ) as Transform;
		scLabel.name = "PowerupLabel";
		scLabel.transform.parent = inGamePanel.transform;		
		scLabel.localScale = new Vector3( 58.54361f,58.54361f,0 );
		Debug.Log(" onFirstCatch ");
	}*/
		
}
