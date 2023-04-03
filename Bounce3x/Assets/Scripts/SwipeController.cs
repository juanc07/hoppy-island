using UnityEngine;
using System.Collections;

public class SwipeController : MonoBehaviour {

#if UNITY_ANDROID || UNITY_IPHONE
	private GameObject paddleObj;
	private PaddleScript paddleController;
	
	
	private GameDataManagerController gdc;
	private Vector2 beginPosition;
	private Vector2 endPosition;	
	private float minDistance = 50f;
	
	
	// Use this for initialization
	void Start (){		
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
					beginPosition = touch.position;					
				}
				
				if(touch.phase == TouchPhase.Ended){
					endPosition =  touch.position;
					Swipe( beginPosition, endPosition );
				}
			}
		}
	}
	
	private void Swipe( Vector2 begin, Vector2 end ){
		if( !gdc.IsSwipe ) return;			
		
		float beginX = begin.x;
		float endX = end.x;
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
		//Debug.Log( " beginPosition " + beginPosition + " endPosition " + endPosition );		
		beginPosition = new Vector2(0,0);
		endPosition = new Vector2(0,0);				
	}
#endif
}
