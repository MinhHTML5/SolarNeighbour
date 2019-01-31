using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_SubUpgradeButton : MonoBehaviour {
	public Upgrade upgrade;
    public Text txtCaption;
	public Text txtPrice;
	
    public void SetParam(Upgrade up) {
		upgrade = up;
        txtCaption.text = upgrade.name;
		txtPrice.text = "" + upgrade.cost;
		//gameObject.GetComponent<Button>().interactable = false;
    }
	
	public void OnClick() {
		SCR_UpgradeUI.instance.ShowUpgradeInfo (upgrade);
	}
	
	private void Update() {
		/*
        if (SCR_Action.instance.resource >= upgrade.cost) {
			gameObject.GetComponent<Button>().interactable = true;	
		}
		else {
			gameObject.GetComponent<Button>().interactable = false;
		}
		*/
    }
}
