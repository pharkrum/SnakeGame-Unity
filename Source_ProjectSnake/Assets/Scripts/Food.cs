using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : MonoBehaviour {
    public int lifeTime;
    protected int value;
    protected int respawnTime;

    public void spawn(Snake snake) {
        System.Random random = new System.Random(); 

        Vector3 posFood = new Vector3(random.Next(1, 23), random.Next(1, 15), 0);

        for (int i = 0; i < snake.GetParts().Count; i++) {
            if (snake.GetParts()[i].transform.position == posFood) {
                posFood = new Vector3(random.Next(1, 23), random.Next(1, 15), 0);
            }
        }
        this.transform.position = posFood;
    }

    public void SelfDestroy() {
        Destroy(gameObject);
    }

    public  abstract void SetValues();

    public int GetValue() {
        return this.value;
    }

    public int RespawnTime() {
        return respawnTime;
    }
}
