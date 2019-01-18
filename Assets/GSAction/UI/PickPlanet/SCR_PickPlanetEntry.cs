using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SCR_PickPlanetEntry : MonoBehaviour {
	public Sprite[] 		IMG_PlanetIcons;
	
	public GameObject		BTN_PickButton;
	public GameObject		BTN_ConfirmButton;
	public GameObject		IMG_MePicked;
	public GameObject		IMG_EnemyPicked;
	public GameObject		IMG_NonePicked;
	public GameObject		IMG_PlanetIcon;
	public Text				TXT_PlanetName;
	public Text				TXT_PlanetMass;
	public Text				TXT_PlanetDistance;
	
	public SCR_PickPlanet	pickPlanetScript;
	
	private int				index;
	
	
    private void Start() {
        
    }

    private void Update() {
        
    }
	
	public void SetPlanetInfo (int id, int image, int mass, float distance) {
		index = id;
		TXT_PlanetName.text = "Solar " + (index + 1);
		IMG_PlanetIcon.GetComponent<Image>().sprite = IMG_PlanetIcons[image];
		
		if (mass == 1) {
			IMG_PlanetIcon.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			TXT_PlanetMass.text = "Mass: Tiny";
		}
		else if (mass == 2) {
			IMG_PlanetIcon.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
			TXT_PlanetMass.text = "Mass: Medium";
		}
		else if (mass == 3) {
			IMG_PlanetIcon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			TXT_PlanetMass.text = "Mass: Giant";
		}
		
		TXT_PlanetDistance.text = "Orbit: " + (Mathf.Round(distance) * 0.01) + " AU";
	}
	
	public void OnSelect() {
		pickPlanetScript.SelectPlanet(index);
		BTN_PickButton.SetActive(false);
		BTN_ConfirmButton.SetActive(true);
		TXT_PlanetName.text = "Confirm?";
	}
	public void OnConfirm() {
		pickPlanetScript.ConfirmPlanet(index);
	}
	public void Deselect() {
		if (!IMG_MePicked.activeSelf && !IMG_EnemyPicked.activeSelf && !IMG_NonePicked.activeSelf) {
			BTN_PickButton.SetActive(true);
			BTN_ConfirmButton.SetActive(false);
			TXT_PlanetName.text = "Solar " + (index + 1);
		}
	}
	
	public void MePick() {
		BTN_PickButton.SetActive(false);
		BTN_ConfirmButton.SetActive(false);
		IMG_MePicked.SetActive(true);
		IMG_EnemyPicked.SetActive(false);
		IMG_NonePicked.SetActive(false);
		TXT_PlanetName.text = "You";
	}
	public void EnemyPick(int playerID) {
		BTN_PickButton.SetActive(false);
		BTN_ConfirmButton.SetActive(false);
		IMG_MePicked.SetActive(false);
		IMG_EnemyPicked.SetActive(true);
		IMG_NonePicked.SetActive(false);
		TXT_PlanetName.text = "Player " + (playerID + 1);
	}
	public void NonePick() {
		if (!IMG_MePicked.activeSelf && !IMG_EnemyPicked.activeSelf && !IMG_NonePicked.activeSelf) {
			BTN_PickButton.SetActive(false);
			BTN_ConfirmButton.SetActive(false);
			IMG_MePicked.SetActive(false);
			IMG_EnemyPicked.SetActive(false);
			IMG_NonePicked.SetActive(true);
			TXT_PlanetName.text = "Solar " + (index + 1);
		}
	}
}
