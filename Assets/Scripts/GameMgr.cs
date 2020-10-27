using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject playerprefab = null;
    // Start is called before the first frame update
    void Start()
    {
        createPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createPlayer()
    {
        float pos = Random.Range(-20.0f, 20.0f);
        PhotonNetwork.Instantiate(playerprefab.name, new Vector3(pos, 1.0f, pos), Quaternion.identity);
    }
}
