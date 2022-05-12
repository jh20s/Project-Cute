using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobManager : SingleToneMaker<AdmobManager>
{
    static bool isAdVideoLoaded = false;

    private RewardedAd videoAd;
    public static bool ShowAd = false;
    public enum AdType
    {
        Supply,
        Resurrection
    }
    public AdType currentType;
    string videoID;
    public void Start()
    {
        //Test ID : "ca-app-pub-3940256099942544/5224354917"
        //���� ID
        videoID = "ca-app-pub-3940256099942544/5224354917";
        videoAd = new RewardedAd(videoID);
        Handle(videoAd);
        Load();
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
    public void Show(AdType _type)
    {
        currentType = _type;
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

    }
    //���� �����ֱ⸦ �������� ��
    public void HandleOnAdFailedToShow(object sender, AdErrorEventArgs args)
    {

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
        //������ �� ���Դϴ�.
        switch (currentType)
        {
            case AdType.Supply:
                DeongunStartManager.Instance.DrawBuff();
                LobbyUIManager.Instance.OpenDoengunPannel();
                break;
            case AdType.Resurrection:
                break;
        }
    }
}
