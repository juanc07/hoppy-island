using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeyzapWrapper : MonoBehaviour {

/*
	public string leaderboardId ="";
	
	public string[] leaderboardIds;
	
	public int iosAppId=0;
	public string iosURLSchema="";
	
	public bool isShowAd=false;
	public bool isBannerAd=false;
	
	private bool isStarted =false;
	
	public string[] achievements;
	public List<string> unlockAchievements = new List<string>();

	// Use this for initialization
	void Start(){		
		//if(platform == Platforms.ANDROID ){
			if(Application.platform == RuntimePlatform.Android){
				# if UNITY_ANDROID			
					//Heyzap.start(0,"",Heyzap.FLAG_NO_OPTIONS);
					Heyzap.start(Heyzap.FLAG_NO_OPTIONS,0);
				#endif
			}
		//}else if(platform == Platforms.IOS ){
			if(Application.platform == RuntimePlatform.IPhonePlayer){
				if(iosAppId == 0){
					Debug.Log("warning: IOS AppId missing!");
				}
				
				if(iosURLSchema == ""){
					Debug.Log("warning: IOS URLSchema missing!");
				}
				
				# if UNITY_IPHONE			
					//Heyzap.start(iosAppId,iosURLSchema,Heyzap.FLAG_NO_OPTIONS);
					Heyzap.start(iosURLSchema,iosAppId);
				#endif
			}
		//}
		
		if(isShowAd){
			InitHeyzapAds();			
			if(isBannerAd){
				ShowBanner();
			}else{
				ShowHeyzapAds();
			}			
		}		
		isStarted = true;
		Debug.Log("start heyzap wrapper started..." + isStarted);
	}	
	
	public void SendHeyzapScore( int score, int level ){
		string currentLevelId = leaderboardIds[ level - 1];
		
		if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
				Heyzap.submitScore(score, score.ToString() +" points", currentLevelId);
			#endif
		}
		
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			# if UNITY_IPHONE
				Heyzap.submitScore(score, score.ToString() +" points", currentLevelId);
			#endif
		}
	}
	
	public void SendHeyzapScoreById(string id, int score ){
		string currentLevelId = id;		
		if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
				Heyzap.submitScore(score, score.ToString() +" points", currentLevelId);
			#endif
		}
		
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			# if UNITY_IPHONE
				Heyzap.submitScore(score, score.ToString() +" points", currentLevelId);
			#endif
		}
	}
	
	public void ShowHeyzapLeaderBoard(){
		if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
				Heyzap.showLeaderboards();		 		
			#endif
		}
		
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			# if UNITY_IPHONE
				Heyzap.showLeaderboards();		 		
			#endif
		}
	}
	
	public void InitHeyzapAds(){
		if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
				HeyzapAds.start(HeyzapAds.FLAG_NO_OPTIONS, 0 );
			#endif
		}
		
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			if(iosAppId == 0){
				Debug.Log("warning: IOS AppId missing!");
			}				
			#if UNITY_IPHONE
				HeyzapAds.start(HeyzapAds.FLAG_NO_OPTIONS, iosAppId );
			#endif
		}
		
		#if UNITY_ANDROID
			HeyzapAds.setDisplayListener(ShowState );
		#endif
	}
	
	#if UNITY_ANDROID
	private void ShowState( string adState, string tag ){
		if (adState == "show") {
            // Do something when the ad shows, like pause your game
        }
	}
	#endif
	
	public void ShowHeyzapAds(){
		if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
				HZInterstitialAd.show();
			#endif			
		}
		
		if(Application.platform == RuntimePlatform.IPhonePlayer){			
			#if UNITY_IPHONE
				HZInterstitialAd.show();
			#endif
		}
	}
	
	public void HideHeyzapAds(){
		if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
				HZInterstitialAd.hide();
			#endif			
		}
		
		if(Application.platform == RuntimePlatform.IPhonePlayer){			
			#if UNITY_IPHONE
				HZInterstitialAd.hide();
			#endif
		}
	}
	
	public void ShowBanner( int location = 0 ){
	}
	
	public void HideBanner(){
	}	
	
	public void UnlockAchievement( string id ){
		Debug.Log( "check UnlockAchievement id " + id );		
		bool found = SearchUnlockAchievementById(id);
		if(!found){
			unlockAchievements.Add(id);
			
			int len = unlockAchievements.Count;
			string[] unlocks;
			unlocks = new string[len];
			
			for(int index = 0; index < len; index++){
				unlocks[index] = unlockAchievements[index];
				Debug.Log( "added unlocks[index] " +  unlocks[index] + index );
			}			
			
			if(Application.platform == RuntimePlatform.Android){
				#if UNITY_ANDROID
					Heyzap.unlockAchievements(unlocks);				
					Debug.Log( "UnlockAchievement successfully id " + id );
				#endif
			}
			
			if(Application.platform == RuntimePlatform.IPhonePlayer){
				#if UNITY_IPHONE
					Heyzap.unlockAchievements(unlocks);				
					Debug.Log( "UnlockAchievement successfully id " + id );
				#endif
			}
		}
	}
	
	private bool SearchUnlockAchievementById(string id){
		int len = unlockAchievements.Count;
		bool found =false;
		
		for(int index = 0; index < len; index++){
			if(unlockAchievements[index] == id){
				found = true;
				Debug.Log( "UnlockAchievement id already exist this call will be ignored id" + id );
				break;
			}
		}		
		return found;
	}
	
	public void ShowAchievement(bool isMinimal =false){
		if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
				if(isMinimal){
					Heyzap.setFlags( Heyzap.FLAG_MINIMAL_ACHIEVEMENT_DIALOG);
				}				
				Heyzap.showAchievements();
			#endif
		}
		
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			#if UNITY_IPHONE								
				Heyzap.showAchievements();
			#endif
		}
	}*/
}
