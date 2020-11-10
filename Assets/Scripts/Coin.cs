using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float CminScale = .4f;
    [SerializeField] float CmaxScale = .6f;
    [SerializeField] float CrotationOffset = 10f;

    Transform myTC;
    Vector3 CrandomRotation;

    void Awake()
    {
        myTC = transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector3 scale = Vector3.one;
        scale.x = Random.Range(CminScale, CmaxScale);
        scale.y = Random.Range(CminScale, CmaxScale);
        scale.z = Random.Range(CminScale, CmaxScale);

        myTC.localScale = scale;

        CrandomRotation.x = Random.Range(-CrotationOffset, CrotationOffset);
        CrandomRotation.y = Random.Range(-CrotationOffset, CrotationOffset);
        CrandomRotation.z = Random.Range(-CrotationOffset, CrotationOffset);
    }

    // Update is called once per frame
    void Update()
    {
        myTC.Rotate(CrandomRotation * Time.deltaTime);

    }
    
}
