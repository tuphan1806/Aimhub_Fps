using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameManager gameManager;


    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        Destroy(gameObject, 1.5f);
    }
}
