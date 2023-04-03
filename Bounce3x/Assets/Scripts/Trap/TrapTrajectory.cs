using UnityEngine;
using System.Collections;

public class TrapTrajectory : MonoBehaviour{	
	
	public Transform spawnTarget;
	private Vector3 startingVelocity;
	private float horizontalOffset = 0.0f;
	public float distance = 1.0f;
	
	//private float lob = 0.75f;
	public float lob;
	private float minLob;
	//private float maxLob;	
	
	public Transform scoreComboLabel;
	
	public Vector3 currentPoint = new Vector3(0,0,0);
	
	private bool hasFire = false;
	private int hitTimes=0;
	
	private GameDataManagerController gdc;	
	public Trap.TrapTypes trapType = Trap.TrapTypes.Bomb;
	
	private int id;
	public int score = 10;
	
	private GameObject trapManager;
	private TrapManagerController trapManagerController;
	
	private ParticleManager particleManager;
	private SoundEffectController sec;	
	public Transform rippleEffect;	
	private Vector3 originalLocation;
	
	
	void Awake(){		
	}
	
	// Use this for initialization
	void Start(){		
		gdc = GameDataManagerController.GetInstance();
		minLob = gdc.minLob;
		//maxLob = gdc.maxLob;
		
		particleManager = GameObject.Find("ParticleManager").GetComponent<ParticleManager>();
		
		trapManager = GameObject.Find("TrapManager");
		trapManagerController = trapManager.GetComponent<TrapManagerController>();
		
		originalLocation = this.gameObject.transform.position;
		//lob = 3.75f;
		lob = minLob;
		if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth){			
			currentPoint = gdc.point1b;
		}else{
			currentPoint = gdc.point1;
		}	
		
		sec = GameObject.Find("SFXManager").GetComponent<SoundEffectController>();		
	}
	
	private void randomLob(){
		if(gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth){
			lob = 0f;
		}else if(gdc.currentPowerup == PowerUpChecker.Powerups.Slow){
			lob = 5f;
		}else{
			//lob = Random.Range(minLob,maxLob);
			lob=trapManagerController.GetLob();
		}			
	}
	
	// Update is called once per frame
	void Update (){
		if(!hasFire){
			hasFire =true;
			startingVelocity = GetTrajectoryVelocity(spawnTarget.position, currentPoint, lob, Physics.gravity);				
			FireProjectile();
		}	
		
		horizontalOffset = Mathf.Clamp(horizontalOffset, -45.0f, 35.0f);		
		
		//lob += Input.GetAxis("Mouse ScrollWheel") * 0.01f;
		
		//lob = Mathf.Clamp(lob, 0.25f, 1.2f);
		
		distance = Mathf.Clamp(distance, 0.25f, 1.5f);
		
		//Debug.Log("check animal x " + this.transform.position.x + " y " + this.transform.position.y );
	}
	
	private void FixedUpdate(){		
		float animalX = gameObject.transform.position.x;
		float animalY = gameObject.transform.position.y;
		
		if(animalX <= -350){
			trapManagerController.DeactivateTrapById( Id,true);
			sec.PlaySfx( SoundEffectController.Effects.Drop );
			
		}else if(animalY <= -200){
			trapManagerController.DeactivateTrapById( Id,false);			
			sec.PlaySfx( SoundEffectController.Effects.Goal );			
		}		
	}	
	
	public static Vector3 GetTrajectoryVelocity(Vector3 startingPosition, Vector3 targetPosition, float lob, Vector3 gravity)
	{
		float physicsTimestep = Time.fixedDeltaTime;
	    float timestepsPerSecond = Mathf.Ceil(1f/physicsTimestep);
		
		//By default we set n so our projectile will reach our target point in 1 second
	    float n = lob * timestepsPerSecond;
		
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
	
	void OnTriggerEnter( Collider col ){	
		if(col.gameObject.tag == "paddle"){
			//Debug.Log("animal collide!!");
			rigidbody.velocity = Vector3.zero;
			
			if( gdc.currentPowerup == PowerUpChecker.Powerups.Overgrowth ){
				DirectGoal();
			}else{
				if( hitTimes == 0){
					particleManager.CastParticle2(ParticleManager.ParticleTypes.whaleHit,ParticleManager.Locations.left );
					hitTimes++;				
					randomLob();
					currentPoint = gdc.point2;
					hasFire = false;				
					trapManagerController.AddTrap(0);
				}else if( hitTimes == 1){
					particleManager.CastParticle2(ParticleManager.ParticleTypes.whaleHit,ParticleManager.Locations.center );
					hitTimes++;
					randomLob();				
					currentPoint = gdc.point3;				
					trapManagerController.AddTrap(1);
					hasFire = false;
				}else if( hitTimes >= 2){
					particleManager.CastParticle2(ParticleManager.ParticleTypes.whaleHit,ParticleManager.Locations.right );
					trapManagerController.AddTrap(2);
					DirectGoal();				
				}	
			}			
			sec.PlaySfx( SoundEffectController.Effects.Bounce );			
		}
		
		if(col.gameObject.tag == "RippleTrigger"){			
			Instantiate( rippleEffect, this.gameObject.transform.position, Quaternion.Euler(0,0,0) );			
			//Debug.Log( " animal hit RippleTrigger ");
		}
	}
	
	public void DirectGoal(){
		hitTimes = 2;
		currentPoint = gdc.point4;
		hasFire = false;
		lob = 3f;
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
			currentPoint = gdc.point1b;
		}else{
			currentPoint = gdc.point1;
		}		
	}
}
