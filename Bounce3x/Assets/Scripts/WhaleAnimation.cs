using Holoville.HOTween;
using UnityEngine;
using System.Collections;
using System;

public class WhaleAnimation : MonoBehaviour{	
	private Transform model;
	
	private ShopManagerController shopMc;	
	private AvatarCustomizer avatarCustomizer;
	public bool isFake=false;

	private Action ScaleToNormalComplete;
	public event Action OnScaleToNormalComplete{
		add{ScaleToNormalComplete+=value;}
		remove{ScaleToNormalComplete-=value;}
	}

	private Action ScaleToOverGrowthComplete;
	public event Action OnScaleToOverGrowthComplete{
		add{ScaleToOverGrowthComplete+=value;}
		remove{ScaleToOverGrowthComplete-=value;}
	}
	
	// Use this for initialization
	void Start(){		
		shopMc = ShopManagerController.GetInstance();
		AddEventListener();
		if(shopMc.IsShopItemLoaded){
			UpdateAvatar();
		}
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void UpdateAvatar(){
		//Debug.Log("update avatar " +  shopMc.CurrentAvatar.name);
		avatarCustomizer = this.gameObject.GetComponent<AvatarCustomizer>();
		
		if(!isFake){
			if(shopMc.CurrentAvatar!=null){			
				avatarCustomizer.ChangeAvatarByAvatarType(shopMc.CurrentAvatar.avatarType);
				//Debug.Log("update load avatar " +  shopMc.CurrentAvatar.name);
			}else{
				avatarCustomizer.ChangeAvatarByAvatarType(Item.AvatarList.Whale);
				//Debug.Log("update avatar load default model " +  shopMc.CurrentAvatar.name);
			}
		}else{
			//avatarCustomizer.SelectCharacter(shopMc.GetRandomItem().avatarType.ToString());
			avatarCustomizer.SelectCharacter(shopMc.GetRandomItem().avatarType);
			//avatarCustomizer.RandomizeAvatar();
		}
		
		model = avatarCustomizer.Model;
	}

	private void AddEventListener(){
		shopMc.OnShopItemLoadComplete+=OnShopItemLoadComplete;
	}

	private void RemoveEventListener(){
		shopMc.OnShopItemLoadComplete-=OnShopItemLoadComplete;
	}

	private void OnShopItemLoadComplete(){
		UpdateAvatar();
	}
	
	public void OnMoveLeft(){
		model.animation.Play("left");
	}
	
	public void OnMoveRight(){
		model.animation.Play("right");
	}
	
	private void OnTriggerEnter(Collider col){
		OnHit();
	}
	
	private void OnTriggerExit(Collider col){
		//OnIdle();
	}
	
	public void OnIdle(){
		if( this.model == null ) return;
		model.gameObject.animation.Play("idle");
	}

	public void OnHit(){		
		if(model.gameObject.animation["hit"].enabled == false){
			model.gameObject.animation.Play("hit");
			StartCoroutine(WaitAndAnimate(0.5f));
		}else{
			//Debug.Log("still playing hit can't play");
		}		
	}
	
	IEnumerator WaitAndAnimate(float waitTime) {
        yield return new WaitForSeconds(waitTime);
		OnIdle();        
    }
	
	public void OverGrowth(){		
		TweenParms parms = new TweenParms(); 		
		parms.Prop("position", new Vector3(-33.83417f,-86.13104f,-385.0141f));
		parms.Prop("rotation", new Vector3(0,0,0));
		parms.Prop("localScale", new Vector3(196.09f,11.45f,475.197f));
		parms.Ease(EaseType.EaseOutElastic); 
		parms.Delay(0.4f);
		parms.OnComplete(ScaleToOverGrowthDone);
		HOTween.To(transform, 1, parms );
		OnIdle();
	}	
	
	public void scaleToNormal(){
		TweenParms parms = new TweenParms(); 		
		parms.Prop("position", new Vector3(-40.00574f,-116.2f,-181.7f));
		parms.Prop("rotation", new Vector3(0,0,0));
		parms.Prop("localScale", new Vector3(33.18574f,1.970332f,80.33155f));
		parms.Ease(EaseType.EaseInElastic); 
		parms.Delay(0.4f);
		parms.OnComplete(ScaleToNormalDone);
		HOTween.To(transform, 1, parms );
		OnIdle();
	}

	private void ScaleToNormalDone(){
		if(null!=ScaleToNormalComplete){
			ScaleToNormalComplete();
		}
	}

	private void ScaleToOverGrowthDone(){
		if(null!=ScaleToOverGrowthComplete){
			ScaleToOverGrowthComplete();
		}
	}
	
	public void QuickScaleToNormal(){
		if( this.gameObject == null ){
			return;
		}
		
		TweenParms parms = new TweenParms(); 		
		parms.Prop("position", new Vector3(-40.00574f,-116.2f,-181.7f));
		parms.Prop("rotation", new Vector3(0,0,0));
		parms.Prop("localScale", new Vector3(33.18574f,1.970332f,80.33155f));
		parms.Ease(EaseType.Linear);
		HOTween.To(transform, 0.1f, parms );
		OnIdle();
	}	
}
