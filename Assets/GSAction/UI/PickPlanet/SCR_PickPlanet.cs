﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_PickPlanet : MonoBehaviour {
	public GameObject PFB_PickPlanetEntry;
	public GameObject[] pickPlanetEntries;
	
    private void Start() {
        
    }

    private void Update() {
        
    }
	
	public void SelectPlanet (int index) {
		for (int i=0; i<pickPlanetEntries.Length; i++) {
			pickPlanetEntries[i].GetComponent<SCR_PickPlanetEntry>().Deselect();
			SCR_Action.instance.planets[i].GetComponent<SCR_Planet>().HighlightOrbit(false);
		}
		SCR_Action.instance.planets[index].GetComponent<SCR_Planet>().HighlightOrbit(true);
	}
	public void ConfirmPlanet (int index) {
		
	}
	
	public void CreatePlanetEntries (int[] planetID, int[] planetSize, float[] planetDistance) {
		pickPlanetEntries = new GameObject[planetID.Length];
		
		for (int i=0; i<planetID.Length; i++) {
			pickPlanetEntries[i] = Instantiate (PFB_PickPlanetEntry);
			pickPlanetEntries[i].transform.SetParent (transform);
			pickPlanetEntries[i].transform.localScale = new Vector3 (1, 1, 1);
			pickPlanetEntries[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(15, 420 - i * 120);
			pickPlanetEntries[i].GetComponent<SCR_PickPlanetEntry>().SetPlanetInfo (i, planetID[i], planetSize[i], planetDistance[i]);
			pickPlanetEntries[i].GetComponent<SCR_PickPlanetEntry>().pickPlanetScript = this;
			
		}
	}
}
