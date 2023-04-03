using UnityEngine;
using System.Collections;

public class DoubleTap : MonoBehaviour {
	
	private float lastTouch = 0f;
	private float interval =0f;
	//private float currentTouch =0f;
	
	// Use this for initialization
	void Start (){		
	}
	
	// Update is called once per frame
	void Update (){
		CheckForDoubleTap();		
	}
	
	private void CheckForDoubleTap(){		
		if(lastTouch > 0){
			interval+=Time.deltaTime;
		}
		if (Input.GetButtonDown ("Fire1")){						
			float intervalBetweenTouch = interval - lastTouch;
			interval = 0;
			if(intervalBetweenTouch <=0.5f && lastTouch != 0 ){					
				//Debug.Log("double tap!!");
			}else{
				//Debug.Log("single tap!!");
			}				
			lastTouch = Time.deltaTime;
		}		
	}
}
