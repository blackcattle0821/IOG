using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTarget : MonoBehaviourPunCallbacks
{
    public GameObject CoinEffect;
    void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.transform.parent.gameObject.tag == "me")
            {

                other.transform.parent.gameObject.gameObject.GetComponent<Player>().mineral += 50f;
                other.transform.parent.gameObject.gameObject.GetComponent<Player>().score += 50f;

            }
        }
        if (!photonView.IsMine)
        {
            if (other.transform.parent.gameObject.tag == "me")
            {
                other.transform.parent.gameObject.gameObject.GetComponent<Player>().mineral += 50f;
                other.transform.parent.gameObject.gameObject.GetComponent<Player>().score += 50f;

            }
        }
        photonView.RPC("Die", RpcTarget.AllBuffered);
        GameObject Eff = PhotonNetwork.Instantiate(CoinEffect.name, transform.position, Quaternion.identity);
    }

    [PunRPC]
    void Die()
    {
        Destroy(gameObject);
    }
}
