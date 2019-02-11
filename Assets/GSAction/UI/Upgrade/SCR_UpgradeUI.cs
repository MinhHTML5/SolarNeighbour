using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_UpgradeUI : MonoBehaviour {
	public static SCR_UpgradeUI instance;
	
	public Upgrade 		currentUpgrade;
	public GameObject 	PFB_SubUpgradeButton;
	public GameObject 	UI_UpgradeInfo;
	public Text 		TXT_UpgradeInfo;
	public GameObject   BTN_UpgradeConfirm;
	public GameObject[] BTN_SubUpgrade;
	
	private bool upgradeInfoShowing = false;
	private float upgradeInfoAlpha = 0;
	
    
    private void Start() {
        instance = this;
		
		SCR_Config.InitUpgrade();
		
		BTN_SubUpgrade = new GameObject[(int)UpgradeType.COUNT];
		for (int i=0; i<(int)UpgradeType.COUNT; i++) {
			BTN_SubUpgrade[i] = Instantiate (PFB_SubUpgradeButton);
			BTN_SubUpgrade[i].transform.SetParent (transform);
			BTN_SubUpgrade[i].transform.localScale = new Vector3(1, 1, 1);
			BTN_SubUpgrade[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(53, -36 - 92 * i, 0);
			BTN_SubUpgrade[i].GetComponent<SCR_SubUpgradeButton>().SetParam (SCR_Config.upgrades[i]);
		}
		
		UI_UpgradeInfo.SetActive (false);
    }

    private void Update() {
		float dt = Time.deltaTime;
        if (upgradeInfoShowing == false) {
			upgradeInfoAlpha -= dt * 3;
			if (upgradeInfoAlpha < 0) {
				upgradeInfoAlpha = 0;
				UI_UpgradeInfo.SetActive (false);
			}
		}
		else {
			upgradeInfoAlpha += dt * 3;
			if (upgradeInfoAlpha > 1) upgradeInfoAlpha = 1;
		}
		UI_UpgradeInfo.GetComponent<CanvasGroup>().alpha = upgradeInfoAlpha;
		
		if (currentUpgrade != null) {
			if (SCR_Action.instance.resource >= currentUpgrade.cost) {
				BTN_UpgradeConfirm.GetComponent<Button>().interactable = true;	
			}
			else {
				BTN_UpgradeConfirm.GetComponent<Button>().interactable = false;
			}
		}
		
		if (SCR_Action.instance.gameState == GameState.ACTION) {
			for (int i=0; i<(int)UpgradeType.COUNT; i++) {
				if (SCR_Action.instance.upgradeState[i] == true) {
					BTN_SubUpgrade[i].GetComponent<Button>().interactable = false;
				}
			}
		}
    }
	
	public void ShowUpgradeInfo(Upgrade upgrade) {
		upgradeInfoShowing = true;
		TXT_UpgradeInfo.text = upgrade.desc;
		UI_UpgradeInfo.SetActive (true);
		currentUpgrade = upgrade;
	}
	public void HideUpgradeInfo() {
		upgradeInfoShowing = false;
	}
	
	public void ConfirmUpgrade() {
		SCR_Action.instance.Upgrade ((int)currentUpgrade.type);
		HideUpgradeInfo();
	}
}
