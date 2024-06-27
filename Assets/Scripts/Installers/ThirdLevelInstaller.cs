using Player;
using Services.SceneServices;
using UI.Factory;
using Zenject;

namespace Installers
{
    public class ThirdLevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            IUIFactory uiFactory = Container.Resolve<IUIFactory>();
            uiFactory.CreateHpBar();
            
            PlayerController player = Container.Resolve<PlayerController>();
            player.LightOn();
        }
    }
}