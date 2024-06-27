using System;
using Data;
using Services.SceneServices;
using UnityEngine;
using Zenject;

namespace DailyReward
{
    public class Coin2D : MonoBehaviour
    {
        public void Collect(Player2DCollector player)
        {
           player.AddMoney(1);
           Destroy(gameObject);
        }
    }
}