using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SCR_PickPlanetEntry : MonoBehaviour {
	public Sprite[] 		IMG_PlanetIcons;
	
	public GameObject		BTN_PickButton;
	public GameObject		BTN_ConfirmButton;
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
		BTN_PickButton.SetActive(true);
		BTN_ConfirmButton.SetActive(false);
		TXT_PlanetName.text = "Solar " + (index + 1);
	}
}
