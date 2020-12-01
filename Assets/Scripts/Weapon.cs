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
    //무기 공격 속도. 클수록 느림
    public float FireRate;
    //공격 시간 딜레이
    private float nextFire;

    //호밍 공격레이어. 추후 수정
    [SerializeField] LayerMask m_layerMask = 10;
    //public Transform missilePos = null;
    //public GameObject missile;
    //public GameObject enemy;

    //플레이어 시점, 미사일 발사 위치, 미사일 프리팹 오브젝트.
    public Camera PlayCam;
    //미사일 발사 위치
    public Camera MissilePosition;
    //미사일 프리펩
    public GameObject MissilePrefab;

    //기본총 총구 이펙트
    public ParticleSystem BasicMuzzleFlash;
    //기본총 피격 이펙트
    public GameObject BasicHitEffect;
    //호밍 총구 이펙트
    public ParticleSystem HomingMuzzleFlash;
    //호밍 피격 이펙트
    public GameObject HomingHitEffect;
    //샷건 총구 이펙트
    public ParticleSystem ShotgunMuzzleFlash;
    //샷건 피격 이펙트
    public GameObject ShotgunHitEffect;
    //운석 파괴 이펙트
    public GameObject AsteroidEffect;

    // private float nextTimeToFire = 0f;
    void Update()
    {
        if (!photonView.IsMine) return;

        //마우스 좌, 기본 무기일때
        if (Input.GetButton("Fire1") && value == 0 && Time.time > nextFire) 
        {
            nextFire = Time.time + FireRate;
            photonView.RPC("BasicShoot", RpcTarget.AllBuffered);
        }
        //마우스 좌, 호밍
        if (Input.GetButton("Fire1") && value == 1 && ammo > 0 && Time.time > nextFire)
        {
            nextFire = Time.time + FireRate;
            photonView.RPC("SearchEnemy", RpcTarget.AllBuffered);
            ammo -= 1;
        }
        //마우스 좌, 샷건
        if (Input.GetButton("Fire1") && value == 2 && ammo > 0 && Time.time > nextFire)
        {
            nextFire = Time.time + FireRate;
            for (int i = 0; i < num; i++)
            {
                photonView.RPC("shotgun", RpcTarget.AllBuffered);
                ammo -= 1;
            }
        }
        //마우스 우, 미사일
        if (Input.GetButton("Fire2") && value == 3 && mAmmo > 0 && Time.time > nextFire)
        {
            nextFire = Time.time + FireRate;
            photonView.RPC("mShoot", RpcTarget.AllBuffered);
            mAmmo -= 1;
        }
    }

    //기본 공격
    [PunRPC]
    void BasicShoot()
    {
        //총구 이펙트 플레이
        BasicMuzzleFlash.Play();
        RaycastHit hit = new RaycastHit();
        
        //디버그용 레이
        Debug.DrawRay(PlayCam.transform.position, PlayCam.transform.forward*range, Color.red, 0.5f);
        if (Physics.Raycast(PlayCam.transform.position, PlayCam.transform.forward, out hit, range))
        {
            //총탄 피격 이펙트 생성
            GameObject hitClone = PhotonNetwork.Instantiate(BasicHitEffect.name, hit.point, Quaternion.LookRotation(hit.normal));
            Target target = hit.collider.GetComponentInParent<Target>();
            if (target != null)
            {
                Debug.Log(hit.collider.name);
                target.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, Damage);
                if (target.health <= 0f && hit.collider.gameObject.CompareTag("Asteroid"))
                {
                    GameObject me = GameObject.FindGameObjectWithTag("me");
                    me.gameObject.GetComponent<Player>().mineral += 100f;
                    me.gameObject.GetComponent<Player>().score += 100f;
                    GameObject AEffect = PhotonNetwork.Instantiate(AsteroidEffect.name, hit.transform.position, Quaternion.identity);
                }
            }
        }
    }

    //범위 기본 공격
    [PunRPC]
    void SearchEnemy()
    {
        if (photonView.IsMine) //포톤뷰 구별 해줘야 자살 안 함
        {
            //총구 이펙트 플레이
            HomingMuzzleFlash.Play();
            Collider[] m_cols = Physics.OverlapSphere(transform.position, range, m_layerMask);

            for (int i = 0; i < m_cols.Length; i++)
            {
                // Debug.Log(m_cols[i].gameObject.tag);
                Debug.Log(m_cols[i].gameObject.layer);
                //총탄 피격 이펙트 생성
                GameObject hitClone = PhotonNetwork.Instantiate(HomingHitEffect.name, m_cols[i].transform.position, Quaternion.identity);
                Debug.Log(m_cols[i].transform.name);
                Debug.Log(m_cols[i].transform.tag);
                Target target = m_cols[i].transform.GetComponent<Target>();
                if (target != null)
                {
                    target.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, Damage);
                    if (target.health <= 0f && m_cols[i].gameObject.CompareTag("Asteroid"))
                    {
                        GameObject me = GameObject.FindGameObjectWithTag("me");
                        me.gameObject.GetComponent<Player>().mineral += 100f;
                        me.gameObject.GetComponent<Player>().score += 100f;
                        GameObject AEffect = PhotonNetwork.Instantiate(AsteroidEffect.name, m_cols[i].transform.position, Quaternion.identity);
                    }
                }

            }
        }
    }


    [PunRPC]
    void shotgun()
    {
        //총구 이펙트 플레이
        ShotgunMuzzleFlash.Play();
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
            //총탄 피격 이펙트 생성
            GameObject hitClone = PhotonNetwork.Instantiate(ShotgunHitEffect.name, hit.point, Quaternion.LookRotation(hit.normal));
            Target target = hit.collider.GetComponentInParent<Target>();
            if (target != null)
            {
                target.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, Damage);
                if (target.health == 0f && hit.collider.gameObject.CompareTag("Asteroid"))
                {
                    GameObject me = GameObject.FindGameObjectWithTag("me");
                    me.gameObject.GetComponent<Player>().mineral += 100f;
                    me.gameObject.GetComponent<Player>().score += 100f;
                    GameObject AEffect = PhotonNetwork.Instantiate(AsteroidEffect.name, hit.transform.position, Quaternion.identity);
                }
            }
        }
    }

    //미사일 발사 함수. 문제 많음. 수정 필요
    [PunRPC]
    void mShoot()
    {
        if (photonView.IsMine)
        {
            GameObject MissileObject = PhotonNetwork.Instantiate(MissilePrefab.name, MissilePosition.transform.position, Quaternion.identity);
            MissileObject.transform.up = PlayCam.transform.forward; //미사일 윗부분이 앞을 향하게
            MissileObject.tag = "MyMissile";
        }

    }
}

