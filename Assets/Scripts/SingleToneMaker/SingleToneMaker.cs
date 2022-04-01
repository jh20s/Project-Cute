using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleToneMaker<T> : MonoBehaviour where T : class
{
    private static readonly object _padLock = new object();
    private static T instance = null;
    public static T Instance
    {
        get
        {
            lock (_padLock)
            {
                if (instance == null)
                {
                    //�̱��� ������Ʈ�� 1���� �־���ϱ⿡ �������� ���ӿ�����Ʈ�� ����ִٸ� �� ��ü�� �̹� �̱����� �ƴ�
                    instance = GameObject.FindObjectOfType(typeof(T)) as T;
                    if (instance == null)
                    {
                        instance = new GameObject(typeof(T).ToString()).AddComponent(typeof(T)) as T;
                    }
                }
                return instance;
            }
        }
    }

}
