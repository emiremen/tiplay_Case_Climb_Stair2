using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private GameData gameData;
    [SerializeField] private TextMeshPro scoreBoardText;
    [SerializeField] private GameObject bestScoreBoard;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private float levelLength = 500;
    [SerializeField] private float score = 500;
    private float money;

    [SerializeField] private GameObject tapToStartPanel;
    [SerializeField] private GameObject touchPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject incrementalsPanel;
    private bool isTouchedPanel = false;

    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private Image progressbar;

    [SerializeField] private TextMeshProUGUI staminaCostTxt;
    [SerializeField] private TextMeshProUGUI staminaLevelTxt;
    [SerializeField] private TextMeshProUGUI incomeCostTxt;
    [SerializeField] private TextMeshProUGUI incomeLevelTxt;
    [SerializeField] private TextMeshProUGUI speedCostTxt;
    [SerializeField] private TextMeshProUGUI speedLevelTxt;

    [SerializeField] private ParticleSystem moneyParticle;

    private void OnEnable()
    {
        EventManager.saveMoneyData += SaveMoneyData;
        EventManager.saveBestScore += SaveBestScore;
        EventManager.decreaseScore += DecreaseScore;
        EventManager.gainMoney += GainMoney;
        EventManager.setScoreBoardPosition += SetScoreBoardPosition;
        EventManager.showGameOverPanel += ShowGameOverPanel;
        EventManager.getIsTouchedScreen += GetIsTouchedPanel;
    }

    private void OnDisable()
    {
        EventManager.saveMoneyData -= SaveMoneyData;
        EventManager.saveBestScore -= SaveBestScore;
        EventManager.decreaseScore -= DecreaseScore;
        EventManager.gainMoney -= GainMoney;
        EventManager.setScoreBoardPosition -= SetScoreBoardPosition;
        EventManager.showGameOverPanel -= ShowGameOverPanel;
        EventManager.getIsTouchedScreen -= GetIsTouchedPanel;
    }

    private void Start()
    {
        gameData = EventManager.getGameData?.Invoke();
        levelTxt.text = $"Level {gameData.level}";
        score = levelLength;
        scoreBoardText.text = score.ToString("0.0");
        incrementalsPanel.SetActive(true);
        money = gameData.money;
        moneyText.text = money.ToString();
        tapToStartPanel.SetActive(true);
        touchPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        SetIncrementalsValues();
        SetBestScoreBoardPosition();
    }


    void Update()
    {
        IncrementProgressBar();
    }

    private void IncrementProgressBar()
    {
        progressbar.fillAmount = (levelLength - score) * (1 / levelLength);
    }

    private void GainMoney()
    {
        money += 0.5f;
        moneyText.text = money.ToString();
    }

    private void SaveMoneyData()
    {
        gameData.money = money;
    }

    private void SaveBestScore()
    {
        if (score < gameData.bestScore)
        {
            gameData.bestScore = score;
            gameData.bestScoreYPos = EventManager.getLastSpawnedStairPos.Invoke().position.y;
        }
    }

    private void DecreaseScore()
    {
        if (score > .2f)
        {
            score -= .2f;
            scoreBoardText.text = score.ToString("0.0");
        }
        else
        {
            ShowWinPanel();
        }
    }

    private void SetScoreBoardPosition()
    {
        scoreBoardText.transform.parent.transform.position = EventManager.getLastSpawnedStairPos.Invoke().position + new Vector3(0, .225f, 0);
    }

    private void SetBestScoreBoardPosition()
    {
        bestScoreBoard.transform.position += new Vector3(0, gameData.bestScoreYPos, 0);
        bestScoreBoard.GetComponentInChildren<TextMeshPro>().text = gameData.bestScore.ToString("0.0") + "m";

    }

    private void ShowGameOverPanel()
    {
        SaveMoneyData();
        SaveBestScore();
        gameOverPanel.SetActive(true);
        touchPanel.SetActive(false);
        EventManager.setGameState?.Invoke(GameState.end);
        EventManager.getPlayerAnimator?.Invoke().SetBool("isIdle", true);
        EventManager.getPlayerAnimator?.Invoke().SetBool("isRunning", false);
    }

    private void ShowWinPanel()
    {
        SaveMoneyData();
        SaveBestScore();
        winPanel.SetActive(true);
        touchPanel.SetActive(false);
        gameData.level++;
        gameData.bacgroundColor = Random.ColorHSV(0, 1, 0, 1, .7f, 1);
        gameData.bestScore = 0;
        EventManager.setGameState?.Invoke(GameState.end);
        EventManager.getPlayerAnimator?.Invoke().SetBool("isIdle", true);
        EventManager.getPlayerAnimator?.Invoke().SetBool("isRunning", false);
    }

    public void TapToStartPanel()
    {
        EventManager.setGameState?.Invoke(GameState.start);
        tapToStartPanel.SetActive(false);
        touchPanel.SetActive(true);
        incrementalsPanel.SetActive(false);
        Time.timeScale = (gameData.maxLevel + gameData.speedLevel) * (1 / gameData.maxLevel);
    }

    public void TouchPanelDown()
    {
        isTouchedPanel = true;
        EventManager.setIsTouchedScreen?.Invoke(isTouchedPanel);
    }

    public void TouchPanelUp()
    {
        isTouchedPanel = false;
        EventManager.setIsTouchedScreen?.Invoke(isTouchedPanel);
    }

    private bool GetIsTouchedPanel()
    {
        return isTouchedPanel;
    }

    public void RestartLevel()
    {
        gameOverPanel.SetActive(false);
        touchPanel.SetActive(false);
        Rigidbody[] rigidbodies = EventManager.getStairSpawner?.Invoke().GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
            rb.AddForce(new Vector3(Random.Range(0, 1), Random.Range(0, 2), Random.Range(0, 1)) * 2.2f, ForceMode.Impulse);
        }
        Invoke(nameof(RestartLevelDelayed), 2);
    }

    public void RestartLevelDelayed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ContinueLevel()
    {
        gameOverPanel.SetActive(false);
        touchPanel?.SetActive(true);
        EventManager.setGameState?.Invoke(GameState.start);
        EventManager.setPlayerVisibility?.Invoke(true);
    }

    public void IncreaseStamina()
    {
        if (money >= gameData.staminaCost && gameData.staminaLevel < gameData.maxLevel)
        {
            moneyParticle.Play();
            staminaLevelTxt.transform.parent.DOComplete();
            staminaLevelTxt.transform.parent.DOShakeScale(.5f, .15f);
            gameData.staminaLevel++;
            gameData.money -= gameData.staminaCost;
            gameData.staminaCost += 20;
            SetIncrementalsValues();
        }
    }

    public void IncreaseIncome()
    {
        if (money >= gameData.incomeCost && gameData.incomeLevel < gameData.maxLevel)
        {
            moneyParticle.Play();
            incomeLevelTxt.transform.parent.DOComplete();
            incomeLevelTxt.transform.parent.DOShakeScale(.5f, .15f);
            gameData.incomeLevel++;
            gameData.money -= gameData.incomeCost;
            gameData.incomeCost += 20;
            SetIncrementalsValues();
        }
    }

    public void IncreaseSpeed()
    {
        if (money >= gameData.speedCost && gameData.speedLevel < gameData.maxLevel)
        {
            moneyParticle.Play();
            speedLevelTxt.transform.parent.DOComplete();
            speedLevelTxt.transform.parent.DOShakeScale(.5f, .15f);
            gameData.speedLevel++;
            gameData.money -= gameData.speedCost;
            gameData.speedCost += 20;
            SetIncrementalsValues();
        }
    }

    private void SetIncrementalsValues()
    {
        staminaCostTxt.text = gameData.staminaCost.ToString();
        staminaLevelTxt.text = $"Level {gameData.staminaLevel}";
        incomeCostTxt.text = gameData.incomeCost.ToString();
        incomeLevelTxt.text = $"Level {gameData.incomeLevel}";
        speedCostTxt.text = gameData.speedCost.ToString();
        speedLevelTxt.text = $"Level {gameData.speedLevel}";

        money = gameData.money;
        moneyText.text = money.ToString();
    }
}
