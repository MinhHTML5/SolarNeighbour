﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class SCR_UIPickPlanet : MonoBehaviour {
	public static SCR_UIPickPlanet instance;
	public GameObject PFB_PickPlanetEntry;
	
	
	public GameObject[] pickPlanetEntries;
	
    private void Start() {
        instance = this;
    }

    private void Update() {
        
    }
	
	public void SelectPlanet (int index) {
		for (int i=0; i<pickPlanetEntries.Length; i++) {
			pickPlanetEntries[i].GetComponent<SCR_UIPickPlanetEntry>().Deselect();
			
			if (SCR_Action.instance.planets[i].GetComponent<SCR_Planet>().playerID == -1) {
				SCR_Action.instance.planets[i].GetComponent<SCR_Planet>().HighlightOrbit(0);
			}
		}
		SCR_Action.instance.planets[index].GetComponent<SCR_Planet>().HighlightOrbit(1);
	}
	public void ConfirmPlanet (int index) {
		SCR_Client.instance.ChoosePlanet (index);
	}
	
	public void CreatePlanetEntries (int[] planetID, int[] planetSize, float[] planetDistance) {
		pickPlanetEntries = new GameObject[planetID.Length];
		
		for (int i=0; i<planetID.Length; i++) {
			pickPlanetEntries[i] = Instantiate (PFB_PickPlanetEntry);
			pickPlanetEntries[i].transform.SetParent (transform);
			pickPlanetEntries[i].transform.localScale = new Vector3 (1, 1, 1);
			pickPlanetEntries[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(15, 420 - i * 120);
			pickPlanetEntries[i].GetComponent<SCR_UIPickPlanetEntry>().SetPlanetInfo (i, planetID[i], planetSize[i], planetDistance[i]);
			pickPlanetEntries[i].GetComponent<SCR_UIPickPlanetEntry>().pickPlanetScript = this;
		}
		
		GetComponent<RectTransform>().DOAnchorPosX (0, 0.5f, true);
	}
	
	public void Hide () {
		GetComponent<RectTransform>().DOAnchorPosX (500, 0.5f, true);
	}
}
