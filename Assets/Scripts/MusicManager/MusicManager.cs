using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : SingleToneMaker<MusicManager>
{
    private bool DEBUG = false;
    [SerializeField]
    private GameObject mBackgroundMusic;
    /*
     * �ʿ��� - ���� BGM(�����ǹ���2) 
     * ������ - ���� BGM(����������) 
     * ȭ��� - ���� BGM(�����ǹ���1) 
     * ������ - Ÿ��Ʋ BGM(�����ǹ���2)
     */

    private AudioSource mAudioSource;

    //projectile���� ���� SoundData�˻�
    private Dictionary<string, SoundData> mAudioClipData;

    public struct SoundData
    {
        public AudioClip soundName;
        public float volume;
    }
    
    private void Awake()
    {
        mBackgroundMusic = GameObject.Find("BackgroundMusic");
    }
    
    void Start()
    {
        mAudioClipData = new Dictionary<string, SoundData>();
        mAudioSource = GetComponent<AudioSource>();
        //sound ����ȭ
        mAudioSource.dopplerLevel = 0f;
        mAudioSource.reverbZoneMix = 0f;
        LoadSoundEffect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //mAudioClipData�� _name���� �Ǿ��ִ� soundEffect ���
    public void OneShotProjectileSound(string _name)
    {
        if (!mAudioClipData.ContainsKey(_name))
        {
            if(DEBUG)
                Debug.Log(_name + "���ε� clip�� �����Ǿ����� �ʽ��ϴ�");
            return;
        }
        if(DEBUG)
            Debug.Log("projectile�� :" + _name + ",����� Ŭ���� : " + mAudioClipData[_name].soundName);
        mAudioSource.volume = mAudioClipData[_name].volume;
        mAudioSource.PlayOneShot(mAudioClipData[_name].soundName);

    }

    //�ʿ� ������ background������ play
    public void OnBackgroundMusic()
    {
        mBackgroundMusic.GetComponent<AudioSource>().clip
                    = Resources.Load("Sound\\BGM\\" + MapManager.Instance.CurrentMapType.ToString()) as AudioClip;
        if(DEBUG)   
            Debug.Log("���� on: " +mBackgroundMusic.GetComponent<AudioSource>().clip.name);
        mBackgroundMusic.GetComponent<AudioSource>().Play();
        mBackgroundMusic.GetComponent<AudioSource>().volume = 0.3f;
        mBackgroundMusic.GetComponent<AudioSource>().loop = true;
    }

    //sound effect load
    private void LoadSoundEffect()
    {
        List<Dictionary<string, object>> soundData = CSVReader.Read("CSVFile\\SoundEffect");

        //AudioClip �޾ƿ� ��ųʸ�ȭ
        AudioClip[] soundEffectsList = Resources.LoadAll<AudioClip>("Sound\\Effect");
        Debug.Log("���� ���� :"+ soundEffectsList.Length);

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
            Debug.Log(soundData[idx]["SoundName"].ToString());
            data.soundName = soundDict[soundData[idx]["SoundName"].ToString()];
            data.volume = float.Parse(soundData[idx]["Volume"].ToString());
            mAudioClipData[soundData[idx]["ProjectileName"].ToString()] = data;
        }
    }
}
