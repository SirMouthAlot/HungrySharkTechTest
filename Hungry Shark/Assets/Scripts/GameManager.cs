using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject _gameHUD = null;
    public GameObject _fishSpawner = null;
    public Text _scoreValue = null;
    public Image _targetFishImage = null;
    public Text _healthValue = null;

    public PlayerController _player = null;
    public float _spawnRadiusPlayer = 10.0f;

    public int _maxNumSpawnedFish = 20;
    private int _totalFishSpawned = 0;
    public List<GameObject> _fishObjects = new List<GameObject>();
    public List<string> _fishNames = new List<string>();
    public List<Sprite> _fishSprites = new List<Sprite>();
    public List<int> _numEachFishType;

    private int _targetFish = -1;

    public static GameManager _instance = null;

    private void Start()
    {
        //Store the only existing version of game manager as instance
        _instance = FindObjectOfType<GameManager>();

        //Keep this object, fish spawner, and HUD around even after a load
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(_fishSpawner);
        DontDestroyOnLoad(_gameHUD);
    }

    public void StartGame()
    {
        //Load into game
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneIndex.MAINGAME);

        //Enable score canvas
        _gameHUD.SetActive(true);

        PopulateSea();
        ChooseNewTarget();
    }

    public void PopulateSea()
    {
        for (int i = _totalFishSpawned; i < _maxNumSpawnedFish; i++)
        {
            SpawnFish();
        }
    }

    private void SpawnFish()
    {
        int randomNum = UnityEngine.Random.Range(0, _fishNames.Count);

        GameObject _spawnedObj = Instantiate(_fishObjects[randomNum], _fishSpawner.transform);
        _numEachFishType[randomNum]++;
        _totalFishSpawned++;

        _spawnedObj.transform.position = new Vector3((UnityEngine.Random.Range(0.0f, 1.0f) * _spawnRadiusPlayer),
                                                        (UnityEngine.Random.Range(0.0f, 1.0f) * _spawnRadiusPlayer),
                                                            0.0f);
    }

    public bool CheckConsumeTarget(string name)
    {
        //If there's a set target fish
        if (_targetFish != -1)
        {
            //Return whether target matches eaten fish
            return (name == _fishNames[_targetFish]);
        }

        return false;
    }

    public void OnConsumeTarget()
    {
        ChooseNewTarget();
    }

    public void RemoveFishOfType(string name)
    {
        //Find fish type that was just ate
        int index = Array.IndexOf(_fishNames.ToArray(), name);

        _numEachFishType[index]--;
        _totalFishSpawned--;
    }

    private void ChooseNewTarget()
    {
        _targetFish = UnityEngine.Random.Range(0, _fishNames.Count);

        //Check if the chosen fish type has any spawned
        if (_numEachFishType[_targetFish] <= 0)
        {
            //If not we just choose a new target
            ChooseNewTarget();
        }

        _targetFishImage.sprite = _fishSprites[_targetFish];
    }

    private void LateUpdate()
    {
        _scoreValue.text = ScoreManager.GetCurrentScore().ToString();

        if (_player)
        {
            _healthValue.text = _player._playerHealth.ToString() + "%";
        }
    }

    public void QuitGame()
    {
        //Quit game
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(0);
#endif
    }
}
