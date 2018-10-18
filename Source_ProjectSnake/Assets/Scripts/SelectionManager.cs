using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {
    public bool isSnakeSelected, isSpeedSelected = false;
    public static string snakeName;
    public static int speed;
    public SceneLoader loader;

    public void SnakeSelected(string name) {
        if(!isSnakeSelected){
            isSnakeSelected = true;
            snakeName = name;
        }
    }

    public void SpeedSelected(int speedValue) {
        if (!isSpeedSelected) {
            isSpeedSelected = true;
            speed = speedValue;
        }
    }

    public void Update() {
        if(isSpeedSelected & isSnakeSelected) {
            loader.LoadScene(2);
        }
    }

}
