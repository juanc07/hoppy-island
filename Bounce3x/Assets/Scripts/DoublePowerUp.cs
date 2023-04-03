using UnityEngine;
using System.Collections;

public class DoublePowerUp : MonoBehaviour {

	private GameObject powerUpManager;
	private PowerUpManagerController powerUpManagerController;
	
	// Use this for initialization
	void Start () {
		powerUpManager = GameObject.Find("PowerUpManager");
		powerUpManagerController = powerUpManager.GetComponent<PowerUpManagerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void OnTriggerEnter( Collider col ){		
		if( col.gameObject.name != "fakewhale" && col.gameObject.tag == "paddle" ){
			powerUpManagerController.spawnWhale();
			Debug.Log("on OnTriggerEnter DoublePowerup!!!");		
		}	
	}
}
