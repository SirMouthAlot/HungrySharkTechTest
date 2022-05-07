using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject _scoreCanvas = null;
    public Text _scoreValue = null;

    public static GameManager _instance = null;

    private void Start()
    {
        //Store the only existing version of game manager as instance
        _instance = FindObjectOfType<GameManager>();

        //Keep this object and score canvas around even after a load
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(_scoreCanvas);
    }

    public void StartGame()
    {
        //Load into game
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneIndex.MAINGAME);

        //Enable score canvas
        _scoreCanvas.SetActive(true);
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
