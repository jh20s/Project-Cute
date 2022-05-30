using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    #region variables

    [SerializeField]
    private SkillSpec spec = new SkillSpec();
    public SkillSpec Spec
    {
        get { return spec; }
        set { spec = value; }
    }
    [SerializeField]
    private int currentUseCount;
    public int CurrentUseCount
    {
        get { return currentUseCount; }
        set { currentUseCount = value; }
    }
    [SerializeField]
    private int currentCoolTimeIndex;
    public int CurrentCoolTimeIndex
    {
        get { return currentCoolTimeIndex; }
        set { currentCoolTimeIndex = value; }
    }
    [SerializeField]
    private float mSkillBuffValue;
    public float SkillBuffValue
    {
        set
        {
            mSkillBuffValue = value;
        }
        get
        {
            return mSkillBuffValue;
        }
    }
    [SerializeField]
    private ESkillBuffType mSkillBuffType;
    public ESkillBuffType SkillBuffType
    {
        set
        {
            mSkillBuffType = value;
        }
        get
        {
            return mSkillBuffType;
        }
    }
    public enum ESkillBuffType
    {
        None,
        PlayerSPD,
        PlayerPosition,
        PlayerDash,
        PlayerWeaponSprite
    }

    #endregion

    #region method

    public void BuffOn(GameObject _obj)
    {
        _obj.GetComponent<IStatus>()
             .ChangeStatusForSkill(mSkillBuffType, mSkillBuffValue);
    }

    public void BuffOff(GameObject _obj)
    {
        _obj.GetComponent<IStatus>()
     .ChangeStatusForSkillOff(mSkillBuffType, mSkillBuffValue);
    }
    #endregion
}
