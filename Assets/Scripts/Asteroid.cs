using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float AminScale = 3.0f;
    [SerializeField] float AmaxScale = 3.4f;
    [SerializeField] float ArotationOffset = 100f;

    Transform myTA;
    Vector3 ArandomRotation;

    void Awake()
    {
        myTA = transform;
        this.gameObject.layer = 10;
    }


    // Start is called before the first frame update
    void Start()
    {
        // 랜덤 사이즈
        Vector3 scale = Vector3.one;
        scale.x = Random.Range(AminScale, AmaxScale);
        scale.y = Random.Range(AminScale, AmaxScale);
        scale.z = Random.Range(AminScale, AmaxScale);

        myTA.localScale = scale;

        // 랜덤 rotation
        ArandomRotation.x = Random.Range(-ArotationOffset, ArotationOffset);
        ArandomRotation.y = Random.Range(-ArotationOffset, ArotationOffset);
        ArandomRotation.z = Random.Range(-ArotationOffset, ArotationOffset);


    }

    // Update is called once per frame
    void Update()
    {
        myTA.Rotate(ArandomRotation * Time.deltaTime);
    }

   
}