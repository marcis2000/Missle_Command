using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private Spaceship _spaceship;
    [SerializeField] private GameObject _endOfRoundScreen;
    [SerializeField] private GameObject _endOfGameScreen;
    [SerializeField] private TMPro.TextMeshProUGUI _pointsUI;
    [SerializeField] private TMPro.TextMeshProUGUI _levelUI;
    [SerializeField] private EnemyMissileLauncher _enemyLauncher;
    [SerializeField] private FriendlyMissileLauncherManager _friendlyLaunchersManager;
    [SerializeField] private List<Building> _buildings = new List<Building>();

    private TMPro.TextMeshProUGUI _missilesPointsUI;
    private TMPro.TextMeshProUGUI _buildingsPointsUI;
    private TMPro.TextMeshProUGUI _secondsToNextRoundUI;

    private int _spaceshipsPerRound = 0;
    private int _currentPoints = 0;
    private int _level = 0;
    private int _bulidingsDestroyed = 0;
    private int _launchedEnemyMissiles = 0;
    private int _destroyedEnemyMissiles = 0;
    private bool _pausedToCalculatePoints = false;

    void Start()
    {
        Time.timeScale = 1;
        GetAllReferences();
        StartNextLevel();
    }

    void Update()
    {
        if (!_pausedToCalculatePoints && _destroyedEnemyMissiles == _launchedEnemyMissiles && _enemyLauncher.NumberOfMissileWaves == 0)
        {
            FinishLevel();
        }
        if (_bulidingsDestroyed == 6)
        {
            FinishGame();
        }

    }
    //
    private void FinishLevel()
    {
        _pausedToCalculatePoints = true;
        _destroyedEnemyMissiles = 0;
        _launchedEnemyMissiles = 0;
        _spaceship.gameObject.SetActive(false);
        _endOfRoundScreen.SetActive(true);
        AddPointsFromBuildingsLeft();
        AddPointsFromMissilesLeft();
        StartCoroutine(WaitForNextLevelToStart());
    }
    private void StartNextLevel()
    {
        _pausedToCalculatePoints = false;
        _endOfRoundScreen.SetActive(false);
        _level++;
        _levelUI.text = $"Level {_level}";
        _spaceshipsPerRound = _level;
        _spaceship.gameObject.SetActive(true);
        _friendlyLaunchersManager.ActivateMissileLaunchers();
        _enemyLauncher.SetNewParameters();
    }

    private void FinishGame()
    {
        Time.timeScale = 0;
        _endOfGameScreen.SetActive(true);
    }
    
    // Is used by PLAY AGAIN button.
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);  
    }

    private void AddPointsFromBuildingsLeft()
    {
        int buildingsLeft = 6 - _bulidingsDestroyed;    // At the beggining there are 6 buildings on the scene.
        int pointsFromBuildings = (buildingsLeft) * 100;   //100 points for each building left.
        _buildingsPointsUI.text = $"Points from buildings left: {pointsFromBuildings}";
        AddPoints(pointsFromBuildings);
    }

    private void AddPointsFromMissilesLeft()
    {
        int missilesLeft = 0;
        foreach (FriendlyMissileLauncher missileLauncher in _friendlyLaunchersManager.FriendlyLaunchers)
        {
            missilesLeft += missileLauncher.missilesLeft;
        }
        int pointsFromMissilesleft = missilesLeft * 5;   // 5 points for every missile left.
        _missilesPointsUI.text = $"Points form missiles left: {pointsFromMissilesleft}";
        AddPoints(pointsFromMissilesleft);
    }
    private void AddPoints(int points)
    {
        _currentPoints += points;
        _pointsUI.text = $"{_currentPoints}";
    }
    private void AddMissilePoints(object sender, EventArgs e)
    {
        AddPoints(25);
    }
    private void AddSpaceshipPoints(object sender, EventArgs e)
    {
        AddPoints(200);
        _spaceshipsPerRound--;
        if (_spaceshipsPerRound > 0)
        {
            _spaceship.gameObject.SetActive(true); 
        }
    }

    private void BuildingDestroyed(object sender, EventArgs e)
    {
        _bulidingsDestroyed++;
    }
    private void AddToLaunchedEnemyMissilesCount(object sender, EventArgs e)
    {
        _launchedEnemyMissiles++;
    }
    private void AddToDestroyedEnemyMissilesCount(object sender, EventArgs e)
    {
        _destroyedEnemyMissiles++;
    }

    private void GetAllReferences()
    {
        _enemyLauncher.OnAddToLaunchedEnemyMissilesCount += AddToLaunchedEnemyMissilesCount;
        foreach (GameObject missile in ObjectPool.Instance.PooledMissiles)
        {
            missile.GetComponent<Missile>().OnAddMissilePoints += AddMissilePoints;
            missile.GetComponent<Missile>().OnAddToDestroyedEnemyMissilesCount += AddToDestroyedEnemyMissilesCount;
        }
        foreach (Building building in _buildings)
        {
            building.OnBuildingDestroyed += BuildingDestroyed;
        }
        _missilesPointsUI = _endOfRoundScreen.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        _buildingsPointsUI = _endOfRoundScreen.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
        _secondsToNextRoundUI = _endOfRoundScreen.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
        _spaceship.OnAddSpaceshipPoints += AddSpaceshipPoints;
    }

    private IEnumerator WaitForNextLevelToStart()
    {
        int i = 4;
        while(i > 0)
        {
            i--;
            _secondsToNextRoundUI.text = $"Next round starts in: {i} ";
            yield return new WaitForSeconds(1);
        }
        StartNextLevel();
    }


}