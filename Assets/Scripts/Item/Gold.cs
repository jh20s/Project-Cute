using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Item
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.activeInHierarchy)
        {
            ObjectPoolManager.Instance.DisableGameObject(gameObject);
            if (Gold != 0) { 
                collision.gameObject.GetComponent<PlayerStatus>().Gold += Gold;
            }
        }
    }
}
