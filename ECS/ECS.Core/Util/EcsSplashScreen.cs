using System.Reflection;
using System.Windows;

namespace ECS.Core.Util
{
    public class EcsSplashScreen
    {
        private static SplashScreen sp = null;

        public EcsSplashScreen() => Init();

        public static void Show()
        {
            Init();
            sp.Show(true);
        } 

        private static void Init()
        {
            if (sp != null) return;

            string resourceName = "Resources/circle.gif";
            Assembly resourceAssembly = Assembly.GetAssembly(typeof(EcsSplashScreen));

            if (resourceAssembly == null)
                sp = new SplashScreen(resourceName);
            else
                sp = new SplashScreen(resourceAssembly, resourceName);
        }

    }

}
