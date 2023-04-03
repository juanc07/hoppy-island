using UnityEngine;
using System.Collections;

public class MouseClickController : MonoBehaviour {

#if UNITY_WEBPLAYER || UNITY_EDITOR
	private GameObject paddleObj;
	private PaddleScript paddleController;
	private GameDataManagerController gdc;
	private float halfScreenSize;

	// Use this for initialization
	void Start () {
		halfScreenSize = Screen.width * 0.5f;
		gdc = GameDataManagerController.GetInstance();
		paddleObj = GameObject.Find("Whale");
		paddleController = paddleObj.GetComponent<PaddleScript>();
	}
	
	// Update is called once per frame

	void Update () {
		if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth || !gdc.isGetSetGoDone){
			return;
		}

		if (Input.GetButtonDown("Fire1")) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 pt = ray.GetPoint(0);
			//Debug.Log("mouse clicked check ray " + pt.x + " mousex " + Input.mousePosition.x );

			//if (Physics.Raycast(ray)){
			//	Debug.Log("mouse clicked hit something!");
			//}else{
				if( Input.mousePosition.x < halfScreenSize && gdc.currentPowerup != PowerUpChecker.Powerups.AutoPilot ){
					paddleController.moveLeft();
				}
				
				if( Input.mousePosition.x > halfScreenSize && gdc.currentPowerup != PowerUpChecker.Powerups.AutoPilot ){
					paddleController.moveRight();
				}
			//}
		}
	}
#endif
}
