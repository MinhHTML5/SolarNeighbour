using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class SCR_UIRightControl : MonoBehaviour {
	public static SCR_UIRightControl instance;
	
	public GameObject PNL_ShootButtonMask;
	public GameObject TXT_ShootButtonPrice;
	public GameObject BTN_Shoot;
	public GameObject BTN_CancelShoot;
	public GameObject BTN_Upgrade;
	public GameObject UI_Upgrade;
	
	private bool upgradeShowing = false;
	
	
    private void Start() {
        instance = this;
		GetComponent<RectTransform>().anchoredPosition = new Vector2 (400, 0);
    }
    private void Update() {
		if (SCR_Action.instance.gameState == GameState.ACTION) {
			int price = SCR_Config.MISSILE_BASE_PRICE;
			if (SCR_Action.instance.upgradeState[(int)UpgradeType.MATERIAL] == true) {
				price = SCR_Config.MISSILE_UPGRADE_PRICE;
			}
			if (SCR_Action.instance.upgradeState[(int)UpgradeType.CLUSTER] == true) {
				price *= 2;
			}
			TXT_ShootButtonPrice.GetComponent<Text>().text = "" + price;
		}
    }
	
	public void Show() {
		GetComponent<RectTransform>().DOAnchorPosX (0, 0.5f, true);
	}
	
	public void Hide() {
		GetComponent<RectTransform>().DOAnchorPosX (400, 0.5f, true);
	}
	
	public void SetShootCooldown(float cd) {
		if (SCR_Action.instance.gameState == GameState.ACTION) {
			if (cd > 0) {
				BTN_Shoot.GetComponent<Button>().interactable = false;
			}
			else {
				int price = SCR_Config.MISSILE_BASE_PRICE;
				if (SCR_Action.instance.upgradeState[(int)UpgradeType.MATERIAL] == true) {
					price = SCR_Config.MISSILE_UPGRADE_PRICE;
				}
				if (SCR_Action.instance.upgradeState[(int)UpgradeType.CLUSTER] == true) {
					price *= 2;
				}
				if (SCR_Action.instance.resource < price) {
					BTN_Shoot.GetComponent<Button>().interactable = false;
				}
				else {
					BTN_Shoot.GetComponent<Button>().interactable = true;
				}
			}
			PNL_ShootButtonMask.GetComponent<RectTransform>().sizeDelta = new Vector2 (155, cd * 155);
		}
	}
	
	public void ShowUpgrade() {
		if (!upgradeShowing) {
			BTN_Shoot.GetComponent<RectTransform>().DOAnchorPosX (-450, 0.5f, true);
			BTN_CancelShoot.GetComponent<RectTransform>().DOAnchorPosX (-450, 0.5f, true);
			BTN_Upgrade.GetComponent<RectTransform>().DOAnchorPosX (-450, 0.5f, true);
			UI_Upgrade.GetComponent<RectTransform>().DOAnchorPosX (0, 0.5f, true);
			upgradeShowing = true;
		}
		else {
			HideUpgrade();
		}
	}
	
	public void HideUpgrade() {
		if (upgradeShowing) {
			BTN_Shoot.GetComponent<RectTransform>().DOAnchorPosX (0, 0.5f, true);
			BTN_CancelShoot.GetComponent<RectTransform>().DOAnchorPosX (0, 0.5f, true);
			BTN_Upgrade.GetComponent<RectTransform>().DOAnchorPosX (0, 0.5f, true);
			UI_Upgrade.GetComponent<RectTransform>().DOAnchorPosX (450, 0.5f, true);
			
			SCR_UpgradeUI.instance.HideUpgradeInfo();
			upgradeShowing = false;
		}
	}
}
