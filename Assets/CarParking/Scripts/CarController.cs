using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [Header("Settings")]
    public bool isMovement;
    [SerializeField] private float carSpeed;
    [SerializeField] private Transform parent; //arabay� platforma yerle�tirmek i�in


    [Header("Elements")]
    [SerializeField] private GameObject[] rutTrail; 
    GameObject platform;

    private void Start()
    {
        platform = GameManager.instance.GetPlatform(); //levelde kulland���m�z plaftormu al�yoruz
    }
    // Update is called once per frame
    void Update()
    {
        if (isMovement)
            CarMovement();

        if (GameManager.instance.GetIsRises()==true) //platform y�kselmesi varsa
        {
            if (GameManager.instance.GetElevationValue() > platform.transform.position.y) //platformun y de�eri istenilen de�erden k���kse
            {
                platform.transform.position = Vector3.Lerp(platform.transform.position, new Vector3(
                    platform.transform.position.x,
                    platform.transform.position.y + 1.6f,
                    platform.transform.position.z), .010f); 
            }
            else
            {
                GameManager.instance.SetIsRises(false); //y�kselme tamamlan�nca
            }
        }


    }
    public void CarMovement() //araba hareketi
    {
        transform.position+= transform.forward*carSpeed*Time.deltaTime;
    }
    public void CloseAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }

    public bool GetMovement(bool state) //araba hareket edip etmeyece�ini de�i�tiriyoruz
    {
        return isMovement=state;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ParkTrigger")) //istenilen park yerine ula��nca
        {
            GetMovement(false);
            transform.SetParent(parent);
            for (int i = 0; i < rutTrail.Length; i++)
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                rutTrail[i].gameObject.SetActive(false);


                if (GameManager.instance.isThereRise)
                {
                    GameManager.instance.SetElevationValue(platform.transform.position.y + 1.6f);
                    GameManager.instance.SetIsRises(true);
                }

                
                GameManager.instance.GetNewCar();
            }
        }

        else if (collision.gameObject.CompareTag("PlatformMid")) //park yerine park edemezse
        {
            GameManager.instance.GetNewCar();
            Destroy(gameObject);
            GameManager.instance.SetGameState(GameState.Gameover);
        }
        else if (collision.gameObject.CompareTag("Gem")) //almas� al�nca
        {
            collision.gameObject.SetActive(false);
            GemManager.instance.SetGem(1);
            UIManager.instance.gemText.text = GemManager.instance.GetGem().ToString();
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        else if (collision.gameObject.CompareTag("Car")) //engellere veya park edilmi� arabaya �arparsa
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GameManager.instance.GetNewCar();
            GameManager.instance.SetGameState(GameState.Gameover);
            Destroy(gameObject);
        }

    }
}
