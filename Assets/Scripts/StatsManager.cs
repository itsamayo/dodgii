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
        coinsCollectedUI.text = "coins collected: " + PlayerPrefs.GetFloat("coinsCollected").ToString();
        biggerCoinsCollectedUI.text = "gems collected: " + PlayerPrefs.GetFloat("biggerCoinsCollected").ToString();
        godModesCollectedUI.text = "buffs collected: " + PlayerPrefs.GetFloat("godModesCollected").ToString();
        ObstaclesDestroyedUI.text = "obstacles destroyed: " + PlayerPrefs.GetFloat("totalObstaclesDestroyed").ToString();
    }

    void BackToMainMenu()
    {
        FindObjectOfType<GameManage>().mainMenuScreen.SetActive(true);
        statsMenu.SetActive(false);
    }
}
