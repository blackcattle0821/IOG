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
            if (other.transform.parent.transform.parent.gameObject.tag == "me")
            {

                Debug.Log(other.transform.parent.gameObject.GetComponentInParent<Player>().mineral);
                other.transform.parent.gameObject.GetComponentInParent<Player>().mineral += 50f;
                other.transform.parent.gameObject.GetComponentInParent<Player>().score += 50f;

            }
        }
        if (!photonView.IsMine)
        {
            if (other.transform.parent.transform.parent.gameObject.tag == "me")
            {
                other.transform.parent.gameObject.GetComponentInParent<Player>().mineral += 50f;
                other.transform.parent.gameObject.GetComponentInParent<Player>().score += 50f;

            }
        }
        photonView.RPC("Die", RpcTarget.All);
        Debug.Log(other);

        GameObject Eff = PhotonNetwork.Instantiate(CoinEffect.name, transform.position, Quaternion.identity);
    }

    [PunRPC]
    void Die()
    {
        Destroy(gameObject);
    }
}
