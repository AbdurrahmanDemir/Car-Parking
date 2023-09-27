using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum GameState { Menu, Game, LevelComplete, Gameover }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header(" Settings ")]
    private GameState gameState;

    [Header("Elements")]
    [SerializeField] private GameObject platform1;
    [SerializeField] private GameObject platform2;
    [SerializeField] private GameObject[] cars;
    [SerializeField] private int currentCarIndex=0;
    [SerializeField] private GameObject gem;
    [SerializeField] private Transform[] gemSpawnPoint;
    private int randomValue;

    [Header("Level Settings")]
    [SerializeField] private int levelCarCount;
    [SerializeField] private int levelGemCount;
    float elevationValue;
    bool isRises;


    [Header(" Actions ")]
    public static Action<GameState> onGameStateChanged; 


    [Header("Settings")]
    [SerializeField] private float[] rotateSpeed; //platformlarýn hýzlarýný kontrol ediyoruz
    public static Action onGetNewCar; //yeni bir araba çaðýrýrken çalýþan action
    public bool isThereRise; //mevcut levelde yükselen yapýya sahip platfrom 

    private void Awake()
    {
        if(instance == null)
            instance=this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        SetGameState(GameState.Menu);
       
        for (int i = 0; i < levelGemCount; i++) //levelde istenen kadar elmasý oluþturuyoruz
        {
            Instantiate(gem, gemSpawnPoint[i].position, gemSpawnPoint[i].rotation, gemSpawnPoint[i]);
        }

        cars[currentCarIndex].SetActive(true); 
    }
    private void Update()
    {
        //eðer UI objesine týklanmamýþsa arabamýzý sürüyoruz
        if (Input.touchCount==1 || Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            cars[currentCarIndex].GetComponent<CarController>().GetMovement(true);
            currentCarIndex++;
        }
       
    }
    private void FixedUpdate()
    {
        PlatformRotate(); 
    }
    public void SetGameState(GameState gameState) //gameState deðiþtirmek için bu metodu kullanýyoruz
    {
        this.gameState = gameState;
        onGameStateChanged?.Invoke(gameState);
    }

    public void GetNewCar() //Yeni arabayý çaðýrýyoruz
    {
        if (currentCarIndex < levelCarCount) //levelde istenen araba sayýsýna ulaþmamýþsak yeni araba aktif ediyoruz
        {
            cars[currentCarIndex].SetActive(true);
        }
        else
        {
            //levelde istenilen araba sayýsýna ulaþýlmýþ, oyunu bitiriyoruz
            SetGameState(GameState.LevelComplete);
        }
        onGetNewCar?.Invoke();
            
    }
    public int GetLevelCarCount() //Diðer scriptlerde levelde olacak araba satýsýný almak için
    {
        return levelCarCount;
    }
    public int GetCurrentCarCount() //Diðer scriptlerde aktif araba indexini almak için
    {
        return currentCarIndex;
    }
    public GameObject GetPlatform() //car scriptinde platformu çaðýrmak için
    {
        return platform1;
    }
    public float GetElevationValue() // car sciptinde platformun yükselme deðerini alýyoruz
    {
        return elevationValue;
    }
    public float SetElevationValue(float value) // car sciptinde platformun yükselme deðerini deðiþtirmek için 
    {
        return elevationValue+= value;
    }
    public bool GetIsRises() // car sciptinde platformun yükselip yükselmeyeceðini alýyoruz
    {
        return isRises;
    }
    public bool SetIsRises(bool value) // car sciptinde platformun yükselip yükselmeyeceðini deðiþtiriyoruz
    {
        return isRises=value;
    }
    public void PlatformRotate() //platformu deðiþtiriyoruz
    {
        platform1.transform.Rotate(new Vector3(0, 0, -rotateSpeed[0]), Space.Self);
        if(platform2!=null) //eðer 2.platform varsa bu kodu çalýþtýrýyoruz.
            platform2.transform.Rotate(new Vector3(0, 0, rotateSpeed[1]), Space.Self);
    }


}
