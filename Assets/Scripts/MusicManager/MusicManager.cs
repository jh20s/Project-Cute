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
    private void Awake()
    {
        mBackgroundMusic = GameObject.Find("BackgroundMusic");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMusic()
    {
        mBackgroundMusic.GetComponent<AudioSource>().clip
                    = Resources.Load("Sound\\BGM\\" + MapManager.Instance.CurrentMapType.ToString()) as AudioClip;
        if(DEBUG)   
            Debug.Log("���� on: " +mBackgroundMusic.GetComponent<AudioSource>().clip.name);
        mBackgroundMusic.GetComponent<AudioSource>().Play();
        mBackgroundMusic.GetComponent<AudioSource>().loop = true;
    }
}
