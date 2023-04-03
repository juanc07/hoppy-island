using UnityEngine;
using System.Collections;


public class MusicManagerController : MonoBehaviour {
	
	public AudioClip bgm1;
	public AudioClip bgm2;
	private int rnd;
	private bool isPlaying;
	
	private GameDataManagerController gdc;
	
	public enum BgmList{
		BGM1,
		BGM2		
	}
	
	void Awake(){
		gdc = GameDataManagerController.GetInstance();
	}
	
	// Use this for initialization
	void Start (){				
		PlayRandomBGM();
	}
	
	// Update is called once per frame
	void Update (){	
	}
	
	public void PlayRandomBGM(){
		/*rnd = Random.Range(0,2);		
		if(rnd == 0){
			PlayBGM( BgmList.BGM1 );
		}else{
			PlayBGM( BgmList.BGM2 );
		}*/
		PlayBGM( BgmList.BGM1 );
	}
	
	public void Mute(){
		audio.volume = 0f;
	}
	
	public void UnMute(){
		audio.volume = 0.75f;
	}
	
	public void PlayBGM( BgmList bgm ){
		if( !gdc.IsMusicOn ){
			return;
		}
		
		switch(bgm){
			case BgmList.BGM1:
				audio.clip = bgm1;				
				audio.loop = true;
				audio.Play(  );	
				isPlaying = true;
				//Debug.Log( " play bgm1 " );
			break;
			
			case BgmList.BGM2:
				audio.clip = bgm2;				
				audio.loop = true;
				audio.Play(  );	
				isPlaying = true;
				//Debug.Log( " play bgm2 " );
			break;			
		}
	}
	
	public bool IsPlaying{
		get{ return isPlaying; }
		set{ isPlaying = value;}
	}
}
