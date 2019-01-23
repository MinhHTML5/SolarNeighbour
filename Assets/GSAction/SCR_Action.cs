using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vectrosity;

public enum GameState {
	INIT = 0,
	CHOOSE_PLANET,
	ACTION
}

public class SCR_Action : MonoBehaviour {
	// Const
	public const float 			MARKING_RADIUS 			= 15.0f;
	// Instance
	public static SCR_Action 	instance;
	// Prefab
	public GameObject 			PFB_Planet;
	public GameObject 			PFB_Missile;
	public Material				MAT_LineHighlight;
	// Object
	public GameObject 			sun;
	public GameObject[] 		planets;
	public GameObject[] 		missile;
	public GameObject 			homePlanet;
	
	public GameObject 			BTN_ShootMode;
	public GameObject 			BTN_CancelShootMode;
	
	// Public shit
	public GameState			gameState;
	public int					playerID;
	public int					planetID;
	public int					population;
	public int					resource;
	
	
	// Private shit
	private bool  				mouseDown				= false;
	private float				mouseDownX				= 0;
	private float 				mouseDownY				= 0;
	private float 				showMainControlDelay	= 0;
	private bool  				shootMode				= false;
		
	
	
	// Init
	private void Awake() {
		instance = this;
	}
    private void Start() {
		if (SCR_Loading.firstTimeRun) {
			SCR_Loading.LoadScene ("GSMenu/SCN_Menu");
			return;
		}
		
		gameState = GameState.INIT;
		
		SCR_UIMainInfoPanel.instance.ShowPanel();
		SCR_UIMainInfoPanel.instance.ShowWaitingForPlayers();
		
		missile = new GameObject[100];
    }
	
	// Update
    private void Update() {
		float dt = Time.deltaTime;
		
		// Handle mouse
        float touchX = Input.mousePosition.x;
		float touchY = Input.mousePosition.y;
		// Mouse drag
		if (!shootMode) {
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
					
					SCR_Camera.instance.Rotate (deltaX, deltaY);
				}
			}
			else {
				if (mouseDown) {
					mouseDown = false;
				}
			}
		}
		else {
			SCR_UIShootMode.instance.MouseHover (Input.mousePosition);
			if (Input.GetMouseButtonDown(0)) {
				Shoot (SCR_UIShootMode.instance.aimAngle, 1);
			}
		}
		
		// Mouse wheel
		if (Input.GetAxis("Mouse ScrollWheel") != 0) {
			SCR_Camera.instance.Zoom (Input.GetAxis("Mouse ScrollWheel"));
		}
		
		
		// Delay show main control
		if (showMainControlDelay > 0) {
			showMainControlDelay -= dt;
			if (showMainControlDelay <= 0) {
				showMainControlDelay = 0;
				ShowMainControl();
			}
		}
    }
	
	public void ShowMainControl () {
		SCR_UILeftControl.instance.Show();
		SCR_UIRightControl.instance.Show();
	}
	
	public void HideMainControl () {
		SCR_UILeftControl.instance.Hide();
		SCR_UIRightControl.instance.Hide();
	}
	
	public void ShootMode () {
		BTN_ShootMode.SetActive (false);
		BTN_CancelShootMode.SetActive (true);
		SCR_Camera.instance.TacticalView();
		shootMode = true;
		mouseDown = false;
		SCR_UIShootMode.instance.Show();
	}
	public void CancelShootMode () {
		BTN_ShootMode.SetActive (true);
		BTN_CancelShootMode.SetActive (false);
		SCR_Camera.instance.CasualView();
		shootMode = false;
		SCR_UIShootMode.instance.Hide();
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	// ==============================================================================================================================
	// Client / server shit
	public void CreatePlanet (int[] planetID, int[] planetSize, float[] planetDistance, float[] planetAngle, float[] planetSpeed) {
		// Create all planet objects based on server instruction
		planets = new GameObject[planetID.Length];
		for (int i=0; i<planetID.Length; i++) {
			planets[i] = Instantiate(PFB_Planet);
			planets[i].GetComponent<SCR_Planet>().Init (planetID[i], planetSize[i], planetDistance[i], planetAngle[i], planetSpeed[i]);
		}
		
		gameState = GameState.CHOOSE_PLANET;
		
		SCR_Camera.instance.PickPlanetView();
		SCR_UIMainInfoPanel.instance.ShowChooseAPlanet();
		SCR_UITimer.instance.Show();
		SCR_UITimer.instance.SetTime(SCR_Config.PICK_PLANET_TIME);
		SCR_UITimer.instance.Start();
		SCR_UIPickPlanet.instance.CreatePlanetEntries (planetID, planetSize, planetDistance);
	}
	
	public void PickPlanet (int playerIndex, int planetIndex) {
		planets[planetIndex].GetComponent<SCR_Planet>().playerID = playerIndex;
		if (playerID == playerIndex) {
			planetID = planetIndex;
			homePlanet = planets[planetIndex];
			for (int i=0; i<SCR_UIPickPlanet.instance.pickPlanetEntries.Length; i++) {
				SCR_UIPickPlanet.instance.pickPlanetEntries[i].GetComponent<SCR_UIPickPlanetEntry>().NonePick();
				SCR_UIPickPlanet.instance.pickPlanetEntries[i].GetComponent<SCR_UIPickPlanetEntry>().Deselect();
				
				if (planets[i].GetComponent<SCR_Planet>().playerID == -1) {
					planets[i].GetComponent<SCR_Planet>().HighlightOrbit(0);
				}
			}
			SCR_UIPickPlanet.instance.pickPlanetEntries[planetID].GetComponent<SCR_UIPickPlanetEntry>().MePick();
			planets[planetID].GetComponent<SCR_Planet>().HighlightOrbit(3);
		}
		else {
			SCR_UIPickPlanet.instance.pickPlanetEntries[planetIndex].GetComponent<SCR_UIPickPlanetEntry>().EnemyPick(playerIndex);
			planets[planetIndex].GetComponent<SCR_Planet>().HighlightOrbit(2);
		}
	}
	
	public void StartGame () {
		gameState = GameState.ACTION;
		SCR_Camera.instance.CasualView();
		SCR_UIMainInfoPanel.instance.HidePanel();
		SCR_UITimer.instance.Hide();
		SCR_UIPickPlanet.instance.Hide();
		
		showMainControlDelay = 1.5f;
	}
	
	public void UpdatePlanet (float[] a, int[] p, int[] r) {
		// Update all planet position based on server instruction
		for (int i=0; i<planets.Length; i++) {
			planets[i].GetComponent<SCR_Planet>().UpdateInfo (a[i], p[i], r[i]);
			
			
		
			if (i == SCR_Action.instance.planetID) {
				population = p[i];
				resource = r[i];
				SCR_UI.instance.SetResourceNumber (population, resource);
			}
		}
	}
	
	public void Shoot (float angle, float force) {
		SCR_Client.instance.Shoot (angle + Camera.main.gameObject.transform.localEulerAngles.y, force);
		CancelShootMode ();
	}
	
	public void SpawnMissile (int i, float x, float y) {
		if (missile[i] == null) {
			missile[i] = Instantiate (PFB_Missile);
		}
		missile[i].GetComponent<SCR_Missile>().Spawn(x, y);
	}
	
	public void UpdateMissile (int i, float x, float y, float vx, float vy) {
		if (missile[i] != null) {
			missile[i].GetComponent<SCR_Missile>().UpdatePos (x, y, vx, vy);
		}
	}
	
	public void KillMissile (int i) {
		if (missile[i] != null) {
			missile[i].GetComponent<SCR_Missile>().Kill();
		}
	}
	// ==============================================================================================================================
}
