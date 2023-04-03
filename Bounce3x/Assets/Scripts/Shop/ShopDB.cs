using UnityEngine;
using System.Collections;
using System;

public class ShopDB{

	private Action LoadComplete;
	public event Action OnLoadComplete{
		add{LoadComplete+=value;}
		remove{LoadComplete-=value;}
	}

	//public string DBLocation = "Assets/Resources/ShopDatabase.asset";		
	public string DBLocation = "ShopDatabase";
	public UnityEngine.Object dbaseAsset {get;set;}
	public ShopContentHolder db{get;set;}	
	
	public void Load(){		
		//dbaseAsset = AssetDatabase.LoadAssetAtPath(DBLocation, typeof(ShopContentHolder));
		//dbaseAsset = Resources.LoadAssetAtPath(DBLocation, typeof(ShopContentHolder));
		dbaseAsset = Resources.Load(DBLocation,typeof(ShopContentHolder));
		//Debug.Log(" check  dbaseAsset " + dbaseAsset);		
        ShopContentHolder shopDB =(ShopContentHolder)dbaseAsset;
        db = shopDB;

		if(null!=LoadComplete){
			LoadComplete();
		}
	}
}

