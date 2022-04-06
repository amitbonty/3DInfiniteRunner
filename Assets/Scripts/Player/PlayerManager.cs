using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public static bool isGameStarted;
    public static int numberOfCoins;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject startingText;
    [SerializeField]
    private Text coinsText;
    void Start()
    {
        Time.timeScale=1;
        gameOver = false;
        isGameStarted = false;
        numberOfCoins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
        coinsText.text = "Coins: " +  numberOfCoins.ToString();
        if(Input.anyKeyDown)
        {
            isGameStarted = true;
            Destroy(startingText);
        }
    }
}
