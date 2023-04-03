using UnityEngine;
using System.Collections;

public class BallGenerator : MonoBehaviour {
	
	public Rigidbody ballPrefab;
    public Transform generatorPos;
	private float delay = 9f;
	public GameObject animalHolder;
	
	private ArrayList animals = new ArrayList();
	
	// Use this for initialization
	void Start (){
		addBall();
		startTimer();
	}
	
	void startTimer(){
		//print("Starting " + Time.time);
        StartCoroutine(WaitAndPrint(delay));
        //print("Before WaitAndPrint Finishes " + Time.time);
	}
	
	IEnumerator WaitAndPrint(float waitTime) {
        yield return new WaitForSeconds(waitTime);
       // print("WaitAndPrint " + Time.time);
		addBall();
		if( delay > 3f ){
			delay -= 0.2f;
		}		
		startTimer();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void addBall(){
		Rigidbody animalInstance;		
		Vector3 vec = new Vector3(0,0,0);
        animalInstance = Instantiate(ballPrefab, ( generatorPos.position- vec), generatorPos.rotation) as Rigidbody;		
		animalInstance.transform.parent = animalHolder.transform;
		animals.Add(animalInstance.gameObject);        
	}
	
	public void clearAllAnimals(){
		animals.Clear();
	}
}
