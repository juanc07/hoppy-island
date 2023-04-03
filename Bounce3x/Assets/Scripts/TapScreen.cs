using UnityEngine;
using System.Collections;

public class TapScreen : MonoBehaviour {
	
	public Transform effects;	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update (){	
		
		if (Input.GetButtonDown("Fire1")) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray))
				CastEffect();                
            
        }
	}
	
	private void CastEffect(){
		Transform effect = Instantiate( effects, this.transform.position, this.transform.rotation ) as Transform;
		Vector3 currPos  =effect.transform.position;
		currPos.y += 50;
		effect.transform.position = currPos;
		//effect.transform.localScale =  this.transform.localScale;
	}
}
