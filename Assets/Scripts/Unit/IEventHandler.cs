using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEventHandler : MonoBehaviour
{
    /*
     * TO-DO : �̺�Ʈ�ڵ鷯�� ���ʸ��ϰ� ©���ִ� ����� �־�δ� delegate�� �� �̺�Ʈ������ ������ ������ִ� ����̶� �ߺ��Ǵ� �ڵ尡 �ʹ�����.
     *         �ذ����� ã�ƺ���./
     * ���� https://docs.microsoft.com/ko-kr/dotnet/csharp/programming-guide/events/how-to-publish-events-that-conform-to-net-framework-guidelines
     */


    public delegate void HpObserver(int _hp, GameObject _obj);
    public event HpObserver HpObserverEvent;

    // ���������� ���� �̵��ӵ� ��� ���� ������
    public delegate void MoveSpeedObserver(float _moveSpeed, GameObject _obj);
    public event MoveSpeedObserver MoveSpeedObserverEvent;

    // ���������� ���� �̵��ӵ� ��� ���� ������
    public delegate void AttackSpeedObserver(float _attackSpeed, GameObject _obj);
    public event AttackSpeedObserver AttackSpeedObserverEvent;

    // ���� ���ݷ�
    public delegate void AttackPointObserver(int _attackPoint, GameObject _obg);
    public event AttackPointObserver AttackPointObserverEvent;

    // �⺻ �߻�ü ������
    public delegate void ProjectileCountObserver(int _count, GameObject _obg);
    public event ProjectileCountObserver ProjectileCountObserverEvent;

    // �⺻ �߻�ü ���� ������
    public delegate void ProjectileScaleObserver(float _scale, GameObject _obg);
    public event ProjectileScaleObserver ProjectileScaleObserverEvent;

    // �⺻ ���� �����ð�
    public delegate void StiffTimeObserver(float _time, GameObject _obg);
    public event StiffTimeObserver StiffTimeObserverEvent;

    // �⺻ ���� Ƚ��
    public delegate void RAttackCountObserver(int _count, GameObject _obg);
    public event RAttackCountObserver RAttackCountObserverEvent;

    // �⺻ ���� �����
    public delegate void PassCountObserver(int _count, GameObject _obg);
    public event PassCountObserver PassCountObserverEvent;

    // HP
    public virtual void registerHpObserver(HpObserver _obs)
    {
        //HpObserverEvent�� null�̿��� -���꿡���� ������ �߻����� ����
        HpObserverEvent -= _obs;
        HpObserverEvent += _obs;
    }
    public virtual void UnRegisterHpObserver(HpObserver _obs)
    {
        HpObserverEvent -= _obs;
    }
    public virtual void ChangeHp(int _hp, GameObject _obj)
    {
        HpObserverEvent?.Invoke(_hp, _obj);
    }

    // MoveSpeed
    public virtual void registerMoveSpeedObserver(MoveSpeedObserver _obs)
    {
        MoveSpeedObserverEvent -= _obs;
        MoveSpeedObserverEvent += _obs;
    }
    public virtual void UnRegisterMoveSpeedObserver(MoveSpeedObserver _obs)
    {
        MoveSpeedObserverEvent -= _obs;
    }
    public virtual void ChangeMoveSpeed(float _moveSpeed, GameObject _obj)
    {
        MoveSpeedObserverEvent?.Invoke(_moveSpeed, _obj);
    }

    // AttackSpeed
    public virtual void registerAttackSpeedObserver(AttackSpeedObserver _obs)
    {
        AttackSpeedObserverEvent -= _obs;
        AttackSpeedObserverEvent += _obs;
    }
    public virtual void UnRegisterAttackSpeedObserver(AttackSpeedObserver _obs)
    {
        AttackSpeedObserverEvent -= _obs;
    }
    public virtual void ChangeAttackSpeed(float _attackSpeed, GameObject _obj)
    {
        AttackSpeedObserverEvent?.Invoke(_attackSpeed, _obj);
    }

    // AttackPoint
    public virtual void registerAttackPointObserver(AttackPointObserver _obs)
    {
        AttackPointObserverEvent -= _obs;
        AttackPointObserverEvent += _obs;
    }
    public virtual void UnRegisterAttackPointObserver(AttackPointObserver _obs)
    {
        AttackPointObserverEvent -= _obs;
    }
    public virtual void ChangeAttackPoint(int _attackPoint, GameObject _obj)
    {
        AttackPointObserverEvent?.Invoke(_attackPoint, _obj);
    }

    // ProjectileCount
    public virtual void registerProjectileCountObserver(ProjectileCountObserver _obs)
    {
        ProjectileCountObserverEvent -= _obs;
        ProjectileCountObserverEvent += _obs;
    }
    public virtual void UnRegisterProjectileCountObserver(ProjectileCountObserver _obs)
    {
        ProjectileCountObserverEvent -= _obs;
    }
    public virtual void ChangeProjectileCount(int _count, GameObject _obj)
    {
        ProjectileCountObserverEvent?.Invoke(_count, _obj);
    }

    // ProjectileScale
    public virtual void registerProjectileScaleObserver(ProjectileScaleObserver _obs)
    {
        ProjectileScaleObserverEvent -= _obs;
        ProjectileScaleObserverEvent += _obs;
    }
    public virtual void UnRegisterProjectileScaleObserver(ProjectileScaleObserver _obs)
    {
        ProjectileScaleObserverEvent -= _obs;
    }
    public virtual void ChangeProjectileScale(float _scale, GameObject _obj)
    {
        ProjectileScaleObserverEvent?.Invoke(_scale, _obj);
    }

    // StiffTime
    public virtual void registerStiffTimeObserver(StiffTimeObserver _obs)
    {
        StiffTimeObserverEvent -= _obs;
        StiffTimeObserverEvent += _obs;
    }
    public virtual void UnRegisterStiffTimeObserver(StiffTimeObserver _obs)
    {
        StiffTimeObserverEvent -= _obs;
    }
    public virtual void ChangeStiffTime(float _time, GameObject _obj)
    {
        StiffTimeObserverEvent?.Invoke(_time, _obj);
    }

    // RAttackCount
    public virtual void registerRAttackCountObserver(RAttackCountObserver _obs)
    {
        RAttackCountObserverEvent -= _obs;
        RAttackCountObserverEvent += _obs;
    }
    public virtual void UnRegisterRAttackCountObserver(RAttackCountObserver _obs)
    {
        RAttackCountObserverEvent -= _obs;
    }
    public virtual void ChangeRAttackCount(int _count, GameObject _obj)
    {
        RAttackCountObserverEvent?.Invoke(_count, _obj);
    }

    // PassCount
    public virtual void registerPassCountObserver(PassCountObserver _obs)
    {
        PassCountObserverEvent -= _obs;
        PassCountObserverEvent += _obs;
    }
    public virtual void UnRegisterPassCountObserver(PassCountObserver _obs)
    {
        PassCountObserverEvent -= _obs;
    }
    public virtual void ChangePassCount(int _count, GameObject _obj)
    {
        PassCountObserverEvent?.Invoke(_count, _obj);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
