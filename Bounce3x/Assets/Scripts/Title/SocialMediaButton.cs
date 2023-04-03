using UnityEngine;
using System.Collections;

public class SocialMediaButton : MonoBehaviour {	
	public string url="";	
	private void OnClick(){		 
		 Application.OpenURL(url);		
	}
}
