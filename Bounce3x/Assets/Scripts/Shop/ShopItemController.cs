using UnityEngine;
using System.Collections;

public class ShopItemController : MonoBehaviour {

	public GameObject itemPrefab;
	public float itemScale = 0.5100784f;
	private ShopManagerController shopManagerController;

	private UITableController uiTableController;

	// Use this for initialization
	void Start (){
		uiTableController = this.gameObject.GetComponent<UITableController>();
		shopManagerController = ShopManagerController.GetInstance();
		AddEventListener();
		if(shopManagerController.IsShopItemLoaded){
			AddItem();
		}
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener(){
		shopManagerController.OnShopItemLoadComplete+=OnShopItemLoadComplete;
	}

	private void RemoveEventListener(){
		if(shopManagerController!=null){
			shopManagerController.OnShopItemLoadComplete-=OnShopItemLoadComplete;
		}
	}

	private void OnShopItemLoadComplete(){
		AddItem();
	}

	private void AddItem(){
		int itemCount = shopManagerController.ShopItemDB.Count;
		for(int index=0;index<itemCount;index++){
			GameObject item = NGUITools.AddChild(this.gameObject,itemPrefab) as GameObject;
			item.gameObject.transform.localScale = new Vector3(itemScale,itemScale,itemScale);

			ShopBtnController shopBtnController = item.gameObject.GetComponent<ShopBtnController>();
			shopBtnController.ItemData = shopManagerController.ShopItemDB[index];
		}

		uiTableController.IsReposition = true;
	}
}
