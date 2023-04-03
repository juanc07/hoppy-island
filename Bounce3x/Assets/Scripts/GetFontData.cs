using UnityEngine;
using System.Collections;

public class GetFontData : MonoBehaviour {
	
	private UIFont font;	
	private UILabel label;
	
	void Awake(){
		label = this.GetComponent<UILabel>();
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public UIFont Font{				
		get{ return label.font; }		
		set{ label.font = value; }		
	}	
}
