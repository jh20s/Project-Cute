using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventHandler : IEventHandler
{
    public delegate void LevelObserver(int _level);
    public delegate void ExpObserver(int _exp);

    public event LevelObserver LevelObserverEvent;
    public event ExpObserver ExpObserverEvent;




    //Level EventHandler
    public virtual void registerLevelObserver(LevelObserver _obs)
    {
        //HpObserverEvent�� null�̿��� -���꿡���� ������ �߻����� ����
        LevelObserverEvent -= _obs;
        LevelObserverEvent += _obs;
    }
    public virtual void UnRegisterLevelObserver(LevelObserver _obs)
    {
        LevelObserverEvent -= _obs;
    }

    public virtual void ChangeLevel(int _level)
    {
        LevelObserverEvent?.Invoke(_level);
    }



    //Exp EventHandler
    public virtual void registerExpObserver(ExpObserver _obs)
    {
        //HpObserverEvent�� null�̿��� -���꿡���� ������ �߻����� ����
        ExpObserverEvent -= _obs;
        ExpObserverEvent += _obs;
    }
    public virtual void UnRegisterExpObserver(ExpObserver _obs)
    {
        ExpObserverEvent -= _obs;
    }

    public virtual void ChangeExp(int _exp)
    {
        ExpObserverEvent?.Invoke(_exp);
    }



}
