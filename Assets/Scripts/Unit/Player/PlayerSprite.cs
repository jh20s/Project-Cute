using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerSprite : MonoBehaviour
{
    [SerializeField]
    private List<SpriteRenderer> mSkinSpriteRenderer = new List<SpriteRenderer>();
    [SerializeField]
    private List<SpriteRenderer> mEyesSpriteRenderer = new List<SpriteRenderer>();
    [SerializeField]
    private SpriteRenderer mFaceHairSpriteRenderer;
    [SerializeField]
    private SpriteRenderer mHairSpriteRenderer;

    [SerializeField]
    private SpriteRenderer mHelmetSpriteRenderer;
    [SerializeField]
    private bool mShowHelmet;
    public bool ShowHelmet => mShowHelmet;
    [SerializeField]
    private GameObject mShowHelmetButton;

    [SerializeField]
    private int[] mSaveInfo;
    public int[] SaveInfo 
    {
        get => mSaveInfo;
        set
        {
            mSaveInfo = value;
        }
    }
    [SerializeField]
    private Color[] mSaveColorInfo;
    public Color[] SaveColorInfo
    {
        get => mSaveColorInfo;
        set
        {
            mSaveColorInfo = value;
        }
    }

    private const string SPRITE_NUM = "Sprite_";
    public string SpriteKey => SPRITE_NUM;
    private const string SPRITE_COLOR_NUM = "Sprite_Color_";
    public string ColorKey => SPRITE_COLOR_NUM;
    private const string SHOW_KEY = "Show_";
    public string ShowKey => SHOW_KEY;
    private void Start()
    {
        mSaveInfo = new int[] { 0, 0, 0, 0 };
        mSaveColorInfo = new Color[4];
        // Skin
        {
            mSkinSpriteRenderer.Add(GameObject.Find("20_L_Arm").GetComponent<SpriteRenderer>());
            mSkinSpriteRenderer.Add(GameObject.Find("-20_R_Arm").GetComponent<SpriteRenderer>());
            mSkinSpriteRenderer.Add(GameObject.Find("Body").GetComponent<SpriteRenderer>());
            mSkinSpriteRenderer.Add(GameObject.Find("_3L_Foot").GetComponent<SpriteRenderer>());
            mSkinSpriteRenderer.Add(GameObject.Find("_12R_Foot").GetComponent<SpriteRenderer>());
            mSkinSpriteRenderer.Add(GameObject.Find("5_Head").GetComponent<SpriteRenderer>());
        }
        // Eyes
        {
            mEyesSpriteRenderer.Add(GameObject.Find("P_REye")
                .transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>());
            mEyesSpriteRenderer.Add(GameObject.Find("P_REye")
                .transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>());
            mEyesSpriteRenderer.Add(GameObject.Find("P_LEye")
                .transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>());
            mEyesSpriteRenderer.Add(GameObject.Find("P_LEye")
                .transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>());
        }
        // FaceHair
        mFaceHairSpriteRenderer = GameObject.Find("6_FaceHair").GetComponent<SpriteRenderer>();
        // Hair
        mHairSpriteRenderer = GameObject.Find("7_Hair").GetComponent<SpriteRenderer>();
        // Helmet
        mHelmetSpriteRenderer = GameObject.Find("11_Helmet1").GetComponent<SpriteRenderer>();

        CustomizingManager.Instance.LoadCostumeInfo();
    }
    // 저장되어있는 커스터마이징 정보 입히기
    public void InitSprite()
    {
        for (int i = 0; i < mSaveInfo.Length; i++)
            if(PlayerPrefs.HasKey(SpriteKey + i))
                mSaveInfo[i] = PlayerPrefs.GetInt(SpriteKey + i);

        for (int i = 0; i < mSaveColorInfo.Length; i++)
        {
            if (PlayerPrefs.HasKey(ColorKey + i + "r"))
                mSaveColorInfo[i].r = PlayerPrefs.GetFloat(ColorKey + i + "r");
            if (PlayerPrefs.HasKey(ColorKey + i + "g"))
                mSaveColorInfo[i].g = PlayerPrefs.GetFloat(ColorKey + i + "g");
            if (PlayerPrefs.HasKey(ColorKey + i + "b"))
                mSaveColorInfo[i].b = PlayerPrefs.GetFloat(ColorKey + i + "b");
            mSaveColorInfo[i].a = 1f;
        }
        if (PlayerPrefs.HasKey(ShowKey))
            mShowHelmet = bool.Parse(PlayerPrefs.GetString(ShowKey));
        else
            mShowHelmet = false;
    }

    // Sprite 변경
    public void SetHair(Sprite _sprite)
    {
        mHairSpriteRenderer.sprite = _sprite;
    }
    public void SetFaceHair(Sprite _sprite)
    {
        mFaceHairSpriteRenderer.sprite = _sprite;
    }
    public void SetSkin(List<Sprite> _sprite)
    {
        for(int i = 0; i < mSkinSpriteRenderer.Count; i++)
        {
            mSkinSpriteRenderer[i].sprite = _sprite[i];
        }
    }
    public void SetEye(Sprite _back, Sprite _front)
    {
        for (int i = 0; i < mEyesSpriteRenderer.Count; i++)
        {
            if(i % 2 == 0)
                mEyesSpriteRenderer[i].sprite = _back;
            else
                mEyesSpriteRenderer[i].sprite = _front;
        }
    }

    // Color변경
    public void SetHairColor(Color _color)
    {
        mHairSpriteRenderer.color = _color;
    }
    public void SetFaceHairColor(Color _color)
    {
        mFaceHairSpriteRenderer.color = _color;
    }
    public void SetEyeColor(Color _color)
    {
        for (int i = 0; i < mEyesSpriteRenderer.Count; i++)
        {
            if (i % 2 == 1)
                mEyesSpriteRenderer[i].color = _color;
        }
    }

    // 투명모자
    public void ChangeHairHelmet()
    {
        if(mHelmetSpriteRenderer.sprite != null)
        {
            mHairSpriteRenderer.enabled = mShowHelmet;
            mHelmetSpriteRenderer.enabled = !mShowHelmet;
        }

    }
    public void ClickShowHelmet()
    {
        mShowHelmet = !mShowHelmet;
        ChangeHairHelmet();
        if (mShowHelmetButton != null)
            mShowHelmetButton.transform.GetChild(0).GetChild(0).gameObject.SetActive(mShowHelmet);
    }
}
