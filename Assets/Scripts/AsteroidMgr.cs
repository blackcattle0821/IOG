using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AsteroidMgr : MonoBehaviour
{
    [SerializeField] Asteroid asteroid;
    [SerializeField] int numberOfAsteroidsOnAnAxis = 10; // 격자 간격
    [SerializeField] int gridSpacing = 100;
    // Start is called before the first frame update
    void Start()
    {
        PlaceAsteroids();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 배치
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

    // 만들기
    void InstantiateAsteroid(int x, int y, int z)
    {
        Instantiate(asteroid, new Vector3(transform.position.x + (x * gridSpacing) + AsteroidOffset(),
            transform.position.y + (y * gridSpacing) + AsteroidOffset(),
            transform.position.z + (z * gridSpacing) + AsteroidOffset()), Quaternion.identity, transform);
    }

    float AsteroidOffset()
    {
        return Random.Range(-gridSpacing / 2f, gridSpacing / 2f);
    }

}