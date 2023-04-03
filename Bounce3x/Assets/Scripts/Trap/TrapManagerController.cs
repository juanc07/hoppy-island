using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrapManagerController : MonoBehaviour {
	
	public Rigidbody[] trapPrefabs;
    public Transform trapManagerPos;
	//private float delay = 7f;
	public GameObject trapHolder;
	
	private GameDataManagerController gdc;
	
	private int idCount = 0;
	private List<Trap> trapPool = new List<Trap>();
	
	private GameObject animalGenerator;
	private AnimalGeneratorController animalGenController;
	private GameManagerController gameManagerController;
		
	// Use this for initialization
	void Start (){
		GetGameManagerController();

		gdc = GameDataManagerController.GetInstance();		
		animalGenerator = GameObject.Find("AnimalGenerator");
		animalGenController = animalGenerator.GetComponent<AnimalGeneratorController>();		

		AddEventListener();
	}

	private void OnEnable(){
		GetGameManagerController();
	}
	
	private void OnDestroy(){
		RemoveEventListener();
	}
	
	private void AddEventListener(){
		Messenger.AddListener( GameEvent.Level_Failed, onLevelFailed );
		//Messenger.AddListener( GameEvent.Level_Restart, onLevelRestart );
		Messenger.AddListener( GameEvent.HitIndex0, onHitIndex0 );
		Messenger.AddListener( GameEvent.HitIndex1, onHitIndex1 );
		Messenger.AddListener( GameEvent.HitIndex2, onHitIndex2 );
	}
	
	private void RemoveEventListener(){
		if(gameManagerController!=null){
			gameManagerController.OnLevelRestart-=OnLevelRestart;
		}
	}
	
	private void GetGameManagerController(){
		if(gameManagerController==null){
			gameManagerController = GameManagerController.GetInstance();
			gameManagerController.OnLevelRestart+=OnLevelRestart;
		}
	}

	private void OnLevelRestart(){
		if(this == null)return;		
		DeactivateAllTrap();
		Debug.Log( " onLevelRestart clearAllTraps " );
	}
	
	/*private void onLevelRestart(){
		if(this == null)return;		
		DeactivateAllTrap();
		Debug.Log( " onLevelRestart clearAllTraps " );
	}*/
	
	private void onLevelFailed(){
		if(this == null)return;
		//clearAllTraps();
		DeactivateAllTrap();
		Debug.Log( " onLevelFailed clearAllTraps " );
	}
	
	private void onHitIndex0(){
		if(this ==null)return;
		AddTrap(0);
	}
	
	private void onHitIndex1(){
		if(this ==null)return;
		AddTrap(1);
	}
	
	private void onHitIndex2(){
		if(this ==null)return;
		AddTrap(2);
	}
	
	void Update(){		
		//AddTrap(-1);
	}
	
	/*private IEnumerator  WaitAndAddAnother(float waitingTime){
		yield return new WaitForSeconds(waitingTime);
		addTrap();
	}*/
	
	public void AddTrap(int index){		
		if(gdc.GetLife() <= 0 || gdc.IsGameOver){
			return;
		}
		
		int rnd = Random.Range(0,101);
		
		if(gdc.currentLevel >= 5 && rnd >= 95){
			if( animalGenController.GetActiveAnimalCount() > 0 && index == 1 && GetActiveTrapCount() == 0 ){		
				Debug.Log("add trap now!!!");
				Invoke("CreateTrap", 1f);			
			}else if( animalGenController.GetActiveAnimalCount() > 0 && index == 2 && GetActiveTrapCount() == 0 ){		
				Debug.Log("add trap now!!!");
				Invoke("CreateTrap", 4f);
			}
		}				
	}	
	
	public void CreateTrap(){
		if(gdc.GetLife() <= 0 ||  gdc.IsGameOver){
			return;
		}
		
		//int rnd = Random.Range(0,trapPrefabs.Length);
		int rnd =trapPrefabs.Length - 1;
		Trap.TrapTypes trapType = ConvertIntToTrapType(rnd);
		bool found = ActivateTrapByTrapType(trapType);
		
		if(!found){	
			Rigidbody trapInstance;		
			Vector3 vec = new Vector3(0,0,0);
			Quaternion trapRotation = Quaternion.Euler( 0,0,0);		
	        trapInstance = Instantiate(trapPrefabs[rnd], ( trapManagerPos.position- vec), trapRotation) as Rigidbody;
			
			if(trapInstance!=null){
				trapInstance.transform.parent = trapHolder.transform;		
				TrapTrajectory trapTrajectory = trapInstance.gameObject.GetComponent<TrapTrajectory>();			
				trapTrajectory.Id = idCount;		
				Trap trap = new Trap();
				trap.isActive = true;
				trap.id = idCount;
				trap.trapType = trapType;
				trap.name = trapType.ToString();
				trapInstance.gameObject.name= trap.name+idCount;
				trap.obj = trapInstance;				
				trapPool.Add(trap);
				idCount++;
			}
		}
	}
	
	public void DeactivateTrapById(int id,bool hasDamage){
		int len =trapPool.Count;
		for(int index=0;index<len;index++){
			if(trapPool[index].id == id && trapPool[index].isActive){
				trapPool[index].isActive = false;
				trapPool[index].obj.velocity = Vector3.zero;
				trapPool[index].obj.gameObject.GetComponent<TrapTrajectory>().Reset();
				trapPool[index].obj.gameObject.GetComponent<TrailRenderer>().enabled = false;
				trapPool[index].obj.gameObject.SetActive(false);
				//TrapTrajectory trapTrajectory  = trapPool[index].obj.gameObject.GetComponent<TrapTrajectory>();
				if(hasDamage){
					gdc.UpdateLife(-1);
					if(GetActiveTrapCount() == 0){
						AddTrap(-1);
					}
				}							
				break;
			}
		}
	}
	
	public void DeactivateAllTrap(){
		int len =trapPool.Count;
		for(int index=0;index<len;index++){
			if(trapPool[index].isActive){
				trapPool[index].isActive = false;
				trapPool[index].obj.velocity = Vector3.zero;
				trapPool[index].obj.gameObject.GetComponent<TrailRenderer>().enabled = false;
				trapPool[index].obj.gameObject.GetComponent<TrapTrajectory>().Reset();
				trapPool[index].obj.gameObject.SetActive(false);				
			}
		}
	}
	
	public bool ActivateTrapByTrapType(Trap.TrapTypes trapType){
		int len =trapPool.Count;
		bool found =false;
		
		for(int index=0;index<len;index++){
			if(!trapPool[index].isActive && trapPool[index].trapType == trapType ){
				trapPool[index].isActive = true;				
				trapPool[index].obj.gameObject.SetActive(true);
				trapPool[index].obj.gameObject.GetComponent<TrapTrajectory>().Reset();				
				trapPool[index].obj.gameObject.GetComponent<TrailRenderer>().enabled = true;
				found =true;
				break;
			}
		}		
		return found;
	}
	
	public int GetActiveTrapCount(){
		int len =trapPool.Count;
		int count = 0;
		for(int index=0;index<len;index++){
			if(trapPool[index].isActive){
				count++;
			}
		}		
		return count;
	}
	
	private Trap.TrapTypes ConvertIntToTrapType(int val){
		Trap.TrapTypes trapType = Trap.TrapTypes.Bomb;
		switch(val){
			case 0:
				trapType = Trap.TrapTypes.Bomb;
			break;			
		}
		
		return trapType;
	}
	
	public void GoDirectGoalAll(){				
		int trapCount = trapPool.Count;
		for( int trapIndex = 0; trapIndex < trapCount; trapIndex++ ){
			Trap trap = trapPool[trapIndex];
			if(trap != null){
				TrapTrajectory trapTrajectory  = trap.obj.gameObject.GetComponent<TrapTrajectory>();
				trapTrajectory.DirectGoal();	
			}						
		}
	}
	
	public int GetNearestToFall(){
		int index = 0;		
		if(GetActiveTrapCount() > 0){		
			trapPool.Sort(new sortTrapPosY());			
			Trap trap= trapPool[0];			
			TrapTrajectory trapTrajectory = trap.obj.gameObject.GetComponent<TrapTrajectory>();
			if( trapTrajectory.currentPoint == gdc.point1 ){
				index =  0;
			}else if( trapTrajectory.currentPoint == gdc.point2 ){
				index =  1;
			}else if( trapTrajectory.currentPoint == gdc.point3 ){
				index =  2;
			}			
		}		
		return index;		
	}
	
	public float GetLob(){
		float lobTotal= 0f;		
		float min = 0;
		float mid = 0;
		float max = 0;
		
		for( int trapIndex = 0; trapIndex < trapPool.Count; trapIndex++ ){			
			Trap trap = trapPool[trapIndex];
			TrapTrajectory trapTrajectory = trap.obj.gameObject.GetComponent<TrapTrajectory>();
			
			if(trapTrajectory.lob == gdc.minLob){
				min++;
			}else if(trapTrajectory.lob == gdc.midLob){
				mid++;
			}else if(trapTrajectory.lob == gdc.maxLob){
				max++;
			}		
			lobTotal +=trapTrajectory.lob;
		}		
		
		
		min = ( min/trapPool.Count) * 100;
		mid = ( mid/trapPool.Count) * 100;
		max = ( max/trapPool.Count) * 100;		
		
		
		if(min >= 50){
			return gdc.midLob;
		}else if(max >= 50){
			return gdc.minLob;
		}else if(mid >= 50){
			return gdc.maxLob;
		}else{			
			return gdc.midLob;			
		}			
	}
	
	
	private static int ComparePosY(Trap a, Trap b){
		Transform p1	=	a.obj.gameObject.GetComponent<Transform>();
		Transform p2 	=	b.obj.gameObject.GetComponent<Transform>();
			
		if (p1.position.y < p2.position.y)
			return -1;
		else if (p1.position.y > p2.position.y)
			return 1;
		else
			return 0;
	}
}


public class sortTrapPosY: IComparer<Trap>{	
	 int IComparer<Trap>.Compare(Trap a, Trap b){			
		Transform p1=a.obj.gameObject.GetComponent<Transform>();
		Transform p2 = b.obj.gameObject.GetComponent<Transform>();
			
		if (p1.position.y < p2.position.y)
			return -1;
		else if (p1.position.y > p2.position.y)
			return 1;
		else
			return 0;
	}	
}