using UnityEngine;
using System.Collections;

public class PreloaderController : MonoBehaviour {
	
	private AsyncOperation async;
	public UILabel preloaderLabel;
	private string displayProgressText;

	private ScreenManagerController screenManagerController;

	void Start() {
		screenManagerController = ScreenManagerController.GetInstance();
		async = Application.LoadLevelAsync(screenManagerController.LevelToLoad);
		async.allowSceneActivation = false;
		displayProgressText = "Loading...";
		//Debug.Log("start Loading " + screenManagerController.LevelToLoad);
    }	
	
	// Update is called once per frame
	void Update (){
		if(async!=null){
			if(!async.isDone){
				//displayProgressText = string.Format("Loading... {0:0.##}%",async.progress);
				preloaderLabel.text = displayProgressText;
				//Debug.Log(displayProgressText );
				if(async.progress >= 0.9f){
					async.allowSceneActivation = true;
				}
			}else{
				//Debug.Log("Loading complete");
			}
		}	
	}
}
