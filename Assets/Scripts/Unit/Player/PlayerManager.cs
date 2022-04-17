using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : SingleToneMaker<PlayerManager>
{
    // Start is called before the first frame update
    [SerializeField]
    private SpriteRenderer mWeaponSprite;

    // �÷��̾��� ��������Ʈ(�ڽ�Ƭ)
    [SerializeField]
    private List<SpriteRenderer> mCostumes;

    [SerializeField]
    private Text mTestCostumeRankText;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public SpriteRenderer getPlayerWeaponSprite()
    {
        return mWeaponSprite;
    }

    public SpriteRenderer GetPlayerCostumeSpriteRenderer(int _num)
    {
        return mCostumes[_num];
    }

    public void SettingGameStart()
    {
        GameObject.Find("PlayerObject").GetComponent<PlayerAttack>().getProjectiles();
        string weaponType = GameObject.Find("PlayerObject").GetComponent<PlayerStatus>().PlayerCurrentWeapon.GetComponent<Weapon>().Spec.Type;
        weaponType = weaponType.Substring(0, 2);
        string costumeName = "";
        float chance = 0.5f;
        foreach(string cName in EquipmentManager.Instance.Costumes.Keys)
        {
            if (cName.Replace("cst", "").Contains(weaponType))
            {
                int random = Random.Range(0, 100);
                if( random < 50)
                {
                    costumeName = cName;
                    break;
                }
                else
                {
                    chance *= 0.5f;
                }
                Debug.Log(cName);
            }
        }
        if(costumeName == "")
        {
            costumeName = "cstbase";
            mTestCostumeRankText.text = Mathf.Round(chance * 10000f) * 0.01f + "%�� Ȯ���� �⺻ �ڽ�Ƭ ��÷!!";
        }
        else
        {
            mTestCostumeRankText.text = Mathf.Round(chance * 10000f) * 0.01f + "%�� Ȯ���� " + costumeName.Substring(8, 1) + "�ܰ��� �ڽ�Ƭ ��÷!!";
        }
        EquipmentManager.Instance.ChangeCostume(costumeName);
        
    }
}
