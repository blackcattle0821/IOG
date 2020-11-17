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
    [SerializeField] public Image opt;
    [SerializeField] public Image upg;
    //[SerializeField] public Text mineralText;// 자원UI......

    private GameObject user;
  

    // Start is called before the first frame update
    void Start()
    {
        createPlayer();
        HP.value = 100;
        opt.gameObject.SetActive(false);        //옵션창, 업그레이드창 안보이게함
        upg.gameObject.SetActive(false);
        
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
        //mineralText.text = string.Format("{0}",mineral); // UI연결......
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

}
