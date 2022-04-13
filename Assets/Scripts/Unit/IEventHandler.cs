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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
