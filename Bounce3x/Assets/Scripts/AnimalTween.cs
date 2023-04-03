using Holoville.HOTween;
using UnityEngine;
using System.Collections;

public class AnimalTween : MonoBehaviour {

	// Use this for initialization
	void Start (){	
		HOTween.Init(true, true, true);		
		// C# TweenParms parms = new TweenParms(); // UnityScript 
		TweenParms parms = new TweenParms(); 
		// Both C# than UnityScript 
		parms.Prop("position", new Vector3(-33.83417f,-86.13104f,-385.0141f));
		parms.Prop("rotation", new Vector3(0,0,0));
		parms.Prop("localScale", new Vector3(196.09f,11.45f,475.197f));
		//parms.Ease(EaseType.EaseOutBounce); 
		parms.Delay(1); 		
		HOTween.To(transform, 1, parms );
		//HOTween.To(transform, 4, "position", new Vector3(-3, 6, 0));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
