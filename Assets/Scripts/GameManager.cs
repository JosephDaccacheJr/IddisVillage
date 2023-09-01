using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Gameplay Stats")]
    public float gameplayTimer;

    [Header("Player Stats")]
    public int playerMaxHealth;
    public int playerHealth;
    public int playerDamage;
    public int gold;
    public int villageRating;
    public int villageRatingGoal;
    [HideInInspector]
    public bool gameDone;

    [Header("References")]
    public GameObject player;
    [HideInInspector]
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> plots = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> builtBuildings = new List<GameObject>();

    [Header("UI Elements")]
    public Image healthBar;
    public Image ratingBar;
    public Text textGoldAmount;
    public GameObject panelBuilding;
    public List<GameObject> spawnerPointers = new List<GameObject>();
    public GameObject panelGameOver, panelVictory;

    [Header("Map Icons")]
    public RectTransform mapIconPlayer;
    public GameObject mapIconEnemy;
    public GameObject mapIconBuilding;
    public GameObject mapIconsHolder;
    public List<RectTransform> mapIconEnemies = new List<RectTransform>();
    public List<RectTransform> mapIconBuildings = new List<RectTransform>();

    [Header("Spawn Controllers")]
    public List<GameObject> enemyLevels = new List<GameObject>();
    public GameObject spawnerPrefab;
    public List<GameObject> spawnerList = new List<GameObject>(); // This will hold all the spawners on the map
    public List<GameObject> activeSpawners = new List<GameObject>();
    public float spawnerTimerSet;
    public float spawnerTimer;

    [Header("Audio Sources")]
    public AudioSource musicNoPer;
    public AudioSource musicFull;
    public bool _playFullMusic;

    public Building_Plot currentPlot { get; private set; } // The current plot the player is interacting with

    private void Awake()
    {
        if (GameManager.instance == null)
        {
            instance = this;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        foreach (GameObject plot in GameObject.FindGameObjectsWithTag("Plot"))
        {
            plots.Add(plot);
        }
    }
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        BuildControls();
        spawnController();
        drawMap();
    }

    private void FixedUpdate()
    {
        controlMusic();
        musicController();
        gameplayTimer += Time.deltaTime;
    }

    public void setCurrentPlot(Building_Plot plot)
    {
        currentPlot = plot;
    }

    public void buildOnCurrentPlot(int buildingNum)
    {
        if (currentPlot == null || currentPlot.isBuilt) return;
        switch (buildingNum)
        {
            case 1:
                if(gold >= 100)
                {
                    currentPlot.selectHouse();
                    panelBuilding.SetActive(false);
                    changeGold(-100);
                }

                break;
            case 2:
                if(gold >= 200)
                {
                    currentPlot.selectBlacksmith();
                    panelBuilding.SetActive(false);
                    changeGold(-200);
                }
                break;
            case 3:
                if (gold >= 200)
                {
                    currentPlot.selectApothecary();
                    panelBuilding.SetActive(false);
                    changeGold(-200);
                }
                break;
        }
    }

    void BuildControls()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            buildOnCurrentPlot(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            buildOnCurrentPlot(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            buildOnCurrentPlot(3);
        }
    }

    public void setMaxHealth(int set)
    {
        playerMaxHealth = set;
    }

    public void fullHeal()
    {
        playerHealth = playerMaxHealth;
        updateHealthBar();
    }

    public void setHealth(int set)
    {
        playerHealth = set;
        updateHealthBar();
    }

    public void changeHealth(int change)
    {
        playerHealth += change;
        updateHealthBar();
        if (playerHealth <= 0)
        {
            gameOver();
        }
    }

    public void updateHealthBar()
    {
        healthBar.fillAmount = (float)playerHealth / (float)playerMaxHealth;
    }

    public void changeGold(int amt)
    {
        gold += amt;
        textGoldAmount.text = "$" + gold.ToString();
    }

    public void changeVillageRating(int amt)
    {
        villageRating += amt;
        ratingBar.fillAmount = (float)villageRating / (float)villageRatingGoal;
        if (villageRating >= villageRatingGoal)
        {
            victory();
        }
    }

    public void spawnController()
    {
        spawnerTimer -= Time.deltaTime;
        if (spawnerTimer <= 0 && !gameDone)
        {
            spawnerTimer = spawnerTimerSet;
            int spawnerIndex = (int)Random.Range(0, spawnerList.Count-1);
            Instantiate(spawnerPrefab, spawnerList[spawnerIndex].transform.position, Quaternion.identity);

        }
    }


    public void drawMap()
    {
        float playerMapX = player.transform.position.x / 50f;
        float playerMapY = player.transform.position.z / 50f;
        mapIconPlayer.anchoredPosition = new Vector2(50f * playerMapX, 50f * playerMapY);

        int enemyIndex = 0;
        foreach (GameObject currentEnemy in spawnedEnemies)
        {
            float enemyMapX = currentEnemy.transform.position.x / 50f;
            float enemyMapY = currentEnemy.transform.position.z / 50f;
            mapIconEnemies[enemyIndex].anchoredPosition = new Vector2(50f * enemyMapX, 50f * enemyMapY);
            mapIconEnemies[enemyIndex].gameObject.SetActive(true);
            enemyIndex++;
        }
        int buildingIndex = 0;
        foreach (GameObject currentBuilding in builtBuildings)
        {
            float buildingMapX = currentBuilding.transform.position.x / 50f;
            float buildingMapY = currentBuilding.transform.position.z / 50f;
            mapIconBuildings[buildingIndex].anchoredPosition = new Vector2(50f * buildingMapX, 50f * buildingMapY);
            mapIconBuildings[buildingIndex].gameObject.SetActive(true);
            buildingIndex++;
        }
    }

    public void addEnemy(GameObject newEnemy)
    {
        spawnedEnemies.Add(newEnemy);
        GameObject newIcon = Instantiate(mapIconEnemy, mapIconsHolder.transform);
        mapIconEnemies.Add(newIcon.GetComponent<RectTransform>());
    }

    public void removeEnemy(GameObject enemyToRemove)
    {
        spawnedEnemies.Remove(enemyToRemove);
        GameObject iconToRemove = mapIconEnemies[mapIconEnemies.Count - 1].gameObject;
        mapIconEnemies.RemoveAt(mapIconEnemies.Count - 1);
        Destroy(iconToRemove);
    }

    public void addBuilding(GameObject newBuilding)
    {
        builtBuildings.Add(newBuilding);
        GameObject newIcon = Instantiate(mapIconBuilding, mapIconsHolder.transform);
        mapIconBuildings.Add(newIcon.GetComponent<RectTransform>());

    }

    public void gameOver()
    {
        Time.timeScale = 0f;
        panelGameOver.SetActive(true);
    }

    public void victory()
    {

        gameDone = true;
        foreach (GameObject curEnemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            curEnemy.GetComponent<Enemy_Base>().Kill();
        }
        panelVictory.SetActive(true);
    }

    public void goBackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void controlMusic()
    {
        bool enemyIsClose = false;
        foreach (GameObject enemy in spawnedEnemies)
        {
            if(Vector3.Distance(enemy.transform.position,player.transform.position) <= 20f)
            {
                enemyIsClose = true;
            }
        }
        _playFullMusic = enemyIsClose;
    }

    public void musicController()
    {
        switch (_playFullMusic)
        {
            case true:
                musicNoPer.volume = Mathf.MoveTowards(musicNoPer.volume, 0f, Time.deltaTime);
                musicFull.volume = Mathf.MoveTowards(musicFull.volume, 0.3f, Time.deltaTime);
                break;
            case false:
                musicNoPer.volume = Mathf.MoveTowards(musicNoPer.volume, 0.3f, Time.deltaTime);
                musicFull.volume = Mathf.MoveTowards(musicFull.volume, 0f, Time.deltaTime);
                break;
        }
    }
}
