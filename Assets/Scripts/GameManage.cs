using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManage : MonoBehaviour
{
        
    public GameObject mainMenuScreen;
    public GameObject quitConfirm;
    public GameObject notSignedIn;
    public GameObject signInFailed;

    public Text highScoreUI;

    public Button startButton;
    public Button retryButton;
    public Button leaderBoard;
    public Button achievements;
    public Button confirmQuit;
    public Button statsButton;
    public Button quitYes;
    public Button quitNo;
    public Button signin;
    public Button cancelSignin;
    public Button retrySignin;
    public Button cancelSigninRetry;

    float highScore;
    public bool hasStarted;
    public bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        Button start = startButton.GetComponent<Button>();
        start.onClick.AddListener(StartGame);
        Button retry = retryButton.GetComponent<Button>();
        retry.onClick.AddListener(Retry);

        Button stats = statsButton.GetComponent<Button>();
        stats.onClick.AddListener(GoToStats);

        Button leaderBoardButton = leaderBoard.GetComponent<Button>();
        leaderBoardButton.onClick.AddListener(ShowLeaderBoardUI);
        Button achievementsButton = achievements.GetComponent<Button>();
        achievementsButton.onClick.AddListener(ShowAchievementsUI);
        Button signinButton = signin.GetComponent<Button>();
        signinButton.onClick.AddListener(Signin);
        Button cancelSigninButton = cancelSignin.GetComponent<Button>();
        cancelSigninButton.onClick.AddListener(CancelSignin);
        Button retrySigninButton = retrySignin.GetComponent<Button>();
        retrySigninButton.onClick.AddListener(Signin);
        Button cancelSigninRetryButton = cancelSigninRetry.GetComponent<Button>();
        cancelSigninRetryButton.onClick.AddListener(CancelSignin);


        Button quitConfirm = confirmQuit.GetComponent<Button>();
        quitConfirm.onClick.AddListener(ConfirmQuit);
        Button yesQuit = quitYes.GetComponent<Button>();
        yesQuit.onClick.AddListener(QuitGame);
        Button noQuit = quitNo.GetComponent<Button>();
        noQuit.onClick.AddListener(CancelQuit);

        highScore = PlayerPrefs.GetFloat("highScore", highScore);
        highScoreUI.text = highScore.ToString();        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mainMenuScreen.SetActive(false);
                hasStarted = true;
            }
        }
    }

    void StartGame() 
    {
        if (!hasStarted)
        {
            mainMenuScreen.SetActive(false);
            hasStarted = true;
            FindObjectOfType<ScoreManager>().coinsCollected = 0;
            FindObjectOfType<ScoreManager>().biggerCoinsCollected = 0;
            FindObjectOfType<ScoreManager>().godModesCollected = 0;
        }
    }

    void Retry()
    {
        SceneManager.LoadScene(0);
    }

    void GoToStats()
    {
        FindObjectOfType<StatsManager>().statsMenu.SetActive(true);
        mainMenuScreen.SetActive(false);
    }

    public void ShowLeaderBoardUI()
    {
        FindObjectOfType<GooglePlayServices>().ShowLeaderBoard();
    }

    public void ShowAchievementsUI()
    {
        FindObjectOfType<GooglePlayServices>().ShowAchievementUI();
    }

    public void Signin()
    {
        FindObjectOfType<GooglePlayServices>().SignIn();
        notSignedIn.SetActive(false);
        signInFailed.SetActive(false);

    }

    public void CancelSignin()
    {
        notSignedIn.SetActive(false);
        signInFailed.SetActive(false);
    }

    void ConfirmQuit()
    {
        quitConfirm.SetActive(true);
    }

    void QuitGame() 
    {
        Application.Quit();
    }    

    void CancelQuit() 
    {
        quitConfirm.SetActive(false);
    }
}
