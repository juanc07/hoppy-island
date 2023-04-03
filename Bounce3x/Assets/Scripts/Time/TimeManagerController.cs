using UnityEngine;
using System.Collections;

public class TimeManagerController : MonoBehaviour {
	
	private float 	min;
	private float 	sec;	
	
  	public Vector2	newOffset;
  	public Vector2  newTiling;
	
	private CrossFade cf;
	private int index;

	public int crossFadeDelay = 30;
	
	public Texture[] textures = new Texture[4];
	
	// Use this for initialization
	void Start (){
		//index = -1;
		//CrossFadeBg();
	}
	
	// Update is called once per frame
	void Update () {
		sec+= Time.deltaTime;		
		if(sec>crossFadeDelay){
			sec=0;
			min++;
			if(min == 3 || min == 6 || min == 9 || min == 12 || min == 15 ){
			//if(min == 1 || min == 2 || min == 3 || min == 4 || min == 5 ){
				CrossFadeBg();
			}else if(min>15){
			//}else if(min>5){
				min = 0;
			}
		}
		//Debug.Log( "min: " + min + " sec " + sec );
	}
	
	private void CrossFadeBg(){
		index++;
		if(index>4){
			index=0;
		}
		cf =  GameObject.Find("GameBG").GetComponent<CrossFade>();
		cf.CrossFadeTo(textures[index],newOffset, newTiling);
	}
}
