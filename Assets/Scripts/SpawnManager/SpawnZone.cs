using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [SerializeField]
    private bool mSpawnalbe = true;
    public bool Spawnable
    {
        get { return mSpawnalbe; }   
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tilemap"))
        {
            mSpawnalbe = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tilemap"))
        {
            mSpawnalbe = true;
        }
    }
}
