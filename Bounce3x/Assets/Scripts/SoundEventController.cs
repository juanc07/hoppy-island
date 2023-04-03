using UnityEngine;
using System.Collections;

public class SoundEventController : MonoBehaviour {
	
	private SoundEffectController sec;
	// Use this for initialization
	void Start () {
		sec = GameObject.Find("SFXManager").GetComponent<SoundEffectController>();
		Messenger.AddListener( GameEvent.Levelup, OnLevelup );
	}
	
	private void OnLevelup(){
		if(this == null)return;
		sec.PlaySfx( SoundEffectController.Effects.Levelup );	
	}	
}
