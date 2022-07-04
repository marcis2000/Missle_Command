using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private Spaceship spaceship;
    [SerializeField] private GameObject endOfRoundScreen;
    [SerializeField] private GameObject endOfGameScreen;
    [SerializeField] private TMPro.TextMeshProUGUI pointsUI;
    [SerializeField] private TMPro.TextMeshProUGUI levelUI;
    [SerializeField] EnemyMissileLauncher enemyLauncher;
    [SerializeField] private FriendlyMissileLauncherManager friendlyLaunchersManager;
    [SerializeField] private List<Building> buildings = new List<Building>();
    private TMPro.TextMeshProUGUI missilesPointsUI;
    private TMPro.TextMeshProUGUI buildingsPointsUI;
    private TMPro.TextMeshProUGUI secondsToNextRoundUI;

    private int spaceshipsPerRound = 0;
    private int currentPoints = 0;
    private int level = 0;
    private int bulidingsDestroyed = 0;
    private int launchedEnemyMissiles = 0;
    private int destroyedEnemyMissiles = 0;
    private bool pausedToCalculatePoints = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        GetAllReferences();
        StartNextLevel();
    }

    void Update()
    {
        if (!pausedToCalculatePoints && destroyedEnemyMissiles == launchedEnemyMissiles && enemyLauncher.numberOfMissileWaves == 0)
        {
            FinishLevel();
        }
        if (bulidingsDestroyed == 6)
        {
            FinishGame();
        }

    }
    private void FinishLevel()
    {
        pausedToCalculatePoints = true;
        destroyedEnemyMissiles = 0;
        launchedEnemyMissiles = 0;
        endOfRoundScreen.SetActive(true);
        spaceship.gameObject.SetActive(false);
        AddPointsFromBuildingsLeft();
        AddPointsFromMissilesLeft();
        StartCoroutine(WaitForNextLevelToStart());
    }
    private void StartNextLevel()
    {
        pausedToCalculatePoints = false;
        endOfRoundScreen.SetActive(false);
        level++;
        levelUI.text = $"Level {level}";
        spaceshipsPerRound = level - 1;
        spaceship.gameObject.SetActive(true);
        friendlyLaunchersManager.ActivateMissileLaunchers();
        enemyLauncher.SetNewParameters();
    }

    private void FinishGame()
    {
        Time.timeScale = 0;
        endOfGameScreen.SetActive(true);
    }
    
    public void RestartGame()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void AddPointsFromBuildingsLeft()
    {
        int pointsFromBuildings = (6 - bulidingsDestroyed) * 100;
        buildingsPointsUI.text = $"Points from buildings left: {pointsFromBuildings}";
        AddPoints(pointsFromBuildings);
    }

    private void AddPointsFromMissilesLeft()
    {
        int missilesLeft = 0;
        foreach (FriendlyMissileLauncher missileLauncher in friendlyLaunchersManager.friendlyLaunchers)
        {
            missilesLeft += missileLauncher.missilesLeft;
        }
        int pointsFromMissilesleft = missilesLeft * 5;
        missilesPointsUI.text = $"Points form missiles left: {pointsFromMissilesleft}";
        AddPoints(pointsFromMissilesleft);
    }
    private void AddPoints(int points)
    {
        currentPoints += points;
        pointsUI.text = $"{currentPoints}";
    }
    private void AddMissilePoints(object sender, EventArgs e)
    {
        AddPoints(25);
    }
    private void AddSpaceshipPoints(object sender, EventArgs e)
    {
        AddPoints(200);
        spaceshipsPerRound--;
        if (spaceshipsPerRound > 0)
        {
            spaceship.gameObject.SetActive(true);
        }
    }
    private void BuildingDestroyed(object sender, EventArgs e)
    {
        bulidingsDestroyed++;
    }
    private void AddToDeLaunchedEnemyMissilesCount(object sender, EventArgs e)
    {
        launchedEnemyMissiles++;
    }
    private void AddToDestroyedEnemyMissilesCount(object sender, EventArgs e)
    {
        destroyedEnemyMissiles++;
    }

    private void GetAllReferences()
    {
        enemyLauncher.OnAddToDeLaunchedEnemyMissilesCount += AddToDeLaunchedEnemyMissilesCount;
        foreach (GameObject missile in ObjectPool.instance.pooledMissiles)
        {
            missile.GetComponent<Missile>().OnAddMissilePoints += AddMissilePoints;
            missile.GetComponent<Missile>().OnAddToDestroyedEnemyMissilesCount += AddToDestroyedEnemyMissilesCount;
        }
        foreach (Building building in buildings)
        {
            building.OnBuildingDestroyed += BuildingDestroyed;
        }
        missilesPointsUI = endOfRoundScreen.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        buildingsPointsUI = endOfRoundScreen.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
        secondsToNextRoundUI = endOfRoundScreen.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
        spaceship.OnAddSpaceshipPoints += AddSpaceshipPoints;
    }

    private IEnumerator WaitForNextLevelToStart()
    {
        int i = 4;
        while(i > 0)
        {
            i--;
            secondsToNextRoundUI.text = $"Next round starts in: {i} ";
            yield return new WaitForSeconds(1);
        }
        StartNextLevel();
    }


}