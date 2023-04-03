using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpChecker : MonoBehaviour {

	private GameObject powerUpManager;
	private PowerUpManagerController powerUpManagerController;
	private GameDataManagerController gdc;
	
	public Powerups PowerUpType =Powerups.Duplicate;	
	public Transform powerupLabel;
	
	private SoundEffectController sec;
	
	public enum Powerups{
		none,
		Duplicate,
		Overgrowth,
		ClearAll,
		Slow,
		AutoPilot,
		Life,
		AdvanceLevel
	}
	
	private ParticleManager particleManager;
	// Use this for initialization
	void Start (){		
		gdc =GameDataManagerController.GetInstance();
		particleManager = GameObject.Find("ParticleManager").GetComponent<ParticleManager>();
		
		powerUpManager = GameObject.Find("PowerUpManager");
		powerUpManagerController = powerUpManager.GetComponent<PowerUpManagerController>();
		
		sec = GameObject.Find("SFXManager").GetComponent<SoundEffectController>();
	}
	
	// Update is called once per frame
	void Update () {
		//float posX = gameObject.transform.position.x;
		float posY = gameObject.transform.position.y;
		
		if(posY <= -380){
			powerUpManagerController.DeactivatePowerupByPowerupType(PowerUpType);
			//Destroy(this.gameObject);
		}
	}
	
	private void OnTriggerEnter( Collider col ){		
		if( col.gameObject.name != "fakewhale" && col.gameObject.tag == "paddle" && !gdc.hasPowerUp ){
			
			switch(PowerUpType){
				case Powerups.Duplicate:
					powerUpManagerController.spawnWhale();
					sec.PlaySfx(SoundEffectController.Effects.Powerup);
				break;
				
				case Powerups.Overgrowth:
					powerUpManagerController.OverGrowth();
					//sec.PlaySfx(SoundEffectController.Effects.Powerup);
					sec.PlaySfx(SoundEffectController.Effects.Overgrowth);
				break;
				
				case Powerups.ClearAll:
					powerUpManagerController.ClearAnimals();
					sec.PlaySfx(SoundEffectController.Effects.Powerup);
				break;
				
				case Powerups.Slow:
					powerUpManagerController.GoSlowSpeed();
					sec.PlaySfx(SoundEffectController.Effects.Powerup);
				break;
				
				case Powerups.AutoPilot:
					powerUpManagerController.GoAutoPilot();
					sec.PlaySfx(SoundEffectController.Effects.Powerup);
				break;
				
				case Powerups.Life:
					powerUpManagerController.AddExtraLife();
					sec.PlaySfx(SoundEffectController.Effects.Life);
				break;

				case Powerups.AdvanceLevel:
					powerUpManagerController.AdvanceLevel();
					sec.PlaySfx(SoundEffectController.Effects.Levelup);
				break;
				
				default:
					powerUpManagerController.spawnWhale();			
					sec.PlaySfx(SoundEffectController.Effects.Powerup);
				break;
			}			
			
			ParticleManager.Locations loc;
			if(gdc.locationIndex == 0){
				loc = ParticleManager.Locations.left;
			}else if(gdc.locationIndex == 1){
				loc = ParticleManager.Locations.center;
			}else{
				loc = ParticleManager.Locations.right;
			}
			
			GameObject inGamePanel = GameObject.Find("InGamePanel");		
			Transform scLabel  =  Instantiate(powerupLabel, new Vector3(0,0,0), inGamePanel.transform.rotation ) as Transform;
			scLabel.name = "PowerupLabel";
			scLabel.transform.parent = inGamePanel.transform;		
			scLabel.localScale = new Vector3( 58.54361f,58.54361f,0 );
			
			
			particleManager.CastParticle2(ParticleManager.ParticleTypes.getPower,loc );
			//Destroy( this.gameObject );
			powerUpManagerController.DeactivatePowerupByPowerupType(PowerUpType);
		}	
	}
}
