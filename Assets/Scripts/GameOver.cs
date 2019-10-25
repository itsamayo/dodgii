using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverScreen;

    public Text scoreUI;
    public Text gameOverText;

    public string[] gameOverHeadings;

    public bool gameOver;

    // Start is called before the first frame update
    void Start()
    {        
        FindObjectOfType<PlayerController> ().OnPlayerDeath += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver){
            if(Input.GetKeyDown (KeyCode.Space)){
                SceneManager.LoadScene(0);
            }
        }
    }
    
    void OnGameOver() {
        long longScore = System.Convert.ToInt64(FindObjectOfType<ScoreManager>().score);
        FindObjectOfType<GooglePlayServices>().AddScoreToLeaderBoard(GPGSIds.leaderboard_high_scores, longScore);
        FindObjectOfType<SoundManager>().PlayDeathSound();
        gameOverScreen.SetActive(true);
        FindObjectOfType<ScoreManager>().scoreUI.SetActive(false);
        FindObjectOfType<Difficulty>().gameHasStartedTime = 0;
        gameOverText.text = gameOverHeadings[Random.Range(0, gameOverHeadings.Length)];
        scoreUI.text = PlayerPrefs.GetFloat("score", 0).ToString();
        gameOver = true;
    }
    
}
