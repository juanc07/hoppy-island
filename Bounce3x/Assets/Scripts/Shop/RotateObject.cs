using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {
	
	public float speed = 70f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.transform.Rotate( Vector3.up * (Time.deltaTime * speed) );
	}
}
