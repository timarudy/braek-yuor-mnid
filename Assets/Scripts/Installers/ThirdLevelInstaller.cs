using Player;
using Services.Factory;
using Services.SceneServices;
using UI.Factory;
using Zenject;

namespace Installers
{
    public class ThirdLevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindEnemiesFactory();
            
            IUIFactory uiFactory = Container.Resolve<IUIFactory>();
            uiFactory.CreateHpBar();
            
            PlayerController player = Container.Resolve<PlayerController>();
            player.LightOn();
        }

        private void BindEnemiesFactory()
        {
            Container
                .Bind<IEnemiesFactory>()
                .To<EnemiesFactory>()
                .AsSingle();
        }
    }
}