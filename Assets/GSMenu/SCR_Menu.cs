using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class SCR_Menu : MonoBehaviour {
	public InputField INP_Name;
	
    private void Start() {
        if (SCR_Loading.firstTimeRun) {
			SCR_Loading.LoadScene ("GSMenu/SCN_Menu");
			return;
		}
		
		DOTween.Init();
    }

    private void Update() {
        
    }
	
	public void OnPlay() {
		if (INP_Name.text != "") {
			SCR_Action.playerName = INP_Name.text;
		}
		else {
			SCR_Action.playerName = "Anonymous";
		}
		SCR_Loading.LoadScene ("GSAction/SCN_Action");
	}
	
	public void OnMulti() {
		if (INP_Name.text != "") {
			SCR_Action.playerName = INP_Name.text;
			//SCR_Loading.LoadScene ("GSAction/SCN_Action");
		}
	}
}
