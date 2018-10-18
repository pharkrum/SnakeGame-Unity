using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSnake : Snake {

    [SerializeField] AudioClip powered;

    public GhostSnake() {
        snakeName = "Ghost Snake";
        PowerColor = new Color(0f, 0f, 1f, 0.1f);
    }

    protected override void PowerEffect() {
        AudioSource.PlayClipAtPoint(powered, this.transform.position);
        this.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.7f);
        for (int i = 0; i < parts.Count; i++) {
            parts[i].GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.7f);
        }
    }

    protected override void OnCollision_with_healthy_food() {
        GameObject instance = Instantiate(Resources.Load("Prefabs/SnakePart", typeof(GameObject))) as GameObject;
        instance.GetComponent<SpriteRenderer>().sprite = snakeSprites[4];

        if (IsPowered) {
            instance.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.7f);
        }
        parts.Add(instance);
        IsHealthyFoodCollision = true;
    }
    protected override void OnCollision_with_poison_food() {
        IsPoisonFoodCollision = true;
    }
    protected override void OnCollision_with_power_food() {
        IsPowerFoodCollision = true;
    }
    protected override void OnCollision_with_wall() {
        IsWallCollision = true;
    }
    protected override void OnCollision_with_snakePart() {
        if (!IsPowered) {
            IsSnakePartCollision = true;
        }
    }

    public override void LoadSprites() {
        snakeSprites = Resources.LoadAll<Sprite>("Sprites/GhostSnake");
    }
}
