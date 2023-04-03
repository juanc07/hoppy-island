using UnityEngine;
using System.Collections;

public class ballScript : MonoBehaviour {	
	
	//private float forwardForce = 0.1f;
	private float time;
	//private Vector2 initVelocity;
	private Vector2 positionOfPlayerWhenThrown;
	
	// Use this for initialization
	void Start (){		
		positionOfPlayerWhenThrown = rigidbody.transform.position;
		//initVelocity = new Vector3(0,0,0);
	}
	
	void FixedUpdate(){
		animate();
	}
	
	// Update is called once per frame
	void Update (){		
		
	}
	
	void MoveForward(){
		//rigidbody.AddForce(new Vector3(0,0,forwardForce), ForceMode.Impulse);		
	}
	
	
	void OnCollisionEnter (Collision col)
    {
		//MoveForward();
		//Debug.Log("check collide with name " + col.gameObject.name );
        /*if(col.gameObject.name == "prop_powerCube"){
            Destroy(col.gameObject);
        }*/
    }
	
	void OnCollisionExit(Collision col ){
		//Debug.Log("OnCollisionExit with name " + col.gameObject.name );
	}
	
	/*
	private Vector3 calculateTrajectory(Vector3 initialVelocity, float gravityInY, float time){
		
		Vector3 outDisplacament;
		
		outDisplacament.x = initialVelocity.x * time;
		outDisplacament.z = initialVelocity.z * time;
		//outDisplacament.y = initialVelocity.y * time;
		
		float timeSquared = time * time;
		
		//outDisplacament.y = (initialVelocity.y * time ) + 0.5f * (gravityInY * timeSquared);
		outDisplacament.y = (initialVelocity.y * time ) + ( 0.1f * (gravityInY * timeSquared));
		
		return outDisplacament;
	}
	
	
	
	private void animate(){
		const float grav = -9.8f;
		time++;
		
		Vector3 grenadeDisplacement = calculateTrajectory(initVelocity, grav, time);
		this.rigidbody.transform.position = positionOfPlayerWhenThrown + grenadeDisplacement;
		
	}*/
	
	
	private Vector2 calculateTrajectory( float height, float width, float horizDisplacement ){
		Vector2 outDisplacement;
		
		
		outDisplacement.x = horizDisplacement;
		outDisplacement.y = -4*height*horizDisplacement*(horizDisplacement - width)/ (width*width);
		
		
		return outDisplacement;
	}
	
	private void animate(){
		time++;
		Vector2 grenadeDisplacement = calculateTrajectory(1, 1, time);
		this.rigidbody.transform.position = positionOfPlayerWhenThrown +  grenadeDisplacement;
	}
	
}
