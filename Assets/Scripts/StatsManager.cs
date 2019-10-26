using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{

    public GameObject statsMenu;

    public Text highScoreUI;
    public Text coinsCollectedUI;
    public Text biggerCoinsCollectedUI;
    public Text godModesCollectedUI;
    public Text ObstaclesDestroyedUI;

    public Button backToMainMenu;

    // Start is called before the first frame update
    void Start()
    {
        Button mainMenu = backToMainMenu.GetComponent<Button>();
        mainMenu.onClick.AddListener(BackToMainMenu);

        highScoreUI.text = PlayerPrefs.GetFloat("highScore").ToString();
        coinsCollectedUI.text = "yellow coins - " + PlayerPrefs.GetFloat("totalCoinsCollected").ToString();
        biggerCoinsCollectedUI.text = "green gems - " + PlayerPrefs.GetFloat("totalBiggerCoinsCollected").ToString();
        godModesCollectedUI.text = "blue buffs - " + PlayerPrefs.GetFloat("totalGodModesCollected").ToString();
        ObstaclesDestroyedUI.text = "obstacles - " + PlayerPrefs.GetFloat("totalObstaclesDestroyed").ToString();
    }

    void BackToMainMenu()
    {
        FindObjectOfType<GameManage>().mainMenuScreen.SetActive(true);
        statsMenu.SetActive(false);
    }
}
