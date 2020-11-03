using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5.0f;
    public float rotSpeed = 100.0f;
    public GameObject icon;

    public Camera PlayCam;

   // public Rigidbody rigid;
    public Transform tr;
    //private PhotonView pv = null;

    float xRotation = 0f;

   // public Text nameText;

    void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        //마우스 커서 없앰.
        Cursor.lockState = CursorLockMode.Locked;

        //내가 아닌 플레이어의 카메라 끊음
        if (!photonView.IsMine)
        {
            Destroy(PlayCam);
        }

        // 방에 들어올 때마다 랜덤하게 아이콘 색이 변함
        icon.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1.0f), Random.Range(0f, 1.0f), Random.Range(0f, 1.0f));

    }

    //void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        //내 캐릭터면 움직임.
        if (photonView.IsMine)
        {
            Move();
            RotMove();
        }
    }

    private void Update()
    {
    }
    //Addforce 이용한 move()
    //void Move()
    //{

    //    if (Input.GetKey("w"))
    //    {
    //        rigid.AddForce(PlayCam.transform.forward * moveSpeed * Time.deltaTime, ForceMode.Impulse);
    //    }
    //    if (Input.GetKey("a"))
    //    {
    //        rigid.AddForce(-PlayCam.transform.right * moveSpeed * Time.deltaTime, ForceMode.Impulse);
    //    }
    //    if (Input.GetKey("s"))
    //    {
    //        rigid.AddForce(-PlayCam.transform.forward * moveSpeed * Time.deltaTime, ForceMode.Impulse);
    //    }
    //    if (Input.GetKey("d"))
    //    {
    //        rigid.AddForce(PlayCam.transform.right * moveSpeed * Time.deltaTime, ForceMode.Impulse);
    //    }
    //}


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

}