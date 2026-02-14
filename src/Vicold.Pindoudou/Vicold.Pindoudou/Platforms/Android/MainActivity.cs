using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace Vicold.Pindoudou
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private const int RequestStoragePermissionCode = 100;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestStoragePermissions();
        }

        private void RequestStoragePermissions()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                // Android 13+ 权限模型
                if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.ReadMediaImages) != Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(this, new[] { Android.Manifest.Permission.ReadMediaImages }, RequestStoragePermissionCode);
                }
            }
            else if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                // Android 6.0+ 到 12 权限模型
                if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.ReadExternalStorage) != Permission.Granted ||
                    ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.WriteExternalStorage) != Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(this, new[] { Android.Manifest.Permission.ReadExternalStorage, Android.Manifest.Permission.WriteExternalStorage }, RequestStoragePermissionCode);
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if (requestCode == RequestStoragePermissionCode)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    // 权限授予成功
                }
                else
                {
                    // 权限授予失败，可以显示提示信息
                }
            }
        }
    }
}
