using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDropItem : MonoBehaviour
{

    private void OnEnable()
    {
        gameObject.GetComponent<MonsterEventHandler>().registerIsDieObserver(registerMonsterDie);
    }

    public void registerMonsterDie(bool _die, GameObject _obj)
    {
        if (_die)
        {
            int rank = gameObject.GetComponent<MonsterStatus>().MonsterRank;
            if (!MonsterManager.Instance.GetMonsterData(gameObject.name, rank).monsterDrop.Equals("null")) { 

                string[] itemList = MonsterManager.Instance.GetMonsterData(gameObject.name, rank).monsterDrop.Split('/');
                int rateTotalSum = 0;
                int randSum = 0;
                int randNum;

                //랜덤 sum을 구함
                foreach (string item in itemList)
                {
                    int num = int.Parse(item);
                    rateTotalSum += ItemManager.Instance.GetItemData(num).dropRate;
                }
                randNum = Random.Range(0, rateTotalSum);


                //랜덤 sum에따른 아이템 active 
                foreach (string itemStr in itemList)
                {
                    int itemId = int.Parse(itemStr);
                    if (ItemManager.Instance.GetItemData(itemId).mustDrop ==1)
                        continue;
                    randSum += ItemManager.Instance.GetItemData(itemId).dropRate;
                    if (randNum <= randSum)
                    {
                        if(ItemManager.Instance.GetItemData(itemId).mustDrop == 0) { 
                            GameObject itemObj = ObjectPoolManager.Instance.EnableGameObject(ItemManager.Instance.GetItemData(itemId).itemInName);
                            setItemData(ref itemObj, itemId);
                            itemObj.transform.position = gameObject.transform.position;
                            itemObj.SetActive(true);
                        }
                        break;
                    }

                }

                //확정적 아이템 drop
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

                        //여러개 드랍시 가로, 세로로만 나오는것을 방지하기위해 랜덤한 위치에 드랍
                        if (CustomRayCastManager.Instance.NomarlizeMoveableWithRay(transform.position, x, y, 0.49f, 0.49f, false, ref pos))
                            itemObj.transform.position = pos;
                        else
                            itemObj.transform.position = transform.position;
                        itemObj.SetActive(true);
                    }
                }
            }
            
            //몬스터에 설정된 경험치 드랍
            PlayerManager.Instance.Player.GetComponent<PlayerStatus>().PlayerGetExp = MonsterManager.Instance.GetMonsterData(gameObject.name, rank).monsterExp;
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
