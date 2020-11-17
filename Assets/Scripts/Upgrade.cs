using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviourPunCallbacks
{
    //내 플레이어 오브젝트
    public GameObject myPlayer;
    //플레이어의 미사일 오브젝트
    public GameObject playerMissile;
    //public float money;
    //우주선 업그레이드 비용
    public float SpaceShipPrice;
    //무기 구매 비용
    public float WeaponPrice;
    //기본 무기 업그레이드 비용
    public float BasicPrice;
    //호밍 업그레이드 비용
    public float HomingPrice;
    //샷건 업그레이드 비용
    public float ShotgunPrice;

    private void Start()
    {
        SpaceShipPrice = 100f;
        WeaponPrice = 300f;
        BasicPrice = 100f;
        HomingPrice = 100f;
        ShotgunPrice = 100f;
    }

    //private void Update()
    //{
    //    playMoney();
    //}


    //public void playMoney() //머니라는 변수와 각 플레이어의 미네랄 동기화
    //{
    //    myPlayer = GameObject.FindGameObjectWithTag("me");
    //    money = myPlayer.GetComponent<Player>().mineral;
    //}

    //우주선 내구도 업그레이드
    public void HpUpgrade()
    {
        //내 포톤뷰일때
        if (photonView.IsMine)
        {
            //me 라는 태그를 지닌 게임오브젝트를 찾는다. me는 생성시 플레이어 자신에게만 부여
            myPlayer = GameObject.FindGameObjectWithTag("me");
            Debug.Log(myPlayer.GetComponent<Player>().mineral);
            //플레이어의 미네랄 >= 우주선 업그레이드 비용
            if (myPlayer.GetComponent<Player>().mineral >= myPlayer.GetComponent<Upgrade>().SpaceShipPrice)
            {
                myPlayer.GetComponent<Player>().mineral -= myPlayer.GetComponent<Upgrade>().SpaceShipPrice;
                //HP가 5%상승
                myPlayer.GetComponent<Target>().health = myPlayer.GetComponent<Target>().health * 1.05f;
                //구매비용도 상승
                myPlayer.GetComponent<Upgrade>().SpaceShipPrice += 50f;
            }

        }
    }

    //우주선 스피드 업그레이드
    public void SpeedUpgrade()
    {
        if (photonView.IsMine)
        {
            myPlayer = GameObject.FindGameObjectWithTag("me");
            Debug.Log(myPlayer.GetComponent<Player>().mineral);
            if (myPlayer.GetComponent<Player>().mineral >= myPlayer.GetComponent<Upgrade>().SpaceShipPrice)
            {
                myPlayer.GetComponent<Player>().mineral -= myPlayer.GetComponent<Upgrade>().SpaceShipPrice;
                myPlayer.GetComponent<Player>().moveSpeed = myPlayer.GetComponent<Player>().moveSpeed * 1.05f;
                myPlayer.GetComponent<Upgrade>().SpaceShipPrice += 50f;
            }

        }
    }

    //우주선 사격 범위 업그레이드
    public void RangeUpgrade()
    {
        if (photonView.IsMine)
        {
            myPlayer = GameObject.FindGameObjectWithTag("me");
            Debug.Log(myPlayer.GetComponent<Player>().mineral);
            if (myPlayer.GetComponent<Player>().mineral >= myPlayer.GetComponent<Upgrade>().SpaceShipPrice)
            {
                myPlayer.GetComponent<Player>().mineral -= myPlayer.GetComponent<Upgrade>().SpaceShipPrice;
                myPlayer.GetComponentInChildren<Weapon>().range = myPlayer.GetComponentInChildren<Weapon>().range * 1.05f;
                myPlayer.GetComponent<Upgrade>().SpaceShipPrice += 50f;
            }

        }
    }

    //호밍 무기 구매
    public void BuyHoming()
    {
        if (photonView)
        {
            myPlayer = GameObject.FindGameObjectWithTag("me");
            //플레이어 미네랄 >= 무기 구매비용 && 무기를 아직 사지 않았을 경우
            if (myPlayer.GetComponent<Player>().mineral >= myPlayer.GetComponent<Upgrade>().WeaponPrice && myPlayer.GetComponent<Player>().hasWeapons[1] ==false)
            {
                myPlayer.GetComponent<Player>().mineral -= myPlayer.GetComponent<Upgrade>().WeaponPrice;
                //무기를 산 것으로 표기
                myPlayer.GetComponent<Player>().hasWeapons[1] = true;
            }
        }
    }

    //샷건 구매
    public void BuyShotgun()
    {
        if (photonView)
        {
            myPlayer = GameObject.FindGameObjectWithTag("me");
            if (myPlayer.GetComponent<Player>().mineral >= myPlayer.GetComponent<Upgrade>().WeaponPrice && myPlayer.GetComponent<Player>().hasWeapons[2] == false)
            {
                myPlayer.GetComponent<Player>().mineral -= myPlayer.GetComponent<Upgrade>().WeaponPrice;
                myPlayer.GetComponent<Player>().hasWeapons[2] = true;
            }
        }
    }

    //기본무기 업그레이드
    public void BasicUpgrade()
    {
        if (photonView.IsMine)
        {
            myPlayer = GameObject.FindGameObjectWithTag("me");
            Debug.Log(myPlayer.GetComponent<Player>().mineral);
            //미네랄 >= 기본무기 업그레이드 비용일때 기본무기가 확실하면 진행.
            if (myPlayer.GetComponent<Player>().mineral >= myPlayer.GetComponent<Upgrade>().BasicPrice)
            {
                if (myPlayer.GetComponentInChildren<Weapon>().value == 0)
                {
                    myPlayer.GetComponent<Player>().mineral -= myPlayer.GetComponent<Upgrade>().BasicPrice;
                    //공격력 5% 상승
                    myPlayer.GetComponentInChildren<Weapon>().Damage = myPlayer.GetComponentInChildren<Weapon>().Damage * 1.05f;
                    myPlayer.GetComponent<Upgrade>().BasicPrice += 50f;
                }
            }

        }
    }

    //호밍 업그레이드
    public void HomingUpgrade()
    {
        if (photonView.IsMine)
        {
            myPlayer = GameObject.FindGameObjectWithTag("me");
            Debug.Log(myPlayer.GetComponent<Player>().mineral);
            if (myPlayer.GetComponent<Player>().mineral >= myPlayer.GetComponent<Upgrade>().HomingPrice)
            {
                if (myPlayer.GetComponentInChildren<Weapon>().value == 1)
                {
                    myPlayer.GetComponent<Player>().mineral -= myPlayer.GetComponent<Upgrade>().HomingPrice;
                    myPlayer.GetComponentInChildren<Weapon>().Damage = myPlayer.GetComponentInChildren<Weapon>().Damage * 1.05f;
                    myPlayer.GetComponent<Upgrade>().HomingPrice += 50f;
                }
            }

        }
    }

    //샷건 업그레이드
    public void ShotgunUpgrade()
    {
        if (photonView.IsMine)
        {
            myPlayer = GameObject.FindGameObjectWithTag("me");
            Debug.Log(myPlayer.GetComponent<Player>().mineral);
            if (myPlayer.GetComponent<Player>().mineral >= myPlayer.GetComponent<Upgrade>().ShotgunPrice)
            {
                if (myPlayer.GetComponentInChildren<Weapon>().value == 2)
                {
                    myPlayer.GetComponent<Player>().mineral -= myPlayer.GetComponent<Upgrade>().ShotgunPrice;
                    myPlayer.GetComponentInChildren<Weapon>().Damage = myPlayer.GetComponentInChildren<Weapon>().Damage * 1.05f;
                    myPlayer.GetComponent<Upgrade>().ShotgunPrice += 50f;
                }
            }

        }
    }

    //무기 재장전
    public void Reload()
    {
        if (photonView.IsMine)
        {
            myPlayer = GameObject.FindGameObjectWithTag("me");
            Debug.Log(myPlayer.GetComponent<Player>().mineral);
            //호밍이고 미네랄 충분하면
            if (myPlayer.GetComponentInChildren<Weapon>().value == 1 && myPlayer.GetComponent<Player>().mineral >= 100f)
            {
                myPlayer.GetComponent<Player>().mineral -= 100f;
                myPlayer.GetComponentInChildren<Weapon>().ammo = 50f;
            }
            //샷건이고 미네랄 충분하면
            else if (myPlayer.GetComponentInChildren<Weapon>().value == 2 && myPlayer.GetComponent<Player>().mineral >= 100f)
            {
                myPlayer.GetComponent<Player>().mineral -= 100f;
                Debug.Log(myPlayer.GetComponent<Player>().mineral);
                myPlayer.GetComponentInChildren<Weapon>().ammo = 50f;
            }
        }
    }

    //미사일 재장전
    public void mReload()
    {
        if (photonView.IsMine)
        {
            myPlayer = GameObject.FindGameObjectWithTag("me");
            Debug.Log(myPlayer.transform.GetChild(4).GetChild(3));
            Debug.Log(myPlayer.GetComponent<Player>().mineral);
            if (myPlayer.GetComponent<Player>().mineral >= 50f)
            {
                myPlayer.GetComponent<Player>().mineral -= 50f;
                //플레이어의 5번째 하위 오브젝트의 3번째 하위 오브젝트. 즉, 미사일 오브젝트의 weapon 컴포넌트의 mAmmo를 2로 만듦.
                myPlayer.transform.GetChild(4).GetChild(3).GetComponent<Weapon>().mAmmo = 2f;
            }
        }
    }


}
