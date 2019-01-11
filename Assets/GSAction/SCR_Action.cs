using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Action : MonoBehaviour {    
	public float			ORBIT_DISTANCE_MIN;
	public float			ORBIT_DISTANCE_MAX;
	public int				NUMBER_OF_PLANET;
	public float			GRAVITY_CONSTANT;
	
	public static float		G;
	
	
	public GameObject[] 	PFB_Planet;

	public GameObject 		sun;
	public GameObject[] 	planets;
	
	private float currentDistance = 0;
	
	
    private void Start() {
		G = GRAVITY_CONSTANT;
		
        List<GameObject> planetsToPickFrom = new List<GameObject>(PFB_Planet);
		
		planets = new GameObject[NUMBER_OF_PLANET];
		for (int i=0; i<NUMBER_OF_PLANET; i++) {
			currentDistance += Random.Range (ORBIT_DISTANCE_MIN, ORBIT_DISTANCE_MAX);
			int chosen = Random.Range(0, planetsToPickFrom.Count);
			planets[i] = Instantiate(planetsToPickFrom[chosen]);
			planets[i].GetComponent<SCR_Planet>().Init (currentDistance, Random.Range(0, 360));
			planetsToPickFrom.RemoveAt (chosen);
		}
    }
	
    private void Update() {
		float dt = Time.deltaTime;
        float touchX = Input.mousePosition.x;
		float touchY = Input.mousePosition.y;
		
		
    }
}
