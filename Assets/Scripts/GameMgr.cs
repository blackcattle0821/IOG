using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEditor;

public class GameMgr : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject playerprefab = null;
    [SerializeField] public Slider HP;
    [SerializeField] public Text HP_text;
    [SerializeField] public Image opt;
    [SerializeField] public Image upg;
    [SerializeField] public Image scr;
    public Text MineralText;
    public Text UpgradeMineralText;
    public Text BulletText;
    public GameObject myPlayer = null;
    //[SerializeField] public Text mineralText;// 자원UI......

    private GameObject user;
  

    // Start is called before the first frame update
    void Start()
    {
        createPlayer();
        HP.value = 100;
        HP_text.text = HP.value.ToString();
        opt.gameObject.SetActive(false);        //옵션창, 업그레이드창 안보이게함
        upg.gameObject.SetActive(false);
        scr.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))       //O로 옵션창 띄우고 마우스 락 해제
        {
            opt.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.U))       //U로 업그레이드창 띄우고 마우스 락 해제
        {
            upg.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        UpdateScore();
    }

    void createPlayer()
    {
        float pos = Random.Range(-20.0f, 20.0f);
        user = PhotonNetwork.Instantiate(playerprefab.name, new Vector3(pos, 1.0f, pos), Quaternion.identity) as GameObject;
    }
        
    public void opt_Continue()                 //옵션 계속하기 버튼
    {        
        opt.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;   
    }

    public void upg_Continue()                  //업그레이드 나가기 버튼
    {
        upg.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Restart()                       //옵션 다시하기 버튼
    {
        SceneManager.LoadScene("LoginScene");
        PhotonNetwork.LeaveRoom();
    }
 
    public void ToOver()                        //옵션 종료화면 버튼
    {
        SceneManager.LoadScene("GameoverScene");
        PhotonNetwork.LeaveRoom();
    }

    void UpdateScore()                          //점수 획득하면 UI에 표시하기.
    {
        myPlayer = GameObject.FindGameObjectWithTag("me");
        MineralText.text = myPlayer.GetComponent<Player>().mineral.ToString();
        UpgradeMineralText.text = myPlayer.GetComponent<Player>().mineral.ToString();
        //BulletText.text = myPlayer.GetComponent<Weapon>().ammo.ToString();   하는중..
    }

}
