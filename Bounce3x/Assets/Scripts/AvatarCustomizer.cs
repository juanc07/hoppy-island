using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvatarCustomizer : MonoBehaviour {
	
	public GameObject modelHolder;
	private Transform model;
	public string AvatarDirectory = "Avatars/Whales/";
	private ShopManagerController shopManagerController;
	public bool isVerbose = false;

	private List<Item> itemCollection = new List<Item>();
	private Transform originalParent;
	private Vector3 originalPosition;
	private Quaternion originalRotation;
	private Vector3 originalLocalScale;

	private Item.AvatarList currentAvatar;

	// Use this for initialization
	void Start (){
		GetShopManagerController();
	}

	private void OnDestroy(){
		UnloadItem();
		AvatarDirectory = "";
		modelHolder = null;
		//shopManagerController = null;
		//Debug.Log("Ondestroy Avatar Customizer!");
	}

	private void OnEnable(){
		GetShopManagerController();
	}

	private void GetShopManagerController(){
		if(shopManagerController == null){
			shopManagerController = ShopManagerController.GetInstance();
		}
	}
	
	public void ChangeAvatarByAvatarType( Item.AvatarList avatarType ){
		//SelectCharacter(avatarType.ToString());
		SelectCharacter(avatarType);
	}	
	
	/*public void SelectCharacter(string avatarName ){
		if(model != null ){
			model.gameObject.SetActive(false);
		}
		
		DisplayLog( " check avatarName " + avatarName );

		model =  SearchForItemByName(avatarName,true);

		if(model == null){
			model = (Transform)Instantiate(Resources.Load(AvatarDirectory + avatarName,typeof(Transform)));

			if(model!=null){
				Item item = new Item();
				item.name = avatarName;
				item.itemTransform = model;
				itemCollection.Add(item);
			}

			model.name = avatarName;
			model.gameObject.transform.parent = modelHolder.transform;
			
			string sceneName = Application.loadedLevelName;
			
			if( sceneName== GameScreen.GAME  ){
				model.transform.localRotation = Quaternion.Euler( 349.5752f,90f,0f);
				model.transform.localScale = new Vector3(1f,1f,1f);
				model.transform.localPosition = new Vector3(0,0,0);	
			}else{
				model.transform.localRotation = Quaternion.Euler(0f,0f,0f);
				model.transform.localScale = new Vector3(1f,1f,1f);
				model.transform.localPosition = new Vector3(0,0,0);			
				ChangeLayersRecursively(model,"NGUILayer");
			}
		}						
	}*/

	public void SelectCharacter(Item.AvatarList avatarType ){
		if(model != null ){
			model.gameObject.SetActive(false);
		}
		
		DisplayLog( " check avatarName " + avatarType.ToString() );
		
		model =  SearchForItemByName(avatarType.ToString(),true);
		
		if(model == null){
			model = (Transform)Instantiate(Resources.Load(AvatarDirectory + avatarType.ToString(),typeof(Transform)));
			//model = (Transform)Instantiate(shopManagerController.SearchItemByAvatarType(avatarType).itemTransform);
			
			if(model!=null){
				Item item = new Item();
				item.name = avatarType.ToString();
				item.itemTransform = model;
				itemCollection.Add(item);
			}
			
			model.name = avatarType.ToString();
			model.gameObject.transform.parent = modelHolder.transform;
			
			string sceneName = Application.loadedLevelName;
			
			if( sceneName== GameScreen.GAME  ){
				model.transform.localRotation = Quaternion.Euler( 349.5752f,90f,0f);
				model.transform.localScale = new Vector3(1f,1f,1f);
				model.transform.localPosition = new Vector3(0,0,0);	
			}else{
				model.transform.localRotation = Quaternion.Euler(0f,0f,0f);
				model.transform.localScale = new Vector3(1f,1f,1f);
				model.transform.localPosition = new Vector3(0,0,0);			
				ChangeLayersRecursively(model,"NGUILayer");
			}
		}						
	}


	
	private void ChangeLayersRecursively( Transform trans, string name ){
		foreach (Transform child in trans){
			child.gameObject.layer = LayerMask.NameToLayer(name);
			ChangeLayersRecursively(child, name);
		}
	}
	
	public Transform Model{
		get{return model;}
		set{ model =value;}
	}

	private void DisplayLog(string message){
		if(isVerbose){
			Debug.Log(message);
		}
	}

	private Transform SearchForItemByName(string avatarName, bool showHide){
		int itemCount = itemCollection.Count;
		Transform item = null;
		for(int index=0; index < itemCount; index++){
			if(itemCollection[index]!=null){
				if(itemCollection[index].name.Equals(avatarName,System.StringComparison.Ordinal)){
					item = itemCollection[index].itemTransform;
					item.gameObject.SetActive(showHide);
					break;
				}
			}
		}

		return item;
	}

	private void UnloadItem(){
		int itemCount = itemCollection.Count-1;

		for(int index=itemCount; index >=0; index--){
			if(itemCollection[index]!=null){
				Destroy(itemCollection[index].itemTransform.gameObject);
				itemCollection[index] = null;
				itemCollection.RemoveAt(index);
			}
		}

		itemCollection.Clear();
		model = null;
		//Debug.Log("unload shop items!");
	}
}
