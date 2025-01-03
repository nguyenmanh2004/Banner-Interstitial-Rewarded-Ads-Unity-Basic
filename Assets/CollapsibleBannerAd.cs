using UnityEngine;
using System;
using GoogleMobileAds.Api;

public class CollapsibleBannerAd : MonoBehaviour
{
    private BannerView _bannerView;
    private string _adUnitId;

    // Khởi chạy ứng dụng
    void Start()
    {
        
    }
    public void showad()
    {
        // Đặt mã đơn vị quảng cáo (ad unit ID) theo nền tảng
#if UNITY_ANDROID
        _adUnitId = "ca-app-pub-9273469039395735/2469808759"; // Thay bằng ID quảng cáo Android của bạn
#elif UNITY_IPHONE
        _adUnitId = "ca-app-pub-xxxxxxxxxxxxxxxx/xxxxxxxxxx"; // Thay bằng ID quảng cáo iOS của bạn
#else
        _adUnitId = "unexpected_platform";
#endif

        // Khởi tạo Google Mobile Ads SDK
        MobileAds.Initialize((InitializationStatus status) =>
        {
            Debug.Log("Mobile Ads SDK Initialized");
            LoadCollapsibleBannerAd();
        });
    }
    // Tải quảng cáo biểu ngữ có thể thu gọn
    private void LoadCollapsibleBannerAd()
    {
        // Tạo BannerView
        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Tạo AdRequest với tham số bổ sung `collapsible`
        AdRequest adRequest = new AdRequest();
        adRequest.Extras.Add("collapsible", "bottom");

        // Đăng ký sự kiện để theo dõi trạng thái
        _bannerView.OnBannerAdLoaded += OnBannerAdLoaded;
        _bannerView.OnBannerAdLoadFailed += OnBannerAdFailedToLoad;

        // Tải quảng cáo
        _bannerView.LoadAd(adRequest);
        Debug.Log("Loading collapsible banner ad...");
    }

  

    // Xử lý khi quảng cáo được tải thành công
    private void OnBannerAdLoaded()
    {
        Debug.Log("Banner ad loaded successfully.");

        // Kiểm tra xem quảng cáo có thể thu gọn hay không
        if (_bannerView.IsCollapsible())
        {
            Debug.Log("Banner is collapsible.");
        }
        else
        {
            Debug.Log("Banner is not collapsible.");
        }
    }

    // Xử lý khi quảng cáo không tải được
    private void OnBannerAdFailedToLoad(LoadAdError error)
    {
        Debug.LogError("Failed to load banner ad: " + error.GetMessage());
    }

    // Dọn dẹp tài nguyên khi đối tượng bị hủy
    void OnDestroy()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }
    }
}

