using UnityEngine;
using System.Collections;

public class CrossFadeTrigger : MonoBehaviour {
	
	public Texture    newTexture;
  	public Vector2    newOffset;
  	public Vector2    newTiling;
	
	private CrossFade cf;
	
	// Use this for initialization
	void Start () {
		cf =  this.GetComponent<CrossFade>();
		cf.CrossFadeTo(newTexture,newOffset, newTiling);
	}	
}
