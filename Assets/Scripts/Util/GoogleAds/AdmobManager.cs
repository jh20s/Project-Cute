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

    // 보상형 광고가 정상적으로 시청이 끝났음을 알려주는 bool형 변수
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
    // 모바일 환경에서는 애드몹이 게임 쓰레드를 정지 후 애드몹을 실행시키는데
    // 이때, 게임 쓰레드가 정지된 상태에서 게임 쓰레드의 함수를 호출하여 보상을 주게되면
    // 크래시가 나서 앱이 비정상적으로 종료되므로, Update문에서 한프레임 쉬고 게임 쓰레드가
    // 정상적으로 돌아왔을 떄, 해당 API를 호출
    // 아래 함수는 게임의 쓰레드가 정상적으로 돌아왔을 때  재 실행
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
    // 모바일 환경에서 한프레임 기다린후 실행하는 코루틴 API
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

    //오브젝트 참조해서 불러줄 함수
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

    //광고가 로드되었을 때
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {

    }
    //광고 로드에 실패했을 때
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        LobbyUIManager.Instance.OpenAlertEnterPannel("인터넷 연결이 필요합니다.");
    }
    //광고 보여주기를 실패했을 때
    public void HandleOnAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        LobbyUIManager.Instance.OpenAlertEnterPannel("인터넷 연결이 필요합니다.");
    }
    //광고가 제대로 실행되었을 때
    public void HandleOnAdOpening(object sender, EventArgs args)
    {

    }
    //광고가 종료되었을 때
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        //새로운 광고 Load
        this.videoAd = ReloadAd();
    }
    //광고를 끝까지 시청하였을 때
    public void HandleOnUserEarnedReward(object sender, EventArgs args)
    {
        // 보상
        curVideoCompleteReward = true;
    }
}
