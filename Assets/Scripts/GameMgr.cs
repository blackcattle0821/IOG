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
    public GameObject myGun;
    public Image gun1;
    public Image gun2;          //상단 총 아이콘 이미지
    public Text HighscoreText;
    

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
        gun1.gameObject.SetActive(false);
        gun2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))       //O로 옵션창 띄우고 마우스 락 해제
        {
            opt.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.U))       //U로 업그레이드창 띄우고 마우스 락 해제
        {
            upg.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        UpdateScore();
        Highscore();
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
        Time.timeScale = 1;
    }

    public void upg_Continue()                  //업그레이드 나가기 버튼
    {
        upg.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    public void Restart()                       //옵션 다시하기 버튼
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LoginScene");
        PhotonNetwork.LeaveRoom();
    }
 
    public void ToOver()                        //옵션 종료화면 버튼
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameoverScene");
        PhotonNetwork.LeaveRoom();
    }

    void UpdateScore()                          //점수 획득하면 UI에 표시하기.
    {
        myPlayer = GameObject.FindGameObjectWithTag("me");
        MineralText.text = myPlayer.GetComponent<Player>().mineral.ToString();
        UpgradeMineralText.text = myPlayer.GetComponent<Player>().mineral.ToString();
        HP.value = myPlayer.GetComponent<Target>().health;
        HP_text.text = HP.value.ToString();

        myGun = GameObject.FindGameObjectWithTag("gun");
        if(myPlayer.GetComponent<Player>().equipWeapon == myPlayer.GetComponent<Player>().weapons[0])
        {
            BulletText.text = "∞";
        }
        else
        {
            BulletText.text = "50/" + myGun.GetComponent<Weapon>().ammo;
        }
    }

    void Highscore()
    {
        myPlayer = GameObject.FindGameObjectWithTag("me");
        ExitGames.Client.Photon.Hashtable pps = new ExitGames.Client.Photon.Hashtable();
        if (!pps.ContainsKey("score"))
        {
            pps.Add("score", myPlayer.GetComponent<Player>().score);
            PhotonNetwork.SetPlayerCustomProperties(pps);
        }
        else
        {
            pps["score"] = myPlayer.GetComponent<Player>().score;
            PhotonNetwork.SetPlayerCustomProperties(pps);
        }

        HighscoreText.text = "점수 랭킹\n\n";

        for(int i=0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            int j = i + 1;
            HighscoreText.text += j + ". " + PhotonNetwork.PlayerList[i].NickName + " : " + PhotonNetwork.PlayerList[i].CustomProperties["score"] + "\n";
        }
    }

}
