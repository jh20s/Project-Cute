using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MessageBoxManager : SingleToneMaker<MessageBoxManager>
{
    #region variables
    // �޼����ڽ� Ÿ�Ժ� ������Ʈ Dic�Դϴ�.
    [SerializeField]
    private StringGameObject messageTypes = new StringGameObject();

    public enum BoxType
    {
        PlayerDamage,
        MonsterDamage,
        CriticalDamage,
        PlayerCoin,
        PlayerHpPotion,
        BossWarning,
    }
    #endregion
    #region method

    // Start is called before the first frame update
    void Start()
    {
        // �Ŵ����� ��������� �����͸� �ҷ��ɴϴ�.
        initAllMessageBoxData();
        // ������ƮǮ�Ŵ����� ���Ͽ� �޽����ڽ����� ������ƮǮ�� ����
        addObjectPool();
    }

    private void initAllMessageBoxData()
    {
        // MessageBox �������� �ҷ��´�
        GameObject[] boxList = Resources.LoadAll<GameObject>("Prefabs\\MessageBox");
        // Dic�� �����صд�.
        foreach (GameObject item in boxList)
        {
            messageTypes.Add(item.name, item);
        }

        List<Dictionary<string, object>> messageData = CSVReader.Read("CSVFile\\MessageBox");

        for(int n = 0; n < messageTypes.Count; n++)
        {
            string name = messageData[n]["MB_Name"].ToString();
            MessageBox newMg = messageTypes[name].GetComponent<MessageBox>();
            string[] rgba = messageData[n]["MB_Color"].ToString().Split('/');
            newMg.Alpha = new Color(
                float.Parse(rgba[0])/255, float.Parse(rgba[1])/255, float.Parse(rgba[2])/255, float.Parse(rgba[3]));
            newMg.TextCom = newMg.GetComponent<TextMesh>();
            newMg.TextCom.color = newMg.Alpha;
            newMg.MoveSpeed = float.Parse(messageData[n]["MB_MoveSpeed"].ToString());
            newMg.AlphaSpeed = float.Parse(messageData[n]["MB_AlphaSpeed"].ToString());
            newMg.DestroyTime = float.Parse(messageData[n]["MB_DestroyTime"].ToString());
            newMg.FontSize = int.Parse(messageData[n]["MB_FontSize"].ToString());
            newMg.TextCom.fontSize = newMg.FontSize;
            //Debug.Log(name + newMg.Alpha);
        }
    }

    private void addObjectPool()
    {
        foreach(GameObject item in messageTypes.Values)
        {
            ObjectPoolManager.Instance.CreateDictTable(item, 100, 50);
        }
    }

    /*
     * �޽����ڽ��� �����ϴ� api�Դϴ�.
     *  _type : �޽����ڽ� Ÿ���Դϴ�.
     *  _desc : �޽����ڽ��� �� �����Դϴ�.
     *  _pos : �޽����ڽ��� ������ ��ġ ���Ͱ��Դϴ�.
     */
    public void createMessageBox(BoxType _type, string _desc, Vector3 _pos)
    {
        string typeName = _type.ToString();
        StartCoroutine(CreateMessageCoroutine(typeName, _desc, _pos));
    }
    IEnumerator CreateMessageCoroutine(string _typeName, string _desc, Vector3 _pos)
    {
        GameObject messageBox = ObjectPoolManager.Instance.EnableGameObject(_typeName);
        messageBox.GetComponent<MessageBox>().setEnable(_desc, _pos);
        yield return new WaitForSeconds(messageBox.GetComponent<MessageBox>().DestroyTime);
        messageBox.GetComponent<MessageBox>().setDisable();
        ObjectPoolManager.Instance.DisableGameObject(messageBox);
    }
    #endregion
}