using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Item
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
        Debug.Log("Ʈ���ŴµǴ�?");

        if (collision.gameObject.CompareTag("Player") && gameObject.activeInHierarchy)
        {
            Debug.Log("�������?");
            gameObject.SetActive(false);
            ObjectPoolManager.Instance.DisableGameObject(gameObject);
            if (Gold != 0) { 
                Debug.Log("gold ��? : "+Gold);
                Debug.Log("�浹������ �̸�: "+collision.gameObject.name);
                collision.gameObject.GetComponent<PlayerStatus>().Gold += Gold;
            }
        }
    }
}
