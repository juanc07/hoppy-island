using UnityEngine;
using System.Collections;

public class UITableController : MonoBehaviour {

	private UITable uiTable;
	public bool isRunOnce = true;

	[SerializeField]
	private bool isReposition;

	// Use this for initialization
	void Start () {
		GetUITable();
	}	

	private void OnEnable(){
		GetUITable();
	}

	private void GetUITable(){
		if(uiTable==null){
			uiTable = this.gameObject.GetComponent<UITable>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(isReposition){
			GetUITable();
			uiTable.enabled = true;
			uiTable.Reposition();
			if(isRunOnce){
				isReposition = false;
			}
		}
	}

	public bool IsReposition{
		set{isReposition =value;}
	}
}
