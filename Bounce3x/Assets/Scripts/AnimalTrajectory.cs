using UnityEngine;
using System.Collections;

public class AnimalTrajectory : MonoBehaviour{	
	
	public Transform spawnTarget;
	private Vector3 startingVelocity;
	private float horizontalOffset = 0.0f;
	public float distance = 1.0f;	

	public float lob;
	private float minLob;
	
	public Transform scoreComboLabel;
	
	public Vector3 currentPoint = new Vector3(0,0,0);	

	public bool isReady = false;
	private bool hasFire = false;
	private int hitTimes=0;
	
	private GameDataManagerController gdc;
	public Animals animalType = Animals.Pig;
	private int id;
	public int score = 10;
	
	private GameObject animalGenerator;
	private AnimalGeneratorController animalGenController;
	
	private ParticleManager particleManager;
	private SoundEffectController sec;
	
	private Vector3 originalLocation;
	private float rotateSpeed = 60f;
	private Vector3 rotateDir;

	private bool isHit = false;
	private float hitDelay = 0.3f;

	private TextComboManager textComboManager;
	private float slowMaxFallSpeed = -200f;
	private Vector3 tempRigidForce;

	private bool isSlow;
	public bool IsSlow{
		set{isSlow = value;}
		get{return isSlow;}
	}

	//new
	[SerializeField]
	private bool isReverse=false;
	
	public enum Animals{
		Pig,
		Chicken,
		Cow,
		Cat,
		Sheep,
		Parrot,
		Dog,
		Frog
	}

	private PowerUpManagerController powerUpManagerController;
	
	// Use this for initialization
	void Start(){		
		gdc = GameDataManagerController.GetInstance();
		textComboManager = GameObject.FindObjectOfType<TextComboManager>();

		GetPowerUpManager();

		minLob = gdc.minLob;
		
		particleManager = GameObject.Find("ParticleManager").GetComponent<ParticleManager>();		
		originalLocation = this.gameObject.transform.position;

		int rndRotate = Random.Range(0,3);
		rotateSpeed = Random.Range(50f,90f);
		
		
		if(rndRotate == 0){
			rotateDir = Vector3.forward;	
		}else if(rndRotate == 1){
			rotateDir = Vector3.up;
		}else if(rndRotate == 2){
			rotateDir = Vector3.down;
		}		
		
		sec = GameObject.Find("SFXManager").GetComponent<SoundEffectController>();
		AnimalVoice();
		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void OnEnable(){
		GetPowerUpManager();
	}

	private void GetPowerUpManager(){
		if(powerUpManagerController==null){
			powerUpManagerController = GameObject.FindObjectOfType<PowerUpManagerController>();
		}
	}

	private void AddEventListener(){
		if(powerUpManagerController!=null){
			powerUpManagerController.OnActivatePowerup+=OnActivatePowerup;
		}
	}

	private void RemoveEventListener(){
		if(powerUpManagerController!=null){
			powerUpManagerController.OnActivatePowerup-=OnActivatePowerup;
		}
	}

	private void OnActivatePowerup(PowerUpType powerUpType){
		if(this ==null) return;

		if(powerUpType != PowerUpType.Slow ){
			if(!isReverse){
				//DirectGoal();
				ClearDirectGoal();
			}else{
				//DirectGoalLeft();
				ClearDirectGoalLeft();
			}
		}
	}	
	
	private void OnDeactivateSlowPowerup(){
		if(this ==null) return;

		/*hasActivatedSlowEffect = false;
		hasTriggeredSlowCheck = false;*/

		if(!isReverse){
			//DirectGoal();
			ClearDirectGoal();
		}else{
			//DirectGoalLeft();
			ClearDirectGoalLeft();
		}
	}
	
	public void AnimalVoice(){
		switch(animalType){
		case Animals.Pig:
			sec.PlaySfx(SoundEffectController.Effects.Pig, 0.5f);
			break;
			
		case Animals.Chicken:
			sec.PlaySfx(SoundEffectController.Effects.Chicken, 0.5f);
			break;
			
		case Animals.Cow:
			sec.PlaySfx(SoundEffectController.Effects.Cow, 0.5f);
			break;
			
		case Animals.Cat:
			sec.PlaySfx(SoundEffectController.Effects.Cat, 0.5f);
			break;
			
		case Animals.Sheep:
			sec.PlaySfx(SoundEffectController.Effects.Sheep,0.5f);
			break;
			
		case Animals.Dog:
			sec.PlaySfx(SoundEffectController.Effects.Dog,0.5f);
			break;
			
		case Animals.Frog:
			sec.PlaySfx(SoundEffectController.Effects.Frog,0.5f);
			break;
			
		case Animals.Parrot:
			sec.PlaySfx(SoundEffectController.Effects.Parrot,0.5f);
			break;
		}
	}
	
	// Update is called once per frame
	void Update (){
		if(isReady){
			if(!hasFire){
				hasFire =true;
				if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
					startingVelocity = GetTrajectoryVelocity(spawnTarget.position,currentPoint,4f, Physics.gravity);
				}else if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth){
					startingVelocity = GetTrajectoryVelocity(spawnTarget.position, currentPoint, 1f, Physics.gravity);
				}else{
					startingVelocity = GetTrajectoryVelocity(spawnTarget.position, currentPoint, 3f, Physics.gravity);
				}
				FireProjectile();
			}
		}
	}
	
	private void FixedUpdate(){		
		float animalX = gameObject.transform.position.x;
		float animalY = gameObject.transform.position.y;

		if(!isReverse){
			if(animalX <= -350 || animalY > 350 ){		
				animalGenController.DeactivateAnimalById(Id,false);
				particleManager.CastParticle(ParticleManager.ParticleTypes.firework,ParticleManager.Locations.upRight );
				sec.PlaySfx( SoundEffectController.Effects.Goal );
			}
		}else{
			if(animalX >= 350 || animalY > 350 ){		
				animalGenController.DeactivateAnimalById(Id,false);
				particleManager.CastParticle(ParticleManager.ParticleTypes.firework,ParticleManager.Locations.upleft );
				sec.PlaySfx( SoundEffectController.Effects.Goal );
			}
		}

		if(animalY <= -150){
			FallAnimal();			
		}

		//extra down force to make the animal fall faster
		if(this.rigidbody.useGravity){
			if(gdc.currentPowerup != PowerUpChecker.Powerups.Slow){
				this.rigidbody.AddForce(0,-150f,0, ForceMode.Acceleration);
			}

			if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow && isSlow){
				if(this.rigidbody.velocity.y <= slowMaxFallSpeed){
					tempRigidForce = this.rigidbody.velocity;
					tempRigidForce.y = slowMaxFallSpeed;
					this.rigidbody.velocity = tempRigidForce;
					//Debug.Log(" limit fall speed: " + this.rigidbody.velocity.y);
				}
			}
		}
		
		Quaternion deltaRotation = Quaternion.Euler(rotateDir * (Time.deltaTime * rotateSpeed));
		rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
	}
	
	private void FallAnimal(){
		gdc.CurrentCombo=0;
		animalGenController.DeactivateAnimalById(Id,true);
		Messenger.Broadcast(GameEvent.ANIMAL_FELL);
	}
	
	public static Vector3 GetTrajectoryVelocity(Vector3 startingPosition, Vector3 targetPosition, float lob, Vector3 gravity)
	{
		float physicsTimestep = Time.fixedDeltaTime;

		//makes animal jumps higher
		float timestepsPerSecond = Mathf.Ceil(1f/physicsTimestep);
		//Debug.Log(" timestepsPerSecond: " + timestepsPerSecond);

		//By default we set n so our projectile will reach our target point in 1 second
		//lob will also make the projectile fire higher and steeper
		float n = lob * timestepsPerSecond;
		//Debug.Log(" n " + n + " lob " + lob);
		
		Vector3 a = physicsTimestep * physicsTimestep * gravity;
		Vector3 p = targetPosition;
		Vector3 s = startingPosition;
		
		Vector3 velocity = (s + (((n * n + n) * a) / 2f) - p) * -1 / n;
		
		//This will give us velocity per timestep. The physics engine expects velocity in terms of meters per second
		velocity /= physicsTimestep;
		return velocity;
	}
	
	
	void FireProjectile(){		
		this.rigidbody.AddForce(startingVelocity, ForceMode.Impulse);	
	}
	
	public void SetCurrentIndex(int index){
		if(gdc==null){
			gdc = GameDataManagerController.GetInstance();
		}
		
		if(index == 0){
			hitTimes = 0;
			
			if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
				lob = gdc.initLob;
			}else{
				lob = gdc.initLob;
			}		

			if(!isReverse){
				if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth){
					currentPoint = gdc.point1OverGrowth;
				}else if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
					currentPoint = gdc.instantSlowPoint1;
				}else{
					currentPoint = gdc.point1;
				}
			}else{
				if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth){
					currentPoint = gdc.reverseOverGrowthPoint1;
					//Debug.Log("over growth reverse triggered!");
				}else if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
					currentPoint = gdc.instantReverseSlowPoint3;
				}else{
					currentPoint = gdc.reversePoint3;
				}
			}
		}else if(index == 1){
			hitTimes =1;

			if(!isReverse){
				if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
					currentPoint = gdc.instantSlowPoint2;
				}else if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth){
					currentPoint = gdc.point2OverGrowth;
				}else{
					currentPoint = gdc.instantPoint2;
				}
			}else{
				if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
					currentPoint = gdc.instantReverseSlowPoint2;
				}else{
					currentPoint = gdc.instantReversePoint2;
				}
			}
		}else if(index == 2){
			hitTimes =2;

			if(!isReverse){
				if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
					currentPoint = gdc.instantSlowPoint3;
				}else{
					currentPoint = gdc.instantPoint3;
				}
			}else{
				if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
					currentPoint = gdc.instantReverseSlowPoint1;
				}else{
					currentPoint = gdc.instantReversePoint1;
				}
			}
		}
		
		isReady = true;
		//Debug.Log("set current index "+ index);
	}
	
	void OnTriggerEnter( Collider col ){
		if(!isHit && col.gameObject.tag == "paddle"){
			isHit= true;
			Invoke("RefreshHit", hitDelay);
			rigidbody.velocity = Vector3.zero;

			if( hitTimes == 0){				
				hitTimes++;
				hasFire = false;

				if(!isReverse){
					if(gdc.currentPowerup != PowerUpChecker.Powerups.Overgrowth){
						particleManager.CastParticle2(ParticleManager.ParticleTypes.whaleHit,ParticleManager.Locations.left );
					}

					if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
						currentPoint = gdc.point2Slow;
					}else if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth){
						currentPoint = gdc.point2OverGrowth;
					}else{
						currentPoint = gdc.point2;
					}
				}else{
					if(gdc.currentPowerup != PowerUpChecker.Powerups.Overgrowth){
						particleManager.CastParticle2(ParticleManager.ParticleTypes.whaleHit,ParticleManager.Locations.right );
					}

					if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth){
						currentPoint = gdc.reverseOverGrowthPoint2;
						//Debug.Log("reverse overgrowth 2");
					}else if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
						currentPoint = gdc.reverseSlowPoint2;
					}else{
						currentPoint = gdc.reversePoint2;
					}
				}
			}else if( hitTimes == 1){
				if(gdc.currentPowerup != PowerUpChecker.Powerups.Overgrowth){
					particleManager.CastParticle2(ParticleManager.ParticleTypes.whaleHit,ParticleManager.Locations.center );
				}
				
				hitTimes++;
				hasFire = false;

				if(!isReverse){
					if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
						currentPoint = gdc.point3Slow;
					}else{
						currentPoint = gdc.point3;
					}
				}else{
					if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
						currentPoint = gdc.reverseSlowPoint1;
					}else{
						currentPoint = gdc.reversePoint1;
					}
				}
			}else if( hitTimes >= 2){
				if(!isReverse){
					if(gdc.currentPowerup != PowerUpChecker.Powerups.Overgrowth){
						particleManager.CastParticle2(ParticleManager.ParticleTypes.whaleHit,ParticleManager.Locations.right );
					}
					DirectGoal();
				}else{
					if(gdc.currentPowerup != PowerUpChecker.Powerups.Overgrowth){
						particleManager.CastParticle2(ParticleManager.ParticleTypes.whaleHit,ParticleManager.Locations.left );
					}
					DirectGoalLeft();
				}
			}
			
			ShowScore();
			TutorialChecker();
			sec.PlaySfx( SoundEffectController.Effects.Bounce);
		}

		if(col.gameObject.tag == "RippleTrigger"){
			//Debug.Log( " animal hit RippleTrigger ");			
			ParticleManager.Locations loc;			
			if( hitTimes == 0){
				loc = ParticleManager.Locations.left;
			}else if( hitTimes == 1){
				loc = ParticleManager.Locations.center;
			}else{
				loc = ParticleManager.Locations.right;				
			}			
			
			sec.PlaySfx( SoundEffectController.Effects.Drop );
			particleManager.ShowParticle(ParticleManager.ParticleTypes.waterRipple,this.gameObject.transform.position,Quaternion.Euler(0,0,0) );
			particleManager.CastParticle(ParticleManager.ParticleTypes.splash,loc, -2.2f );
		}
	}

	private void RefreshHit(){
		isHit = false;
	}

	private void TutorialChecker(){	
		if(gdc.HasTutorial==0){
			Messenger.Broadcast(GameEvent.FirstTap);
		}				
	}
	
	private void ShowScore(){		
		gdc.CurrentCombo++;		
		int score = (gdc.currentLevel * gdc.CurrentCombo);
		gdc.UpdateScore( score );			

		textComboManager.ShowTextCombo();		
		Messenger.Broadcast(GameEvent.GOT_SCORE);
	}
	
	public void DirectGoal(){
		hitTimes = 2;		
		hasFire = false;

		if(gdc.prevPowerup == PowerUpChecker.Powerups.Slow){
			currentPoint = gdc.pointSlowClearAll;
		}else{
			lob = 100f;
			currentPoint = gdc.pointClearAll;
		}
	}

	public void ClearDirectGoal(){
		hitTimes = 2;		
		hasFire = false;
		
		if(gdc.prevPowerup == PowerUpChecker.Powerups.Slow){
			currentPoint = gdc.pointSlowClearAllPowerUp;
		}else{
			currentPoint = gdc.pointClearAllPowerUp;
		}
	}

	public void DirectGoalLeft(){
		hitTimes = 2;		
		hasFire = false;
		currentPoint = gdc.pointLeftClearAll;
	}

	public void ClearDirectGoalLeft(){
		hitTimes = 2;		
		hasFire = false;
		currentPoint = gdc.pointLeftClearAllPowerUp;
	}

	public void HiDirectGoal(){
		if(!isReverse){
			ClearDirectGoal();
			/*if(gdc.prevPowerup == PowerUpChecker.Powerups.Slow){
				currentPoint = gdc.point4Slow;
			}else if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth){
				currentPoint = gdc.point2OverGrowth;
			}else if(gdc.prevPowerup == PowerUpChecker.Powerups.ClearAll ){
				currentPoint = gdc.pointClearAll;
			}else{
				currentPoint = gdc.point4;
			}*/
		}else{
			ClearDirectGoalLeft();
			//currentPoint = gdc.pointLeftClearAllPowerUp;
		}
	}
	
	public int Id{
		set{ id = value;}
		get{ return id; }
	}
	
	public void Reset(){
		this.gameObject.transform.position = originalLocation;
		hasFire = false;
		hitTimes = 0;
		
		minLob = gdc.minLob;
		
		lob = minLob;
		if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth){
			currentPoint = gdc.point1OverGrowth;
		}else if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
			currentPoint = gdc.point1Slow;
		}else{
			currentPoint = gdc.point1;
		}		
	}

	public int HitTimes{
		get{return hitTimes;}
	}

	public bool IsReverse{
		set{isReverse = value;}
		get{return isReverse;}
	}

	public AnimalGeneratorController AnimalGeneratorController{
		set{animalGenController=value;}
		get{return animalGenController;}
	}

	public Vector3 OriginalLocation{
		set{originalLocation=value;}
		get{return originalLocation;}
	}
}
