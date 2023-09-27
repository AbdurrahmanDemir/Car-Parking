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
    [SerializeField] private float[] rotateSpeed; //platformlar�n h�zlar�n� kontrol ediyoruz
    public static Action onGetNewCar; //yeni bir araba �a��r�rken �al��an action
    public bool isThereRise; //mevcut levelde y�kselen yap�ya sahip platfrom 

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
       
        for (int i = 0; i < levelGemCount; i++) //levelde istenen kadar elmas� olu�turuyoruz
        {
            Instantiate(gem, gemSpawnPoint[i].position, gemSpawnPoint[i].rotation, gemSpawnPoint[i]);
        }

        cars[currentCarIndex].SetActive(true); 
    }
    private void Update()
    {
        //e�er UI objesine t�klanmam��sa arabam�z� s�r�yoruz
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
    public void SetGameState(GameState gameState) //gameState de�i�tirmek i�in bu metodu kullan�yoruz
    {
        this.gameState = gameState;
        onGameStateChanged?.Invoke(gameState);
    }

    public void GetNewCar() //Yeni arabay� �a��r�yoruz
    {
        if (currentCarIndex < levelCarCount) //levelde istenen araba say�s�na ula�mam��sak yeni araba aktif ediyoruz
        {
            cars[currentCarIndex].SetActive(true);
        }
        else
        {
            //levelde istenilen araba say�s�na ula��lm��, oyunu bitiriyoruz
            SetGameState(GameState.LevelComplete);
        }
        onGetNewCar?.Invoke();
            
    }
    public int GetLevelCarCount() //Di�er scriptlerde levelde olacak araba sat�s�n� almak i�in
    {
        return levelCarCount;
    }
    public int GetCurrentCarCount() //Di�er scriptlerde aktif araba indexini almak i�in
    {
        return currentCarIndex;
    }
    public GameObject GetPlatform() //car scriptinde platformu �a��rmak i�in
    {
        return platform1;
    }
    public float GetElevationValue() // car sciptinde platformun y�kselme de�erini al�yoruz
    {
        return elevationValue;
    }
    public float SetElevationValue(float value) // car sciptinde platformun y�kselme de�erini de�i�tirmek i�in 
    {
        return elevationValue+= value;
    }
    public bool GetIsRises() // car sciptinde platformun y�kselip y�kselmeyece�ini al�yoruz
    {
        return isRises;
    }
    public bool SetIsRises(bool value) // car sciptinde platformun y�kselip y�kselmeyece�ini de�i�tiriyoruz
    {
        return isRises=value;
    }
    public void PlatformRotate() //platformu de�i�tiriyoruz
    {
        platform1.transform.Rotate(new Vector3(0, 0, -rotateSpeed[0]), Space.Self);
        if(platform2!=null) //e�er 2.platform varsa bu kodu �al��t�r�yoruz.
            platform2.transform.Rotate(new Vector3(0, 0, rotateSpeed[1]), Space.Self);
    }


}
