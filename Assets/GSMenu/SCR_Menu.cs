using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Menu : MonoBehaviour {
    private void Start() {
        if (SCR_Loading.firstTimeRun) {
			SCR_Loading.LoadScene ("GSMenu/SCN_Menu");
			return;
		}
    }

    private void Update() {
        
    }
	
	public void OnPlay() {
		SCR_Loading.LoadScene ("GSAction/SCN_Action");
	}
}
