using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEventHandler : IEventHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ChangeHp(int _hp, GameObject _obj)
    {
        if (_hp <= 0 && !GetComponent<MonsterStatus>().mIsDieToKillCount)
        {
            GetComponent<MonsterStatus>().mIsDieToKillCount = true;
            //TO-DO IEventHandler�� eventȭ ���ѳ�����
            SpawnManager.currentKillMosterCount++;            
            StartCoroutine(MonsterDie(_obj));

        }


        base.ChangeHp(_hp, _obj);
    }

    IEnumerator MonsterDie(GameObject _obj)
    {
        transform.GetComponent<Animator>().SetTrigger("Die");
        _obj.GetComponent<MonsterMove>().IsDie = true;
        _obj.GetComponent<MonsterAttack>().enabled = false;
        _obj.GetComponent<BoxCollider2D>().enabled = false;

        //TO-DO ���Ͱ� �׾ UseSkill���� Moveable�� walk�� �ٲٸ鼭 true�� �����̴� �κ��� �־�
        //��ų��� �� ����� ���Ͱ� �����̴� ���װ� �߻�.  handler�ʿ��� �ڷ�ƾ�� �������������� MonsterAttack���� 
        //�Ź� hpeventhandler�����ϱ�� ��Ұ� ū�Ͱ��� ��� handler�� �������� MonsterAttack���� stop�ϵ��� �����ʿ�
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
