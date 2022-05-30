using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGameEndCheck : MonoBehaviour
{

    private void OnEnable()
    {
        gameObject.GetComponent<MonsterEventHandler>().registerIsDieObserver(RegisterBossIsDieObserver);
    }

    public void RegisterBossIsDieObserver(bool _die, GameObject _obj)
    {
        if (_die)
        {
            Debug.Log("��������(�巡��) ���");
            UIManager.Instance.OpenSecondEndingPannel();
        }
    }
}
