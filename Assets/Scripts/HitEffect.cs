using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviourPunCallbacks
{
    //public RaycastHit Hit;
    public float LifeTime;
    public float LifeTimer;
    // Start is called before the first frame update
    void Start()
    {
        LifeTimer = LifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        LifeTimer -= Time.deltaTime;
        if (LifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
