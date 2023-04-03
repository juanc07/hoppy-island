using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int count = Input.touchCount;
		if(count > 0){
			for( int i=0;i<count;i++ ){
				Touch touch = Input.GetTouch(i);
				if(touch.phase == TouchPhase.Began){
					Debug.Log("touch screen!!!");
					Application.LoadLevel("Game");
				}
			}
		}		
	}
}
