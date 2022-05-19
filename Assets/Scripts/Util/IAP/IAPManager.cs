using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Events;

public class IAPManager : MonoBehaviour
{
    [SerializeField]
    private IAPButton mDiamond_20;
    [SerializeField]
    private IAPButton mDiamond_110;
    [SerializeField]
    private IAPButton mAdsPass;
    [SerializeField]
    private LobbyPlayerInfo mPlayer;
    [SerializeField]
    private GameObject mPassPannel;
    // Start is called before the first frame update
    void Start()
    {
        mPlayer = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info;
        // 다이아20팩 구매성공 시
        mDiamond_20.onPurchaseComplete.AddListener(new UnityAction<Product>((product) =>
        {
            mPlayer.Diamond += 20;
            LobbyUIManager.Instance.OpenAlertEnterPannel("구매에 성공하였습니다.");
        }));
        // 다이아20팩 구매실패 시
        mDiamond_20.onPurchaseFailed.AddListener(new UnityAction<Product, PurchaseFailureReason>((product, reason) =>
        {
            LobbyUIManager.Instance.OpenAlertEnterPannel("구매에 실패하였습니다.");
        }));
        // 다이아110팩 구매성공 시
        mDiamond_110.onPurchaseComplete.AddListener(new UnityAction<Product>((product) =>
        {
            mPlayer.Diamond += 110;
            LobbyUIManager.Instance.OpenAlertEnterPannel("구매에 성공하였습니다.");
        }));
        // 다이아110팩 구매실패 시
        mDiamond_110.onPurchaseFailed.AddListener(new UnityAction<Product, PurchaseFailureReason>((product, reason) =>
        {
            LobbyUIManager.Instance.OpenAlertEnterPannel("구매에 실패하였습니다.");
        }));
        // 광고 패스 구매성공 시
        mAdsPass.onPurchaseComplete.AddListener(new UnityAction<Product>((product) =>
        {
            mPlayer.IsAdsPass = true;
            mPassPannel.SetActive(true);
            LobbyUIManager.Instance.OpenAlertEnterPannel("구매에 성공하였습니다.");
        }));
        // 광고 패스 구매실패 시
        mAdsPass.onPurchaseFailed.AddListener(new UnityAction<Product, PurchaseFailureReason>((product, reason) =>
        {
            LobbyUIManager.Instance.OpenAlertEnterPannel("구매에 실패하였습니다.");
        }));
    }

}
