using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
    private Snake snake;
    private HealthyFood healthyFoodInstance;
    private PoisonFood poisonFoodInstance;
    private PowerFood PowerFoodInstance;
    private int frameCount = 0;
    private int snakePower = 0;
    private float powerDurationTimer, powerTimer, normalTimer, poisonTimer, pauseTimer = 0.0f;
    private bool gameIsPaused, gameIsStarted, loadWinScreen = false;

    public Image canvasLayer;
    public Text startGameMessage;
    public Text pressSpace;
    public SceneLoader sceneLoader;
    public Slider snakePowerSlider;
    public static int frameRate = 12; 
    public static int Score = 0;
    public bool isFlashing = false;

    [SerializeField] Text scoreLabel;
    [SerializeField] AudioClip hit;

    void Start() {
        scoreLabel.text = "Points: 0";
    }

    void Update() {
        if (!gameIsStarted) {
            StartGame();
        }
        
        if (!gameIsPaused && gameIsStarted) {
            frameCount++;
            snake.CaptureInputs();

            //Limiting Frames to Set Motion Speed
            if (frameCount >= frameRate) {
                snake.Move();
                frameCount = 0;
            }

            if (snake.IsHealthyFoodCollision) {
                Score += healthyFoodInstance.GetValue();
                scoreLabel.text = "Points: " + Score.ToString();
                EnableFloatValueLabel(healthyFoodInstance.GetValue());
                healthyFoodInstance.SelfDestroy();
                normalTimer = 0;
                snake.IsHealthyFoodCollision = false;
            }

            if (snake.IsPoisonFoodCollision) {
                Score += poisonFoodInstance.GetValue();
                scoreLabel.text = "Points: " + Score.ToString();
                EnableFloatValueLabel(poisonFoodInstance.GetValue());
                poisonFoodInstance.SelfDestroy();
                poisonTimer = 0;
                snake.IsPoisonFoodCollision = false;
            }

            if (snake.IsPowerFoodCollision) {
                snakePower += PowerFoodInstance.GetValue();
                snake.Power = snakePower;
                snakePowerSlider.value += snakePower;
                powerTimer = 0;
                PowerFoodInstance.SelfDestroy();
                snake.IsPowerFoodCollision = false;
            }

            if (snake.IsWallCollision) {
                AudioSource.PlayClipAtPoint(hit, snake.transform.position);
                PauseGame();
            }

            if (snake.IsSnakePartCollision) {
                AudioSource.PlayClipAtPoint(hit, snake.transform.position);
                PauseGame();
            }

            if (snake.IsPowered) {
                canvasLayer.color = snake.PowerColor;
                powerDurationTimer += Time.deltaTime;
                int powerDurationInSeconds = Convert.ToInt32(powerDurationTimer % 60);
                if (powerDurationInSeconds >= 1) {
                    snakePower -= 10;
                    snake.Power = snakePower;
                    snakePower = snake.Power;
                    powerDurationTimer = 0;
                }
            }
            else {
                canvasLayer.color = Color.Lerp(canvasLayer.color, Color.clear, 5 * Time.deltaTime);
            }

            if(snake.Power == 100) {
                pressSpace.text = "Press Space!";
            }
            else {
                pressSpace.text = "";
            }

            snakePowerSlider.value = snake.Power;

            powerTimer += Time.deltaTime;
            poisonTimer += Time.deltaTime;
            normalTimer += Time.deltaTime;

            ItemRespawn(healthyFoodInstance, 0.5f, "Healthy", ref normalTimer);
            ItemRespawn(poisonFoodInstance, 8, "Poison", ref poisonTimer);
            ItemRespawn(PowerFoodInstance, 10, "Power", ref powerTimer);
        }
        //Wait 3 seconds after the defeat condition to load the new screen
        if (loadWinScreen) {
            pauseTimer += Time.deltaTime;
            int timeInSeconds = Convert.ToInt32(pauseTimer % 60);
            if (timeInSeconds == 3) {
                Debug.Log(timeInSeconds);
                pauseTimer = 0;
                sceneLoader.LoadScene(3);
            }
        }
    }

    private void ItemRespawn(Food item, float respawnTime, string name, ref float timer) {
        int timeInSeconds = Convert.ToInt32(timer % 60);
        if (item == null) {
            if (timeInSeconds >= respawnTime) {
                if (name == "Healthy") { item = Instantiate(Resources.Load("Prefabs/HealthyFood", typeof(HealthyFood)) as HealthyFood); this.healthyFoodInstance = (HealthyFood)item; }
                else if (name == "Poison") { item = Instantiate(Resources.Load("Prefabs/PoisonFood", typeof(PoisonFood)) as PoisonFood); this.poisonFoodInstance = (PoisonFood)item; }
                else if (name == "Power") { item = Instantiate(Resources.Load("Prefabs/PowerFood", typeof(PowerFood)) as PowerFood); this.PowerFoodInstance = (PowerFood)item; }
                item.spawn(snake);
                item.SetValues();
                timer = 0;
            }
        }
        else if (timeInSeconds >= item.lifeTime) {
            item.SelfDestroy();
            timer = 0;
        }
        // Blinking the item when it is close to its lifetime
        else if (item != null) {
            int restOfLifeTime = item.lifeTime - timeInSeconds;
            if (restOfLifeTime  == 3) {
                item.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.8f);
            }
            if (restOfLifeTime == 2) {
                item.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
            }
            if (restOfLifeTime == 1) {
                item.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
            }
        }
    }

    private void EnableFloatValueLabel(int value) {
        GameObject tempTextBox = Instantiate(Resources.Load("Prefabs/FloatPoints", typeof(GameObject))) as GameObject;
        if(value < 0) {
            tempTextBox.GetComponentInChildren<Text>().text = ""+value;
        }
        else {
            tempTextBox.GetComponentInChildren<Text>().text = "+" + value;
        }
        tempTextBox.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform, false);

        //Dividing by the configured aspect ratio(12:9) to get the values in WorldUnity
        float width = Screen.width / 24;
        float height = Screen.height / 18;
        Vector3 snakePosInPixels = new Vector3(snake.transform.position.x * width, snake.transform.position.y * height, 0);
        tempTextBox.transform.position = snakePosInPixels;

        Animator anim = tempTextBox.GetComponent<Animator>();
        float time = 0;
        RuntimeAnimatorController ac = anim.runtimeAnimatorController;    
        for (int i = 0; i < ac.animationClips.Length; i++) {                 
            if (ac.animationClips[i].name == "FloatingLabel") {       
                time = ac.animationClips[i].length;
            }
        }
        Destroy(tempTextBox.gameObject, time);
    }

    //Sets the first direction when the game starts
    public void StartGame() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            SetInitialDirection(Snake.Directions.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            SetInitialDirection(Snake.Directions.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            SetInitialDirection(Snake.Directions.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            SetInitialDirection(Snake.Directions.down);
        }
    }

    private void SetInitialDirection(Snake.Directions dir) {
        LoadSnake();
        snake.NewDirection = dir;
        gameIsStarted = true;
        Destroy(startGameMessage.GetComponentInChildren<Image>());
        Destroy(startGameMessage);
       
    }

    public void LoadSnake() {
        if (SelectionManager.snakeName == "Ghost") {
            snake = Instantiate(Resources.Load("Prefabs/GhostSnake", typeof(GhostSnake))) as GhostSnake;
        }
        else if (SelectionManager.snakeName == "Stone") {
            snake = Instantiate(Resources.Load("Prefabs/StoneSnake", typeof(StoneSnake))) as StoneSnake;
        }
        else if (SelectionManager.snakeName == "Army") {
            snake = Instantiate(Resources.Load("Prefabs/CamouflagedSnake", typeof(CamouflagedSnake))) as CamouflagedSnake;
        }
    }

    private void PauseGame() {
        loadWinScreen = true;
        gameIsPaused = true;
    }
}
