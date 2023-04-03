using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SunlightManagerController : MonoBehaviour {
	
	public Transform sunlightPrefab;
	private List<Sunlight> sunlightPool= new List<Sunlight>();
	
	private Vector3 point1;
	private Vector3 point2;
	private Vector3 point3;
	private Vector3 currentPosition;
	private int currentIndex;
	
	private GameObject sunlightContainer;
	private float min = 10f;
	private float max = 30f;
	private float delay = 0f;
	private GameDataManagerController gdc;
	private GameManagerController gameManagerController;
	private int addSunlightThreshold;
	public bool isVerbose = false;
	
	// Use this for initialization
	void Start (){
		GetGameManagerController();
		gdc = GameDataManagerController.GetInstance();
		addSunlightThreshold = gdc.SunlightThreshold;

		InitValue();		
		randomDelay();
		StartTimer();
		
		Messenger.AddListener(GameEvent.Level_Failed, OnLevelFailed);
		//Messenger.AddListener(GameEvent.Level_Restart, OnLevelRestart);

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
			gameManagerController =GameManagerController.GetInstance();
			gameManagerController.OnLevelRestart+=OnLevelRestart;
		}
	}

	private void OnLevelRestart(){
		if(this==null)return;
		DeactivateAllSunlight();
	}
	
	private void OnLevelFailed(){
		if(this==null)return;
		DeactivateAllSunlight();
	}
	
	/*private void OnLevelRestart(){
		if(this==null)return;
		DeactivateAllSunlight();
	}*/
	
	private void StartTimer(){		
		StartCoroutine(SpawnSunlight(delay));
	}
	
	IEnumerator SpawnSunlight(float interval){
		yield return new WaitForSeconds(interval);
		AddSunlight();
		randomDelay();
		StartTimer();
	}
	
	private void randomDelay(){
		delay = Random.Range(min,max);
	}
	
	private void AddSunlight(){
		if(gdc.HasTutorial==0 || gdc.GetLife() <= 0 || !gdc.isGetSetGoDone 
		   || gdc.currentPowerup == PowerUpChecker.Powerups.AutoPilot
		   || gdc.SaveAnimal < addSunlightThreshold
		   || gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth
		  ){
			return;
		}
		
		RandomSpawn();
		bool found = ActivateSunlight();
		
		if(sunlightContainer==null){
			sunlightContainer = new GameObject();
			sunlightContainer.name ="SunlightContainer";
		}		
		
		if(!found){
			Transform sunlightObject =  Instantiate( sunlightPrefab, currentPosition, Quaternion.Euler( 0,0,0 ) ) as Transform;
			SunlightTween tween = sunlightObject.GetComponent<SunlightTween>();			
			Sunlight sunlight = new Sunlight();			
			sunlight.id = sunlightPool.Count.ToString();
			sunlightObject.gameObject.name = "sunlight"+sunlight.id;
			tween.Id = sunlight.id.ToString();
			sunlight.obj = sunlightObject;
			sunlight.isActive = true;
			sunlight.localIndex = currentIndex;
			sunlight.obj.gameObject.transform.parent = sunlightContainer.transform;
			sunlightPool.Add(sunlight);
			DisplayLog("add new sunlight!! count "+ sunlightPool.Count);
		}	
	}
	
	public void DeactivateSunlightById(string id){
		int len = sunlightPool.Count;
		for(int index=0;index<len;index++){
			if(sunlightPool[index].id == id && sunlightPool[index].isActive){
				sunlightPool[index].isActive =false;
				sunlightPool[index].obj.gameObject.SetActive(false);
				sunlightPool[index].obj.gameObject.transform.position = new Vector3(-11000f, 0,0);
				DisplayLog("deactivate sunlight!! count "+ sunlightPool.Count );
				break;
			}
		}
	}
	
	public void DeactivateAllSunlight(){
		int len = sunlightPool.Count;
		for(int index=0;index<len;index++){
			if(sunlightPool[index]!=null && sunlightPool[index].isActive){
				sunlightPool[index].isActive =false;
				sunlightPool[index].obj.gameObject.SetActive(false);
				sunlightPool[index].obj.gameObject.transform.position = new Vector3(-11000f, 0,0);
				DisplayLog("deactivate sunlight!! count "+ sunlightPool.Count );
				break;
			}
		}
	}
	
	public bool ActivateSunlight(){
		int len = sunlightPool.Count;
		bool found =false;
		
		for(int index=0;index<len;index++){
			if(!sunlightPool[index].isActive){
				SunlightTween tween = sunlightPool[index].obj.gameObject.GetComponent<SunlightTween>();	
				sunlightPool[index].isActive =true;
				sunlightPool[index].localIndex = currentIndex;
				sunlightPool[index].obj.gameObject.SetActive(true);
				sunlightPool[index].obj.gameObject.transform.position = currentPosition;
				tween.Tween();
				found =true;
				DisplayLog("reuse sunlight!! count "+ sunlightPool.Count);
				break;
			}
		}		
		return found;
	}
	
	private void InitValue(){
		point1 = new Vector3(75f, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
	 	point2 = new Vector3 (-40f, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		point3 = new Vector3(-160f, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
	}
	
	private void RandomSpawn(){
		int randomPosition = Random.Range(1,4);
		
		if( randomPosition == 1 ){
			currentIndex = 0;
			currentPosition = point1;
		}else if( randomPosition == 2 ){
			currentIndex = 1;
			currentPosition = point2;
		}else if( randomPosition == 3 ){
			currentIndex = 2;
			currentPosition = point3;
		}
	}
	
	public int GetActiveSunlight(){
		int len =sunlightPool.Count;
		int count = 0;
		for(int index=0;index<len;index++){
			if(sunlightPool[index].isActive){
				count++;
			}
		}		
		return count;
	}
	
	
	public int GetNearestToFall(){
		int index = 0;		
		if(GetActiveSunlight() > 0){
			sunlightPool.Sort(new sortSunlightPosY());			
			Sunlight sunlight= sunlightPool[0];			
			//AnimalTrajectory animalTrajectory = animal.obj.gameObject.GetComponent<AnimalTrajectory>();
			if( sunlight.localIndex == 0 ){
				index =  0;
			}else if( sunlight.localIndex == 1 ){
				index =  1;
			}else if( sunlight.localIndex == 2 ){
				index =  2;
			}		
		}		
		return index;		
	}

	private void DisplayLog(string message){
		if(isVerbose){
			Debug.Log(message);
		}
	}
}


public class sortSunlightPosY: IComparer<Sunlight>{	
	 int IComparer<Sunlight>.Compare(Sunlight a, Sunlight b){			
		Transform p1=a.obj.gameObject.GetComponent<Transform>();
		Transform p2 = b.obj.gameObject.GetComponent<Transform>();
			
		if (p1.position.y < p2.position.y)
			return -1;
		else if (p1.position.y > p2.position.y)
			return 1;
		else
			return 0;
	}	
}
