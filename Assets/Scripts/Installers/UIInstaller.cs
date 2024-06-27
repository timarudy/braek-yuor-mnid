using Inputs.Services;
using UI.Factory;
using UI.Service;
using Zenject;

namespace Installers
{
    public class UIInstaller : MonoInstaller
    {
        private IUIFactory _uiFactory;

        public override void InstallBindings()
        {
            _uiFactory = Container.Resolve<IUIFactory>();
            _uiFactory.CreateUIRoot();
        }
    }
}