using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    //이동 속도
    public float moveSpeed = 5.0f;
    //카메라 회전 속도
    public float rotSpeed = 100.0f;
    public GameObject icon;
    public float health;
    int weaponIndex = -1;
    public float mineral;
    public float score;
    //무기를 가졌는지 저장
    public bool[] hasWeapons;
    //무기 저장
    [SerializeField] public GameObject[] weapons = null;
    //장비한 무기 저장
    [SerializeField] public GameObject equipWeapon;
    public GameObject enemy;
    //메인씬 게임매니저 찾기위한 변수
    public GameObject gm;

    public Collider col;

    bool sDown1;
    bool sDown2;
    bool sDown3;

    public Camera PlayCam;
    public GameObject hitScreen;

    public Rigidbody rigid;
    public Transform tr;
    //private PhotonView pv = null;

    float xRotation = 0f;

    private Vector3 currPos;
    private Quaternion currRot;
    //입력 횟수를 늘리던가 해야 됨. 수정 필요함.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
        //    stream.SendNext(sDown1);
        //    stream.SendNext(sDown2);
        //    stream.SendNext(sDown3);
        //    stream.SendNext(weaponIndex);
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);

        }
        else
        {
            //sDown1 = (bool)stream.ReceiveNext();
            //sDown2 = (bool)stream.ReceiveNext();
            //sDown3 = (bool)stream.ReceiveNext();
            //weaponIndex = (int)stream.ReceiveNext();

            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

    //가장 먼저 무기를 초기화. 수정 필요함
    void Awake()
    {
        hasWeapons[0] = true;
        hasWeapons[1] = false;
        hasWeapons[2] = false;
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
        if (photonView.IsMine)
        {
            health = 100;
            this.gameObject.tag = "me";
            this.gameObject.layer = 12;
            for (int i = 0; i < this.transform.childCount - 4; i++)
            {
                this.transform.GetChild(i).gameObject.tag = "me";
                this.transform.GetChild(i).gameObject.layer = 12;
            }
            //for (int i = 0; i < this.transform.GetChild(3).childCount; i++)
            //{
            //    this.transform.GetChild(3).GetChild(i).gameObject.tag = "me";
            //}
            //게임매니저에 걸어둔 태그
            gm = GameObject.FindGameObjectWithTag("GameController");
        }
        //내가 아닌 플레이어. 카메라 끊고 태그, 레이어등 변경
        if (!photonView.IsMine)
        {
            enemy = GameObject.FindGameObjectWithTag("Player");
            enemy.GetComponent<Player>().PlayCam.enabled = false;
            enemy.transform.GetChild(6).GetComponent<Camera>().enabled = false;
            //enemy.transform.GetChild(2).GetChild(0).gameObject.layer = 10;
            GetComponentInChildren<AudioListener>().enabled = false;
            this.gameObject.tag = "Player";
            this.gameObject.layer = 10;
            for (int i = 0; i < this.transform.childCount - 3; i++)
            {
                this.transform.GetChild(i).gameObject.tag = "Player";
                this.transform.GetChild(i).gameObject.layer = 10;
            }
        }

        // 방에 들어올 때마다 랜덤하게 아이콘 색이 변함
        icon.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1.0f), Random.Range(0f, 1.0f), Random.Range(0f, 1.0f));

    }


    // Update is called once per frame
    //void FixedUpdate()
    //{

    //}

    private void Update()
    {
        //내 캐릭터면 움직임.
        if (photonView.IsMine)
        {
            Move();
            RotMove();

            GetInput();
            //hitScreen.SetActive(false);
        }
        photonView.RPC("Swap", RpcTarget.AllBuffered);
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
        //1, 2, 3 누르면 인덱스 값 바뀜.
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;



        if (sDown1 || sDown2 || sDown3)
        {
            //가진 무기의 인덱스에 true가 있으면 무기 구매한 것임. 교체해도 됨.
            if (hasWeapons[weaponIndex] == true)
            {
                //교체 전에 원래 쓰던 것 비활성화하고 교체
                if (equipWeapon != null)
                {
                    equipWeapon.SetActive(false);
                }
                equipWeapon = weapons[weaponIndex];
                equipWeapon.SetActive(true);
            }
        }
    }

    void Move()
    {
        if (photonView.IsMine)
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
        else
        {
            if ((tr.position - currPos).sqrMagnitude >= 10.0f * 10.0f)
            {
                tr.position = currPos;
                tr.rotation = currRot;
            }
            else
            {
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
                tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            }
        }
    }

    //시점 이동
    void RotMove()
    {
        if (photonView.IsMine)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);

            PlayCam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            this.transform.Rotate(Vector3.up * mouseX);
        }
        else
        {
            if ((tr.position - currPos).sqrMagnitude >= 10.0f * 10.0f)
            {
                tr.position = currPos;
                tr.rotation = currRot;
            }
            else
            {
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
                tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            }
        }
    }

    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Asteroid"))
        {
           if (photonView.IsMine)
            {
                this.gameObject.GetComponent<Target>().health -= 10;
                StartCoroutine(ShowHitScreen());
            }
        }
        if (other.gameObject.CompareTag("Player"))
        {
            rigid.isKinematic = true;
        }
    }

    IEnumerator ShowHitScreen()
    {
        gm.GetComponent<GameMgr>().scr.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        gm.GetComponent<GameMgr>().scr.gameObject.SetActive(false);
    }

}