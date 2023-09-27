using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GemManager : MonoBehaviour
{
    public static GemManager instance;

    private int gem;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadGem();
    }
    public void SetGem(int gemCount) //elmas� artt�rma i�lemi
    {
        gem += gemCount;
        SaveGem();
    }
    public void SaveGem() //elmas� kaydediyoruz
    {
        PlayerPrefs.SetInt("gem", gem);
    }
    public void LoadGem() //sahip oldu�umuz elmas de�erini al�yoruz
    {
        gem= PlayerPrefs.GetInt("gem", gem);
    }
    public int GetGem() //sahip oldu�umuz elmas de�erini �a��r�yoruz
    {
        return gem;
    }

}
