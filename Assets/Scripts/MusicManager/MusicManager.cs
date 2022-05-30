using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : SingleToneMaker<MusicManager>
{
    private bool DEBUG = false;
    [SerializeField]
    private GameObject mBackgroundMusic;
    /*
     * 초원맵 - 전투 BGM(오락실버전2) 
     * 설원맵 - 전투 BGM(설원맵전용) 
     * 화산맵 - 전투 BGM(오락실버전1) 
     * 던전맵 - 타이틀 BGM(오락실버전2)
     */

    private AudioSource mAudioSource;
    // UI 소리를 출력해주는 소스
    [SerializeField]
    private AudioSource mUISource;
    //projectile명을 통해 SoundData검색
    private Dictionary<string, SoundData> mAudioClipData;
    
    public float mAmplificationScale;
    public struct SoundData
    {
        public AudioClip soundName;
        public float volume;
    }
    [SerializeField]
    private GameObject mBGMOn;
    [SerializeField]
    private GameObject mBGMOff;
    [SerializeField]
    private GameObject mEFMOn;
    [SerializeField]
    private GameObject mEFMOff;

    // UI Sound Data
    [SerializeField]
    private UIAudoiData mUISoundData;

    private void Awake()
    {
        mBackgroundMusic = GameObject.Find("BackgroundMusic");
    }
    
    void Start()
    {
        mAmplificationScale = 1f;
        mAudioClipData = new Dictionary<string, SoundData>();
        mAudioSource = GetComponent<AudioSource>();
        //sound 최적화
        mAudioSource.dopplerLevel = 0f;
        mAudioSource.reverbZoneMix = 0f;
        LoadSoundEffect();
    }
    // UI Sound 출력해주는 API
    public void ClickUISound(LobbyMusicManager.AudioType _type)
    {
        if (!mUISoundData.ContainsKey(_type))
            return;
        mUISource.PlayOneShot(mUISoundData[_type]);
    }

    //mAudioClipData에 _name으로 되어있는 soundEffect 재생
    public void OneShotProjectileSound(string _name)
    {
        if (!mAudioClipData.ContainsKey(_name))
        {
            if(DEBUG)
                Debug.Log(_name + "으로된 clip은 설정되어있지 않습니다");
            return;
        }
        if(DEBUG)
            Debug.Log("projectile명 :" + _name + ",재생할 클립명 : " + mAudioClipData[_name].soundName);



        mAudioSource.volume = mAudioClipData[_name].volume * mAmplificationScale;
        mAudioSource.PlayOneShot(mAudioClipData[_name].soundName);
        StartCoroutine(SoundAmplificationDecrease());
    }

    //맵에 설정된 background음악을 play
    public void OnBackgroundMusic()
    {
        mBackgroundMusic.GetComponent<AudioSource>().clip
                    = Resources.Load("Sound\\BGM\\" + MapManager.Instance.CurrentMapType.ToString()) as AudioClip;
        if(DEBUG)   
            Debug.Log("음악 on: " +mBackgroundMusic.GetComponent<AudioSource>().clip.name);
        mBackgroundMusic.GetComponent<AudioSource>().Play();
        mBackgroundMusic.GetComponent<AudioSource>().volume = 0.3f;
        mBackgroundMusic.GetComponent<AudioSource>().loop = true;
    }

    //sound effect load
    private void LoadSoundEffect()
    {
        List<Dictionary<string, object>> soundData = CSVReader.Read("CSVFile\\SoundEffect");

        //AudioClip 받아와 딕셔너리화
        AudioClip[] soundEffectsList = Resources.LoadAll<AudioClip>("Sound\\Effect");

        Dictionary<string, AudioClip> soundDict = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in soundEffectsList)
        {
            soundDict[clip.name]=  clip;
        }


        for (int idx = 0; idx < soundData.Count; idx++)
        {
            SoundData data = new SoundData();
            if (soundData[idx]["SoundName"].ToString().Equals("None"))
                continue;
            data.soundName = soundDict[soundData[idx]["SoundName"].ToString()];
            data.volume = float.Parse(soundData[idx]["Volume"].ToString());
            mAudioClipData[soundData[idx]["ProjectileName"].ToString()] = data;
        }
    }


    IEnumerator SoundAmplificationDecrease()
    {
        mAmplificationScale *= 0.9f;
        yield return new WaitForSeconds(0.3f);
        mAmplificationScale *= 10 / 9f;
    }

    public void MuteBGM(bool _mute)
    {
        mBackgroundMusic.GetComponent<AudioSource>().mute = _mute;
        mBGMOn.transform.GetChild(0).gameObject.SetActive(!_mute);
        mBGMOff.transform.GetChild(0).gameObject.SetActive(_mute);
    }

    public void MuteEFM(bool _mute)
    {
        mAudioSource.mute = _mute;
        mUISource.mute = _mute;
        mEFMOn.transform.GetChild(0).gameObject.SetActive(!_mute);
        mEFMOff.transform.GetChild(0).gameObject.SetActive(_mute);
    }
}
