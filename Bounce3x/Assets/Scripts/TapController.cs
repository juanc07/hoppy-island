using UnityEngine;
using System.Collections;

public class TapController : MonoBehaviour {

#if UNITY_ANDROID || UNITY_IPHONE
	private GameObject paddleObj;
	private PaddleScript paddleController;
	private GameDataManagerController gdc;
	private float halfScreenSize;
	
	// Use this for initialization
	void Start (){	
		halfScreenSize = Screen.width * 0.5f;
		gdc = GameDataManagerController.GetInstance();
		paddleObj = GameObject.Find("Whale");
		paddleController = paddleObj.GetComponent<PaddleScript>();
	}
	
	// Update is called once per frame
	void Update(){
		if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth || !gdc.isGetSetGoDone){
			return;
		}

		int count = Input.touchCount;
		if(count > 0){
			for( int i=0;i<count;i++ ){
				Touch touch = Input.GetTouch(i);
				if(touch.phase == TouchPhase.Began){
					Tap(touch);					
				}				
			}
		}
	}
	
	private void Tap(  Touch touch){
		if( !gdc.IsTap ) return;
		
		if( touch.position.x < halfScreenSize && gdc.currentPowerup != PowerUpChecker.Powerups.AutoPilot ){
			paddleController.moveLeft();
		}
		
		if( touch.position.x > halfScreenSize && gdc.currentPowerup != PowerUpChecker.Powerups.AutoPilot ){
			paddleController.moveRight();
		}
	}
#endif
}
