using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    // Use this for initialization
    void Start () {
        SetUpSingleton();
	}


    // Make sure that there is only one instance of this object in the game, so the song is not reloaded every time a new scene is loaded.
    private void SetUpSingleton() {
        if(FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }
}
