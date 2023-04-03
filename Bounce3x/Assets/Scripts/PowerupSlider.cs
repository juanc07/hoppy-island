using UnityEngine;
using System.Collections;

public class PowerupSlider : MonoBehaviour {
	
	private UISlider slider;
	//private UILabel label;
	//private float speed = 0.001f;
	private float speed = 0.05f;
	public bool isActive= false;
	
	
	private GameObject inGamePanel;
	private Transform powerupGauge;
	private Transform powerupGaugeImageLabels;
	private Transform powerupLabel;
	private UILabel powerupUILabel;

	private PowerUpSliderBlinkController powerUpSliderBlinkController;
	private SoundManager soundManager;
	private GameManagerController gameManagerController;
	private bool isStop = false;

	private float sfxBlinkerThreshold = 0.2f;
	private float sfxBlinkerThresholdRemove = 0.01f;

	private ScreenManagerController screenManagerController;

	//new
	private UISprite[] powerUpImages;
	private float tick;
	private float currentTimeInterval;
	
	// Use this for initialization
	void Start () {
		gameManagerController =  GameManagerController.GetInstance();
		screenManagerController = ScreenManagerController.GetInstance();
		soundManager = SoundManager.GetInstance();
		powerUpSliderBlinkController = this.gameObject.GetComponent<PowerUpSliderBlinkController>();
		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener(){
		gameManagerController.OnPausedGame+=OnPausedGame;
		gameManagerController.OnUnPausedGame+=OnUnPausedGame;
		gameManagerController.OnLevelRestart+=OnLevelRestart;
		gameManagerController.OnLeveFailed+=OnLeveFailed;

		screenManagerController.OnLoadScreen+=OnLoadScreen;
	}

	private void RemoveEventListener(){
		gameManagerController.OnPausedGame-=OnPausedGame;
		gameManagerController.OnUnPausedGame-=OnUnPausedGame;
		gameManagerController.OnLevelRestart-=OnLevelRestart;
		gameManagerController.OnLeveFailed-=OnLeveFailed;

		screenManagerController.OnLoadScreen-=OnLoadScreen;
	}

	private void OnLoadScreen(){
		StopBlinkerSfx();
	}

	private void OnLeveFailed(){
		StopBlinkerSfx();
	}

	private void OnLevelRestart(){
		StopBlinkerSfx();
	}

	private void OnPausedGame(){
		isStop = true;
		StopBlinkerSfx();
	}

	private void OnUnPausedGame(){
		isStop = false;

		if(soundManager.IsSfxOn){
			if(powerUpSliderBlinkController.HasStarted){
				PlayBlinkerSfx();
			}
		}else{
			if(powerUpSliderBlinkController.HasStarted){
				StopBlinkerSfx();
			}
		}
	}

	private void StopBlinkerSfx(){
		if(soundManager != null){
			soundManager.StopSfx(SFX.PowerUpTimerEnding);
		}
	}

	private void PlayBlinkerSfx(){
		if(soundManager != null){
			soundManager.PlaySfx(SFX.PowerUpTimerEnding,0.4f,true);
		}
	}
	
	// Update is called once per frame
	void Update (){
		if(!isStop){
			Progress();
			CheckValue();
		}
	}

	public void Activate(PowerUpChecker.Powerups activePowerup,float timeInterval){
		tick = timeInterval;
		currentTimeInterval = timeInterval;

		inGamePanel = GameObject.Find("InGameLeftPanel");		
		powerupGauge = inGamePanel.transform.Find("powerupGauge");
		powerupGaugeImageLabels = inGamePanel.transform.Find("powerupGauge/ImageLabels");
		powerupLabel = inGamePanel.transform.Find("powerupGauge/PowerupLabel");		
		powerupUILabel = powerupLabel.GetComponent<UILabel>();
		
		
		powerupGauge.gameObject.SetActive(true);
		isActive = true;
		
		slider = this.GetComponent<UISlider>();
		slider.sliderValue = 1;
		
		Transform powerupImageLabel;
		
		switch(activePowerup){
			case PowerUpChecker.Powerups.Duplicate:				
				powerupUILabel.text = "Help";
			break;
			
			case PowerUpChecker.Powerups.Overgrowth:				
				powerupUILabel.text = "Overgrowth";
			break;
			
			case PowerUpChecker.Powerups.ClearAll:				
				powerupUILabel.text = "ClearAll";
			break;
			
			case PowerUpChecker.Powerups.Slow:				
				powerupUILabel.text = "Slow";
			break;
			
			case PowerUpChecker.Powerups.AutoPilot:				
				powerupUILabel.text = "AutoPilot";
			break;
			
			default:				
				powerupUILabel.text = "ClearAll";
			break;
			
		}	
		
		powerupLabel.transform.gameObject.SetActive(true);
	}
	
	public void Deactivate(){
		if(slider){
			slider.value = 0;
			isActive = false;

			if(powerUpImages==null){
				powerUpImages = powerupGaugeImageLabels.GetComponentsInChildren<UISprite>();
			}

			foreach( UISprite child in powerUpImages ){				
				child.GetComponent<UISprite>().gameObject.SetActive(false);
			}

			powerupGauge.gameObject.SetActive(false);	
			powerupLabel.transform.gameObject.SetActive(false);
		}	
	}
	
	private void Progress(){
		if(slider != null){
			if(isActive && slider.value != 0){
				//slider.sliderValue -= speed;
				tick -= (Time.fixedDeltaTime * speed);
				slider.value = (tick /currentTimeInterval);
				//slider.value -= (Time.fixedDeltaTime * speed);
				if(slider.value < sfxBlinkerThreshold && slider.sliderValue > sfxBlinkerThresholdRemove){
					if(!powerUpSliderBlinkController.HasStarted){
						powerUpSliderBlinkController.StartTween();
						PlayBlinkerSfx();
					}
				}else if(slider.value <= sfxBlinkerThresholdRemove){
					if(powerUpSliderBlinkController.HasStarted){
						powerUpSliderBlinkController.StopTween();
						if(soundManager != null){
							soundManager.StopSfx(SFX.PowerUpTimerEnding);						
						}
					}
				}
			}	
		}				
	}
	
	private void CheckValue(){
		if(slider != null){
			if(slider.sliderValue==0){
				Deactivate();							
			}
		}
	}
}
