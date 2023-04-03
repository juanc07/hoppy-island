using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {

	public GameObject origin;
	public GameObject target;
	private Rigidbody rigidBody;
	public float timeTravel = 1f;

	// Use this for initialization
	void Start () {
		rigidBody = this.gameObject.GetComponent<Rigidbody>();
		Vector3 resultForce = CalculateBestThrowSpeed(origin.gameObject.transform.position,target.gameObject.transform.position,timeTravel);
		rigidBody.AddForce(resultForce,ForceMode.VelocityChange);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnCollisionEnter(Collision collision){
		Debug.Log("collision enter");
		foreach (ContactPoint contact in collision.contacts) {
			Debug.DrawRay(contact.point, contact.normal, Color.white);
		}
		rigidBody.useGravity = false;
		rigidBody.velocity = Vector3.zero;
		rigidBody.constraints = RigidbodyConstraints.FreezeAll;
	}

	private Vector3 CalculateBestThrowSpeed( Vector3 origin, Vector3 target,float timeToTarget ){
		Vector3 toTarget = target - origin;
		Vector3 toTargetXZ = toTarget;
		toTargetXZ.y =0;

		float y = toTarget.y;
		float xz = toTargetXZ.magnitude;

		float t = timeToTarget;
		float v0y = y/t + 0.5f * Physics.gravity.magnitude * t;
		float v0xz = xz /t;

		Vector3 result = toTargetXZ.normalized;
		result *= v0xz;
		result.y = v0y;


		return result;
	}
}
