using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviourPunCallbacks, IPunObservable
{
    //인스펙터에서 직접 값 입력.
    public float health;
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
}
