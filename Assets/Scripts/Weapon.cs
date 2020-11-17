using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon : MonoBehaviourPunCallbacks
{
    //무기 공격력
    public float Damage;
    //무기 범위
    public float range;
    //무기 구분하는 값
    public float value;
    //무기 탄약
    public float ammo;
    //미사일 탄약
    public float mAmmo;
    //샷건 퍼져나가는 총알 수
    int num = 10;

    //호밍 공격레이어. 추후 수정
    [SerializeField] LayerMask m_layerMask = 0;
    //public Transform missilePos = null;
    //public GameObject missile;

    //플레이어 시점, 미사일 발사 위치, 미사일 프리팹 오브젝트.
    public Camera PlayCam;
    public Camera MissilePosition;
    public GameObject MissilePrefab;

    // private float nextTimeToFire = 0f;
    void Update()
    {
        if (!photonView.IsMine) return;

        //마우스 좌, 기본 무기일때
        if (Input.GetButtonDown("Fire1") && value == 0) 
        {
            photonView.RPC("BasicShoot", RpcTarget.AllBuffered);
        }
        //마우스 좌, 호밍
        if (Input.GetButtonDown("Fire1") && value == 1 && ammo > 0)
        {
            photonView.RPC("SearchEnemy", RpcTarget.AllBuffered);
            ammo -= 1;
        }
        //마우스 좌, 샷건
        if (Input.GetButtonDown("Fire1") && value == 2 && ammo > 0)
        {
            for (int i = 0; i < num; i++)
            {
                photonView.RPC("shotgun", RpcTarget.AllBuffered);
                ammo -= 1;
            }
        }
        //마우스 우, 미사일
        if (Input.GetButtonDown("Fire2") && value == 3 && mAmmo > 0)
        {
            photonView.RPC("mShoot", RpcTarget.AllBuffered);
            mAmmo -= 1;
        }
    }

    //기본 공격
    [PunRPC]
    void BasicShoot()
    {
        RaycastHit hit = new RaycastHit();
        //디버그용 레이
        Debug.DrawRay(PlayCam.transform.position, PlayCam.transform.forward*range, Color.red, 0.5f);
        if (Physics.Raycast(PlayCam.transform.position, PlayCam.transform.forward, out hit, range))
        {
           // Debug.Log(hit.collider.gameObject.layer);
            Target target = hit.collider.GetComponent<Target>();
           // Debug.Log(target.health);
            if (target != null)
            {
                Debug.Log(hit.collider.name);
                target.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, Damage);
            }
        }
    }

    //범위 기본 공격
    [PunRPC]
    void SearchEnemy()
    {
        Collider[] m_cols = Physics.OverlapSphere(transform.position, range, m_layerMask);

        for (int i = 0; i < m_cols.Length; i++)
        {
            Target target = m_cols[i].transform.GetComponentInParent<Target>();
            if (target != null)
            {
                target.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, Damage);
            }
        }
    }


    [PunRPC]
    void shotgun()
    {
        Vector3 direction = PlayCam.transform.forward;
        Vector3 spread = PlayCam.transform.position + PlayCam.transform.forward;
        //spread로 퍼지는 범위 조절
        spread += PlayCam.transform.up * Random.Range(-1000f, 1000f);
        spread += PlayCam.transform.right * Random.Range(-1000f, 1000f);
        direction += spread.normalized * Random.Range(0f, 0.2f);

        RaycastHit hit = new RaycastHit();
        Debug.DrawLine(PlayCam.transform.position, direction*100, Color.green, 0.3f);
        if (Physics.Raycast(PlayCam.transform.position, direction*100, out hit, range))
        {
           
            //Debug.Log(hit.collider.gameObject.layer);
            Target target = hit.collider.GetComponent<Target>();
            //Debug.Log(target.health);
            if (target != null)
            {
                //Debug.Log(hit.collider.name);
                target.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, Damage);
            }
        }
        //else
        //{
        //    Debug.DrawRay(PlayCam.transform.position, direction, Color.red);
        //}
    }

    //미사일 발사 함수. 문제 많음. 수정 필요
    [PunRPC]
    void mShoot()
    {
        //Quaternion q = Quaternion.LookRotation(new Vector3(90, 90, 90));
        Quaternion q = Quaternion.identity;
        q.eulerAngles = new Vector3(90, 90, 0);
        GameObject MissileObject = PhotonNetwork.Instantiate(MissilePrefab.name, MissilePosition.transform.position, transform.rotation);
        MissileObject.transform.forward = PlayCam.transform.forward;

        //GameObject MissileObject = Instantiate(MissilePrefab);

    }
}

