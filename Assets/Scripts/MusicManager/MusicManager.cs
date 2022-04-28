using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private GameObject BackgroundMusic;
    [SerializeField]
    private AudioSource backmusic;
    private void Awake()
    {
        Debug.Log("MusicManager");
        BackgroundMusic = GameObject.Find("BackgroundMusic");
        backmusic = BackgroundMusic.GetComponent<AudioSource>(); //������� �����ص�
        Debug.Log(backmusic.isPlaying);

        if (backmusic.isPlaying) return; //��������� ����ǰ� �ִٸ� �н�
        else
        {
            Debug.Log("���� ����");
            backmusic.Play();
            DontDestroyOnLoad(BackgroundMusic); //������� ��� ����ϰ�(���� ��ư�Ŵ������� ����)
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
