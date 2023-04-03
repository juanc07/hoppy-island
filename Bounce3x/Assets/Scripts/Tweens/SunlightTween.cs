using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class SunlightTween : MonoBehaviour {
	
	private string id;
	private SunlightManagerController smc;
	private GameDataManagerController gdc;
	private ParticleManager particleManager;
	
	private SoundEffectController sec;	
	private TweenParms parms;

	public bool isVerbose = false;

	// Use this for initialization
	void Start (){
		gdc = GameDataManagerController.GetInstance();
		smc = GameObject.Find("SunlightManager").GetComponent<SunlightManagerController>();
		sec = GameObject.Find("SFXManager").GetComponent<SoundEffectController>();
		particleManager =  GameObject.Find("ParticleManager").GetComponent<ParticleManager>();
		Tween();
	}
	
	// Update is called once per frame
	void Update () {
	
	
	}
	
	private void OnTriggerEnter( Collider col ){		
		if( col.gameObject.name != "fakewhale" && col.gameObject.tag == "paddle" ){
			HOTween.Kill(this.gameObject.transform);
			smc.DeactivateSunlightById(Id);
			if(!gdc.IsKid){
				gdc.TotalGold++;	
			}else{
				gdc.TotalGold+=10;
			}
			
			gdc.SavePlayerCoin();
			//sec.PlaySfxHash(SoundEffectController.Effects.Sunlight);
			sec.PlaySfx(SoundEffectController.Effects.Sunlight);
			
			
			ParticleManager.Locations loc;
			if(gdc.locationIndex == 0){
				loc = ParticleManager.Locations.left;
			}else if(gdc.locationIndex == 1){
				loc = ParticleManager.Locations.center;
			}else{
				loc = ParticleManager.Locations.right;
			}
			
			particleManager.CastParticle(ParticleManager.ParticleTypes.sunlightHit,loc);
			DisplayLog("hit whale sunlight!!");
		}
	}
	
	public void Tween(){		
		parms = new TweenParms(); 		
		parms.Prop("position", new Vector3(this.gameObject.transform.position.x,-200f,this.gameObject.transform.position.z));		
		parms.Ease(EaseType.Linear);
		parms.UpdateType(UpdateType.Update);
		//parms.UpdateType(UpdateType.TimeScaleIndependentUpdate);
		//parms.Delay(0.4f);
		parms.AutoKill(true);
		parms.OnComplete(TweenComplete);		
		HOTween.To(this.gameObject.transform, 6f, parms );
	}
	
	private void TweenComplete(){
		DisplayLog("Sunlight Tween Done");
		smc.DeactivateSunlightById(Id);
	}
	
	public string Id{
		get{ return id;}
		set{ id = value;}
	}

	private void DisplayLog(string message){
		if(isVerbose){
			Debug.Log(message);
		}
	}
}
