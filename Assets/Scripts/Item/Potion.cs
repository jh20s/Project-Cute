using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            ObjectPoolManager.Instance.DisableGameObject(gameObject);
            if (Hp != 0)
                collision.gameObject.GetComponent<PlayerStatus>().PotionHp = Hp;
        }
    }

}
