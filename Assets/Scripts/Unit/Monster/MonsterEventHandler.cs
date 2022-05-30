using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEventHandler : IEventHandler
{
    // Start is called before the first frame update

    public override void ChangeIsDie(bool _dieCheck, GameObject _obj)
    {
        if (_dieCheck)
        {
            SpawnManager.currentKillMosterCount++;
            StartCoroutine(MonsterDie(_obj));
            // 죽은 몬스터가 보스이면 보스킬 카운트 증가
            if (_obj.GetComponent<MonsterStatus>().MonsterGrade == MonsterManager.MonsterGrade.Boss)
                SpawnManager.currentKillBossMonsterCount++;
        }
        base.ChangeIsDie(_dieCheck, _obj);
    }


    IEnumerator MonsterDie(GameObject _obj)
    {
        transform.GetComponent<Animator>().SetTrigger("Die");
        _obj.GetComponent<MonsterAttack>().enabled = false;
        _obj.GetComponent<CapsuleCollider2D>().enabled = false;
        if(gameObject.GetComponent<MonsterStatus>().MonsterGrade == MonsterManager.MonsterGrade.Boss)
           _obj.GetComponent<CircleCollider2D>().enabled = false;
        _obj.GetComponent<MonsterAttack>().StopAllCoroutines();


        for (int i = 10; i >= 0; i--)
        {
            float transparency = i / 10f;
            Color monsterColor = _obj.GetComponent<SpriteRenderer>().color;
            monsterColor.a = transparency;
            _obj.GetComponent<SpriteRenderer>().color = monsterColor;
            yield return new WaitForSeconds(0.15f);
        }

        ObjectPoolManager.Instance.DisableGameObject(_obj);
    }
}
