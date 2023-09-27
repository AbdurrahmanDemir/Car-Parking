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
    public void SetGem(int gemCount) //elmasý arttýrma iþlemi
    {
        gem += gemCount;
        SaveGem();
    }
    public void SaveGem() //elmasý kaydediyoruz
    {
        PlayerPrefs.SetInt("gem", gem);
    }
    public void LoadGem() //sahip olduðumuz elmas deðerini alýyoruz
    {
        gem= PlayerPrefs.GetInt("gem", gem);
    }
    public int GetGem() //sahip olduðumuz elmas deðerini çaðýrýyoruz
    {
        return gem;
    }

}
