using UnityEngine;
using System.Collections;

public class PaddleScript : MonoBehaviour
{	
	private GameObject model;
	private Transform originalRotation;   
	private WhaleAnimation whaleAnimation;

	private float moveTime = 0;
	private float moveDuration = 0.2f;
	private int locationIndex = 0;			

	
	private Vector3 point1;
	private Vector3 point2;
	private Vector3 point3;
	
	private Vector3 targetDirection;
	private bool isMove =false;
	
	
	private bool isRotate = false;
	private bool isLeft = false;
	private bool isRight = false;
	private bool isIdle =true;
	
	private float rotationTime = 0;
	private float rotationDuration = 0.2f;
	
	//game data
	private GameDataManagerController gdc;
	private GameManagerController gameManagerController;

	private PowerUpManagerController powerupManagerController;
	private int activatePowerUpIndex=-1;
	private bool isAdjustingPosition = false;
	
	// Use this for initialization
	void Awake(){		
		gdc = GameDataManagerController.GetInstance();
		gameManagerController = GameManagerController.GetInstance();
		whaleAnimation = this.GetComponent<WhaleAnimation>();
		powerupManagerController = GameObject.FindObjectOfType<PowerUpManagerController>();
		
		model = this.gameObject;
		originalRotation = model.gameObject.transform;
		
		point1 = gdc.PaddlePoint1;
		point2 = gdc.PaddlePoint2;
		point3 = gdc.PaddlePoint3;
	}
	
	void Start (){	
		InitPosition(1);
		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener(){
		gameManagerController.OnLevelRestart+=OnLevelRestart;
		powerupManagerController.OnActivatePowerup+=OnActivatePowerup;
		powerupManagerController.OnRemovePowerup+=OnRemovePowerup;
		powerupManagerController.OnScaleToNormalStart+=OnScaleToNormalStart;
		powerupManagerController.OnScaleToNormalDone+=OnScaleToNormalDone;
	}

	private void RemoveEventListener(){
		gameManagerController.OnLevelRestart-=OnLevelRestart;
		powerupManagerController.OnActivatePowerup-=OnActivatePowerup;
		powerupManagerController.OnRemovePowerup-=OnRemovePowerup;
		powerupManagerController.OnScaleToNormalStart-=OnScaleToNormalStart;
		powerupManagerController.OnScaleToNormalDone-=OnScaleToNormalDone;
	}

	private void OnActivatePowerup(PowerUpType powerupType){
		activatePowerUpIndex = locationIndex;
	}

	private void OnScaleToNormalDone(){
		isAdjustingPosition = false;
		LocationCenter();
	}

	private void OnScaleToNormalStart(){
		isAdjustingPosition = true;
	}

	private void OnRemovePowerup(PowerUpChecker.Powerups lastPowerUp){
		/*if(lastPowerUp == PowerUpChecker.Powerups.Overgrowth){
			isAdjustingPosition = true;
			LocationCenter();
			isAdjustingPosition = false;
		}*/
	}

	private void OnLevelRestart(){
		isAdjustingPosition = true;
		LocationCenter();
		isAdjustingPosition = false;
	}
	
	
	private void OnWhaleSetModelComplete(){
		InitPosition(0);
		Debug.Log("OnWhaleSetModelComplete");
	}
	
	void Update (){
		if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth || isAdjustingPosition || !gdc.isGetSetGoDone){
			return;
		}

		if(gdc.currentPowerup != PowerUpChecker.Powerups.AutoPilot){
			if (Input.GetKeyDown (KeyCode.LeftArrow) /*&& moveTime == 0*/){
				moveLeft();
			}
			
			if (Input.GetKeyDown (KeyCode.RightArrow) /*&& moveTime == 0*/){
				moveRight();
			}
		}

		if(isMove){
			moveTime += Time.fixedDeltaTime;
			//moveTime += moveSpeed;
			if(moveTime < moveDuration){				
				transform.position = Vector3.Lerp( this.transform.position,targetDirection, moveTime / moveDuration );
				//Debug.Log("paddle script lerping moving targetDirection " + targetDirection);
			}else{
				moveTime = 0;
				isMove = false;
				if(gdc.GetMoveCount() > 0){
					int move = gdc.GetMove();
					if(move == 1){
						moveLeft();
					}else if(move == 2){
						moveRight();
					}
				}
			}
		}		
		
		if(isRotate){
			rotationTime += Time.fixedDeltaTime;
			if( rotationTime < rotationDuration ){
				int modelRotation = 0;
				if( isLeft ){
					modelRotation = 90;
				}else if( isRight ){
					modelRotation = 270;
				}else if( isIdle ){
					modelRotation = 0;
				}
				rotateModel( modelRotation, rotationTime / rotationDuration );
			}else{
				if( !isIdle ){
					isIdle = true;
					isRotate = true;
				}else{
					isRotate = false;
					whaleAnimation.OnIdle();
				}
				
				rotationTime = 0;
				isRight = false;
				isLeft =false;
			}		
		}
	}
	
	private void rotateModel( int rotation,float duration ){
		Quaternion target = Quaternion.Euler(0, rotation, 0);
		model.gameObject.transform.rotation = Quaternion.Slerp(originalRotation.rotation, target, duration);
	}
	
	public void moveLeft(){		
		if(moveTime==0 && !isMove && gdc.currentPowerup != PowerUpChecker.Powerups.Overgrowth && gdc.GetLife() > 0 ){
			//Debug.Log("moveleft");
			if(locationIndex == 1){
				if(gdc.cloneLocationIndex != -1){
					if(gdc.cloneLocationIndex == 0){
						//LocationRight();						
					}else if(gdc.cloneLocationIndex == 2){
						LocationLeft();						
					}						
				}else{
					LocationLeft();
				}				
			}else if(locationIndex == 2){
				if(gdc.cloneLocationIndex != -1){
					if(gdc.cloneLocationIndex == 1 ){
						LocationLeft();						
					}else if(gdc.cloneLocationIndex == 0 ){
						LocationCenter();
						isLeft = true;
						isRight = false;
					}
				}else{
					LocationCenter();
					isLeft = true;
					isRight = false;
				}				
			}
			
			isMove = true;
			isRotate = true;
			rotationTime = 0;
			isIdle = false;
			moveTime = 0;

			//TweenRotation();
			
			if(gdc.TapCount==0 && gdc.HasTutorial == 0 && locationIndex == 0){
				gdc.TapCount++;
				Messenger.Broadcast(GameEvent.FirstTap);
			}			
		}
	}
	
	public void moveRight (){
		if(moveTime==0 && !isMove && gdc.currentPowerup != PowerUpChecker.Powerups.Overgrowth && gdc.GetLife() > 0 ){
			if(locationIndex == 1){
				if(gdc.cloneLocationIndex != -1){
					if(gdc.cloneLocationIndex ==2){
					}else if(gdc.cloneLocationIndex ==0){
						LocationRight();
					}
				}else{
					LocationRight();
				}
			}else if(locationIndex == 0){
				if(gdc.cloneLocationIndex != -1){
					if(gdc.cloneLocationIndex == 1){
						LocationRight();
					}else if(gdc.cloneLocationIndex == 2){
						LocationCenter();
						isLeft = false;
						isRight = true;
					}
				}else{
					LocationCenter();
					isLeft = false;
					isRight = true;
				}
			}
			
			isMove = true;
			isRotate = true;
			rotationTime = 0;
			isIdle = false;
			moveTime = 0;

			//TweenRotation();
			
			if(gdc.TapCount==1 && gdc.HasTutorial == 0){
				gdc.TapCount++;	
			}else if(gdc.TapCount==2 && gdc.HasTutorial == 0){
				gdc.TapCount++;	
			}
		}
	}
	
	private void LocationLeft(){
		//Debug.Log("call location left");
		locationIndex = 0;
		targetDirection = point1;
		gdc.locationIndex = locationIndex;
		isLeft = true;
		isRight = false;
	}
	
	private void LocationCenter(){
		locationIndex = 1;
		targetDirection = point2;
		gdc.locationIndex = locationIndex;		

		//new aug 2014
		isLeft = false;
		isRight = false;
	}
	
	private void LocationRight(){
		locationIndex = 2;
		targetDirection = point3;
		gdc.locationIndex = locationIndex;
		isLeft =false;
		isRight = true;		
	}
	
	/*	
	 	index
	 	0- point1, left
		1- point2, center
		2- point3, right 
	 */
	
	private void InitPosition(int index){
		if( this == null ) return;
		
		
		if(index == 0){
			transform.position = point1;
			LocationLeft();
		}else if(index == 1){
			transform.position = point2;
			LocationCenter();			
		}else if(index == 2){
			transform.position = point3;
			LocationRight();
		}
		
		isMove =true;
	}

	public bool IsAdjustingPosition{
		get{return isAdjustingPosition;}
	}

}