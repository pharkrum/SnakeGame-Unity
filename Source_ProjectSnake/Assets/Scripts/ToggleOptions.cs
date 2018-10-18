using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOptions : MonoBehaviour {

    private Toggle toggle;

	// Use this for initialization
	void Start () {
        toggle = this.GetComponent<Toggle>();
	}
	
	// Update is called once per frame
	void Update () {
        //Set the color change of the Toggle buttons when they are clicked
        if (toggle.isOn) {
            toggle.transform.GetChild(0).GetComponent<Image>().color = new Color32(70, 188, 71, 255);
        }
        else {
            toggle.transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        
	}
}
