using UnityEngine;
using System.Collections;

public class ShopNoBtn : MonoBehaviour {

	// Use this for initialization	
	private ShopPopUpController shopPopUpController;
	
	void Start (){		
		shopPopUpController = GameObject.Find("ShopPopUpAnchor").GetComponent<ShopPopUpController>();
	}
	
	private void OnClick(){
		shopPopUpController.ShowHideShopPopUpPanel(false);		
	}
}
