using UnityEngine;
using System.Collections;

public class SaveAnimalCounter : MonoBehaviour {
	
	private UILabel saveAnimalLabel;
	private GameDataManagerController gdc;
	private GameManagerController gameManagerController;

	// Use this for initialization
	void Start () {
		GetGameManagerController();
		gdc = GameDataManagerController.GetInstance();
		saveAnimalLabel =  GameObject.Find("InGameGUI/Camera/AnchorRight/Panel/SaveAnimalLabel").GetComponent<UILabel>();
		UpdateCounter();
		Messenger.AddListener(GameEvent.SaveAnimal, OnSaveAnimal);
		//Messenger.AddListener( GameEvent.Level_Restart, onLevelRestart);
		AddEventListener();
	}

	private void OnEnable(){
		GetGameManagerController();
	}
	
	private void OnDestroy(){
		RemoveEventListener();
	}
	
	private void AddEventListener(){
		
	}
	
	private void RemoveEventListener(){
		if(gameManagerController!=null){
			gameManagerController.OnLevelRestart-=OnLevelRestart;
		}
	}
	
	private void GetGameManagerController(){
		if(gameManagerController==null){
			gameManagerController = GameManagerController.GetInstance();
			gameManagerController.OnLevelRestart+=OnLevelRestart;
		}
	}

	private void OnLevelRestart(){
		if(this == null) return;
		UpdateCounter();
	}
	
	private void OnSaveAnimal(){
		if(this == null) return;
		UpdateCounter();
	}
	
	/*private void onLevelRestart(){
		if(this == null) return;
		UpdateCounter();
	}*/
	
	private void UpdateCounter(){		
		saveAnimalLabel.text= string.Concat("saved: ", gdc.SaveAnimal.ToString());
		//saveAnimalLabel.text = "saved animals: " + gdc.SaveAnimal.ToString();
	}
}
