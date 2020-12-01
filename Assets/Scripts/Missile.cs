using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviourPunCallbacks
{
    public float speed;
    public float lifeDuration;
    public GameObject MissileHitEffect;
    public GameObject AsteroidEffect;

    private float lifeTimer;
    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = lifeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Asteroid"))
        {
            GameObject effect = PhotonNetwork.Instantiate(MissileHitEffect.name, transform.position, Quaternion.identity);
            Target target = other.gameObject.GetComponentInParent<Target>();

            target.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, 50f);
            if (target.health <= 0f && this.gameObject.CompareTag("MyMissile"))
            {
                GameObject me = GameObject.FindGameObjectWithTag("me");
                if (photonView.IsMine)
                {
                    me.gameObject.GetComponent<Player>().mineral += 100f;
                    me.gameObject.GetComponent<Player>().score += 100f;
                    GameObject AEffect = PhotonNetwork.Instantiate(AsteroidEffect.name, other.transform.position, Quaternion.identity);
                }
            }

            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject effect = PhotonNetwork.Instantiate(MissileHitEffect.name, transform.position, Quaternion.identity);
            Target target = other.gameObject.GetComponentInParent<Target>();

            target.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, 50f);
            Destroy(gameObject);
        }
    }
}
