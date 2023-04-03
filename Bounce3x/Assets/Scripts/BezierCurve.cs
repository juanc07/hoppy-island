using UnityEngine;
using System.Collections;

public class BezierCurve : MonoBehaviour {

	public bool isStart;
	public bool isReset;
	private Vector3 initialPosition;

	public Vector3 startingPoint;
	public Vector3 startingPointHandle;
	public Vector3 endPointHandle;
	public Vector3 endPoint;

	public float time;
	public float speed;

	public GameObject startingPointObj;
	public GameObject startingHandlePointObj;

	public GameObject endingHandlePointObj;
	public GameObject endingPointObj;



	public bool useObjPoint;

	// Use this for initialization
	void Start () {
		initialPosition = this.gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update (){
		if(isStart){
			//this.gameObject.transform.position = CalculateBezierPoint(time,initialPosition,startingPointHandle,endPointHandle,endPoint);
			Vector3 target;

			if(useObjPoint){
				target = CalculateBezierPoint(time,startingPointObj.transform.position,startingHandlePointObj.transform.position,endingHandlePointObj.transform.position,endingPointObj.transform.position);
			}else{
				target = CalculateBezierPoint(time,initialPosition,startingPointHandle,endPointHandle,endPoint);
			}

			this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position,target,speed);
		}

		if(isReset){
			this.gameObject.transform.position = initialPosition;
		}
	}

	private Vector3 CalculateBezierPoint(float t,Vector3 p0,Vector3 p1,Vector3 p2,Vector3 p3){
		float u = 1.0f - t;
		float tt = t * t;
		float uu = u * u;
		float uuu= uu * u;
		float ttt = tt * t;
		
		Vector3 p = uuu * p0; //first term
		p += 3 * uu * t * p1; //second term
		p += 3 * u * tt * p2; //third term
		p += ttt * p3; //fourth term
		
		return p;
	}
}
