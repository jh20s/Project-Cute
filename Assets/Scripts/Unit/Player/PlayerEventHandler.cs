using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventHandler : IEventHandler
{
    public delegate void LevelObserver(int _level);

    //ExpObserver ���� Player���� ����ġ, �ִ����ġ�� �ٲ��� ȣ��ǹǷ� PlayerStatus�� ����ġ�� PlayerExp,PlayerMaxExp�� �����Ͽ� ����� ��
    public delegate void ExpObserver();
    public delegate void GoldObserver(int _gold);

    public event LevelObserver LevelObserverEvent;
    public event ExpObserver ExpObserverEvent;
    public event GoldObserver GoldObserverEvent;



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

    public virtual void ChangeExp()
    {
        ExpObserverEvent?.Invoke();
    }

    //Gold EventHadeler
    public virtual void registerGoldObserver(GoldObserver _obs)
    {
        GoldObserverEvent -= _obs;
        GoldObserverEvent += _obs;
    }
    public virtual void UnRegisterGoldObserver(GoldObserver _obs)
    {
        GoldObserverEvent -= _obs;
    }
    public virtual void ChangeGold(int _gold)
    {
        GoldObserverEvent?.Invoke(_gold);
    }

}
