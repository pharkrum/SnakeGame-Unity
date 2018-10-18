using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Snake : MonoBehaviour {

    // Public
    public enum Directions {
        left, right, up, down
    };

    // Protected
    protected List<GameObject> parts;
    protected Sprite[] snakeSprites;
    protected string snakeName;

    // Private
    private Directions newDirection;
    private Directions currentDirection;

    private Vector2 snakeTmpPosition;
    private Color   powerColor;

    private int power = 0;

    private bool isHealthyFoodCollision = false;
    private bool isPoisonFoodCollision  = false;
    private bool isPowerFoodCollision   = false;
    private bool isSnakePartCollision   = false;
    private bool isWallCollision        = false;
    private bool isPowered              = false; 

    public Snake() {
        parts = new List<GameObject>();
        if(SelectionManager.speed == 1) {
            Controller.frameRate = 18;
        }
        else if (SelectionManager.speed == 2) {
            Controller.frameRate = 12;
        }
        else if (SelectionManager.speed == 3) {
            Controller.frameRate = 8;
        }
    }
    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "HealthyFood") {
            OnCollision_with_healthy_food();
        }
        if (collision.gameObject.tag == "PoisonFood") {
            OnCollision_with_poison_food();
        }
        if (collision.gameObject.tag == "PowerFood") {
            OnCollision_with_power_food();
        }
        if (collision.gameObject.tag == "Wall") {
            OnCollision_with_wall();
        }
        if (collision.gameObject.tag == "SnakePart") {
            OnCollision_with_snakePart();
        }
    }

    protected abstract void OnCollision_with_healthy_food();
    protected abstract void OnCollision_with_power_food();
    protected abstract void OnCollision_with_poison_food();
    protected abstract void OnCollision_with_wall();
    protected abstract void OnCollision_with_snakePart();
    public abstract void LoadSprites();

    // Methods that implement moviment
    public void CaptureInputs() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (currentDirection != Directions.right)
                NewDirection = Directions.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (currentDirection != Directions.left)
                NewDirection = Directions.right;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)){
            if (currentDirection != Directions.down)
                NewDirection = Directions.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)){
            if (currentDirection != Directions.up)
                NewDirection = Directions.down;
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            if(this.Power == 100 && IsPowered == false)
                activePower();
        }
    }

    public void Move() {
        LoadSprites();
        MoveBody();
        MoveHead();
    }

    private void MoveHead() {
        if (NewDirection == Directions.left) {
            snakeTmpPosition = this.transform.position;
            snakeTmpPosition.x--;
            this.GetComponent<SpriteRenderer>().sprite = snakeSprites[0];
            currentDirection = NewDirection;
        }
        else if (NewDirection == Directions.right) {
            snakeTmpPosition = this.transform.position;
            snakeTmpPosition.x++;
            this.GetComponent<SpriteRenderer>().sprite = snakeSprites[1];
            currentDirection = NewDirection;
        }
        else if (NewDirection == Directions.up) {
            snakeTmpPosition = this.transform.position;
            snakeTmpPosition.y++;
            this.GetComponent<SpriteRenderer>().sprite = snakeSprites[2];
            currentDirection = NewDirection;
        }
        else if (NewDirection == Directions.down) {
            snakeTmpPosition = this.transform.position;
            snakeTmpPosition.y--;
            this.GetComponent<SpriteRenderer>().sprite = snakeSprites[3];
            currentDirection = NewDirection;
        }

        // Setup for Stone Snake power
        if (snakeName == "Stone Snake") {
            if (this.transform.position.x >= 23) {
                snakeTmpPosition = new Vector2(1, this.transform.position.y);
            }
            else if (this.transform.position.x <= 0) {
                snakeTmpPosition = new Vector2(22, this.transform.position.y);
            }
            else if (this.transform.position.y >= 15) {
                snakeTmpPosition = new Vector2(this.transform.position.x, 1);
            }
            else if (this.transform.position.y <= 0) {
                snakeTmpPosition = new Vector2(this.transform.position.x, 14);
            }
        }
        this.transform.position = snakeTmpPosition;
    }

    private void MoveBody() {
        if (parts.Count > 0) {
            //moving snake body from back to front (history)
            for (int i = parts.Count - 1; i > 0; i--) {
                parts[i].transform.position = parts[i - 1].transform.position;
            }
            parts[0].transform.position = transform.position;
        }
    }

    // Methods to implement the special power 
    abstract protected void PowerEffect();

    protected void activePower() {
        IsPowered = true;
        PowerEffect();
    }

    protected void disablePower() {
        IsPowered = false;
        this.GetComponent<SpriteRenderer>().color = Color.white;
        for (int i = 0; i < parts.Count; i++) {
            parts[i].GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    // Getters and Setters 
    public bool IsHealthyFoodCollision {
        get { return isHealthyFoodCollision; }
        set { isHealthyFoodCollision = value; }
    }

    public bool IsPoisonFoodCollision {
        get { return isPoisonFoodCollision; }
        set { isPoisonFoodCollision = value; }
    }

    public bool IsPowerFoodCollision {
        get { return isPowerFoodCollision; }
        set { isPowerFoodCollision = value; }
    }

    public bool IsSnakePartCollision {
        get { return isSnakePartCollision; }
        set { isSnakePartCollision = value; }
    }

    public bool IsWallCollision {
        get { return isWallCollision; }
        set { isWallCollision = value; }
    }

    public bool IsPowered {
        get { return isPowered; }
        set { isPowered = value;}
    }

    public Color PowerColor {
        get { return powerColor; }
        set { powerColor = value; }
    }

    public Directions NewDirection {
        get { return newDirection; }
        set { newDirection = value; }
    }

    public int Power {
        get { return power; }
        set {
            if (value > 100) {
                power = 100;
            }
            else if (value < 0) {
                power = 0;
                IsPowered = false;
                disablePower();
            }
            else {
                power = value;
            }
        }
    }

    public List<GameObject> GetParts() {
        return this.parts;
    }
}