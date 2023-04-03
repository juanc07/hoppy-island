using UnityEngine;
using System.Collections;

public class PaddleMobileController : MonoBehaviour {
	
	private GameObject paddleObj;
	private PaddleScript paddleController;
	
	
	private GameDataManagerController gdc;
	private Vector2 beginPosition;
	private Vector2 endPosition;
	
	private float minDistance = 50f;
	
	
	// Use this for initialization
	void Start (){
		Debug.Log( " PaddleMobileController gesture started!!" );
		gdc = GameDataManagerController.GetInstance();
		paddleObj = GameObject.Find("Whale");
		paddleController = paddleObj.GetComponent<PaddleScript>();
	}
	
	// Update is called once per frame
	void Update(){		
		int count = Input.touchCount;
		if(count > 0){
			for( int i=0;i<count;i++ ){
				Touch touch = Input.GetTouch(i);
				if(touch.phase == TouchPhase.Began){
					beginPosition = touch.position;
					//Debug.Log("touch screen!!! " + touch.position );
					
					if( gdc.IsTap ){
						if( touch.position.x < 300 && gdc.currentPowerup != PowerUpChecker.Powerups.AutoPilot ){
							paddleController.moveLeft();
						}else{
							if( gdc.currentPowerup != PowerUpChecker.Powerups.AutoPilot ){
								paddleController.moveRight();
							}
						}	
					}					
				}
				if(touch.phase == TouchPhase.Ended){
					if( gdc.IsSwipe ){
						endPosition =  touch.position;
						float beginX = beginPosition.x;
						float endX = endPosition.x;
						float destX = beginX - endX;
						
						if( destX < 0 ){
							destX *=-1;
						}
						
						if( destX >= minDistance ){
							if(beginX > endX){
								//moveleft
								paddleController.moveLeft();						
							}else{
								//moveright
								paddleController.moveRight();
							}
						}
						
						Debug.Log( " beginPosition " + beginPosition + " endPosition " + endPosition );
						
						beginPosition = new Vector2(0,0);
						endPosition = new Vector2(0,0);					
					}					
				}
			}
		}
	}
}
