using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMove : MonoBehaviour
{
    [SerializeField]
    protected float mSpeed;
    public float Speed
    {
        get { return mSpeed; }
    }
    [SerializeField]
    protected Vector3 mDir;
    [SerializeField]
    protected bool mMoveable = true;
    public bool MMoveable
    {
        get { return mMoveable; }
        set { mMoveable = value; }
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        mDir = new Vector3();
        gameObject.GetComponent<IEventHandler>().registerMoveSpeedObserver(RegisterMoveSpeedObserver);
    }

    private void RegisterMoveSpeedObserver(float _moveSpeed, GameObject _obj)
    {
        mSpeed = _moveSpeed;
    }
    protected virtual void UpdateMove() { }

    public virtual void StopStiffTime(float _time)
    {
        if (mMoveable && gameObject.activeInHierarchy)
        {
            StartCoroutine(CoStopStiffTime(_time));
        }
    }
    /*
     * �̵��� ������ �ɸ��� api�Դϴ�.
     *  _time : �����Ǵ� �ð��Դϴ�.
     */
    public IEnumerator CoStopStiffTime(float _time)
    {
        if (mMoveable)
        {
            mMoveable = false;
            Debug.Log("���� ��ٸ��� ��..");
            yield return new WaitForSeconds(_time);
            mMoveable = true;
        }
        else
            mMoveable = true;
    }
}
