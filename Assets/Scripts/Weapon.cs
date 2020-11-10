using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon : MonoBehaviourPunCallbacks
{
    public float Damage;
    public float range;
    public float value;

    int num = 8;


    [SerializeField] LayerMask m_layerMask = 0;
    //public Transform missilePos = null;
    //public GameObject missile;


    public Camera PlayCam;
    public Camera MissilePosition;
    public GameObject MissilePrefab = null;

    // private float nextTimeToFire = 0f;
    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetButtonDown("Fire1") && value == 0)
        {
            photonView.RPC("BasicShoot", RpcTarget.AllBuffered);
        }
        if (Input.GetButtonDown("Fire1") && value == 1)
        {
            photonView.RPC("SearchEnemy", RpcTarget.AllBuffered);
        }
        if (Input.GetButtonDown("Fire1") && value == 2)
        {
            for (int i = 0; i < num; i++)
            {
                photonView.RPC("shotgun", RpcTarget.AllBuffered);
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            photonView.RPC("mShoot", RpcTarget.AllBuffered);
        }
    }

    //기본 공격
    [PunRPC]
    void BasicShoot()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(PlayCam.transform.position, PlayCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.collider.gameObject.layer);
            Target target = hit.collider.GetComponent<Target>();
            Debug.Log(target.health);
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
        Collider[] m_cols = Physics.OverlapSphere(transform.position, 100f, m_layerMask);

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
        Vector3 spread = PlayCam.transform.position + PlayCam.transform.forward * 1000f;
        //spread로 퍼지는 범위 조절
        spread += PlayCam.transform.up * Random.Range(-3000f, 3000f);
        spread += PlayCam.transform.right * Random.Range(-3000f, 3000f);
        direction += spread.normalized * Random.Range(0f, 0.2f);

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(PlayCam.transform.position, direction, out hit, range))
        {
            //Debug.DrawLine(PlayCam.transform.position, hit.point, Color.green);
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

    [PunRPC]
    void mShoot()
    {
        GameObject MissileObject = PhotonNetwork.Instantiate(MissilePrefab.name, MissilePosition.transform.position, Quaternion.identity);
        MissileObject.transform.forward = PlayCam.transform.forward;
        //GameObject MissileObject = Instantiate(MissilePrefab);

    }
}

