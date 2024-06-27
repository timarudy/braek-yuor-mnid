using Configs;
using UI.Factory;
using UI.Windows;

namespace Services.StaticData
{
    public interface IStaticDataService
    {
        WindowBase ForWindow(WindowType windowType);
        void LoadStaticData();
        ShopItemData ForShopItems(string name);
        LevelData ForLevelItems(int id);
        void OpenNewLevel(int id);
    }
}