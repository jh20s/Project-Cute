using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    /* ��𼭳� �ҷ��ü� �ְ� public
    * ���� ��
    *��Ʈ ũ��, ��Ʈ ����, ��Ʈ ����, ��Ÿ���ų� ������� Ÿ��, ������ �ؽ�Ʈ
    */
    #region variables
    // �������ڽ��� ǥ��� �ؽ�Ʈ ������Ʈ
    [SerializeField]
    private TextMesh textCom;
    public TextMesh TextCom
    {
        get { return textCom; }
        set { textCom = value; }
    }
    // �ؽ�Ʈ �÷�
    [SerializeField]
    private Color alpha;
    public Color Alpha
    {
        get { return alpha; }
        set {
            alpha = value; 
        }
    }
    // �ؽ�Ʈ�� ������ �ӵ�
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    // �ؽ�Ʈ�� ���İ� ���� �ӵ�
    [SerializeField]
    private float alphaSpeed;
    public float AlphaSpeed
    {
        get { return alphaSpeed; }
        set { alphaSpeed = value; }
    }
    // �ؽ�Ʈ�� �ı� �ӵ�
    [SerializeField]
    private float destroyTime;
    public float DestroyTime
    {
        get { return destroyTime; }
        set { destroyTime = value; }
    }
    // �ؽ�Ʈ�� ũ��
    [SerializeField]
    private int fontSize;
    public int FontSize
    {
        get { return fontSize; }
        set { fontSize = value; }
    }
    // �ؽ�Ʈ�ڽ� ���� ���� ����
    [SerializeField]
    private bool isStart = false;
    public bool IsStart
    {
        get { return isStart; }
        set { isStart = value; }
    }

    private Color newAlpha;
    // �ʱ� ���� ��ġ
    #endregion
    #region method
    private void LateUpdate()
    {
        // isStart�� true�� �Ǹ� ����˴ϴ�.
        if (isStart)
        {
            transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));

            newAlpha.a = Mathf.Lerp(newAlpha.a, 0, alphaSpeed * Time.deltaTime);
            textCom.color = newAlpha;

        }
    }
    public void setEnable(string _msg, Vector3 _pos)
    {
        newAlpha = alpha;
        textCom.text = _msg;
        transform.position = _pos;
        gameObject.SetActive(true);
        isStart = true;
    }

    public void setDisable()
    {
        isStart = false;
        textCom.color = alpha;
        gameObject.SetActive(false);
    }
    #endregion
}
