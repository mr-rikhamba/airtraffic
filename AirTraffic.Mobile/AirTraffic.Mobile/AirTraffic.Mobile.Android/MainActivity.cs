using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.GoogleMaps.Android;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace AirTraffic.Mobile.Droid
{
    [Activity(Label = "AirTraffic.Mobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            UserDialogs.Init(() => this);
            var platformConfig = new PlatformConfig
            {
                //   BitmapDescriptorFactory = new CachingNativeBitmapDescriptorFactory()
            };
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState, platformConfig); // initialize for Xamarin.Forms.GoogleMaps
            LoadApplication(new App());
        }
    }

    [Application]
    [MetaData("com.google.android.maps.v2.API_KEY",
            Value = "AIzaSyB8CWuTXCULfV6Jx1UsA6CdAqZWjmzTzhE")]
    public class MyApp : Android.App.Application
    {
        public MyApp(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            Xamarin.Essentials.Platform.Init(this);
        }
    }
}