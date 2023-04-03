using UnityEngine;
using System.Collections;

public class GameManagerCaller : MonoBehaviour {

	private GameManagerController gameManagerController;

	// Use this for initialization
	void Start () {
		gameManagerController = GameManagerController.GetInstance();
	}
}
