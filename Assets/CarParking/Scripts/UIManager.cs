using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Elements")]
    [SerializeField] private GameObject[] levelCarCountImage;
    [SerializeField] private Sprite carCompetedSprite;
    [SerializeField] public TextMeshProUGUI gemText;
    [Header(" Panels ")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject gameoverPanel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        GameManager.onGetNewCar += CarCompletedImage; 

        GameManager.onGameStateChanged += GameStateChangedCallback;

    }
    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;

        GameManager.onGetNewCar -= CarCompletedImage;
    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("gem"))        
            GemManager.instance.SetGem(0);
        else
            gemText.text = GemManager.instance.GetGem().ToString();

        for (int i = 0; i < GameManager.instance.GetLevelCarCount(); i++) //levelde istenen kadar araba görselini aktif ediyoruz
        {
            levelCarCountImage[i].SetActive(true);
        }
    }


    private void GameStateChangedCallback(GameState gameState) //gameState i burdan deðiþtiriyoruz ve actiona baðlýyoruz
    {
        switch (gameState)
        {
            case GameState.Menu:
                menuPanel.SetActive(true);
                gamePanel.SetActive(false);
                levelCompletePanel.SetActive(false);
                gameoverPanel.SetActive(false);
                break;

            case GameState.Game:
                menuPanel.SetActive(false);
                gamePanel.SetActive(true);
                break;

            case GameState.LevelComplete:
                gamePanel.SetActive(false);
                levelCompletePanel.SetActive(true);

                break;

            case GameState.Gameover:
                gamePanel.SetActive(false);
                gameoverPanel.SetActive(true);
                break;
        }
    }


    public void CarCompletedImage() //tamamlanan aracýn görselini tamamlýyoruz
    {
        levelCarCountImage[GameManager.instance.GetCurrentCarCount()-1].GetComponent<Image>().sprite= carCompetedSprite;
    }
    public void PlayButtonCallback()
    {
        GameManager.instance.SetGameState(GameState.Game);
    }

    public void RetryButtonCallback()
    {
        LevelManager.instance.GetLevel();
    }

    public void NextButtonCallback()
    {
        LevelManager.instance.NextLevel();
    }
}
