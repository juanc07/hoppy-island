using UnityEngine;
using System.Collections;

public class SoundEffectController : MonoBehaviour {
		
	public AudioClip bounce;
	public AudioClip bounce2;
	
	public AudioClip powerup;
	public AudioClip extraLife;
	public AudioClip goal;
	public AudioClip drop;
	
	public AudioClip levelup;
	public AudioClip gameover;
	public AudioClip restart;
	
	public AudioClip pig;
	public AudioClip chicken;
	public AudioClip cow;
	public AudioClip cat;
	
	public AudioClip sheep;
	public AudioClip dog;
	public AudioClip frog;
	public AudioClip parrot;
	
	
	public AudioClip click;
	public AudioClip sunlight;
	
	public AudioClip overgrowth;
	
	public Hashtable sfxSet = new Hashtable();
	
	private GameDataManagerController gdc;
	
	public enum Effects{
		Bounce,
		Powerup,
		Goal,
		Life,
		Drop,
		Levelup,
		Gameover,
		Pig,
		Chicken,
		Cow,
		Cat,
		Restart,
		Click,
		Sunlight,
		Sheep,
		Dog,
		Frog,
		Parrot,
		Overgrowth,
		Bounce2,
	}
	
	// Use this for initialization
	void Start () {
		gdc = GameDataManagerController.GetInstance();
		audio.volume = 1f;
		
		//string sunlightKey =Effects.Sunlight.ToString();
		//sfxSet[sunlightKey]=sunlight;
		
		sfxSet[Effects.Bounce]=bounce;
		sfxSet[Effects.Bounce2]=bounce2;
		
		sfxSet[Effects.Powerup]=powerup;
		sfxSet[Effects.Goal]=goal;
		sfxSet[Effects.Life]=extraLife;
		sfxSet[Effects.Drop]=drop;
		sfxSet[Effects.Levelup]=levelup;
		sfxSet[Effects.Gameover]=gameover;
		sfxSet[Effects.Restart]=restart;
		sfxSet[Effects.Pig]=pig;
		sfxSet[Effects.Chicken]=chicken;
		sfxSet[Effects.Cow]=cow;
		sfxSet[Effects.Cat]=cat;		
		sfxSet[Effects.Click]=click;
		sfxSet[Effects.Sunlight]=sunlight;
		
		sfxSet[Effects.Sheep]=sheep;
		sfxSet[Effects.Dog]=dog;
		sfxSet[Effects.Frog]=frog;
		sfxSet[Effects.Parrot]=parrot;
		sfxSet[Effects.Overgrowth]=overgrowth;
	}
	
	// Update is called once per frame
	/*void Update () {
	
	}*/
	
	public void Mute(){
		audio.volume = 0f;
		NGUITools.soundVolume=0f;
	}
	
	public void UnMute(){
		audio.volume = 1f;
		NGUITools.soundVolume=1f;
	}
	
	public void PlaySfxHash(Effects key, float vol = 1f){
		audio.volume = vol;
		audio.PlayOneShot( (AudioClip)sfxSet[key.ToString()] );
	}
	
	public void PlaySfx( Effects effect, float vol = 1f ){
		if( !gdc.IsSFXOn ){
			return;
		}
		
		audio.volume = vol;
		AudioClip currenfSfx = sfxSet[effect] as AudioClip;
		audio.PlayOneShot( currenfSfx );
		
		/*
		switch(effect){
			case Effects.Bounce:
				audio.volume = 0.7f;
				audio.PlayOneShot( bounce );
			break;
			
			case Effects.Powerup:
				//audio.volume = 1f;
				audio.PlayOneShot( powerup );
			break;
			
			case Effects.Goal:
				//audio.volume = 1f;
				audio.PlayOneShot( goal );
			break;
			
			case Effects.Life:
				//audio.volume = 1f;
				audio.PlayOneShot( extraLife );
			break;
			
			case Effects.Drop:
				//audio.volume = 1f;
				audio.PlayOneShot( drop );
			break;
			
			case Effects.Levelup:
				//audio.volume = 1f;
				audio.PlayOneShot( levelup );
			break;
			
			case Effects.Pig:
				//audio.volume = 0.5f;
				audio.PlayOneShot( pig );
			break;			
			
			case Effects.Chicken:
				audio.volume = 1f;
				audio.PlayOneShot( chicken );
			break;
			
			case Effects.Cow:
				audio.volume = 1f;
				audio.PlayOneShot( cow );
			break;
			
			case Effects.Cat:
				//audio.volume = 1f;
				audio.PlayOneShot( cat );
			break;
			
			case Effects.Restart:
				//audio.volume = 1f;
				audio.PlayOneShot( restart );
			break;
			
			case Effects.Click:
				//audio.volume = 1f;
				audio.PlayOneShot( click );
			break;
			
			case Effects.Gameover:
				//audio.volume = 1f;
				audio.clip = gameover;
				audio.PlayDelayed(0.5f);
				//audio.PlayOneShot( gameover );
			break;
		}*/
	}
}
