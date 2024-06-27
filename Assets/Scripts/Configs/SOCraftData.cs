using System;
using System.Collections.Generic;
using InteractionManagement.Craftable;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Craft", menuName = "Crafts/Craft Data")]
    public class SOCraftData : ScriptableObject
    {
        public List<CraftableType> CraftableObjectsNames;
    }
}