using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class GoogleMobileAdsDemoScript : MonoBehaviour
{
    private RewardedInterstitialAd rewardedInterstitialAd;

    // Mã đơn vị quảng cáo xen kẽ có tặng thưởng
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-9273469039395735/3986371728";  // Thay bằng mã quảng cáo của bạn
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/6978759866";  // Thay bằng mã quảng cáo của bạn
#else
    private string _adUnitId = "unused";
#endif

    public void Start()
    {
       
    }
    public void showad()
    {
        // Khởi tạo SDK quảng cáo khi ứng dụng bắt đầu
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("Google Mobile Ads SDK initialized.");
            LoadRewardedInterstitialAd();  // Tải quảng cáo xen kẽ có tặng thưởng
        });
    }
    /// <summary>
    /// Tải quảng cáo xen kẽ có tặng thưởng.
    /// </summary>
    public void LoadRewardedInterstitialAd()
    {
        // Xoá quảng cáo cũ trước khi tải quảng cáo mới
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Destroy();
            rewardedInterstitialAd = null;
        }

        Debug.Log("Loading the rewarded interstitial ad.");

        // Tạo yêu cầu quảng cáo
        var adRequest = new AdRequest();

        // Tải quảng cáo xen kẽ có tặng thưởng
        RewardedInterstitialAd.Load(_adUnitId, adRequest, (RewardedInterstitialAd ad, LoadAdError error) =>
        {
            // Nếu có lỗi, không tải được quảng cáo
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded interstitial ad failed to load: " + error);
                return;
            }

            Debug.Log("Rewarded interstitial ad loaded successfully.");
            rewardedInterstitialAd = ad;

            // Đăng ký các sự kiện cho quảng cáo
            RegisterEventHandlers(rewardedInterstitialAd);
        });
    }

    /// <summary>
    /// Hiển thị quảng cáo xen kẽ có tặng thưởng.
    /// </summary>
    public void ShowRewardedInterstitialAd()
    {
        const string rewardMsg = "User rewarded with {0} {1}";

        if (rewardedInterstitialAd != null && rewardedInterstitialAd.CanShowAd())
        {
            rewardedInterstitialAd.Show((Reward reward) =>
            {
                // Xử lý phần thưởng cho người dùng
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
                // Cung cấp phần thưởng cho người dùng
            });
        }
        else
        {
            Debug.Log("Rewarded interstitial ad is not ready.");
        }
    }

    /// <summary>
    /// Đăng ký các sự kiện cho quảng cáo xen kẽ có tặng thưởng.
    /// </summary>
    private void RegisterEventHandlers(RewardedInterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"Rewarded interstitial ad paid {adValue.Value} {adValue.CurrencyCode}");
        };

        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded interstitial ad recorded an impression.");
        };

        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded interstitial ad was clicked.");
        };

        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded interstitial ad full screen content opened.");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded interstitial ad full screen content closed.");
            // Tải lại quảng cáo để sẵn sàng cho lần hiển thị tiếp theo
            LoadRewardedInterstitialAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded interstitial ad failed to open full screen content: " + error);
            // Tải lại quảng cáo nếu thất bại
            LoadRewardedInterstitialAd();
        };
    }

    /// <summary>
    /// Xoá quảng cáo sau khi sử dụng.
    /// </summary>
    private void DestroyAd()
    {
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Destroy();
            rewardedInterstitialAd = null;
        }
    }
}
