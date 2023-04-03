using UnityEngine;
using System.Collections;

public class SoundEffectBtn : MonoBehaviour {
	
	private bool isSFXOn=false;
	private Transform sfxBtn;
	
	private GameDataManagerController gdc;
	private SoundEffectController sec;
	
	private Transform optionPanel;	
	private GameObject opttionGUIAnchor;

	private SoundManager soundManager;

	// Use this for initialization
	void Start () {
		gdc = GameDataManagerController.GetInstance();
		soundManager = SoundManager.GetInstance();
		sec =  GameObject.Find("SFXManager").GetComponent<SoundEffectController>();
		
		opttionGUIAnchor = GameObject.Find("OptionGUI/Camera/Anchor");
		optionPanel = opttionGUIAnchor.transform.Find("OptionPanel");
		sfxBtn = optionPanel.transform.Find("SFXBtn");

		int hasSave = SaveDataManager.LoadIntSaveData(PlayerDataKey.HAS_SAVE.ToString());
		if(hasSave == 1){
			LoadUserSettings();
		}else{
			NGUITools.soundVolume=1f;
			EnableSoundEffect();
		}

		/*if(!gdc.IsFirstLaunch){
			LoadDefaultSettings();
			NGUITools.soundVolume=0;
			NGUITools.soundVolume=1f;
			NGUITools.soundVolume=0;
			NGUITools.soundVolume=1f;
			Debug.Log("1st lunch nguitool sound on!!");
		}else{
			LoadUserSettings();
		}*/
	}
	
	private void ToggleSoundEffect(){	
		if(isSFXOn){
			DisableSoundEffect();
		}else{
			EnableSoundEffect();
		}
	}
	
	private void EnableSoundEffect(){
		isSFXOn =true;
		sfxBtn.transform.Find("OffBG").gameObject.SetActive(false);
		sfxBtn.transform.Find("OnBG").gameObject.SetActive(true);
		//Debug.Log("on SFX");
		gdc.IsSFXOn = true;
		soundManager.IsSfxOn = true;
		sec.UnMute();

		SaveDataManager.SaveData(PlayerDataKey.SFX.ToString(),1);
		SaveDataManager.SaveData(PlayerDataKey.HAS_SAVE.ToString(),1);
	}
	
	private void DisableSoundEffect(){
		isSFXOn =false;
		sfxBtn.transform.Find("OffBG").gameObject.SetActive(true);
		sfxBtn.transform.Find("OnBG").gameObject.SetActive(false);
		//Debug.Log("off SFX");
		gdc.IsSFXOn = false;
		soundManager.IsSfxOn = false;
		sec.Mute();

		SaveDataManager.SaveData(PlayerDataKey.SFX.ToString(),0);
		SaveDataManager.SaveData(PlayerDataKey.HAS_SAVE.ToString(),1);
	}
	
	private void LoadDefaultSettings(){		
		EnableSoundEffect();
	}
	
	private void LoadUserSettings(){
		int sfx = SaveDataManager.LoadIntSaveData(PlayerDataKey.SFX.ToString());
		if(sfx==1){
			EnableSoundEffect();
		}else{
			DisableSoundEffect();
		}
		/*if(gdc.IsSFXOn){
			EnableSoundEffect();
		}else{
			DisableSoundEffect();
		}*/
	}
	
	private void OnClick(){
		ToggleSoundEffect();
	}
}
