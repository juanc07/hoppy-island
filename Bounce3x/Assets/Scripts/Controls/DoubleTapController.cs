using UnityEngine;
using System.Collections;

public class DoubleTapController : MonoBehaviour {

#if UNITY_ANDROID || UNITY_IPHONE
	private GameObject paddleObj;
	private PaddleScript paddleController;
	private GameDataManagerController gdc;
	private float halfScreenSize;
	
	private float lastTouch = 0f;
	private float interval =0f;
	private float touchSpeed = 0.3f;

	private GameManagerController gameManagerController;

	// Use this for initialization
	void Start (){
		halfScreenSize = Screen.width * 0.5f;
		gdc = GameDataManagerController.GetInstance();
		gameManagerController = GameObject.FindObjectOfType<GameManagerController>();

		paddleObj = GameObject.Find("Whale");
		paddleController = paddleObj.GetComponent<PaddleScript>();

		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener(){
		gameManagerController.OnPausedGame+=OnPausedGame;
		gameManagerController.OnLevelRestart+=OnLevelRestart;
	}

	private void RemoveEventListener(){
		gameManagerController.OnPausedGame-=OnPausedGame;
		gameManagerController.OnLevelRestart-=OnLevelRestart;
	}


	private void OnPausedGame(){
		gdc.ClearMoveQeue();
	}

	private void OnLevelRestart(){
		gdc.ClearMoveQeue();
	}

	// Update is called once per frame
	void Update(){
		if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth || !gdc.isGetSetGoDone){
			return;
		}

		if(lastTouch > 0){
			interval+=Time.deltaTime;
		}
		int count = Input.touchCount;
		if (Input.GetButtonDown ("Fire1") && count > 0 ){
			Touch touch = Input.GetTouch(0);			
			float intervalBetweenTouch = interval - lastTouch;
			interval = 0;			
			if(intervalBetweenTouch <=touchSpeed && lastTouch != 0){				
				if(touch.phase == TouchPhase.Began){
					//if(gdc.locationIndex == 0 || gdc.locationIndex== 2){
						DoubleTap(touch);
					//}
					Debug.Log("double tap!!");
				}				
			}else{
				Debug.Log("single tap!!");
			}
			lastTouch = Time.deltaTime;
		}				
	}
	
	private void DoubleTap(Touch touch){
		if( !gdc.IsTap ) return;
		
		Debug.Log("double tap " + touch.position.x );
		
		if( touch.position.x < halfScreenSize && gdc.currentPowerup != PowerUpChecker.Powerups.AutoPilot ){
			//new 092014
			paddleController.moveLeft();

			gdc.AddMoveQeue(1);
		}

		if( touch.position.x > halfScreenSize && gdc.currentPowerup != PowerUpChecker.Powerups.AutoPilot ){
			paddleController.moveRight();
			gdc.AddMoveQeue(2);
		}
	}
#endif
}
