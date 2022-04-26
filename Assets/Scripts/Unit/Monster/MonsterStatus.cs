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
    protected override void Start()
    {
        base.Start();
        BaseDamage = 0;
    }

    public virtual MonsterManager.MonsterGrade MonsterGrade
    {
        /*
         *  TO-DO :player Attack에서 있어서 동기화가 되는지 확인필요
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
