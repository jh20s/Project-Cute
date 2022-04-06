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
    private int currentUseCount = 0;
    public int CurrentUseCount
    {
        get { return currentUseCount; }
        set { currentUseCount = value; }
    }
    private int currentCoolTimeIndex = 0;
    public int CurrentCoolTimeIndex
    {
        get { return currentCoolTimeIndex; }
        set { currentCoolTimeIndex = value; }
    }
    #endregion

    #region method
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
}