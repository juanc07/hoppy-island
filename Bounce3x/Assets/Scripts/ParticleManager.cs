using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleManager : MonoBehaviour {
	
	//finish line x - 10.76186f, y - 4.875145f, z- 0.5557591f
	//whale center x- 0.5072554f, y - -3.36216f, z - 0.5557558f		
	//whale left x- -5.544641f, y - -3.36216f, z - 0.5557538f
	//whale right x- 6.55912f, y - -3.36216f, z - 0.5557501f
	//left island x- -9.239939f, y - 4.875145f, z- 0.5557602f
	//mid up - x - 0.4290236f,y- 4.875145f, z- 0.5557634f
	//mid mid x- 0.4290236f, y -1.624048f, z- 0.5557634f
	
	private Vector3 rightIsland;
	private Vector3 leftIsland;
	private Vector3 midUp;
	private Vector3 midCenter;
	
	private Vector3 leftBase;
	private Vector3 centerBase;
	private Vector3 rightBase;
	
	private Vector3 defaultPosition;
	private Quaternion defaultRotation;
	
	public Transform fireWorksPrefab;
	public Transform getPowerUpPrefab;
	public Transform whaleMovePrefab;
	public Transform whaleHitPrefab;
	public Transform sunlightPrefab;
	public Transform splashPrefab;
	public Transform waterRipplePrefab;
	
	public List<ParticleData> particlePool = new List<ParticleData>();
	private Hashtable particleSet = new Hashtable();
	private Hashtable positionSet = new Hashtable();
	
	public enum ParticleTypes{
		firework,
		getPower,
		whaleMove,
		whaleHit,
		sunlightHit,
		splash,
		waterRipple
	}
	
	public enum Locations{
		left,
		center,
		right,
		upleft,
		upMiddle,
		upRight,
		middleCenter,
		defaultPosition
	}
	
	void Awake(){		
	}
	
	// Use this for initialization
	void Start (){
		//rightIsland = new Vector3(10.76186f,4.875145f,0.5557591f );
		rightIsland = new Vector3(10.76186f,3.5f,0.5557591f );
		centerBase = new Vector3(0.5072554f,-3.36216f,0.5557558f );
		leftBase = new Vector3(-5.544641f,-3.36216f,0.5557538f );
		rightBase = new Vector3(6.55912f,-3.36216f, 0.5557501f);
		leftIsland = new Vector3(-9.239939f,4.875145f, 0.5557602f);
		midUp = new Vector3(0.4290236f,4.875145f,0.5557634f);
		midCenter = new Vector3(0.4290236f,1.624048f,0.5557634f);
		defaultPosition = new Vector3(0,0,0);
		defaultRotation = Quaternion.Euler(0,0,0);
		
		positionSet[ Locations.left] = leftBase;
		positionSet[ Locations.center] = centerBase;
		positionSet[ Locations.right] = rightBase;
		positionSet[ Locations.upleft] = leftIsland;
		positionSet[ Locations.upRight] = rightIsland;
		positionSet[ Locations.upMiddle] = midUp;
		positionSet[ Locations.middleCenter] = midCenter;
		positionSet[ Locations.upMiddle] = midUp;
		positionSet[ Locations.defaultPosition] = defaultPosition;
		
		particleSet[ ParticleTypes.firework] = fireWorksPrefab;
		particleSet[ ParticleTypes.getPower] = getPowerUpPrefab;
		particleSet[ ParticleTypes.whaleMove] = whaleMovePrefab;
		particleSet[ ParticleTypes.whaleHit] = whaleHitPrefab;
		particleSet[ ParticleTypes.sunlightHit] = sunlightPrefab;
		particleSet[ ParticleTypes.splash] = splashPrefab;
		particleSet[ ParticleTypes.waterRipple] = waterRipplePrefab;
		
		//CastParticle( 1, 0 );	
	}
	
	// Update is called once per frame
	void Update (){
		ParticleChecker();
	}	

	public void ShowParticle( ParticleTypes particleType , Vector3 position, Quaternion rotation){
		//Debug.Log("call show particle'");
		bool found = ActivateParticleByParticleType(particleType,position,rotation);		
		if(!found){
			Transform particlePrefab = GetParticlePrefab(particleType);
			Transform particle  = Instantiate( particlePrefab) as Transform;
			particle.transform.parent = this.transform;
			particle.transform.rotation = rotation;
			particle.transform.position = position;
			
			ParticleData particleData = new ParticleData();
			particleData.id = particlePool.Count;
			particleData.isActive = true;
			particleData.obj = particle;
			particleData.particleType = particleType;
			particleData.name = particleType.ToString() + particleData.id;
			particle.gameObject.name = particleData.name;
			particlePool.Add(particleData);
			//Debug.Log("create new ripple!");
		}
	}
	
	public void CastParticle( ParticleTypes particleType , Locations location, float locationOffsetY = 0f){		
		Transform particlePrefab = GetParticlePrefab(particleType);	
		Transform particle  = Instantiate( particlePrefab) as Transform;
		particle.transform.parent = this.transform;
		particle.transform.localRotation = defaultRotation;
		
		if(particleType == ParticleTypes.splash ){
			Vector3 currentPos = GetLocation(location);			
			currentPos.y += locationOffsetY;			
			particle.transform.localPosition = currentPos;
		}else{
			particle.transform.localPosition = GetLocation(location);
		}		
	}
	
	public void CastParticle2( ParticleTypes particleType , Locations location){
		//Debug.Log( "aim particle" );		
		bool found = ActivateParticleByParticleType(particleType,location);		
		if(!found){
			Transform particlePrefab = GetParticlePrefab(particleType);	
			Transform particle  = Instantiate( particlePrefab) as Transform;
			particle.transform.parent = this.transform;
			particle.transform.localRotation = defaultRotation;
			particle.transform.localPosition = GetLocation(location);
			
			ParticleData particleData = new ParticleData();
			particleData.id = particlePool.Count;
			particleData.isActive = true;
			particleData.obj = particle;
			particleData.particleType = particleType;
			particleData.name = particleType.ToString() + particleData.id;
			particle.gameObject.name = particleData.name;
			particlePool.Add(particleData);			
		}		
	}
	
	private void ParticleChecker(){
		int len = particlePool.Count;
		for(int index =0;index<len;index++){
			if(particlePool[index].isActive){
				ParticleSystem particleSystem = particlePool[index].obj.gameObject.GetComponent<ParticleSystem>();
				if(particleSystem.isStopped){
					//deactivate here
					particlePool[index].isActive =false;
					particleSystem.Stop();
					particlePool[index].obj.gameObject.SetActive(false);
					//Debug.Log("turn off particle index " + index);
				}
			}
		}
	}
	
	private bool ActivateParticleByParticleType( ParticleTypes particleType, Locations location ){
		int len = particlePool.Count;
		bool found =false;
		
		for(int index =0;index<len;index++){			
			if(!particlePool[index].isActive && particlePool[index].particleType == particleType ){
				particlePool[index].isActive =true;
				particlePool[index].obj.gameObject.SetActive(true);
				particlePool[index].obj.gameObject.transform.localPosition = GetLocation(location);
				ParticleSystem particleSystem = particlePool[index].obj.gameObject.GetComponent<ParticleSystem>();
				particleSystem.Play();
				found = true;
				//Debug.Log("reusable particle index " + index);
				break;
			}
		}
		
		return found;
	}

	private bool ActivateParticleByParticleType( ParticleTypes particleType, Vector3 position, Quaternion rotation){
		int len = particlePool.Count;
		bool found =false;
		
		for(int index =0;index<len;index++){			
			if(!particlePool[index].isActive && particlePool[index].particleType == particleType ){
				particlePool[index].isActive =true;
				particlePool[index].obj.gameObject.SetActive(true);
				particlePool[index].obj.gameObject.transform.position = position;
				particlePool[index].obj.gameObject.transform.rotation = rotation;
				ParticleSystem particleSystem = particlePool[index].obj.gameObject.GetComponent<ParticleSystem>();
				particleSystem.Play();
				found = true;
				//Debug.Log("reuse ripple!");
				//Debug.Log("reusable particle index " + index);
				break;
			}
		}
		
		return found;
	}
	
	private Transform GetParticlePrefab(ParticleTypes particleType){
		Transform particlePrefab = particleSet[particleType] as Transform;		
		return particlePrefab;
	}	
	
	private Vector3 GetLocation(Locations location){
		Vector3 currentLocation = new Vector3(0,0,0);
		currentLocation =(Vector3)positionSet[location];		
		return currentLocation;
	}
}