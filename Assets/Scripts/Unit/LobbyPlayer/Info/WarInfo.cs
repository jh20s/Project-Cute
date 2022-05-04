using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WarInfo
{
    // ���� ���� ���� ���� ���
    [SerializeField]
    private DeongunStartManager.GameMode mWarMode;
    public DeongunStartManager.GameMode WarMode
    {
        get { return mWarMode; }
        set { mWarMode = value; }
    }
    // ���� ���� ���� ���� HP
    [SerializeField]
    private int mWarHp;
    public int WarHp
    {
        get { return mWarHp; }
        set { mWarHp = value; }
    }
    // ���� ���� ���� ���� ������
    [SerializeField]
    private int mWarDamage;
    public int WarDamage
    {
        get { return mWarDamage; }
        set { mWarDamage = value; }
    }
    // ���� ���� ���� ���� �̵� �ӵ�
    [SerializeField]
    private float mWarMoveSpeed;
    public float WarMoveSpeed
    {
        get { return mWarMoveSpeed; }
        set { mWarMoveSpeed = value; }
    }

    // ���� ���� ���� ���̾� ����
    [SerializeField]
    private int mWarDiamond;
    public int WarDiamond
    {
        get { return mWarDiamond; }
        set { mWarDiamond = value; }
    }


    // ���� ���� ���� ���� ���� �̸�
    [SerializeField]
    private string mWarWeaponName;
    public string WarWeaponName
    {
        get { return mWarWeaponName; }
        set { mWarWeaponName = value; }
    }
    // ���� ���� ���� ���� �ڽ�Ƭ �̸�
    [SerializeField]
    private string mWarCostumeName;
    public string WarCostumeName
    {
        get { return mWarCostumeName; }
        set { mWarCostumeName = value; }
    }
    // ���� ���� ���� ���� �ڽ�Ƭ �̸�
    [SerializeField]
    private string mWarCostumeShapeName;
    public string WarCostumeShapeName
    {
        get { return mWarCostumeShapeName; }
        set { mWarCostumeShapeName = value; }
    }

    // ���� ���� ���� ���� ���� Ÿ��
    [SerializeField]
    private DeongunStartManager.DeongunBuffType mWarBuff;
    public DeongunStartManager.DeongunBuffType WarBuff
    {
        get { return mWarBuff; }
        set { mWarBuff = value; }
    }

    // ���� ���� ���� ��ų �ر� ����
    [SerializeField]
    private StringBoolean mWarSkillLock;
    public StringBoolean WarSkillLock
    {
        get { return mWarSkillLock; }
        set { mWarSkillLock = value; }
    }
}
