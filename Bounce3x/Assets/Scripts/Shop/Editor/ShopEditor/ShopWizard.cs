using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopWizard : ScriptableWizard{
	static private ShopContentHolder db;
	private bool IsCreated;
	private ShopDBLoader shopDB;
	
	public Vector2 scrollPosition;
	
	[MenuItem("Custom/Shop/Shopping - List Helper")]
    static void CreateWizard () {
        ScriptableWizard.DisplayWizard<ShopWizard>("Shop List Helper", "Create", "Apply");        
    }
	
	void OnWizardCreate () {
		CreateShopDBAsset();
    }
	
	private void CreateShopDBAsset(){
		if(db != null) return;
		
		db = null;
		UpdateMyAssetLocation();
		
		if(db==null){
			//create
			Debug.Log( "Creat shopping list" );			
			//List<Item> content = new List<Item>();
			ShopContentHolder asset = ScriptableObject.CreateInstance("ShopContentHolder") as ShopContentHolder;			
			
	        AssetDatabase.CreateAsset(asset, "Assets/Resources/ShopDatabase.asset");
	        AssetDatabase.SaveAssets();
	        EditorUtility.FocusProjectWindow();
	        Selection.activeObject = asset;
			IsCreated = true;
		}else{
			errorString = "already have shopdb asset!";
			Debug.Log("already have shopdb asset!");
		}
	}
	
	
    void OnWizardUpdate () {
        helpString = "Please Enter shop item and corresponding information!";
		UpdateMyAssetLocation();		
		//use this when prompting error
		//errorString = "ShopWizard has an error!";
    }   
    // When the user pressed the "Apply" button OnWizardOtherButton is called.
    void OnWizardOtherButton (){
		//apply
		Debug.Log( "apply shopping list" );       
    }
	
	
	 void OnGUI(){       
		//Debug.Log(" ShopWizard ONGUI!! ");
		EditorGUILayout.BeginVertical();		
		//EditorGUILayout.Space();
		if(db == null){
			if(!IsCreated){
				if (GUILayout.Button("Create new DB", GUILayout.Height(50))){
					CreateShopDBAsset();
				}	
			}else{
				if (GUILayout.Button("Update", GUILayout.Height(50))){
		            UpdateMyAssetLocation();
					DisplayCurrentShoplist();
		        }			
			}
		}
        
		
		if(db == null) return;	
		
		
		/*
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(name, GUILayout.MaxWidth(0));
        DBLocation = EditorGUILayout.TextField(DBLocation);
		
		if (GUILayout.Button("Browse"))
        {
			db = null;
            DBLocation = EditorUtility.OpenFilePanel(name,DBLocation, "asset");
            UpdateMyAssetLocation();
        }
        EditorGUILayout.EndHorizontal();
		*/
		
		/*if (GUILayout.Button("Update ShopDatabase"))
        {
            UpdateMyAssetLocation();
			DisplayCurrentShoplist();
        }*/	
		
        //EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Backup DB", GUILayout.Height(25),  GUILayout.Width(112))){
			shopDB.Backup();
        }
		
		if (GUILayout.Button("Restore DB", GUILayout.Height(25),  GUILayout.Width(112))){
			shopDB.Restore();
        }
		
		if (GUILayout.Button("Delete DB", GUILayout.Height(25),  GUILayout.Width(112))){
			shopDB.Delete();
			IsCreated = false;
			db = null;			
		}
		
		EditorGUILayout.EndHorizontal();
		
		
        if (GUILayout.Button("Add Item", GUILayout.Height(50))){		
			AddItemWindow.CreateWizard();
        }				
		
		EditorGUILayout.Space();		
		EditorGUILayout.LabelField( "Item List",GUILayout.Width(100) );
		EditorGUILayout.Space();
		
		//scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(350), GUILayout.Height(255));
		scrollPosition = GUILayout.BeginScrollView(scrollPosition,true,true);
        EditorGUILayout.Space();
        DisplayCurrentShoplist();
        EditorGUILayout.Space();		
		EditorGUILayout.EndVertical();
		GUILayout.EndScrollView();
		
		EditorGUILayout.Space();	
	}
	
	public void UpdateMyAssetLocation(){
		if(shopDB == null){
			shopDB = new ShopDBLoader();	
		}		
		shopDB.Load();
		db = shopDB.db;       
    }
	
	void DisplayCurrentShoplist()
    {
		if( db == null || shopDB == null ) return;		
        for (int i = 0; i < db.content.Count; i++ )
        {
            EditorGUILayout.BeginHorizontal();
            db.content[i].name = GUILayout.TextField(db.content[i].name, GUILayout.Width(200));
			
			/*
            if (GUILayout.Button("UP"))
            {
                if (i > 0)
                {
                    Item item = db.content[i];
                    db.content.RemoveAt(i);
                    db.content.Insert(i - 1, item);				
                }
            }
            if (GUILayout.Button("DN"))
            {
				Debug.Log( "check i " + i );
                if (i + 1 < db.content.Count)
                {
                    Item item = db.content[i];
                    db.content.RemoveAt(i);
                    db.content.Insert(i + 1, item);
                }
            }
			*/
			
			if (GUILayout.Button("Edit")){
			  EditItemWindow.CreateWizard( i.ToString() );
            }
			
			
            if (GUILayout.Button("Delete"))
            {
                db.content.Remove(db.content[i]);
            }
            
            //EditorUtility.SetDirty(dbaseAsset);
			EditorUtility.SetDirty(shopDB.dbaseAsset);
            EditorGUILayout.EndHorizontal();			
        }
    }
	
	void AddItem(Item newItem){		 
		//Debug.Log( " check db " + db.content[0].name );
        db.content.Add(newItem);
        //EditorUtility.SetDirty(dbaseAsset);
		EditorUtility.SetDirty(shopDB.dbaseAsset);
    }
}
