using Inputs.Services;
using Player;
using Zenject;

namespace Installers
{
    public class SecondLevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSecondLevelInputService();
            SetAttackZone();
        }

        private void SetAttackZone() =>
            Container
                .Resolve<PlayerController>()
                .GetComponent<PlayerAttack>()
                .InAttackZone = true;

        private void BindSecondLevelInputService() =>
            Container
                .Bind<ISLInputService>()
                .To<SLInputService>()
                .AsSingle();
    }
}