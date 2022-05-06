using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDropItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        gameObject.GetComponent<MonsterEventHandler>().registerIsDieObserver(registerMonsterDie);
    }

    public void registerMonsterDie(bool _die, GameObject _obj)
    {
        if (_die)
        {
            if (!MonsterManager.Instance.GetMonsterData(gameObject.name).monsterDrop.Equals("null")) { 

                string[] itemList = MonsterManager.Instance.GetMonsterData(gameObject.name).monsterDrop.Split('/');
                int rateTotalSum = 0;
                int randSum = 0;
                int randNum;

                //���� sum�� ����
                foreach (string item in itemList)
                {
                    int num = int.Parse(item);
                    rateTotalSum += ItemManager.Instance.GetItemData(num).dropRate;
                }
                randNum = Random.Range(0, rateTotalSum);


                //���� sum������ ������ active 
                foreach (string itemStr in itemList)
                {
                    int itemId = int.Parse(itemStr);
                    if (ItemManager.Instance.GetItemData(itemId).mustDrop ==1)
                        continue;
                    randSum += ItemManager.Instance.GetItemData(itemId).dropRate;
                    if (randNum <= randSum)
                    {
                        GameObject itemObj = ObjectPoolManager.Instance.EnableGameObject(ItemManager.Instance.GetItemData(itemId).itemInName);
                        setItemData(ref itemObj, itemId);
                        itemObj.transform.position = gameObject.transform.position;
                        itemObj.SetActive(true);
                        break;
                    }

                }

                //Ȯ���� ������ drop
                foreach (string itemStr in itemList)
                {
                    int itemId = int.Parse(itemStr);
                    if (ItemManager.Instance.GetItemData(itemId).mustDrop == 1)
                    {
                        GameObject itemObj = ObjectPoolManager.Instance.EnableGameObject(ItemManager.Instance.GetItemData(itemId).itemInName);
                        setItemData(ref itemObj, itemId);
                        int x = Random.Range(0, 6)-3;
                        int y = Random.Range(0, 6) - 3;
                        Vector3 pos = gameObject.transform.position;

                        CustomRayCastManager.Instance.NomarlizeMoveableWithRay(transform.position, x,y, 0.5f, 0.49f, true, ref pos);
                        //������ ����� ����, ���ηθ� �����°��� �����ϱ����� ������ ��ġ�� ���
                        if (CustomRayCastManager.Instance.NomarlizeMoveableWithRay(transform.position, x, y, 0.5f, 0.49f, true, ref pos))
                            itemObj.transform.position = pos;
                        else
                            itemObj.transform.position = transform.position;
                        itemObj.SetActive(true);
                    }
                }
            }
            
            //���Ϳ� ������ ����ġ ���
            PlayerManager.Instance.Player.GetComponent<PlayerStatus>().PlayerGetExp = MonsterManager.Instance.GetMonsterData(gameObject.name).monsterExp;
        }
    }

    private void setItemData(ref GameObject _item, int _id)
    {
        _item.GetComponent<Item>().Hp = ItemManager.Instance.GetItemData(_id).hp;
        _item.GetComponent<Item>().Gold = ItemManager.Instance.GetItemData(_id).gold;
        _item.GetComponent<Item>().Damage = ItemManager.Instance.GetItemData(_id).damage;
        _item.GetComponent<Item>().Scale = ItemManager.Instance.GetItemData(_id).scale;
        _item.GetComponent<Item>().MustDrop = ItemManager.Instance.GetItemData(_id).mustDrop;
    }
}
