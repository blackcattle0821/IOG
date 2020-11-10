using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CoinMgr : MonoBehaviour
{
    [SerializeField]GameObject coin;
    [SerializeField]int numberOfcoinsOnAnAxis = 10; // 격자 간격
    [SerializeField]int gridSpacing = 100;


    // Start is called before the first frame update
    void Start()
    {
        PlaceCoins();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlaceCoins()
    {
        for(int x = 0; x < numberOfcoinsOnAnAxis; x++)
        {
            for(int y = 0; y < numberOfcoinsOnAnAxis; y++)
            {
                for(int z = 0; z < numberOfcoinsOnAnAxis; z++)
                {
                    InstantiateCoin(x, y, z);
                }
            }
        }
    }

    void InstantiateCoin(int x, int y, int z)
    {
        coin = PhotonNetwork.Instantiate("coin", new Vector3(transform.position.x + (x * gridSpacing) + CoinOffset(),
            transform.position.y + (y * gridSpacing) + CoinOffset(),
            transform.position.z + (z * gridSpacing) + CoinOffset()), Quaternion.identity);
        coin.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

    }

    float CoinOffset()
    {
        return UnityEngine.Random.Range(-gridSpacing / 2f, gridSpacing / 2f);
    }

}
