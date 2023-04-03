using UnityEngine;
using System.Collections;

public class MusicBtn : MonoBehaviour {
	
	private bool isMusicOn=false;
	private Transform musicBtn;
	private GameDataManagerController gdc;
	//private MusicManagerController mmc;
	
	private Transform optionPanel;	
	private GameObject opttionGUIAnchor;

	private SoundManager soundManager;
	
	// Use this for initialization
	void Start (){
		gdc = GameDataManagerController.GetInstance();
		soundManager = SoundManager.GetInstance();
		//mmc =  GameObject.Find("MusicManager").GetComponent<MusicManagerController>();
		
		opttionGUIAnchor = GameObject.Find("OptionGUI/Camera/Anchor");
		optionPanel = opttionGUIAnchor.transform.Find("OptionPanel");	
		
		musicBtn = optionPanel.transform.Find("MusicBtn");

		int hasSave = SaveDataManager.LoadIntSaveData(PlayerDataKey.HAS_SAVE.ToString());
		if(hasSave==1){
			LoadUserSettings();
		}else{
			LoadDefaultSettings();
		}

		/*if(!gdc.IsFirstLaunch){
			LoadDefaultSettings();
		}else{
			LoadUserSettings();
		}*/
	}
	
	private void OnMusic(){
		isMusicOn =true;
		musicBtn.transform.Find("OffBG").gameObject.SetActive(false);
		musicBtn.transform.Find("OnBG").gameObject.SetActive(true);		

		if(!soundManager.IsBgmOn && !gdc.IsMusicOn){
			gdc.IsMusicOn = true;
			soundManager.IsBgmOn = true;
			if(soundManager.bgmAudioSource.isPlaying){
				soundManager.UnMuteBGM();
			}else{
				soundManager.PlayBGM(BGM.InGameBGM,1f,true);
			}
		}

		/*mmc.UnMute();
		if(!mmc.IsPlaying){
			mmc.PlayRandomBGM();
		}*/

		SaveDataManager.SaveData(PlayerDataKey.BGM.ToString(),1);
		SaveDataManager.SaveData(PlayerDataKey.HAS_SAVE.ToString(),1);
		//Debug.Log("on music");
	}
	
	private void OffMusic(){
		isMusicOn =false;
		musicBtn.transform.Find("OffBG").gameObject.SetActive(true);
		musicBtn.transform.Find("OnBG").gameObject.SetActive(false);

		if(soundManager.IsBgmOn && gdc.IsMusicOn){
			gdc.IsMusicOn = false;
			soundManager.IsBgmOn = false;
			soundManager.MuteBGM();
		}

		//mmc.Mute();

		SaveDataManager.SaveData(PlayerDataKey.BGM.ToString(),0);
		SaveDataManager.SaveData(PlayerDataKey.HAS_SAVE.ToString(),1);
		//Debug.Log("off music");
	}
	
	private void ToggleMusic(){	
		if(isMusicOn){
			OffMusic();
		}else{
			OnMusic();
		}
	}
	
	private void OnClick(){
		ToggleMusic();
	}
	
	private void LoadDefaultSettings(){		
		OnMusic();		
	}
	
	private void LoadUserSettings(){
		int bgm = SaveDataManager.LoadIntSaveData(PlayerDataKey.BGM.ToString());
		if(bgm==1){
			gdc.IsMusicOn = true;
			OnMusic();
		}else{
			OffMusic();
		}
		/*if(gdc.IsMusicOn){
			OnMusic();
		}else{
			OffMusic();	
		}*/
	}
}
