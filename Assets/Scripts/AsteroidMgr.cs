using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AsteroidMgr : MonoBehaviourPunCallbacks
{
    
    [SerializeField] GameObject asteroid;
    [SerializeField] int numberOfAsteroidsOnAnAxis = 10; // 격자 간격
    [SerializeField] int gridSpacing = 100;
    //[SerializeField] float minSpawnTime = 10.0f; 
    //[SerializeField] float maxSpawnTime = 20.0f;    

    // Start is called before the first frame update
    void Start()
    {
        PlaceAsteroids();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //배치
    void PlaceAsteroids()
    {
        for (int x = 0; x < numberOfAsteroidsOnAnAxis; x++)
        {
            for (int y = 0; y < numberOfAsteroidsOnAnAxis; y++)
            {
                for (int z = 0; z < numberOfAsteroidsOnAnAxis; z++)
                {
                    InstantiateAsteroid(x, y, z);
                }
            }
        }
    }

    //만들기
    void InstantiateAsteroid(int x, int y, int z)
    {
        asteroid = PhotonNetwork.Instantiate("asteroid", new Vector3(transform.position.x + (x * gridSpacing) + AsteroidOffset(),
            transform.position.y + (y * gridSpacing) + AsteroidOffset(),
            transform.position.z + (z * gridSpacing) + AsteroidOffset()), Quaternion.identity);
    }

    float AsteroidOffset()
    {
        return UnityEngine.Random.Range(-gridSpacing / 2f, gridSpacing / 2f);
    }

   
    //private IEnumerator SpawnAsteroid()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnTime, maxSpawnTime));

    //        float spawnPos = UnityEngine.Random.Range(-20.0f, 20.0f);
    //        PhotonNetwork.Instantiate("asteroid", new Vector3(spawnPos, 1.0f, spawnPos), Quaternion.identity);
    //    }
    //}

}