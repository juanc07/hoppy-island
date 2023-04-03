using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalGeneratorController : MonoBehaviour {

	public GameObject originLeft;
	public GameObject originRight;

	public Rigidbody[] animalPrefabs;
	public Transform generatorPos;
	
	private GameDataManagerController gdc;
	
	private ArrayList animals = new ArrayList();
	private int idCount = 0;
	private List<Animal> animalPool = new List<Animal>();	
	
	private int prevIdAnimal;
	
	
	public bool isDebug =false;
	public int testLevel = 0;
	private bool getSetComplete = false;
	
	private float sec;

	private float TOTAL_TIME;

	//optimal 270
	private const float NORMAL_TOTAL_TIME = 270f;

	//optimal 400
	private const float WARM_UP_TOTAL_TIME = 400f;

	//optimal 180f
	private float maxDeduction = 185f;

	//optimal 18f
	private float deductionRate = 18f;

	//optimal 1f
	//1f 100% , 0.5f 50%
	private float servedDelay = 0.5f;

	//optimal 1.1f
	private float slowReductionTimeRateDelay = 0.85f;

	//private float maxDeduction = 170f;
	//private float deductionRate = 15f;
	private int rndIndex;
	private int lastRandom;
	private int lastIndex;

	private float timeDeduction;
	private float currentTime;

	//optimal 15f
	private float servedHeight = 35f;

	public bool stopAllAnimal = false;
	private PowerUpManagerController powerupManagerController;
	private GameManagerController gameManagerController;

	private int reverseSaveAnimalThreshold;
	private int warmpUpAnimalThreshold;
	private int maxActiveAnimal;

	public bool isVerbose = false;

	// Use this for initialization
	void Start (){
		getSetComplete = false;
		gdc = GameDataManagerController.GetInstance();

		//init some values
		maxActiveAnimal = gdc.MaxActiveAnimal;
		reverseSaveAnimalThreshold = gdc.ReverseAnimalThreshold;
		warmpUpAnimalThreshold = gdc.WarmpUpAnimalThreshold;

		if(isDebug){
			gdc.currentLevel = testLevel;
		}
		
		Messenger.AddListener( GameEvent.Level_Failed, onLevelFailed );
		//Messenger.AddListener( GameEvent.Level_Restart, onLevelRestart );
		Messenger.AddListener(GameEvent.GetSetGoComplete, OnGetSetGoComplete);
		//Messenger.AddListener( GameEvent.Levelup, onLevelUp );
		GetPowerupManagerController();
		GetGameManagerController();
		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void GetGameManagerController(){
		if(gameManagerController==null){
			gameManagerController = GameManagerController.GetInstance();
			gameManagerController.OnLevelRestart+=OnLevelRestart;
		}
	}

	private void AddEventListener(){

	}

	private void RemoveEventListener(){
		if(powerupManagerController!=null){
			powerupManagerController.OnActivatePowerup-=OnActivatePowerup;
			powerupManagerController.OnRemovePowerup-=OnRemovePowerup;
		}

		if(gameManagerController!=null){
			gameManagerController.OnLevelRestart-=OnLevelRestart;
		}
	}

	private void OnEnable(){
		GetPowerupManagerController();
	}

	private void OnLevelRestart(){
		if(this == null)return;
		getSetComplete=false;
		DeactivateAllAnimal();
	}

	private void GetPowerupManagerController(){
		if(powerupManagerController==null){
			powerupManagerController = GameObject.FindObjectOfType<PowerUpManagerController>();
			powerupManagerController.OnActivatePowerup+=OnActivatePowerup;
			powerupManagerController.OnRemovePowerup+=OnRemovePowerup;
		}
	}

	private void OnRemovePowerup(PowerUpChecker.Powerups lastPowerUp){
		StopAllAnimal();
		DisplayLog("onremove powerup!");
	}

	private void OnActivatePowerup(PowerUpType powerUpType){
		if(powerUpType == PowerUpType.Slow){
			StopAllAnimal();
		}
	}
	
	private void OnGetSetGoComplete(){
		if(this == null)return;
		getSetComplete=true;
		//AddAnimal();
		//Invoke("ShootFirstAnimal", 0.5f);		
	}
	
	private void onLevelRestart(){
		if(this == null)return;
		//clearAllAnimals();
		getSetComplete=false;
		DeactivateAllAnimal();
		//DisplayLog( " onLevelRestart clearAllAnimals " );
	}
	
	private void onLevelFailed(){
		if(this == null)return;
		//clearAllAnimals();
		getSetComplete=false;
		DeactivateAllAnimal();
		//DisplayLog( " onLevelFailed clearAllAnimals " );
	}

	private void FixedUpdate(){		
		sec++;
		if(getSetComplete && gdc!=null){

			if(gdc.SaveAnimal < warmpUpAnimalThreshold){
				TOTAL_TIME = WARM_UP_TOTAL_TIME;
			}else{
				TOTAL_TIME = NORMAL_TOTAL_TIME;
			}
			
			timeDeduction = (gdc.currentLevel * deductionRate );
			
			
			if(timeDeduction <= maxDeduction){
				if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
					TOTAL_TIME = WARM_UP_TOTAL_TIME;
					currentTime = (TOTAL_TIME * slowReductionTimeRateDelay) - timeDeduction;
				}else{
					currentTime = TOTAL_TIME - timeDeduction;
				}
			}else{
				if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
					TOTAL_TIME = WARM_UP_TOTAL_TIME;
					currentTime = (TOTAL_TIME  * slowReductionTimeRateDelay) - maxDeduction;
				}else{
					currentTime = TOTAL_TIME - maxDeduction;
				}
			}
			
			
			
			//DisplayLog(" currentTime:  " + currentTime);
			int activeCount = GetActiveAnimalCount();
			if(sec >= currentTime || activeCount == 0){
				rndIndex = Random.Range(0,3);
				
				//DisplayLog("rndIndex " +  rndIndex);
				if(rndIndex != lastRandom){
					if(activeCount > maxActiveAnimal ){
						if(AllowToCreate()){
							lastRandom = rndIndex;
							if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth || gdc.SaveAnimal < warmpUpAnimalThreshold){
								rndIndex = 0;
							}
							/*else if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
								rndIndex = 2;
							}else{
								rndIndex = 2;
							}*/
							
							//rndIndex = 0;
							lastIndex = rndIndex;
							CreateAnimal(rndIndex);
							sec =0;
						}else{
							sec =TOTAL_TIME * servedDelay;
						}
					}else{
						lastRandom = rndIndex;
						if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth || gdc.SaveAnimal < warmpUpAnimalThreshold ){
							rndIndex = 0;
						}
						
						/*else if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
							rndIndex = 2;
						}else{
							rndIndex = 2;
						}*/
						
						//rndIndex = 0;
						lastIndex = rndIndex;
						CreateAnimal(rndIndex);
						sec =0;
					}
				}else{
					if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
						sec =(TOTAL_TIME * slowReductionTimeRateDelay);
					}else{
						sec =TOTAL_TIME * servedDelay;
					}
				}
			}
		}
	}

	private void Update(){
		if(stopAllAnimal){
			StopAllAnimal();
			DisplayLog("stop all animal");
		}
	}
	
	private void BroadcastFirstAnimal(){
		Messenger.Broadcast(GameEvent.FirstAnimal);
	}
	
	public void CreateAnimal(int index){
		if(gdc.GetLife() <= 0 ||  gdc.IsGameOver){
			return;
		}

		int rnd =0;		
		
		if( gdc.currentLevel > 2 && gdc.currentLevel < 6){			
			rnd = Random.Range(0,animalPrefabs.Length - 2);
			//DisplayLog( " 2nd set " );
		}else if( gdc.currentLevel >= 6 ){
			rnd = Random.Range(0,animalPrefabs.Length);
			//DisplayLog( " 3rd set " );
		}else{
			rnd = Random.Range(0,4);
			//DisplayLog( " 1st set " );
		}
		
		//DisplayLog( " rnd animal " + rnd );
		
		//int rnd =animalPrefabs.Length - 1;
		AnimalTrajectory.Animals animalType = ConvertIntToAnimalType(rnd);
		bool found = ActivateAnimalByAnimalType(animalType,index);
		
		if(!found){
			Rigidbody animalInstance;
			Quaternion animalRotation = Quaternion.Euler( 0,0,0);

			int isReversed = Random.Range(0,2);
			if(gdc.SaveAnimal <= reverseSaveAnimalThreshold ){
				isReversed = 0;
			}

			if(isReversed == 0){						
				animalInstance = Instantiate(animalPrefabs[rnd],originLeft.gameObject.transform.position, animalRotation) as Rigidbody;	
			}else{
				animalInstance = Instantiate(animalPrefabs[rnd],originRight.gameObject.transform.position, animalRotation) as Rigidbody;	
			}

			if(animalInstance!=null){
				AnimalTrajectory animalTrajectory = animalInstance.gameObject.GetComponent<AnimalTrajectory>();

				//check to apply slow
				if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
					animalTrajectory.IsSlow = true;
				}else{
					animalTrajectory.IsSlow = false;
				}

				//check for reverse
				if(isReversed == 0){
					animalTrajectory.IsReverse = false;
					animalInstance.transform.parent = originLeft.gameObject.transform;
				}else{
					animalTrajectory.IsReverse = true;
					animalInstance.transform.parent = originRight.gameObject.transform;
				}

				animalTrajectory.AnimalGeneratorController = this.gameObject.GetComponent<AnimalGeneratorController>();
				animalTrajectory.Id = idCount;
				animalTrajectory.SetCurrentIndex(index);
				Animal animal = new Animal();
				animal.isActive = true;
				animal.id = idCount;
				animal.animalType = animalType;
				animal.name = animalType.ToString();
				animal.obj = animalInstance;
				animalInstance.gameObject.name = animal.name +idCount;
				animalPool.Add(animal);
				idCount++;
			}
		}
	}
	
	public void DeactivateAnimalById(int id,bool hasDamage){
		int len =animalPool.Count;
		for(int index=0;index<len;index++){
			if(animalPool[index].id == id && animalPool[index].isActive){
				prevIdAnimal = animalPool[index].id;
				animalPool[index].isActive = false;
				animalPool[index].obj.velocity = Vector3.zero;
				animalPool[index].obj.angularVelocity = Vector3.zero;

				AnimalTrajectory animalTrajectory = animalPool[index].obj.gameObject.GetComponent<AnimalTrajectory>();
				animalTrajectory.IsSlow = false;
				animalTrajectory.IsReverse = false;
				animalPool[index].obj.gameObject.transform.parent = originLeft.gameObject.transform;
				animalTrajectory.OriginalLocation = originLeft.gameObject.transform.position;
				animalTrajectory.Reset();

				//animalPool[index].obj.gameObject.GetComponent<AnimalTrajectory>().IsReverse = false;
				//animalPool[index].obj.gameObject.transform.parent = originLeft.gameObject.transform;
				//animalPool[index].obj.gameObject.GetComponent<AnimalTrajectory>().OriginalLocation = originLeft.gameObject.transform.position;

				//animalPool[index].obj.gameObject.GetComponent<AnimalTrajectory>().Reset();
				animalPool[index].obj.gameObject.GetComponent<TrailRenderer>().enabled = false;
				animalPool[index].obj.gameObject.SetActive(false);
				//AnimalTrajectory animalTrajectory  = animalPool[index].obj.gameObject.GetComponent<AnimalTrajectory>();
				animalTrajectory.isReady = false;
				if(!hasDamage){
					gdc.UpdateScore(animalTrajectory.score);
					gdc.SaveAnimal++;
					Messenger.Broadcast(GameEvent.SaveAnimal);					
					
					if(gdc.HasTutorial==0){
						gdc.HasTutorial = 1;
						Messenger.Broadcast(GameEvent.EndTutorial);
						//save data here
						gdc.SavePlayerData();
					}
				}else{
					gdc.UpdateMissCount(1);
					gdc.UpdateLife(-1);
				}

				break;
			}
		}
	}
	
	public void DeactivateAllAnimal(){
		int len =animalPool.Count;
		for(int index=0;index<len;index++){
			if(animalPool[index].isActive){
				prevIdAnimal = animalPool[index].id;				
				animalPool[index].isActive = false;
				animalPool[index].obj.velocity = Vector3.zero;
				animalPool[index].obj.angularVelocity = Vector3.zero;
				animalPool[index].obj.gameObject.GetComponent<TrailRenderer>().enabled = false;

				AnimalTrajectory animalTrajectory = animalPool[index].obj.gameObject.GetComponent<AnimalTrajectory>();
				animalTrajectory.IsReverse = false;
				animalTrajectory.IsSlow = false;
				animalPool[index].obj.gameObject.transform.parent = originLeft.gameObject.transform;
				animalTrajectory.OriginalLocation = originLeft.gameObject.transform.position;
				animalTrajectory.Reset();
				animalPool[index].obj.gameObject.SetActive(false);
			}
		}
	}


	public void StopAllAnimal(){
		if(stopAllAnimal){
			stopAllAnimal = false;
		}
		int len =animalPool.Count;
		for(int index=0;index<len;index++){
			if(animalPool[index].isActive){
				animalPool[index].animalVelocity = animalPool[index].obj.velocity;
				animalPool[index].animalAngularVelocity = animalPool[index].obj.angularVelocity;
				animalPool[index].obj.velocity = Vector3.zero;
				animalPool[index].obj.angularVelocity = Vector3.zero;
				animalPool[index].obj.useGravity = false;
			}
		}
		DisplayLog("stop all animal");
		Invoke("ResumeFall",1f);
	}

	private void ResumeFall(){
		int len =animalPool.Count;
		for(int index=0;index<len;index++){
			if(animalPool[index].isActive){
				animalPool[index].obj.useGravity = true;
				animalPool[index].obj.gameObject.GetComponent<AnimalTrajectory>().HiDirectGoal();
			}
		}
		DisplayLog("resume fall go next");
	}

	public bool ActivateAnimalByAnimalType(AnimalTrajectory.Animals animalType, int currentIndex){
		int len =animalPool.Count;
		bool found =false;
		
		for(int index=0;index<len;index++){
			if(!animalPool[index].isActive && animalPool[index].animalType == animalType &&  animalPool[index].id != prevIdAnimal ){
				//animalPool[index].isActive = true;
				animalPool[index].obj.gameObject.SetActive(true);

				AnimalTrajectory animalTrajectory = animalPool[index].obj.gameObject.GetComponent<AnimalTrajectory>();

				if(gdc.SaveAnimal >= reverseSaveAnimalThreshold ){
					animalTrajectory.IsReverse = Random.Range(0,2)== 0 ? true : false;
					if(!animalTrajectory.IsReverse){
						animalPool[index].obj.gameObject.transform.parent = originLeft.gameObject.transform;
						animalTrajectory.OriginalLocation = originLeft.gameObject.transform.position;
					}else{
						animalPool[index].obj.gameObject.transform.parent = originRight.gameObject.transform;
						animalTrajectory.OriginalLocation = originRight.gameObject.transform.position;
					}
				}

				//check to apply slow
				if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
					animalTrajectory.IsSlow = true;
				}else{
					animalTrajectory.IsSlow = false;
				}

				animalTrajectory.Reset();
				animalTrajectory.AnimalVoice();
				animalTrajectory.SetCurrentIndex(currentIndex);
				animalPool[index].obj.gameObject.GetComponent<TrailRenderer>().enabled = true;
				found =true;
				animalPool[index].isActive = true;
				break;
			}
		}		
		return found;
	}
	
	public int GetActiveAnimalCount(){
		int len =animalPool.Count;
		int count = 0;
		for(int index=0;index<len;index++){
			if(animalPool[index].isActive){
				count++;
			}
		}		
		return count;
	}

	public List<Animal> GetActiveAnimal(){
		int len =animalPool.Count;
		List<Animal> activeAnimalCollection = new List<Animal>();

		for(int index=0;index<len;index++){
			if(animalPool[index].isActive){
				activeAnimalCollection.Add(animalPool[index]);
			}
		}		
		return activeAnimalCollection;
	}
	
	private AnimalTrajectory.Animals ConvertIntToAnimalType(int val){
		AnimalTrajectory.Animals animalType = AnimalTrajectory.Animals.Chicken;
		switch(val){
			case 0:
				animalType = AnimalTrajectory.Animals.Chicken;
				break;
				
			case 1:
				animalType = AnimalTrajectory.Animals.Cat;
				break;
				
			case 2:
				animalType = AnimalTrajectory.Animals.Pig;
				break;
				
			case 3:
				animalType = AnimalTrajectory.Animals.Cow;
			break;			

			case 4:
				animalType = AnimalTrajectory.Animals.Sheep;
			break;

			case 5:
				animalType = AnimalTrajectory.Animals.Parrot;
			break;

			case 6:
				animalType = AnimalTrajectory.Animals.Dog;
			break;

			case 7:
				animalType = AnimalTrajectory.Animals.Frog;
			break;
		}

		return animalType;
	}

	public void GoDirectGoalAll(){				
		int animalCount = animalPool.Count;
		for( int animalIndex = 0; animalIndex < animalCount; animalIndex++ ){
			Animal animal = animalPool[animalIndex];
			if(animal != null){
				AnimalTrajectory animalTrajectory  = animal.obj.gameObject.GetComponent<AnimalTrajectory>();
				//animalTrajectory.DirectGoal();
				animalTrajectory.ClearDirectGoal();
			}						
		}
	}

	private bool AllowToCreate(){
		List<Animal> tempCollection = GetActiveAnimal();
		tempCollection.Sort(new sortAnimalPosY());			
		Animal animal= tempCollection[0];
		if(animal.obj.gameObject.transform.position.y > servedHeight){
			return true;
		}else{
			return false;
		}		 
	}
	
	public int GetNearestToFall(){
		int index = 0;		
		if(GetActiveAnimalCount() > 0){
			List<Animal> tempCollection = GetActiveAnimal();
			tempCollection.Sort(new sortAnimalPosY());			
			Animal animal= tempCollection[0];
			AnimalTrajectory animalTrajectory = animal.obj.gameObject.GetComponent<AnimalTrajectory>();

			if(!animalTrajectory.IsReverse){
				if( animalTrajectory.HitTimes == 0 ){
					index =  0;
				}else if( animalTrajectory.HitTimes == 1 ){
					index =  1;
				}else if( animalTrajectory.HitTimes == 2 ){
					index =  2;
				}else{
					index =  1;
				}
			}else{
				if( animalTrajectory.HitTimes == 0 ){
					index =  2;
				}else if( animalTrajectory.HitTimes == 1 ){
					index =  1;
				}else if( animalTrajectory.HitTimes == 2 ){
					index =  0;
				}else{
					index =  1;
				}
			}												
		}		
		return index;		
	}	
	
	private static int ComparePosY(Animal a, Animal b){
		Transform p1	=	a.obj.gameObject.GetComponent<Transform>();
		Transform p2 	=	b.obj.gameObject.GetComponent<Transform>();
		
		if (p1.position.y < p2.position.y)
			return -1;
		else if (p1.position.y > p2.position.y)
			return 1;
		else
			return 0;
	}

	private void DisplayLog(string message){
		if(isVerbose){
			Debug.Log(message);
		}
	}
}

public class sortAnimalPosY: IComparer<Animal>{	
	int IComparer<Animal>.Compare(Animal a, Animal b){			
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