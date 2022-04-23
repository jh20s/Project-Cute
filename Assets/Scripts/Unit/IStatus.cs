using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class IStatus : MonoBehaviour
{
    //���� Hp
    [SerializeField]
    protected int mHp;
    public virtual int Hp
    {
        /*
         *  TO-DO :player Attack���� �־ ����ȭ�� �Ǵ��� Ȯ���ʿ�
         */
        get { return mHp; }
        set
        {
            mHp = value;
            gameObject.GetComponent<IEventHandler>().ChangeHp(mHp, gameObject);
        }
    }

    //�ִ� HP
    [SerializeField]
    protected int mMaxHp;
    public virtual int MaxHP
    {
        get { return mMaxHp; }
        set
        {
            mMaxHp = value;
        }
    }

    // ������
    [SerializeField]
    private int mSize;
    public virtual int Size
    {
        get { return mSize; }
        set
        {
            mSize = value;
            gameObject.transform.localScale = new Vector3(mSize, mSize, mSize);
        }
    }

    //������ ���� ������
    [SerializeField]
    protected int mDamaged;
    public virtual int DamageHp
    {
        /*
         *  TO-DO :player Attack���� �־ ����ȭ�� �Ǵ��� Ȯ���ʿ�
         */
        get { return mDamaged; }
        set
        {
            if (mIsInvincibility)
                value = 0;
            mDamaged = value;
            mHp = Mathf.Max(0, mHp - mDamaged);
            gameObject.GetComponent<IEventHandler>().ChangeHp(mHp, gameObject);
            MessageBoxManager.BoxType bt = (MessageBoxManager.BoxType)Enum.Parse(typeof(MessageBoxManager.BoxType), gameObject.tag + "Damage");
            MessageBoxManager.Instance.createMessageBox(bt, value.ToString(), gameObject.transform.position);
        }
    }

    //������ ���� ����
    [SerializeField]
    protected int mPotionHp;
    public virtual int PotionHp
    {
        get { return mPotionHp; }
        set
        {
            mPotionHp = value;
            mHp = Mathf.Min(MaxHP, mHp + mPotionHp);
            gameObject.GetComponent<IEventHandler>().ChangeHp(mHp, gameObject);
            MessageBoxManager.Instance.createMessageBox(MessageBoxManager.BoxType.PlayerHpPotion, value.ToString(), gameObject.transform.position);
        }
    }

    //Object �ӵ�
    [SerializeField]
    protected float mMoveSpeed;
    public float MoveSpeed
    {
        get
        {
            return mMoveSpeed;
        }
        set
        {
            mMoveSpeed = value;
            GetComponent<IEventHandler>().ChangeMoveSpeed(mMoveSpeed, gameObject);
        }

    }
    // Object �ӵ� ���(������ �� ��ų)
    [SerializeField]
    private float mMoveSpeedRate = 1f;
    public float MoveSpeedRate
    {
        get
        {
            return mMoveSpeedRate;
        }
        set
        {
            mMoveSpeedRate = value;
            GetComponent<IEventHandler>().ChangeMoveSpeed(mMoveSpeed * (mMoveSpeedRate + mAddSpeed), gameObject);
        }

    }
    // �̵� �ӵ�������(��������)
    [SerializeField]
    private float mAddSpeed; 
    public float AddSpeed
    {
        get { return mAddSpeed; }
        set
        {
            mAddSpeed = value;
            GetComponent<IEventHandler>().ChangeMoveSpeed(mMoveSpeed * (mMoveSpeedRate + mAddSpeed), gameObject);
        }
    }

    // �⺻ ���� �ӵ�
    [SerializeField]
    private float mAttackSpeed;
    public float AttackSpeed
    {
        get 
        { 
            return mAttackSpeed ; 
        }
        set 
        { 
            mAttackSpeed = value;
            GetComponent<IEventHandler>().ChangeAttackSpeed(mAttackSpeed, gameObject);
        }
    }
    // �⺻���� �ӵ� ������
    [SerializeField]
    private float mAddAttackSpeed; 
    public float AddAttackSpeed
    {
        get { return mAddAttackSpeed; }
        set
        {
            mAddAttackSpeed = value;
            GetComponent<IEventHandler>().ChangeAttackSpeed(mAttackSpeed - mAddAttackSpeed, gameObject);

        }
    }

    // �⺻ ������
    [SerializeField]
    protected int mBaseDamage;
    public virtual int BaseDamage
    {
        get { return mBaseDamage; }
        set
        {
            mBaseDamage = value;
            GetComponent<IEventHandler>().ChangeAttackPoint(mBaseDamage, gameObject);
        }
    }

    // �÷��̾ �������� ����
    [SerializeField]
    protected Weapon currentWeapon;
    public Weapon PlayerCurrentWeapon
    {
        set
        {
            currentWeapon = value;
        }
        get
        {
            return currentWeapon;
        }
    }
    // �⺻ ������ ������
    [SerializeField]
    private float mAddAttackPoint;
    public float AddAttackPoint
    {
        get { return mAddAttackPoint; }
        set 
        { 
            mAddAttackPoint = value;
        }
    }
    // ���� ������
    [SerializeField]
    private int mObjectDamage;

    // �⺻ ġ��Ÿ Ȯ��
    [SerializeField]
    private float mCriticalChance;
    public float CriticalChance
    {
        get { return mCriticalChance; }
        set { mCriticalChance = value; }
    }
    // �⺻ ġ��Ÿ Ȯ�� �߰���
    [SerializeField]
    private float mAddCriticalChance;
    public float AddCriticalChance
    {
        get { return mAddCriticalChance; }
        set { mAddCriticalChance = value; }
    }

    // �⺻ ġ��Ÿ ������
    [SerializeField]
    private float mCriticalDamage;
    public float CriticalDamage
    {
        get { return mCriticalDamage; }
        set { mCriticalDamage = value; }
    }
    [SerializeField]
    private int mAddProjectileCount; // �⺻ ���� �߻�ü ����
    public int AddProjectileCount
    {
        get
        {
            return mAddProjectileCount;
        }
        set
        {
            mAddProjectileCount = value;
            GetComponent<IEventHandler>().ChangeProjectileCount(mAddProjectileCount, gameObject);
        }
    }

    // ��,â ����
    // �⺻ ���� ����
    [SerializeField]
    private float mAddAttackRange; 
    public float AddAttackRange
    {
        get
        {
            return mAddAttackRange;
        }
        set
        {
            mAddAttackRange = value;
            GetComponent<IEventHandler>().ChangeProjectileScale(mAddAttackRange, gameObject);
            
        }
    }
    // �⺻ ���� ���� �ð�
    [SerializeField]
    private float mAddStiffTime; 
    public float AddStiffTime
    {
        get
        {
            return mAddStiffTime;
        }
        set
        {
            mAddStiffTime = value;
            GetComponent<IEventHandler>().ChangeStiffTime(mAddStiffTime, gameObject);
        }
    }

    // �����, ������ ����
    [SerializeField]
    // �⺻���� Ƚ��
    private int mAddRAttackCount; 
    public int AddRAttackCount
    {
        get
        {
            return mAddRAttackCount;
        }
        set
        {
            mAddRAttackCount = value;
            GetComponent<IEventHandler>().ChangeRAttackCount(mAddRAttackCount, gameObject);
        }
    }
    // �⺻ ���� ����
    [SerializeField]
    private  int mAddPassCount;
    public int AddPassCount
    {
        get
        {
            return mAddPassCount;
        }
        set
        {
            mAddPassCount = value;
            GetComponent<IEventHandler>().ChangePassCount(mAddPassCount, gameObject);
        }
    }

    [SerializeField]
    private bool mIsLaunch;
    public bool IsLaunch
    {
        get
        {
            return mIsLaunch;
        }
        set
        {
            mIsLaunch = value;
            GetComponent<IEventHandler>().ChangeIsLaunch(mIsLaunch, gameObject);
        }
    }

    public int mPhysicalDefense;
    public int mMagicDefense;
    [SerializeField]
    private bool mIsInvincibility = false;

    protected virtual void Start()
    {
        // �⺻ ������ ���� �ʱ�ȭ
        mAddAttackPoint = 0;
        mAddSpeed = 0;
        mAddAttackSpeed = 0;
        mAddCriticalChance = 0;
        mAddProjectileCount = 0;
        mAddAttackRange = 0;
        mAddStiffTime = 0;
        mAddRAttackCount = 0;
        mAddPassCount = 0;
}

    /*
     *  �� ���� ȣ�⸶�� �ҷ��ͼ� ���ݷ� ����
     *  ���ϰ��� true�� ��� ũ��Ƽ��, false�� ��� �⺻
     */
    public bool AttackPointSetting(GameObject _obj)
    {
        if (_obj.CompareTag("Player"))
        {
            // ���Ŀ� ���⺰ ����� �����ͷ� �޾ƿͼ� ����
            float ran = UnityEngine.Random.Range(0.8f, 1.2f);
            // ũ��Ƽ�� Ȯ���� ���� �����̱�
            int criRan = UnityEngine.Random.Range(0, 100);

            // ũ��Ƽ�� ����
            if (criRan < (mCriticalChance + mAddCriticalChance) * 100)
            {
                mObjectDamage = (int)(((mBaseDamage + currentWeapon.Spec.WeaponDamage) * (ran + mAddAttackPoint)) * mCriticalDamage);
                GetComponent<IEventHandler>().ChangeAttackPoint(mObjectDamage, gameObject);
                return true;
            }
            // �⺻ ����
            else
            {
                mObjectDamage = (int)((mBaseDamage + currentWeapon.Spec.WeaponDamage) * (ran + mAddAttackPoint));
                GetComponent<IEventHandler>().ChangeAttackPoint(mObjectDamage, gameObject);
                return false;
            }
        }
        else
        {
            mObjectDamage = mBaseDamage;
            GetComponent<IEventHandler>().ChangeAttackPoint(mObjectDamage, gameObject);
            return false;
        }

    }

    public void LevelUpStatus(LevelUpStatusManager.Stat _stat)
    {
        switch (_stat.mType)
        {
            // null
            case LevelUpStatusManager.StatType.Null:
                break;
            // �⺻ ���� ���ط� ����
            case LevelUpStatusManager.StatType.AutoAttack:
                AddAttackPoint += _stat.mStatIncrease;
                break;
            // ġ��Ÿ Ȯ�� ����
            case LevelUpStatusManager.StatType.CriticalChance:
                AddCriticalChance += _stat.mStatIncrease;
                break;
            // �⺻ ���� �ӵ� ����
            case LevelUpStatusManager.StatType.AutoAttackSPD:
                AddAttackSpeed += _stat.mStatIncrease;
                break;
            // �̵� �ӵ� ����
            case LevelUpStatusManager.StatType.MoveSPD:
                AddSpeed += _stat.mStatIncrease;
                break;
            // �߻�ü ���� ����
            case LevelUpStatusManager.StatType.AutoAttackTimes:
                AddProjectileCount += (int)_stat.mStatIncrease;
                break;
            // �⺻ ���� ���� ����
            case LevelUpStatusManager.StatType.AutoAttackRange:
                AddAttackRange += _stat.mStatIncrease;
                break;
            // �⺻ ���� ���� ����
            case LevelUpStatusManager.StatType.AutoAttackStiff:
                AddStiffTime += _stat.mStatIncrease;
                break;
            // �߻�ü Ƚ�� ����
            case LevelUpStatusManager.StatType.AutoLaunchSpread:
                AddRAttackCount += (int)_stat.mStatIncrease;
                break;
            // �߻�ü ���� ����
            case LevelUpStatusManager.StatType.AutoLaunchThrough:
                AddPassCount += (int)_stat.mStatIncrease;
                break;
            // HPȸ��
            case LevelUpStatusManager.StatType.RecoverHP:
                Hp += (int)_stat.mStatIncrease;
                break;
        }
    }
    protected IEnumerator InvincibilityCorutine(float time)
    {
        mIsInvincibility = true;
        yield return new WaitForSeconds(time);
        mIsInvincibility = false;
    }
    public int getCurrentWeponeDamage()
    {
        return currentWeapon==null ?0:currentWeapon.Spec.WeaponDamage;
    }
    public void ChangeStatusForCostume(Costume.CostumeBuffType _key, Costume _item)
    {
        // �÷��̾� ü�� ���
        if(_key == Costume.CostumeBuffType.PlayerHP)
        {
            Hp = Hp + _item.GetBuffValue(_key);
            MaxHP = Hp;
        }
        // �÷��̾� �̼� ���
        else if (_key == Costume.CostumeBuffType.PlayerSPD)
        {
            mMoveSpeedRate += _item.GetBuffValue(_key) / 100f;
        }
    }
    public void ChangeStatusForSkill(Skill.ESkillBuffType _type, float value)
    {
        // �ӵ����� ����
        if (_type == Skill.ESkillBuffType.PlayerSPD)
        {
            mMoveSpeedRate += value;
        }
        // �����̵�
        else if (_type == Skill.ESkillBuffType.PlayerPosition)
        {
            Vector3 currentDir = GetComponent<PlayerAttack>().Target;
            currentDir = (currentDir - transform.position) * value;
            RaycastHit2D ray = Physics2D.Raycast(
                new Vector2(transform.position.x + currentDir.x, transform.position.y + currentDir.y),
                Vector3.zero, LayerMask.GetMask("Tilemap"));
            // ���� ������ �����̵�
            if(ray.collider == null)
            {
                gameObject.transform.position = gameObject.transform.position + currentDir;
            }
            // ������ �Ÿ� ���̱�
            else
            {
                ChangeStatusForSkill(_type, value - 1);
            }  
        }
        else if (_type == Skill.ESkillBuffType.PlayerWeaponSprite)
        {
            List<GameObject> wList =  EquipmentManager.Instance.FindWepaonList("sw");
            PlayerManager.Instance.getPlayerWeaponSprite().sprite = wList[(int)value].GetComponent<SpriteRenderer>().sprite;
        }
    }
    public void ChangeStatusForSkillOff(Skill.ESkillBuffType _type, float value)
    {
        if(_type == Skill.ESkillBuffType.PlayerSPD)
        {
            mMoveSpeedRate -= value;
        }
        else if(_type == Skill.ESkillBuffType.PlayerWeaponSprite)
        {
            EquipmentManager.Instance.ChangeWeapon(currentWeapon.Spec.Type);
        }
    }
}
