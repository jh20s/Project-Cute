using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMusicManager : MonoBehaviour
{
    [SerializeField]
    private GameObject BackgroundMusic;
    [SerializeField]
    private AudioSource backmusic;
    private void Awake()
    {
        BackgroundMusic = GameObject.Find("BackgroundMusic");
        backmusic = BackgroundMusic.GetComponent<AudioSource>(); //������� �����ص�

        if (backmusic.isPlaying) return; //��������� ����ǰ� �ִٸ� �н�
        else
        {
            backmusic.Play();
            DontDestroyOnLoad(BackgroundMusic); //������� ��� ����ϰ�(���� ��ư�Ŵ������� ����)
        }
    }
}
