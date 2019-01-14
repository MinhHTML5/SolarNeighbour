using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_Action : MonoBehaviour {
	public static SCR_Action instance;
	
	public GameObject[] 	PFB_Planet;

	public GameObject 		sun;
	public GameObject[] 	planets;
	
	
	public GameObject homePlanet;
	
	
    private void Start() {
		if (SCR_Loading.firstTimeRun) {
			SCR_Loading.LoadScene ("GSMenu/SCN_Menu");
			return;
		}
		
		instance = this;
        
		
		// TEST
		//homePlanet = planets[4];
    }
	
	public void CreatePlanet (int[] planetID, int[] planetDistance, int[] planetAngle, int[] planetSpeed) {
		planets = new GameObject[planetID.Length];
		for (int i=0; i<planetID.Length; i++) {
			Debug.Log(planetID[i]);
			planets[i] = Instantiate(PFB_Planet[planetID[i]]);
			planets[i].GetComponent<SCR_Planet>().Init (planetDistance[i], planetAngle[i], planetSpeed[i]);
		}
	}
	
    private void Update() {
		float dt = Time.deltaTime;
        float touchX = Input.mousePosition.x;
		float touchY = Input.mousePosition.y;
		
		
    }
}
