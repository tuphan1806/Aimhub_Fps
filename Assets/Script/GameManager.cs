using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI countDownText;
    public TextMeshProUGUI highScoreText;

    public GameObject startScreen;
    public GameObject pauseScreen;
    public GameObject gameOverScreen;
    public GameObject countDownScreen;
    public GameObject target;
    
    private int score;
    private float currentDifficulty;
 
    public bool isGameActive;
    public float spawnRate = 1.0f;
    public float remainingTime;
    public int countDownTime;

    private void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
                highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScoreLv1", 0).ToString();
                break;
            case "Level2":
                highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScoreLv1", 0).ToString();
                break;
        }     
    }

    // Update is called once per frame
    void Update()
    {      
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive)
        {
            PauseGame();
        }

        //Time Countdown
        if (isGameActive )
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
            }
            else if (remainingTime < 0)
            {
                remainingTime = 0;
                GameOver();
            }
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("Time: {0:00}:{01:00}", minutes, seconds);
        }
        
    }

    IEnumerator SpawnTargets(float difficulty)
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            //int index = Random.Range(0, targets.Count);
            GameObject targetClone = Instantiate(target/*[index]*/, target.transform.position, target.transform.rotation);
            Destroy(targetClone , 4f/difficulty);
        }

    }

    public void UpdateScore(int scoreAdd)
    {
        score += scoreAdd;
        scoreText.text = "Score: " + score;

        string level1 = "HighScoreLv1";
        string level2 = "HighScoreLv2";

        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
                SetHighScore(level1);
                break;
            case "Level2":
                SetHighScore(level2);  
                break;
        }
    }

    public void SetHighScore(string highScore)
    {
        if (score > PlayerPrefs.GetInt(highScore, 0))
        {
            PlayerPrefs.SetInt(highScore, score);
            highScoreText.text = "High Score: " + score.ToString();
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverScreen.SetActive(true);
        finalScoreText.text = "Final Score: " + score;
        timerText.text = "00:00";
        UnlockCursor();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(float difficulty)
    {
        isGameActive = true;
        StartCoroutine(SpawnTargets(difficulty));
        score = 0;
        UpdateScore(0);
        LockCursor();
        spawnRate /= difficulty;
    } 

    public void PauseGame()
    {
        pauseScreen.gameObject.SetActive(true);
        isGameActive = false;
        UnlockCursor();
    }

    public void BackToGame()
    {
        pauseScreen.gameObject.SetActive(false);     
        isGameActive = true;
        LockCursor();
        StartCoroutine(SpawnTargets(currentDifficulty));
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LockCursor()
    {
        if (isGameActive) Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void CountDownToBeginGame(float difficulty)
    {      
        currentDifficulty = difficulty;
        startScreen.gameObject.SetActive(false);
        countDownScreen.gameObject.SetActive(true);
        StartCoroutine(CountDownToStart(difficulty));
    }

    IEnumerator CountDownToStart(float difficult)
    {
        while(countDownTime > 0)
        {
            countDownText.text = countDownTime.ToString();

            yield return new WaitForSeconds(1f);

            countDownTime--;
        }

        countDownText.text = "GO!";

        yield return new WaitForSeconds(1f);

        countDownScreen.gameObject.SetActive(false);

        StartGame(difficult);
    }
}
