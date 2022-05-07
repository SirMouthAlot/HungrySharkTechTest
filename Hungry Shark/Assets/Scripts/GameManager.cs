using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject _gameHUD = null;
    public Text _scoreValue = null;
    public Image _targetFishImage = null;

    public List<string> _fishNames = new List<string>();
    public List<Sprite> _fishSprites = new List<Sprite>();

    private int _targetFish = -1;

    public static GameManager _instance = null;

    private void Start()
    {
        //Store the only existing version of game manager as instance
        _instance = FindObjectOfType<GameManager>();

        //Keep this object and score canvas around even after a load
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(_gameHUD);
    }

    public void StartGame()
    {
        //Load into game
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneIndex.MAINGAME);

        //Enable score canvas
        _gameHUD.SetActive(true);

        ChooseNewTarget();
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

    private void ChooseNewTarget()
    {
        _targetFish = Random.Range(0, _fishNames.Count);

        _targetFishImage.sprite = _fishSprites[_targetFish];
    }

    private void LateUpdate()
    {
        _scoreValue.text = ScoreManager.GetCurrentScore().ToString();
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
