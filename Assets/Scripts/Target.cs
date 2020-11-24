using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviourPunCallbacks, IPunObservable
{
    //인스펙터에서 직접 값 입력.
    public float health;
    public GameObject gm;

    void Awake()
    {
        if (photonView.IsMine)
        {
            gm = GameObject.FindGameObjectWithTag("GameController");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else 
        {
            health = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (this.gameObject.tag == "me")
        {
            StartCoroutine(ShowHitScreen());
        }
        if (health <= 0f)
        {
            photonView.RPC("Die", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator ShowHitScreen()
    {
        gm.GetComponent<GameMgr>().scr.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        gm.GetComponent<GameMgr>().scr.gameObject.SetActive(false);
    }

}
