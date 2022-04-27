using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpStatusEventHandler : MonoBehaviour
{
    public delegate void IntObserver(int _value, LevelUpStatusManager.StatType _type);
    public event IntObserver IntObserverEvent;

    public void registerIntObserver(IntObserver _obs)
    {
        //HpObserverEvent�� null�̿��� -���꿡���� ������ �߻����� ����
        IntObserverEvent -= _obs;
        IntObserverEvent += _obs;
    }
    public void UnRegisterIntObserver(IntObserver _obs)
    {
        IntObserverEvent -= _obs;
    }
    public void ChangeValue(int _value, LevelUpStatusManager.StatType _type)
    {
        IntObserverEvent?.Invoke(_value, _type);
    }
}
