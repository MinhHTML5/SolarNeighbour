using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_Action : MonoBehaviour {
	// Instance
	public static SCR_Action instance;
	// Prefab
	public GameObject[] 	PFB_Planet;
	// Object
	public GameObject 		sun;
	public GameObject[] 	planets;
	public GameObject 		homePlanet;
	
	public SCR_PickPlanet	uiPickPlanetScript;
	
	// Public shit
	public GameState		gameState;
	public int				playerID;
	public int				planetID;
	
	// Private shit
	private SCR_Camera cameraScript		= null;
	private bool  mouseDown				= false;
	private float mouseDownX			= 0;
	private float mouseDownY			= 0;
	
	
	
	
	
	// Init
    private void Start() {
		if (SCR_Loading.firstTimeRun) {
			SCR_Loading.LoadScene ("GSMenu/SCN_Menu");
			return;
		}
		
		instance = this;
		gameState = GameState.INIT;
		cameraScript = Camera.main.GetComponent<SCR_Camera>();
		
		
		SCR_MainInfoPanel.instance.ShowPanel();
		SCR_MainInfoPanel.instance.ShowWaitingForPlayers();
    }
	
	// Update
    private void Update() {
		float dt = Time.deltaTime;
		
		// Handle mouse
        float touchX = Input.mousePosition.x;
		float touchY = Input.mousePosition.y;
		// Mouse drag
		if (Input.GetMouseButton(0)) {
			if (mouseDown == false) {
				mouseDown = true;
				mouseDownX = Input.mousePosition.x;
				mouseDownY = Input.mousePosition.y;
			}
			else {
				float deltaX = Input.mousePosition.x - mouseDownX;
				float deltaY = Input.mousePosition.y - mouseDownY;
				mouseDownX = Input.mousePosition.x;
				mouseDownY = Input.mousePosition.y;
				
				cameraScript.Rotate (deltaX, deltaY);
			}
		}
		else {
			if (mouseDown) {
				mouseDown = false;
			}
		}
		// Mouse wheell
		if (Input.GetAxis("Mouse ScrollWheel") != 0) {
			cameraScript.Zoom (Input.GetAxis("Mouse ScrollWheel"));
		}
    }
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	// ==============================================================================================================================
	// Client / server shit
	public void CreatePlanet (int[] planetID, int[] planetSize, float[] planetDistance, float[] planetAngle, float[] planetSpeed) {
		// Create all planet objects based on server instruction
		planets = new GameObject[planetID.Length];
		for (int i=0; i<planetID.Length; i++) {
			planets[i] = Instantiate(PFB_Planet[planetID[i]]);
			planets[i].GetComponent<SCR_Planet>().Init (planetSize[i], planetDistance[i], planetAngle[i], planetSpeed[i]);
		}
		
		gameState = GameState.CHOOSE_PLANET;
		cameraScript.PickPlanet();
		
		uiPickPlanetScript.CreatePlanetEntries (planetID, planetSize, planetDistance);
	}
	
	public void PickPlanet (int playerIndex, int planetIndex) {
		planets[planetIndex].GetComponent<SCR_Planet>().playerID = playerIndex;
		if (playerID == playerIndex) {
			planetID = planetIndex;
			homePlanet = planets[planetIndex];
			for (int i=0; i<uiPickPlanetScript.pickPlanetEntries.Length; i++) {
				uiPickPlanetScript.pickPlanetEntries[i].GetComponent<SCR_PickPlanetEntry>().NonePick();
			}
			uiPickPlanetScript.pickPlanetEntries[planetID].GetComponent<SCR_PickPlanetEntry>().MePick();
		}
		else {
			uiPickPlanetScript.pickPlanetEntries[planetIndex].GetComponent<SCR_PickPlanetEntry>().EnemyPick(playerIndex);
		}
	}
	
	public void UpdatePlanet (float[] planetAngle) {
		// Update all planet position based on server instruction
		for (int i=0; i<planets.Length; i++) {
			planets[i].GetComponent<SCR_Planet>().UpdateAngle (planetAngle[i]);
		}
	}
	// ==============================================================================================================================
}
