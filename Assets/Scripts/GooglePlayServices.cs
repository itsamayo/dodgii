using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayServices : MonoBehaviour
{

    public static GooglePlayServices instance;

    public bool signedIn;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {        
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = false;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
        SignIn();
    }

    public void SignIn() 
    {
        // authenticate user:
        Social.localUser.Authenticate((bool success) => {
            // handle success or failure
            if (success)
            {
                signedIn = true;
                Debug.Log("Login succeeded");
            }
            else 
            {
                signedIn = false;
               // FindObjectOfType<GameManage>().signInFailed.SetActive(true);
                Debug.Log("Login failed");
            }
        });
    }

    #region Achievements

    public void UnlockAchievement(string id) 
    {
        // unlock achievement (achievement ID "Cfjewijawiu_QA")
        Social.ReportProgress(id, 100.0f, (bool success) => {
            // handle success or failure
            if (success)
            {
                Debug.Log("Achievement unlocking succeeded");
            }
            else
            {
                Debug.Log("Achievement unlocking failed");
            }
        });
    }

    public void ShowAchievementUI() 
    {
        // show achievements UI
        if (signedIn)
        {
            Social.ShowAchievementsUI();
            Debug.Log("Showing Achievements UI");
        }
        else 
        {
            FindObjectOfType<GameManage>().notSignedIn.SetActive(true);    
        }        
    }

    #endregion

    #region Leaderboard

    public void AddScoreToLeaderBoard(string leaderBoardId, long score)
    {
        Social.ReportScore(score, leaderBoardId, (bool success) => {
            // handle success or failure
            if (success)
            {
                Debug.Log("Score adding to leaderboard succeeded");
            }
            else
            {
                Debug.Log("Score adding to leaderboard failed");
            }
        });
    }

    public void ShowLeaderBoard()
    {
        // show leaderboard UI
        if (signedIn)
        {
            Social.ShowLeaderboardUI();
            Debug.Log("Showing Leaderboard UI");
        }
        else
        {
            FindObjectOfType<GameManage>().notSignedIn.SetActive(true);
        }        
    }

    #endregion

}
