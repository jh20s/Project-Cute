using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class IStatus : MonoBehaviour
{
    //���� Hp
    [SerializeField]
    protected int mHp;
    public virtual int Hp
    {
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

    [SerializeField]
    protected bool mIsDie;
    public virtual bool IsDie
    {
        get { return mIsDie; }
        set
        {
            if (mIsDie != value)
            {
                mIsDie = value;
                gameObject.GetComponent<IEventHandler>().ChangeIsDie(mIsDie, gameObject);
            }
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


    [SerializeField]
    protected int mCloseDamaged;
    public int CloseDamaged
    {
        get { return mCloseDamaged; }
        set
        {
            mCloseDamaged = value;
            if (mIsInvincibility)
                mCloseDamaged = 0;
            mHp = Mathf.Max(0, mHp - mCloseDamaged);
            gameObject.GetComponent<IEventHandler>().ChangeHp(mHp, gameObject);
            MessageBoxManager.BoxType bt =(MessageBoxManager.BoxType)Enum.Parse(typeof(MessageBoxManager.BoxType), gameObject.tag + "Damage");
            MessageBoxManager.Instance.createMessageBox(bt, mCloseDamaged == 0 ? "����": mCloseDamaged.ToString(), transform.position);
        }
    }

    //������ ���� ������(�߻�ü)
    [SerializeField]
    protected int mDamaged;
    public  int DamageHp
    {
        get { return mDamaged; }
        set
        {
            mDamaged = value;
            mHp = Mathf.Max(0, mHp - mDamaged);
            gameObject.GetComponent<IEventHandler>().ChangeHp(mHp, gameObject);
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
            GameObject.Find("Canvas").transform.Find("PausePannel").transform.GetChild(1).GetChild(1).GetComponent<Text>().text = currentWeapon.Spec.TypeName;
        }
        get
        {
            return currentWeapon;
        }
    }
    // �⺻ ������ ������
    [SerializeField]
    protected float mAddAttackPoint;
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
    protected int mObjectDamage;

    // �⺻ ġ��Ÿ Ȯ��
    [SerializeField]
    protected float mCriticalChance;
    public float CriticalChance
    {
        get { return mCriticalChance; }
        set { mCriticalChance = value; }
    }
    // �⺻ ġ��Ÿ Ȯ�� �߰���
    [SerializeField]
    protected float mAddCriticalChance;
    public float AddCriticalChance
    {
        get { return mAddCriticalChance; }
        set { mAddCriticalChance = value; }
    }

    // �⺻ ġ��Ÿ ������
    [SerializeField]
    protected float mCriticalDamage;
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
    public abstract bool AttackPointSetting(GameObject _obj);

    public void LevelUpStatus(Stat _stat)
    {
        switch (_stat.Type)
        {
            // null
            case LevelUpStatusManager.StatType.Null:
                break;
            // �⺻ ���� ���ط� ����
            case LevelUpStatusManager.StatType.AutoAttack:
                AddAttackPoint += _stat.StatIncrease;
                break;
            // ġ��Ÿ Ȯ�� ����
            case LevelUpStatusManager.StatType.CriticalChance:
                AddCriticalChance += _stat.StatIncrease;
                break;
            // �⺻ ���� �ӵ� ����
            case LevelUpStatusManager.StatType.AutoAttackSPD:
                AddAttackSpeed += _stat.StatIncrease;
                break;
            // �̵� �ӵ� ����
            case LevelUpStatusManager.StatType.MoveSPD:
                AddSpeed += _stat.StatIncrease;
                break;
            // �߻�ü ���� ����(�ٰŸ�)
            case LevelUpStatusManager.StatType.AutoAttackTimesMelee:
                AddProjectileCount += (int)_stat.StatIncrease;
                break;
            // �߻�ü ���� ����(���Ÿ�)
            case LevelUpStatusManager.StatType.AutoAttackTimesRange:
                AddProjectileCount += (int)_stat.StatIncrease;
                break;
            // �⺻ ���� ���� ����
            case LevelUpStatusManager.StatType.AutoAttackRange:
                AddAttackRange += _stat.StatIncrease;
                break;
            // �⺻ ���� ���� ����
            case LevelUpStatusManager.StatType.AutoAttackStiff:
                AddStiffTime += _stat.StatIncrease;
                break;
            // �߻�ü Ƚ�� ����
            case LevelUpStatusManager.StatType.AutoLaunchSpread:
                AddRAttackCount += (int)_stat.StatIncrease;
                break;
            // �߻�ü ���� ����
            case LevelUpStatusManager.StatType.AutoLaunchThrough:
                AddPassCount += (int)_stat.StatIncrease;
                break;
            // HPȸ��
            case LevelUpStatusManager.StatType.RecoverHP:
                PotionHp = (int)_stat.StatIncrease;
                break;
        }
    }
    public void Invincibility(float _time)
    {
        StartCoroutine(InvincibilityCorutine(_time));
    }
    protected IEnumerator InvincibilityCorutine(float _time)
    {
        mIsInvincibility = true;
        yield return new WaitForSeconds(_time);
        mIsInvincibility = false;
    }
    public void ChangeStatusForSkill(Skill.ESkillBuffType _type, float value)
    {
        // �ӵ����� ����
        if (_type == Skill.ESkillBuffType.PlayerSPD)
        {
            MoveSpeedRate += value;
        }
        // �����̵�
        else if (_type == Skill.ESkillBuffType.PlayerPosition)
        {
            Vector3 currentDir = GetComponent<PlayerAttack>().Target;
            if (currentDir == Vector3.zero)
            {
                currentDir = Vector3.right;
            }
            currentDir = (currentDir - transform.position).normalized * value;
            RaycastHit2D ray = Physics2D.Raycast(
                new Vector2(transform.position.x + currentDir.x, transform.position.y + currentDir.y),
                Vector3.zero, 1f, LayerMask.GetMask("Tilemap"));
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
            List<GameObject> wList =  EquipmentManager.Instance.FindWepaonList("WS");
            PlayerManager.Instance.getPlayerWeaponSprite().sprite = wList[0].GetComponent<SpriteRenderer>().sprite;
        }
    }
    public void ChangeStatusForSkillOff(Skill.ESkillBuffType _type, float value)
    {
        if(_type == Skill.ESkillBuffType.PlayerSPD)
        {
            MoveSpeedRate -= value;
        }
        else if(_type == Skill.ESkillBuffType.PlayerWeaponSprite)
        {
            PlayerManager.Instance.getPlayerWeaponSprite().sprite = currentWeapon.GetComponent<SpriteRenderer>().sprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Projectile projectile = collision.GetComponent<Projectile>();
            int damage = projectile.IsDamage(gameObject);
            if (projectile.IsActive  && damage !=0)
            {
                // �˹� Ȯ��
                if(projectile.Spec.Knockback > 0)
                {
                    RaycastHit2D ray = Physics2D.Raycast(
                        transform.position + (transform.position - projectile.transform.position).normalized * (GetComponent<CapsuleCollider2D>().size.x * projectile.Spec.Knockback),
                        Vector2.zero,
                        1f,
                        LayerMask.GetMask("Tilemap"));
                    if(ray.collider == null)
                    {
                        
                        transform.Translate(
                            (transform.position - projectile.transform.position).normalized * 
                            GetComponent<CapsuleCollider2D>().size.x * projectile.Spec.Knockback);
                            
                    }
                }
            }
            // ������ ������ �� ������ �ڽ� ����(Ÿ�Ժ�)
            if(!mIsInvincibility)
                ChangeHpForDamage(projectile, projectile.Spec.ProjectileDamageType, damage);
        }
    }

    private void ChangeHpForDamage(Projectile obj, ProjectileManager.DamageType _type, int _damage)
    {
        //�� ���� ������ ������ 0���ϴ� ������ �ڽ��� �������� �߻����� ����
        if (_damage <= 0)
        {
            return;
        }

        switch (_type)
        {
            // �Ϲ���
            case ProjectileManager.DamageType.Normal:
                DamageHp = _damage;
                DrawDamageBox(obj.IsCriticalDamage, _damage, transform.position);
                break;
            // ���ø���
            case ProjectileManager.DamageType.SplitByNumber:
                int num = obj.Spec.ProjectileDamageSplit;
                for (int i = 0; i < num; i++)
                {
                    DamageHp = _damage / num;
                    DrawDamageBox(obj.IsCriticalDamage, _damage / num, transform.position + Vector3.up * i * 0.5f);
                }
                break;
            // ��Ʈ��
            case ProjectileManager.DamageType.SplitByTime:
                int splitnum = (int)(obj.Spec.SpawnTime / obj.Spec.ProjectileDamageSplitSec);
                int damage = _damage / splitnum;
                StartCoroutine(CoSplitByTime(obj.Spec.ProjectileDamageSplitSec, splitnum, damage, obj.IsCriticalDamage));
                break;
        }
    }

    private void DrawDamageBox(bool _isCritical, int _damage, Vector3 _pos)
    {
        if (_isCritical)
        {
            MessageBoxManager.Instance.createMessageBox(
                MessageBoxManager.BoxType.CriticalDamage, _damage.ToString(), _pos);
        }
        else
        {
            MessageBoxManager.BoxType bt = 
                (MessageBoxManager.BoxType)Enum.Parse(typeof(MessageBoxManager.BoxType), gameObject.tag + "Damage");
            MessageBoxManager.Instance.createMessageBox(bt, _damage.ToString(), _pos);
        }
    }

    IEnumerator CoSplitByTime(float _time, int _splitNum, int _damage, bool _isCritical)
    {
        int num = _splitNum;
        while (num > 0)
        {
            num--;
            DamageHp = _damage;
            DrawDamageBox(_isCritical, _damage, transform.position);
            yield return new WaitForSeconds(_time);
        }
    }
}
