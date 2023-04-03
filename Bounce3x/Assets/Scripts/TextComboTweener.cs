using Holoville.HOTween;
using UnityEngine;
using System.Collections;
using System;

public class TextComboTweener : MonoBehaviour {

	private Vector3 targetPosition;
	private UILabel label;
	
	private Camera mainCamera;
	private Camera NGUICamera;
	private GameObject target;
	private Vector3 pos;
	
	
	private GameObject inGamePanel;
	private UILabel comboLabel;
	
	private GameDataManagerController gdc;
	private bool isDoneTween = true;
	private int id;

	private Action <int>TweenComplete;
	public event Action <int>OnTweenComplete{
		add{TweenComplete+=value;}
		remove{TweenComplete-=value;}
	}
	// Use this for initialization	
	
	void Awake(){
		mainCamera = GameObject.Find("Main Camera").camera;
		NGUICamera = GameObject.Find("InGameGUI/Camera").camera;	
		target = GameObject.Find("Whale/TextTarget");	
	}
	
	void Start (){
		gdc = GameDataManagerController.GetInstance();
		label = this.GetComponent<UILabel>();
		label.text ="+" + gdc.currentLevel + "\n combo x" + gdc.CurrentCombo;
		
		if(!gdc.IsComboLabelWarmUp){
			gdc.IsComboLabelWarmUp = true;
			targetPosition = new Vector3( -83.1041f,196.0544f,0f );
		}else{
			FollowTarget();	
		}
		TweenStart();		
	}

	private void OnEnable(){
		/*GetGameDataManager();
		GetUILabel();
		FollowTarget();
		TweenStart();*/
	}

	private void GetGameDataManager(){
		if(gdc==null){
			gdc = GameDataManagerController.GetInstance();
		}
	}

	private void GetUILabel(){
		if(label==null){
			label = this.GetComponent<UILabel>();
		}
	}
	
	private void TweenStart(){
		label.text ="+" + gdc.currentLevel + "\n combo x" + gdc.CurrentCombo;
		label.transform.localScale =  new Vector3( 1f,1f,1f );

		Vector3 tempPosition = label.gameObject.transform.localPosition;
		tempPosition.y += 30f;
		GoTween labelPositionTween = new GoTween(label.gameObject.transform,0.3f,new GoTweenConfig().localPosition(tempPosition,false).setEaseType(GoEaseType.ElasticOut));
		GoTween labelAlphaTween = new GoTween(label,0.3f,new GoTweenConfig().colorProp("color",new Color32(255,0,0,0),false).onComplete(AlphaTweenComplete));


		GoTweenChain alphaTweenChain = new GoTweenChain();
		alphaTweenChain.append(labelPositionTween).append(labelAlphaTween);
		alphaTweenChain.autoRemoveOnComplete = true;
		alphaTweenChain.play();
	}

	private void AlphaTweenComplete(AbstractGoTween abstractGoTween){
		isDoneTween = true;
		if(null!=TweenComplete){
			TweenComplete(id);
		}
		//Destroy(this.gameObject);
	}	
	
	private void FollowTarget(){	
		mainCamera = NGUITools.FindCameraForLayer(target.layer);
		NGUICamera = NGUITools.FindCameraForLayer(gameObject.layer);
		
		pos = mainCamera.WorldToViewportPoint(target.transform.position);
		pos = NGUICamera.ViewportToWorldPoint(pos);		
		
		pos.z = 0f;		
		transform.position = pos;		
		
		//originalPosition = pos;
		targetPosition = pos;
		targetPosition.y += 5;
	}
	
	public void Reset(){
		label.alpha = 1;
		GetGameDataManager();
		GetUILabel();
		FollowTarget();
		TweenStart();
	}

	public bool IsDoneTween{
		get{return isDoneTween;}
	}

	public int ID{
		set{id=value;}
		get{return id;}
	}
}
