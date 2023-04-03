using UnityEngine;
using System.Collections;

public class PaddleButtonScript : MonoBehaviour{	
	private GameObject paddleObj;
	private PaddleScript paddleController;
	
	// Use this for initialization	
	void Start (){
		paddleObj = GameObject.Find("paddle");
		paddleController = paddleObj.GetComponent<PaddleScript>();		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate(){}
	
	void OnGUI () {
		// Make a background box
		//GUI.Box(new Rect(10,10,100,90), "Action");
		
		if(GUI.Button(new Rect(5,550,150,150), "Left")){			
			//Debug.Log("click left!!");
			paddleController.moveLeft();
		}		
		
		//if(GUI.RepeatButton(new Rect(1070,600,100,100), "Right")) {
		if(GUI.Button(new Rect(1010,550,150,150), "Right")) {
			paddleController.moveRight();
			//Debug.Log("click right!!");			
		}		
	}	
}
