using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ShopItems", menuName = "Shop/Shop Items")]
    public class SOShopItems : ScriptableObject
    {
        public List<ShopItemData> ShopItemsData;
    }
}