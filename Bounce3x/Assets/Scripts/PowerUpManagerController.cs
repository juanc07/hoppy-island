using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PowerUpManagerController : MonoBehaviour {
	
	public Rigidbody[] powerups;
    public Transform generatorPos;
	private float delay;
	private float minDelay;
	private float maxDelay;
	private GameObject powerupContainer;
	
	private Vector3 point1;
	private Vector3 point2;
	private Vector3 point3;
	
	private Vector3 currentPosition =  new Vector3(0,0,0);
	private int randomPosition;
	
	public Rigidbody whalePrefab;
	public GameObject whaleHolder;
	private GameDataManagerController gdc;
	
	private ArrayList whales = new ArrayList();
	
	public GameObject animalGenerator;
	private AnimalGeneratorController animalGenController;
	
	private GameObject whale;
	private	WhaleAnimation whaleAnimation;
	
	
	public GameObject powerupGauge;
	private PowerupSlider powerupSlider;
	private Transform slider;
	
	
	private ArrayList powerupCollection = new ArrayList();
	private List<Powerup> powerupPool = new List<Powerup>();
	
	private int powerupCount = 0;

	public bool isDebug =false;
	public PowerUpChecker.Powerups currentPowerup = PowerUpChecker.Powerups.none;
	private int prevPowerup =-1;

	private GameManagerController gameManagerController;
	public bool isVerbose = false;
	private int addPowerUpThreshold;

	private Action <PowerUpChecker.Powerups>RemovePowerup;
	public event Action <PowerUpChecker.Powerups>OnRemovePowerup{
		add{RemovePowerup+=value;}
		remove{RemovePowerup-=value;}
	}

	private Action ScaleToNormalStart;
	public event Action OnScaleToNormalStart{
		add{ScaleToNormalStart+=value;}
		remove{ScaleToNormalStart-=value;}
	}

	private Action ScaleToNormalDone;
	public event Action OnScaleToNormalDone{
		add{ScaleToNormalDone+=value;}
		remove{ScaleToNormalDone-=value;}
	}

	private Action <PowerUpType>ActivatePowerup;
	public event Action <PowerUpType>OnActivatePowerup{
		add{ActivatePowerup+=value;}
		remove{ActivatePowerup-=value;}
	}
	
	// Use this for initialization
	void Start (){
		GetGameManagerController();
		gdc = GameDataManagerController.GetInstance();

		//int some setup 
		delay = gdc.MaxPowerUpDelay;
		minDelay = gdc.MinPowerUpDelay;
		maxDelay = gdc.MaxPowerUpDelay;

		addPowerUpThreshold = gdc.PowerUpThreshold;
		gdc.currentPowerup = currentPowerup;

		animalGenController = animalGenerator.GetComponent<AnimalGeneratorController>();
		powerupCount = powerups.Length;		

		slider = powerupGauge.transform.Find("Slider");

		GetWhale();
		
		InitValue();		
		randomDelay();
		startTimer();		

		DeactivatePowerupSlider();
		Messenger.AddListener( GameEvent.Level_Failed, onLevelFailed );
		//Messenger.AddListener( GameEvent.Level_Restart, onLevelRestart );

		if( slider != null ){
			powerupSlider = slider.GetComponent<PowerupSlider>();
			powerupSlider.Deactivate();
			powerupGauge.gameObject.SetActive(false);
		}

		AddEventListener();
	}

	private void GetWhale(){
		if(whale==null){
			whale = GameObject.Find("Whale");
		}

		if(whaleAnimation==null){
			whaleAnimation = whale.GetComponent<WhaleAnimation>();
			whaleAnimation.OnScaleToNormalComplete+=OnScaleToNormalComplete;
			whaleAnimation.OnScaleToOverGrowthComplete+=OnScaleToOverGrowthComplete;
		}
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
		whaleAnimation.OnScaleToNormalComplete-=OnScaleToNormalComplete;
		whaleAnimation.OnScaleToOverGrowthComplete-=OnScaleToOverGrowthComplete;

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

	private void OnScaleToNormalComplete(){
		DeactivatePowerup();

		if(null != RemovePowerup){
			RemovePowerup(PowerUpChecker.Powerups.Overgrowth);
		}

		if(null!=ScaleToNormalDone){
			ScaleToNormalDone();
		}
	}
	private void OnScaleToOverGrowthComplete(){
		if(null!=ActivatePowerup){
			ActivatePowerup(PowerUpType.Overgrowth);
		}
	}


	private void OnLevelRestart(){
		if( this == null )return;
		ClearPowerupEffect();
		DisplayLog(" onLevelRestart clearAllAnimals ");
	}
	
	private void onLevelFailed(){
		if( this == null )return;
		ClearPowerupEffect();
		DisplayLog(" onLevelFailed clearpowerups ");
	}
	
	private void ClearPowerupEffect(){
		removeWhale();
		QuickScaleToNormal();
		DeactivateAllPowerup();
	}
	
	
	private void ActivatePowerupSlider(PowerUpChecker.Powerups powerupType){
		if( slider != null ){
			powerupSlider = slider.GetComponent<PowerupSlider>();
			float timeInterval = GetPowerUpIntervalByPowerUpType(powerupType);
			powerupSlider.Activate(powerupType,timeInterval );
		}
	}
	
	private void DeactivatePowerupSlider(){					
		if( slider != null ){
			powerupSlider = slider.GetComponent<PowerupSlider>();
			powerupSlider.Deactivate();
			powerupGauge.gameObject.SetActive(false);
		}
	}
	
	private void InitValue(){
		point1 = new Vector3(75f, generatorPos.position.y, generatorPos.position.z);
	 	point2 = new Vector3 (-40f, generatorPos.position.y, generatorPos.position.z);
		point3 = new Vector3(-160f, generatorPos.position.y, generatorPos.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if(isDebug){
			gdc.currentPowerup = currentPowerup;
		}
		CheckPowerUpSlider();
	}
	
	private void CheckPowerUpSlider(){
		if(powerupSlider == null)return;
		
		if(gdc.hasPowerUp){
			if(!powerupSlider.isActive){
				gdc.hasPowerUp =false;
				removePowerup();				
			}			
		}
	}
	
	void startTimer(){
        StartCoroutine(WaitAndAddPowerup(delay));        
	}
	
	IEnumerator WaitAndAddPowerup(float waitTime) {
        yield return new WaitForSeconds(waitTime);
       
		addPowerup();
		randomDelay();
		if( delay > 3f ){
			delay -= 0.2f;
		}		
		startTimer();
    }
	
	private IEnumerator WaitAndDirectAll(float waitTime){
		yield return new WaitForSeconds(waitTime);
		animalGenController.GoDirectGoalAll();
	}
	
	private IEnumerator WaitAndDeactivatePowerup(float waitTime){
		yield return new WaitForSeconds(waitTime);		
		DeactivatePowerup();
	}
	
	private void DeactivatePowerup(){
		gdc.cloneLocationIndex = -1;
		gdc.prevPowerup = gdc.currentPowerup;
		gdc.currentPowerup = PowerUpChecker.Powerups.none;
	}
	
	private void randomDelay(){
		delay = UnityEngine.Random.Range(minDelay,maxDelay);
	}
	
	private void addPowerup(){	
		if(gdc.GetLife() <= 0 || !gdc.isGetSetGoDone || gdc.SaveAnimal < addPowerUpThreshold ){
			return;
		}		
		if(!gdc.hasPowerUp && gdc.HasTutorial == 1 && gdc.currentLevel > 1){
			if(powerupContainer==null){
				powerupContainer = new GameObject();
				powerupContainer.name ="powerupContainer";
			}			

			int randomPower = UnityEngine.Random.Range(0,powerupCount);
			while(randomPower == prevPowerup || (randomPower == 4 && gdc.SaveAnimal > gdc.ReverseAnimalThreshold)){
				randomPower = UnityEngine.Random.Range(0,powerupCount);
			}

			prevPowerup = randomPower;

			PowerUpChecker.Powerups powerType = ConvertIntToPowerType(randomPower);
			bool found = GetPowerupPoolByPowerType(powerType);
			randomSpawn();
			
			if(!found){
				Powerup powerup = new Powerup();
				powerup.id = powerupPool.Count.ToString();
				powerup.powerupType = powerType;
				powerup.isActive = true;
				
				Rigidbody ballInstance;
		        ballInstance =Instantiate(powerups[randomPower], currentPosition, generatorPos.rotation) as Rigidbody;
				ballInstance.gameObject.transform.parent = powerupContainer.transform;		
				powerup.physicsObject = ballInstance;
				powerupPool.Add( powerup );
				DisplayLog("create new powerup reason new poweruptype check powerupPool len " + powerupPool.Count );
			}else{
				ActivatePowerupByPowerupType( powerType );
				//Debug.Log( "reuse powerup reason old poweruptype check powerupPool len " + powerupPool.Count );				
				DisplayLog("reuse powerup reason old poweruptype check powerupPool len " + powerupPool.Count);
			}	
		}
	}
	
	public void ClearPowerupPool(){
		DeactivateAllPowerup();
		int len = powerupPool.Count;		
		for(int index = 0; index<len;index++){
			if(powerupPool[index] != null){
				Destroy( powerupPool[index].physicsObject.gameObject );	
			}			
		}
		powerupPool.Clear();
	}
	
	private bool GetPowerupPoolByPowerType( PowerUpChecker.Powerups powerupType ){
		int len = powerupPool.Count;
		bool found = false;	
		
		for(int index = 0; index<len;index++){
			if(powerupPool[index].powerupType == powerupType &&  !powerupPool[index].isActive){
				found = true;
				break;
			}
		}		
		return found;	
	}
	
	public void DeactivatePowerupByPowerupType( PowerUpChecker.Powerups powerupType){
		int len = powerupPool.Count;		
		for(int index = 0; index<len;index++){
			if(powerupPool[index].powerupType == powerupType && powerupPool[index].isActive ){
				CleanPowerup(powerupPool[index]);
				DisplayLog("Deactivating Powerup preparation for powerup pooling check powerupPool len " + powerupPool.Count);
				break;
			}
		}
	}
	
	private void CleanPowerup( Powerup powerup ){
		powerup.isActive = false;
		powerup.physicsObject.velocity = Vector3.zero;
		powerup.physicsObject.gameObject.transform.position = new Vector3(-10000,0,0);
		powerup.physicsObject.gameObject.SetActive(false);
	}
	
	public void DeactivateAllPowerup(){
		int len = powerupPool.Count;		
		for(int index = 0; index<len;index++){
			if(powerupPool[index]!= null && powerupPool[index].isActive ){
				CleanPowerup(powerupPool[index]);
				DisplayLog("Deactivating All Powerup preparation for powerup pooling check powerupPool len " + powerupPool.Count);
			}
		}
		
		DeactivatePowerup();		
		DeactivatePowerupSlider();
	}
	
	public void ActivatePowerupByPowerupType( PowerUpChecker.Powerups powerupType ){
		int len = powerupPool.Count;		
		for(int index = 0; index<len;index++){
			if(powerupPool[index].powerupType == powerupType && !powerupPool[index].isActive ){
				powerupPool[index].physicsObject.gameObject.SetActive(true);
				powerupPool[index].isActive = true;
				powerupPool[index].physicsObject.gameObject.transform.position = currentPosition;
				DisplayLog("Activating Powerup preparation for powerup pooling check powerupPool len " + powerupPool.Count);
				break;
			}
		}
	}	
	
	private PowerUpChecker.Powerups ConvertIntToPowerType(int val){
		PowerUpChecker.Powerups powerType = PowerUpChecker.Powerups.none;
		switch(val){
			case 0:
				powerType = PowerUpChecker.Powerups.Duplicate;
			break;
			
			case 1:
				powerType = PowerUpChecker.Powerups.Overgrowth;
			break;
			
			case 2:
				powerType = PowerUpChecker.Powerups.ClearAll;
			break;
			
			case 3:
				powerType = PowerUpChecker.Powerups.Slow;
			break;
			
			case 4:
				powerType = PowerUpChecker.Powerups.AutoPilot;
			break;
			
			case 5:
				powerType = PowerUpChecker.Powerups.Life;
			break;
			
			case 6:
				powerType = PowerUpChecker.Powerups.AdvanceLevel;
			break;
		}
		
		return powerType;
	}
	
	private void removePowerup(){
		PowerUpChecker.Powerups lastPowerup = gdc.currentPowerup;

		if(gdc.currentPowerup == PowerUpChecker.Powerups.Duplicate){
			removeWhale();
		}else if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth){
			ScaleToNormal();
		}else if(gdc.currentPowerup == PowerUpChecker.Powerups.ClearAll){
			DeactivatePowerup();
		}else if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
			DeactivatePowerup();
		}else if(gdc.currentPowerup == PowerUpChecker.Powerups.AutoPilot){
			DeactivatePowerup();
		}else if(gdc.currentPowerup == PowerUpChecker.Powerups.Life){
			DeactivatePowerup();
		}

		DeactivatePowerupSlider();

		if(null != RemovePowerup){
			RemovePowerup(lastPowerup);
		}
	}
	
	public void OverGrowth(){
		if(!gdc.hasPowerUp){
			gdc.currentPowerup = PowerUpChecker.Powerups.Overgrowth;
			gdc.hasPowerUp = true;
			GetWhale();
			ActivatePowerupSlider(PowerUpChecker.Powerups.Overgrowth);
			animalGenController.GoDirectGoalAll();
			whaleAnimation.OverGrowth();
			//StartCoroutine(WaitAndDirectAll(0.5f));
		}		
	}
	
	public void ScaleToNormal(){
		GetWhale();
		whaleAnimation.scaleToNormal();
		if(null!=ScaleToNormalStart){
			ScaleToNormalStart();
		}
		//StartCoroutine(WaitAndDeactivatePowerup(0.5f));
	}
	
	public void QuickScaleToNormal(){
		GetWhale();
		if( whale == null ) return;
		whaleAnimation.QuickScaleToNormal();
		if(this == null)return;
		StartCoroutine(WaitAndDeactivatePowerup(0.3f));
	}
	
	private void removeWhale(){
		if(whales.Count > 0){
			gdc.cloneLocationIndex = -1;
			gdc.currentPowerup = PowerUpChecker.Powerups.none;
			GameObject whale = whales[0] as GameObject;
			Destroy(whale);
			whales.RemoveAt(0);			
			//Debug.Log( "destroy whale!!!!" );
		}
	}
	
	public void spawnWhale(){
		//if(!isStartPowerUp){
		if(!gdc.hasPowerUp){
			gdc.currentPowerup = PowerUpChecker.Powerups.Duplicate;
			Rigidbody whaleInstance;
			int randomX = UnityEngine.Random.Range(1,4);		
			Vector3 rndPosition = point1;
			
			while(gdc.locationIndex == ( randomX - 1 )){
				randomX =UnityEngine.Random.Range(1,4);
			}
			
			if( randomX == 1 ){
				rndPosition = gdc.PaddlePoint1;
				gdc.cloneLocationIndex = 0;
			}else if( randomX == 2 ){
				rndPosition = gdc.PaddlePoint2;
				gdc.cloneLocationIndex = 1;
			}else if( randomX == 3 ){
				rndPosition = gdc.PaddlePoint3;
				gdc.cloneLocationIndex = 2;
			}
			
	        whaleInstance = Instantiate(whalePrefab, rndPosition , whalePrefab.rotation) as Rigidbody;		
			whaleInstance.gameObject.name= "Fakewhale";
			
			PaddleScript paddleScript = whaleInstance.GetComponent<PaddleScript>();
			paddleScript.enabled = false;
			
			AutoPilotController autoPilot = whaleInstance.GetComponent<AutoPilotController>();
			autoPilot.enabled = false;
			
			whaleInstance.transform.parent = whaleHolder.transform;
			whales.Add(whaleInstance.gameObject);			

			gdc.hasPowerUp = true;
			ActivatePowerupSlider(PowerUpChecker.Powerups.Duplicate);

			if(null!=ActivatePowerup){
				ActivatePowerup(PowerUpType.Duplicate);
			}
		}		
	}
	
	public void AddExtraLife(){
		if(gdc.GetLife() < gdc.MaxLife ){
			gdc.UpdateLife(1);
		}		
		gdc.currentPowerup= PowerUpChecker.Powerups.Life;
		Messenger.Broadcast(GameEvent.GOT_EXTRA_LIFE);

		if(null!=ActivatePowerup){
			ActivatePowerup(PowerUpType.Life);
		}
	}

	public void AdvanceLevel(){
		gdc.currentLevel++;
		gdc.currentPowerup= PowerUpChecker.Powerups.AdvanceLevel;
		ClearAnimals();
	}
	
	private void randomSpawn(){
		randomPosition = UnityEngine.Random.Range(1,4);
		
		if( randomPosition == 1 ){
			currentPosition = point1;
		}else if( randomPosition == 2 ){
			currentPosition = point2;
		}else if( randomPosition == 3 ){
			currentPosition = point3;
		}
	}
	
	public void ClearAnimals(){		
		gdc.currentPowerup= PowerUpChecker.Powerups.ClearAll;
		if(null!=ActivatePowerup){
			ActivatePowerup(PowerUpType.ClearAll);
		}
	}
	
	public void GoSlowSpeed(){
		if(!gdc.hasPowerUp){
			gdc.currentPowerup= PowerUpChecker.Powerups.Slow;
			gdc.hasPowerUp = true;
			ActivatePowerupSlider(PowerUpChecker.Powerups.Slow);

			if(null!=ActivatePowerup){
				ActivatePowerup(PowerUpType.Slow);
			}
		}
	}
	
	public void GoAutoPilot(){
		if(!gdc.hasPowerUp){
			gdc.currentPowerup= PowerUpChecker.Powerups.AutoPilot;
			gdc.hasPowerUp = true;
			ActivatePowerupSlider(PowerUpChecker.Powerups.AutoPilot);

			if(null!=ActivatePowerup){
				ActivatePowerup(PowerUpType.AutoPilot);
			}
		}
	}

	private float GetPowerUpIntervalByPowerUpType(PowerUpChecker.Powerups powerType){
		float interval = 0;

		switch(powerType){
			case PowerUpChecker.Powerups.Duplicate:
				interval = 1f;
			break;

			case PowerUpChecker.Powerups.Overgrowth:
				interval = 1f;
			break;

			case PowerUpChecker.Powerups.ClearAll:
				interval = 1f;
			break;

			case PowerUpChecker.Powerups.Slow:
				//optimal 2f
				interval = 2f;
			break;

			case PowerUpChecker.Powerups.AutoPilot:
				interval = 1f;
			break;		
		}

		return interval;
	}

	private void DisplayLog(string message){
		if(isVerbose){
			Debug.Log(message);
		}
	}
}
