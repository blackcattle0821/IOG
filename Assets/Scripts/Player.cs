using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public float moveSpeed = 5.0f;
    public float rotSpeed = 100.0f;
    public GameObject icon;
    int weaponIndex = -1;
    public float mineral;
    //public bool[] hasWeapons;
    [SerializeField] GameObject[] weapons = null;
    [SerializeField] GameObject equipWeapon;

    bool sDown1;
    bool sDown2;
    bool sDown3;

    public Camera PlayCam;

   // public Rigidbody rigid;
    public Transform tr;
    //private PhotonView pv = null;

    float xRotation = 0f;

    //입력 횟수를 늘리던가 해야 됨. 수정 필요함.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(sDown1);
            stream.SendNext(sDown2);
            stream.SendNext(sDown3);
            stream.SendNext(weaponIndex);

        }
        else
        {
            sDown1 = (bool)stream.ReceiveNext();
            sDown2 = (bool)stream.ReceiveNext();
            sDown3 = (bool)stream.ReceiveNext();
            weaponIndex = (int)stream.ReceiveNext();
        }
    }

    //가장 먼저 무기를 초기화. 수정 필요함
    void Awake()
    {
        weapons[2].SetActive(false);
        weapons[1].SetActive(false);
        weapons[0].SetActive(false);
        equipWeapon = weapons[0];
        equipWeapon.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        //마우스 커서 없앰.
        Cursor.lockState = CursorLockMode.Locked;

        //내가 아닌 플레이어의 카메라 끊음
        if (!photonView.IsMine)
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
            this.gameObject.layer = 8;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).gameObject.layer = 10;
            }
        }

        // 방에 들어올 때마다 랜덤하게 아이콘 색이 변함
        icon.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1.0f), Random.Range(0f, 1.0f), Random.Range(0f, 1.0f));

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //내 캐릭터면 움직임.
        if (photonView.IsMine)
        {
            Move();
            RotMove();
            photonView.RPC("Swap", RpcTarget.AllBuffered);
            GetInput();
        }
    }

    private void Update()
    {
    }

    void GetInput()
    {
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    //무기 교체 함수
    [PunRPC]
    void Swap()
    {

        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        if (sDown1 || sDown2 || sDown3)
        {
            if (equipWeapon != null)
            {
                equipWeapon.SetActive(false);
            }
            equipWeapon = weapons[weaponIndex];
            equipWeapon.SetActive(true);
        }
    }

    void Move()
    {

        if (Input.GetKey("w"))
        {
            tr.Translate(PlayCam.transform.forward * moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a"))
        {
            tr.Translate(-PlayCam.transform.right * moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s"))
        {
            tr.Translate(-PlayCam.transform.forward * moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d"))
        {
            tr.Translate(PlayCam.transform.right * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    //시점 이동
    void RotMove()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        PlayCam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        this.transform.Rotate(Vector3.up * mouseX);
    }

    // 충돌
    //void OnCollisionEnter(Collider other)
    //{
    //    if (other.gameObjext.CompareTag("Asteroid"))
    //    {
    //        if (photonView.IsMine)
    //        {

    //        }
    //    }
    //}

    // 자원획득
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            if (photonView.IsMine)
            {
                mineral++;
            }
        }
        PhotonNetwork.Destroy(other.gameObject);
    }


}