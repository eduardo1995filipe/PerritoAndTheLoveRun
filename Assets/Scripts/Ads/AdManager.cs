using GoogleMobileAds.Api;
using UnityEngine;

/// <summary>
/// Classe que seria para meter os banners(Admob) enquando o jogo decorria.
/// Nao foi possivel utiliza-la uma vez que os banners nao apareciam no jogo,
/// optei por criar ads intersticiais com as bibliotecas do unity
/// </summary>
public class AdManager : MonoBehaviour
{
    public static AdManager instance = null;

    private BannerView bannerView;

    void Start()
    {
        #if UNITY_EDITOR
                string appId = "unused";
        #elif UNITY_IPHONE
                string appId = "unused";
        #elif UNITY_ANDROID
                string appId = "ca-app-pub-8899468184876323~7791593488";
        #else
                string appId = "unexpected_platform";
        #endif
        
        MobileAds.Initialize(appId);

        print("Banner requerido!!!");
        RequestBanner();
        print("Banner requerido depois");
    }

    public void RequestBanner()
    {
        #if UNITY_EDITOR
                string adUnitId = "unused";
        #elif UNITY_IPHONE
                string adUnitId = "unused";
        #elif UNITY_ANDROID
                string adUnitId = "ca-app-pub-8899468184876323/5575250417"; 
        #else
                string adUnitId = "unexpected_platform";
        #endif
        
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();

        bannerView.LoadAd(request);
        bannerView.Show();
    }
}