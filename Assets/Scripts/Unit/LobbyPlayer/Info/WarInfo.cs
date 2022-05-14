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

    //���� ���� ���� ������ �ڼ���� �Ŀ�
    [SerializeField]
    private float mWarMagnetPower;
    public float WarMagnetPower
    {
        get { return mWarMagnetPower; }
        set { mWarMagnetPower = value; }
    }

    // ���� ���� ���� ȹ�� ��差 ��ġ
    [SerializeField]
    private float mWarGoldRate;
    public float WarGoldRate
    {
        get => mWarGoldRate;
        set
        {
            mWarGoldRate = value;
        }
    }

    // ���� ���� ���� �߰� ��Ȱ Ƚ��
    [SerializeField]
    private int mWarReviveValue;
    public int WarRevuveValue
    {
        get => mWarReviveValue;
        set
        {
            mWarReviveValue = value;
        }
    }

    // ���� ���� ���� �߰� ������ ���� ���ӽð�
    [SerializeField]
    private float mWarStaffShieldTime;
    public float WarStaffShieldTime
    {
        get => mWarStaffShieldTime;
        set
        {
            mWarStaffShieldTime = value;
        }
    }

    // ���� ���� ���� �߰� �ٰŸ� ȸ�� �߰�Ƚ��
    [SerializeField]
    private float mWarMeleeDodgeCount;
    public float WarMeleeDodgeCount
    {
        get => mWarMeleeDodgeCount;
        set
        {
            mWarMeleeDodgeCount = value;
        }
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

    // ���� ���� ���� ���� ���� ����
    [SerializeField]
    private DeongunStartManager.DeongunBuffType mWarDeongunBuffType;
    public DeongunStartManager.DeongunBuffType WarDeongunBuffType
    {
        get { return mWarDeongunBuffType; }
        set { mWarDeongunBuffType = value; }
    }
    [SerializeField]
    private int mWarBuffHp;
    public int WarBuffHp
    {
        get { return mWarBuffHp; }
        set { mWarBuffHp = value; }
    }

    [SerializeField]
    private float mWarBuffSpeedRate;
    public float WarBuffSpeedRate
    {
        get { return mWarBuffSpeedRate; }
        set { mWarBuffSpeedRate = value; }
    }

    // ���� ���� ���� ��ų �ر� ����
    [SerializeField]
    private StringBoolean mWarSkillLock;
    public StringBoolean WarSkillLock
    {
        get { return mWarSkillLock; }
        set { mWarSkillLock = value; }
    }

    // ���� ���� ���� ��� ����
    [SerializeField]
    private bool mIsBossRelay;
    public bool IsBossRelay
    {
        get { return mIsBossRelay; }
        set { mIsBossRelay = value; }
    }

    // ���� ���� ���� �Ϳ������� ���� ��� ����
    [SerializeField]
    private bool mIsWarCutePotion;
    public bool IsWarCutePotion
    {
        get { return mIsWarCutePotion; }
        set { mIsWarCutePotion = value; }
    }
}
