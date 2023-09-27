using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private int currentLevel;
    private int maxLevel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        maxLevel = 11;
        DontDestroyOnLoad(this.gameObject); //levelleri kontrol etmek için destroy etmiyoruz
        if (!PlayerPrefs.HasKey("level"))
            SceneManager.LoadScene(1);

        GetLevel();


    }
    public void GetLevel()
    {
        currentLevel = PlayerPrefs.GetInt("level",1);
        LoadLevel();
    }

    private void LoadLevel() //kaldýðýmýz leveli veya tekrardan oyna tuþunda çalýþtýyoruz
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void NextLevel() //yeni level yüklüyoruz
    {
        currentLevel++;
        if (currentLevel > maxLevel)
        {
            currentLevel = 1;
        }
        PlayerPrefs.SetInt("level", currentLevel);
        LoadLevel();
    }
    


}
