using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameOverMgr : MonoBehaviourPunCallbacks
{

    public Text Myscore;
    public GameObject Scoreboard;
    public Text High;
    // Start is called before the first frame update
    void Start()
    {
        Myscore.text = "My Score : " + PhotonNetwork.LocalPlayer.CustomProperties["score"];
        Myscore.color = new Color(255, 97, 97);

        High = Scoreboard.GetComponent<Text>();
    }

}
