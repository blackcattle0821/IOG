using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NameTag : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text UserName = null;
    public PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            return;
        }
        SetName();

    }

    // Update is called once per frame

    private void SetName()
    {
        UserName.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        UserName.color = PV.IsMine ? Color.green : Color.red;
    }
}
