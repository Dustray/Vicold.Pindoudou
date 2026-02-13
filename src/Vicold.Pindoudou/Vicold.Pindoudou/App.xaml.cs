using Vicold.Pindoudou.Utils;

namespace Vicold.Pindoudou
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "Vicold.Pindoudou" };
        }
        
        protected override void OnStart()
        {
            base.OnStart();
            
            // 初始化资产文件
            try
            {
                AssetManager.InitializeAssets();
                Console.WriteLine("在OnStart中初始化资产文件");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"在OnStart中初始化资产失败: {ex.Message}");
                Console.WriteLine($"异常详情: {ex.StackTrace}");
            }
        }
    }
}
