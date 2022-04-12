using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventHandler : IEventHandler
{
    public delegate void LevelObserver(int _level);
    public event LevelObserver LevelObserverEvent;


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






    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
