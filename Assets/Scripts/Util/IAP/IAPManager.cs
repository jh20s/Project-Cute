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
    private GameObject mPassPannel;
    // Start is called before the first frame update
    void Start()
    {
        // ���̾�20�� ���ż��� ��
        mDiamond_20.onPurchaseComplete.AddListener(new UnityAction<Product>((product) =>
        {
            StartCoroutine(BuyDiamondSuccess(20));
            LobbyUIManager.Instance.OpenAlertEnterPannel("���ſ� �����Ͽ����ϴ�.");
        }));
        // ���̾�20�� ���Ž��� ��
        mDiamond_20.onPurchaseFailed.AddListener(new UnityAction<Product, PurchaseFailureReason>((product, reason) =>
        {
            LobbyUIManager.Instance.OpenAlertEnterPannel("���ſ� �����Ͽ����ϴ�.");
        }));
        // ���̾�110�� ���ż��� ��
        mDiamond_110.onPurchaseComplete.AddListener(new UnityAction<Product>((product) =>
        {
            StartCoroutine(BuyDiamondSuccess(110));
            LobbyUIManager.Instance.OpenAlertEnterPannel("���ſ� �����Ͽ����ϴ�.");
        }));
        // ���̾�110�� ���Ž��� ��
        mDiamond_110.onPurchaseFailed.AddListener(new UnityAction<Product, PurchaseFailureReason>((product, reason) =>
        {
            LobbyUIManager.Instance.OpenAlertEnterPannel("���ſ� �����Ͽ����ϴ�.");
        }));
        // ���� �н� ���ż��� ��
        mAdsPass.onPurchaseComplete.AddListener(new UnityAction<Product>((product) =>
        {
            StartCoroutine(BuyAdsPassSuccess());
            LobbyUIManager.Instance.OpenAlertEnterPannel("���ſ� �����Ͽ����ϴ�.");
        }));
        // ���� �н� ���Ž��� ��
        mAdsPass.onPurchaseFailed.AddListener(new UnityAction<Product, PurchaseFailureReason>((product, reason) =>
        {
            LobbyUIManager.Instance.OpenAlertEnterPannel("���ſ� �����Ͽ����ϴ�.");
        }));
    }

    IEnumerator BuyDiamondSuccess(int _value)
    {
        yield return new WaitForEndOfFrame();
        GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.Diamond += _value;
    }
    IEnumerator BuyAdsPassSuccess()
    {
        yield return new WaitForEndOfFrame();
        if(!GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.IsAdsPass)
            GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.IsAdsPass = true;
        mPassPannel.SetActive(true);
    }
}
