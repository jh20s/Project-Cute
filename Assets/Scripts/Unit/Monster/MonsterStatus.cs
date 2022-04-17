using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterStatus : IStatus
{
    public int mId;
    public string mSpawnMap;
    [SerializeField]
    private string mMonsterInName;
    public bool mIsDieToKillCount = false;
    public bool mIsDieToGetExp = false;
    //public int MonsterAI;
    [SerializeField]
    private MonsterManager.MonsterGrade mMonsterGrade;

     
    public enum MonsterType
    {
        Normal,
        Boss
    };
    // Start is called before the first frame update
    void Start()
    {
        BaseDamage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //TO-DO ��ġ�� �ٲ���� �������� ���ֱ� ���� IStatus���� DamageHp�� �������ߴµ� Player�� ObjectPool�� ���⶧���� ���ʿ� Disable�ڵ带 ������ ����
        //��� disable ��ų���� ����� �ʿ�
        
    }

    public virtual MonsterManager.MonsterGrade MonsterGrade
    {
        /*
         *  TO-DO :player Attack���� �־ ����ȭ�� �Ǵ��� Ȯ���ʿ�
         */
        get { return mMonsterGrade; }
        set
        {
            mMonsterGrade = value;
        }
    }
    public string MonsterInName
    {
        get { return mMonsterInName;}
        set
        {
            mMonsterInName = value;
        }
    }

}
