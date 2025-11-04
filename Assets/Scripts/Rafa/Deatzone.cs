using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deatzone : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.RespawnPlayer(gameObject);
        }
        
    }
}
