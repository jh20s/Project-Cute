using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobManager : SingleToneMaker<AdmobManager>
{
    #region Event
    public delegate void EndRewardAdmob(bool _isEnd);
    public event EndRewardAdmob EndRewardAdmobEvent;

    public void registerEndRewardObserver(EndRewardAdmob _obs)
    {
        EndRewardAdmobEvent -= _obs;
        EndRewardAdmobEvent += _obs;
    }

    public void ChangeEndReward(bool _isEnd)
    {
        EndRewardAdmobEvent?.Invoke(_isEnd);
    }
    #endregion
    private RewardedAd videoAd;
    public static bool ShowAd = false;
    public enum AdType
    {
        Supply,
        Resurrection
    }
    private AdType currentType;
    public AdType CurrentType => currentType;
    string videoID;

    // ������ ���� ���������� ��û�� �������� �˷��ִ� bool�� ����
    [SerializeField]
    private bool curVideoCompleteReward = false;
    public void Start()
    {
#if UNITY_EDITOR
        videoID = "ca-app-pub-3940256099942544/5224354917";
#else
        videoID = "ca-app-pub-9332050250357378/8475019061";
#endif
        videoAd = new RewardedAd(videoID);
        Handle(videoAd);
        Load();
    }
    // ����� ȯ�濡���� �ֵ���� ���� �����带 ���� �� �ֵ���� �����Ű�µ�
    // �̶�, ���� �����尡 ������ ���¿��� ���� �������� �Լ��� ȣ���Ͽ� ������ �ְԵǸ�
    // ũ���ð� ���� ���� ������������ ����ǹǷ�, Update������ �������� ���� ���� �����尡
    // ���������� ���ƿ��� ��, �ش� API�� ȣ��
    // �Ʒ� �Լ��� ������ �����尡 ���������� ���ƿ��� ��  �� ����
#if UNITY_EDITOR
    private void Update()
    {
        StartCoroutine(WaitForReward());
    }
#else
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            StartCoroutine(WaitForReward());
        }
    }
#endif
    // ����� ȯ�濡�� �������� ��ٸ��� �����ϴ� �ڷ�ƾ API
    IEnumerator WaitForReward()
    {
        yield return new WaitForEndOfFrame();
        if (curVideoCompleteReward)
        {
            ChangeEndReward(curVideoCompleteReward);
            curVideoCompleteReward = false;
        }
    }
    private void Handle(RewardedAd videoAd)
    {
        videoAd.OnAdLoaded += HandleOnAdLoaded;
        videoAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        videoAd.OnAdFailedToShow += HandleOnAdFailedToShow;
        videoAd.OnAdOpening += HandleOnAdOpening;
        videoAd.OnAdClosed += HandleOnAdClosed;
        videoAd.OnUserEarnedReward += HandleOnUserEarnedReward;
    }
    private void OnDestroy()
    {
        videoAd.OnAdLoaded -= HandleOnAdLoaded;
        videoAd.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
        videoAd.OnAdFailedToShow -= HandleOnAdFailedToShow;
        videoAd.OnAdOpening -= HandleOnAdOpening;
        videoAd.OnAdClosed -= HandleOnAdClosed;
        videoAd.OnUserEarnedReward -= HandleOnUserEarnedReward;
    }
    private void Load()
    {
        AdRequest request = new AdRequest.Builder().Build();
        videoAd.LoadAd(request);
    }

    public RewardedAd ReloadAd()
    {
        RewardedAd videoAd = new RewardedAd(videoID);
        Handle(videoAd);
        AdRequest request = new AdRequest.Builder().Build();
        videoAd.LoadAd(request);
        return videoAd;
    }

    //������Ʈ �����ؼ� �ҷ��� �Լ�
    public void Show()
    {
        StartCoroutine("ShowRewardAd");
    }

    private IEnumerator ShowRewardAd()
    {
        while (!videoAd.IsLoaded())
        {
            yield return null;
        }
        videoAd.Show();
    }

    //���� �ε�Ǿ��� ��
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {

    }
    //���� �ε忡 �������� ��
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        LobbyUIManager.Instance.OpenAlertEnterPannel("���ͳ� ������ �ʿ��մϴ�.");
    }
    //���� �����ֱ⸦ �������� ��
    public void HandleOnAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        LobbyUIManager.Instance.OpenAlertEnterPannel("���ͳ� ������ �ʿ��մϴ�.");
    }
    //���� ����� ����Ǿ��� ��
    public void HandleOnAdOpening(object sender, EventArgs args)
    {

    }
    //���� ����Ǿ��� ��
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        //���ο� ���� Load
        this.videoAd = ReloadAd();
    }
    //���� ������ ��û�Ͽ��� ��
    public void HandleOnUserEarnedReward(object sender, EventArgs args)
    {
        // ����
        curVideoCompleteReward = true;
    }
}
