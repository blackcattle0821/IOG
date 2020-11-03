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



    [SerializeField] LayerMask m_layerMask = 0;
    //public Transform missilePos = null;
    //public GameObject missile;


    public Camera PlayCam;

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
            SearchEnemy();
        }
    }

    void Interation()
    {
        
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
    
}
