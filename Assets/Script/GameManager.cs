using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    private void Awake()
    {
        manager = this;
    }

    public TextMeshProUGUI carSpeedText;
    public TextMeshProUGUI totalDistanceText;
    public TextMeshProUGUI expectedTimeText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI coinText;
    public GameObject gameOverPanel;
    public Slider damageSlider;

    public int coin;
    public bool isGameStart;
    [HideInInspector]
    public float timer;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey("totalcoins"))
        {
            coin = PlayerPrefs.GetInt("totalcoins");
            coinText.text = coin.ToString();
        }
    }

    public void OnTriggerCoin()
    {
        coin += 10;
        coinText.text = coin.ToString();
        PlayerPrefs.SetInt("totalcoins", coin);
    }

    public void OnTriggerHealthkit(float health)
    {
        StartCoroutine(SliderValueChange(health, "health"));
    }

    public void OnTriggerObstacle(float damage)
    {
        StartCoroutine(SliderValueChange(damage, "damage"));
        StartCoroutine(CarController.controller.CarSlowDownAftertriggerWithObstacle(2));
    }

    public void GameOver()
    {
        isGameStart = false;
        gameOverPanel.SetActive(true);

        if (!PlayerPrefs.HasKey("bestscore"))
            PlayerPrefs.SetInt("bestscore", 0);

        int bestScore = PlayerPrefs.GetInt("bestscore");
        int currentScore = (int)timer;

        if (currentScore > bestScore)
        {
            PlayerPrefs.SetInt("bestscore", currentScore);
        }
        gameOverPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = " Current Score :" + (currentScore / 60).ToString("00") + " min " + (currentScore - ((currentScore / 60)*60)).ToString("00") + " sec ";
        gameOverPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = " Best Score :" + (bestScore / 60).ToString("00") + " min " + (bestScore - ((bestScore / 60)*60)).ToString("00") + " sec ";
    }

    public void Restart()
    {
        timer = 0;
        gameOverPanel.SetActive(false);
        CarController.controller.carSpeed = 0;
        isGameStart = true;
    }

    public void Pause()
    {
        isGameStart = false;    
    }

    public void StartGame()
    {
        isGameStart = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator SliderValueChange(float value, string type)
    {
        float t = 0;
        float a = damageSlider.value;
        float b;

        if (type.Equals("damage"))
            b = a - (value / 100);
        else
            b = a + (value / 100);

        while (t <= 1)
        {
            t += Time.deltaTime;
            damageSlider.value = Mathf.Lerp(a, b, t);
            yield return null;
        }
        if (damageSlider.value == 0)
            GameOver();
    }
}
