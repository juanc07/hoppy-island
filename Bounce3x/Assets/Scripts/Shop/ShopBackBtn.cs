using UnityEngine;
using System.Collections;

public class ShopBackBtn : MonoBehaviour {
	
	private ScreenManagerController smc;
	
	// Use this for initialization
	void Start () {
		smc = ScreenManagerController.GetInstance();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void OnClick(){
		smc.LoadGameByGameScreenName(ScreenType.Title);
	}
}
