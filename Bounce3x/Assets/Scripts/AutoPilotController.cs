using UnityEngine;
using System.Collections;

public class AutoPilotController : MonoBehaviour {
	
	private GameObject animalGenerator;
	private AnimalGeneratorController animalGenController;
	private SunlightManagerController sunlightManagerController;
	
	private GameDataManagerController gdc;
	public bool isAutoPilot =false;
	private GameObject whale;
	private PaddleScript paddleController;

	// Use this for initialization
	void Start (){
		gdc = GameDataManagerController.GetInstance();
		animalGenerator = GameObject.Find("AnimalGenerator");
		animalGenController = animalGenerator.GetComponent<AnimalGeneratorController>();		
		sunlightManagerController = GameObject.Find("SunlightManager").GetComponent<SunlightManagerController>();

		whale = GameObject.Find("Whale");
		paddleController = whale.GetComponent<PaddleScript>();		
	}
	
	// Update is called once per frame
	void Update (){
		if(paddleController.IsAdjustingPosition){
			return;
		}

		if( gdc.currentPowerup == PowerUpChecker.Powerups.AutoPilot){
			AutoPilot();
		}else if(isAutoPilot && gdc.currentPowerup != PowerUpChecker.Powerups.Overgrowth){
			AutoPilot();
		}
	}

	private void AutoPilot(){
		int index = animalGenController.GetNearestToFall();
		
		if( index == 0 && gdc.locationIndex == 1 ){
			paddleController.moveLeft();
			//Debug.Log("auto moveleft");
			//paddleController.ResetLocation(0);			
		}else if( index == 0 && gdc.locationIndex == 2){
			paddleController.moveLeft();
			paddleController.moveLeft();
			//Debug.Log("auto 2x moveleft");
		}else if( index == 1 && gdc.locationIndex == 0){
			paddleController.moveRight();
			//Debug.Log("auto moveRight");
		}else if( index == 1 && gdc.locationIndex == 2){
			paddleController.moveLeft();
			//Debug.Log("auto 2x moveleft");
		}else if( index == 2 && gdc.locationIndex == 0){
			paddleController.moveRight();
			paddleController.moveRight();
			//Debug.Log("auto 2x moveRight");
		}else if( index == 2 && gdc.locationIndex == 1){
			paddleController.moveRight();
			//Debug.Log("auto moveRight");
		}
	}
}
