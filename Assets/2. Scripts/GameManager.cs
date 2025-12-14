using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public enum GameState { Playing, Paused}
    public GameState gameState = GameState.Paused;

    public GameObject hudPanel;
    public GameObject pausePanel;
    public GameObject resumePanel;
    public GameObject deathPanel;
    [FormerlySerializedAs("timerText")] public TMP_Text resumeTimerText;

    public TMP_Text levelTimerText;
    public TMP_Text deathCountText;
    private float levelTimer;


    private void Update()
    {
        levelTimer += Time.deltaTime;
        levelTimerText.text = levelTimer.ToString("F2");
        
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    private void TogglePause()
    {
        switch (gameState)
        {
            case GameState.Paused:
                StartCoroutine(ResumeCountdown());
                break;
            case GameState.Playing:
                ChangeGameState(GameState.Paused); //open pause menu
                break;
        }
    }
    public void ChangeGameState(GameState state)
    {
        gameState = state;
        switch (state)
        {
            case GameState.Paused:
                Time.timeScale = 0;
                break;
            case GameState.Playing:
                Time.timeScale = 1;
                break;
        }
    }

    void Start()
    {
        
        levelTimer = 0;
        StartLevel();
        ChangeGameState(GameState.Paused);
    }

    void StartLevel()
    {
        deathCountText.text = StatManager.GetDeaths().ToString();
        StartCoroutine(ResumeCountdown());
    }

    public void RestartLevel() => StartCoroutine(ReloadLevel());

    private IEnumerator ReloadLevel()
    {
        ChangeGameState(GameState.Paused);
        SetActivePanel(deathPanel);
        StatManager.AddDeath();
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator ResumeCountdown()
    {
        SetActivePanel(resumePanel);
        resumeTimerText.text = "3";
        yield return new WaitForSecondsRealtime(1);
        resumeTimerText.text = "2";
        yield return new WaitForSecondsRealtime(1);
        resumeTimerText.text = "1";
        yield return new WaitForSecondsRealtime(1);
        SetActivePanel(hudPanel);
        ChangeGameState(GameState.Playing);
    }

    private void SetActivePanel(GameObject panel)
    {
        switch (panel.name)
        {
            case "Pause UI":
                pausePanel.SetActive(true);
                resumePanel.SetActive(false);
                hudPanel.SetActive(false);
                deathPanel.SetActive(false);
                break;
            case "Resume UI":
                resumePanel.SetActive(true);
                pausePanel.SetActive(false);
                hudPanel.SetActive(false);
                deathPanel.SetActive(false);
                break;
            case "In-Game UI":
                hudPanel.SetActive(true);
                resumePanel.SetActive(false);
                pausePanel.SetActive(false);
                deathPanel.SetActive(false);
                break;
            case "Death UI":
                deathPanel.SetActive(true);
                resumePanel.SetActive(false);
                pausePanel.SetActive(false);
                hudPanel.SetActive(false);
                break;
        }
    }
}
