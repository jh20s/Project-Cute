using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEventHandler : MonoBehaviour
{
    /*
     * TO-DO : 이벤트핸들러를 제너릭하게 짤수있는 방법이 있어보인다 delegate를 각 이벤트에대해 일일히 만들어주는 방식이라 중복되는 코드가 너무많다.
     *         해결방법을 찾아보자./
     * 참고 https://docs.microsoft.com/ko-kr/dotnet/csharp/programming-guide/events/how-to-publish-events-that-conform-to-net-framework-guidelines
     */


    public delegate void HpObserver(int _hp, GameObject _obj);
    public event HpObserver HpObserverEvent;

    public delegate void DieObserver(bool _isDie, GameObject _obj);
    public event DieObserver DieObserverEvent;

    // 아이템으로 인한 이동속도 계수 변경 옵저버
    public delegate void MoveSpeedObserver(float _moveSpeed, GameObject _obj);
    public event MoveSpeedObserver MoveSpeedObserverEvent;

    // 아이템으로 인한 이동속도 계수 변경 옵저버
    public delegate void AttackSpeedObserver(float _attackSpeed, GameObject _obj);
    public event AttackSpeedObserver AttackSpeedObserverEvent;

    // 최종 공격력
    public delegate void AttackPointObserver(int _attackPoint, GameObject _obg);
    public event AttackPointObserver AttackPointObserverEvent;

    // 기본 발사체 증가수
    public delegate void ProjectileCountObserver(int _count, GameObject _obg);
    public event ProjectileCountObserver ProjectileCountObserverEvent;

    // 기본 발사체 범위 증가율
    public delegate void ProjectileScaleObserver(float _scale, GameObject _obg);
    public event ProjectileScaleObserver ProjectileScaleObserverEvent;

    // 기본 공격 경직시간
    public delegate void StiffTimeObserver(float _time, GameObject _obg);
    public event StiffTimeObserver StiffTimeObserverEvent;

    // 기본 공격 횟수
    public delegate void RAttackCountObserver(int _count, GameObject _obg);
    public event RAttackCountObserver RAttackCountObserverEvent;

    // 기본 공격 관통수
    public delegate void PassCountObserver(int _count, GameObject _obg);
    public event PassCountObserver PassCountObserverEvent;

    //발사체 발사중인지 확인(LaunchCorutines 실행중이면 true, 끝나면 false)
    public delegate void IsLaunchObserver(bool _state, GameObject _obg);
    public event IsLaunchObserver IsLaunchObserverEvent;

    // HP
    public virtual void registerHpObserver(HpObserver _obs)
    {
        //HpObserverEvent는 null이여도 -연산에대해 에러가 발생하지 않음
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
        if(_hp<=0 && !gameObject.GetComponent<IStatus>().IsDie)
        {
            gameObject.GetComponent<IStatus>().IsDie = true;
        }
    }

    public virtual void registerIsDieObserver(DieObserver _obs)
    {
        //HpObserverEvent는 null이여도 -연산에대해 에러가 발생하지 않음
        DieObserverEvent -= _obs;
        DieObserverEvent += _obs;
    }
    public virtual void UnRegisterIsDieObserver(DieObserver _obs)
    {
        DieObserverEvent -= _obs;
    }
    public virtual void ChangeIsDie(bool _dieCheck, GameObject _obj)
    {
        DieObserverEvent?.Invoke(_dieCheck, _obj);
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

    // IsLaunch
    public virtual void registerIsLaunchObserver(IsLaunchObserver _obs)
    {
        IsLaunchObserverEvent -= _obs;
        IsLaunchObserverEvent += _obs;
    }
    public virtual void UnregisterIsLaunchObserver(IsLaunchObserver _obs)
    {
        IsLaunchObserverEvent -= _obs;
    }
    public virtual void ChangeIsLaunch(bool _state, GameObject _obj)
    {
        IsLaunchObserverEvent?.Invoke(_state, _obj);
    }

}
