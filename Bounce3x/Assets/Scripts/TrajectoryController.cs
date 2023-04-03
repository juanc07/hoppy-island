using UnityEngine;
using System.Collections;

public class TrajectoryController : MonoBehaviour {
	
	public Rigidbody projectile;
	public Transform spawnTarget;
	
	public LineRenderer predictionLine;
	private Vector3 startingVelocity;
	private float horizontalOffset = 0.0f;
	public float distance = 1.0f;
	public bool enablePrediction = true;
	private float lob = 0.75f;
	//private float lob = 1f;
	
	//test
	//private GameObject paddle;
	
	// Use this for initialization
	void Start () {
		//paddle = GameObject.Find("paddle");
		predictionLine.SetWidth(0.2f,0.2f);
	}
	
	// Update is called once per frame
	void Update (){
		//startingVelocity = Quaternion.AngleAxis(horizontalOffset, Vector3.up) * (transform.forward * (20f * distance));
		startingVelocity = GetTrajectoryVelocity(spawnTarget.position, GetMousePosition(), lob, Physics.gravity);	
		//startingVelocity = GetTrajectoryVelocity(spawnTarget.position, paddle.transform.position, lob, Physics.gravity);	
		
		if( Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) ){
			//Debug.Log("space!!");
			FireProjectile();
		}
		
		if(Input.GetKey(KeyCode.LeftArrow)) {
			horizontalOffset -= 25 * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.RightArrow)) {
			horizontalOffset += 25 * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.UpArrow)) {
			distance += 0.75f * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.DownArrow)) {
			distance -= 0.75f * Time.deltaTime;
		}
		
		horizontalOffset = Mathf.Clamp(horizontalOffset, -45.0f, 35.0f);		
		
		lob += Input.GetAxis("Mouse ScrollWheel") * 0.01f;
		
		lob = Mathf.Clamp(lob, 0.25f, 1.2f);
		
		distance = Mathf.Clamp(distance, 0.25f, 1.5f);
		
		if(enablePrediction)
			UpdatePredictionLine();
	}
	
	/*void UpdatePredictionLine(){
	   predictionLine.SetVertexCount(180);
	   for(int i = 0; i < 180; i++)
	   {
	      Vector3 posN = GetTrajectoryPoint(spawnTarget.position, startingVelocity, i, Physics.gravity);
	      predictionLine.SetPosition(i,posN);
	   }
	}*/
	
	void UpdatePredictionLine() 
	{
		predictionLine.SetVertexCount(180);
		Vector3 previousPosition = spawnTarget.position;
		for(int i = 0; i < 180; i++)
		{
			Vector3 currentPosition = GetTrajectoryPoint(spawnTarget.position, startingVelocity, i, 1, Physics.gravity);
			Vector3 direction = currentPosition - previousPosition;
			direction.Normalize();
			
			float distance = Vector3.Distance(currentPosition, previousPosition);
			
			RaycastHit hitInfo = new RaycastHit();
			if(Physics.Raycast(previousPosition, direction, out hitInfo, distance))
			{
				predictionLine.SetPosition(i,hitInfo.point);
				predictionLine.SetVertexCount(i);
				break;
			}
			
			previousPosition = currentPosition;
			predictionLine.SetPosition(i,currentPosition);
		}
	}
	
	/*Vector3 GetTrajectoryPoint(Vector3 startingPosition, Vector3 initialVelocity, float timestep, Vector3 gravity)
	{
	    float physicsTimestep = Time.fixedDeltaTime;
	    Vector3 stepVelocity = physicsTimestep * initialVelocity;
	 
	    //Gravity is already in meters per second, so we need meters per second per second
	    Vector3 stepGravity = physicsTimestep * physicsTimestep * gravity;
	 
	    return startingPosition + (timestep * stepVelocity) + ((( timestep * timestep + timestep) * stepGravity ) / 2.0f);
	}*/
	
	Vector3 GetTrajectoryPoint(Vector3 startingPosition, Vector3 initialVelocity, float timestep, float lob, Vector3 gravity)
	{
    	float physicsTimestep = Time.fixedDeltaTime;
    	Vector3 stepVelocity = initialVelocity * physicsTimestep;
		
		//Gravity is already in meters per second, so we need meters per second per second
		Vector3 stepGravity = gravity * physicsTimestep * physicsTimestep;
		
		return startingPosition + (timestep * stepVelocity) + ((( timestep * timestep + timestep) * stepGravity ) / 2.0f);
	}
	
	Vector3 GetMousePosition()
	{
		//Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//Debug.Log("mouse pos " + Input.mousePosition );
		
		RaycastHit[] hits = Physics.RaycastAll(ray);
		
		if(hits.Length > 0)
			return hits[0].point;
		else
			return Vector3.zero;
	}
	
	public static Vector3 GetTrajectoryVelocity(Vector3 startingPosition, Vector3 targetPosition, float lob, Vector3 gravity)
	{
		float physicsTimestep = Time.fixedDeltaTime;
	    float timestepsPerSecond = Mathf.Ceil(1f/physicsTimestep);
		
		//By default we set n so our projectile will reach our target point in 1 second
	    float n = lob * timestepsPerSecond;
		
	    Vector3 a = physicsTimestep * physicsTimestep * gravity;
		Vector3 p = targetPosition;
		Vector3 s = startingPosition;
	    
		Vector3 velocity = (s + (((n * n + n) * a) / 2f) - p) * -1 / n;
		
		//This will give us velocity per timestep. The physics engine expects velocity in terms of meters per second
		velocity /= physicsTimestep;
		return velocity;
	}
	
	
	void FireProjectile() {
		Vector3 offsetZ =new Vector3( 0,0, 3 );
		//Rigidbody newProjectile = Instantiate(projectile, spawnTarget.position + offsetZ, Quaternion.identity) as Rigidbody;    		
		Rigidbody newProjectile = Instantiate(projectile, spawnTarget.position, Quaternion.identity) as Rigidbody;
   		newProjectile.AddForce(startingVelocity, ForceMode.Impulse);
		
		/*Rigidbody rb = (Rigidbody)Instantiate(projectile);
		rb.transform.position = spawnTarget.position;// + new Vector3( 0,0, 5 );		
		rb.AddForce(startingVelocity, ForceMode.Impulse);*/
	}
}
