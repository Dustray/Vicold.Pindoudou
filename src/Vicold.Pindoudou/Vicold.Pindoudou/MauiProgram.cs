using Microsoft.Extensions.Logging;
using Vicold.Pindoudou.Services;

namespace Vicold.Pindoudou
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
			builder.Logging.AddDebug();
#endif

            // 注册图片选择服务
            builder.Services.AddTransient<IImagePickerService, DefaultImagePickerService>();

            // 注册文件保存服务
            #if ANDROID
            builder.Services.AddTransient<IFileSaveService, Platforms.Android.FileSaveService>();
            #else
            builder.Services.AddTransient<IFileSaveService, DefaultFileSaveService>();
            #endif

            return builder.Build();
        }
    }
}
