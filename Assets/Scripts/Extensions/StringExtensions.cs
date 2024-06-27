using System;
using Infrastructure.AssetManagement;
using Services.SceneServices;
using UI.Windows;

namespace Extensions
{
    public static class StringExtensions
    {
        private const string Cat = "Animal_Cat";
        private const string Pig = "Animal_Pig";
        private const string Chicken = "Animal_Chicken";
        private const string Lama = "Animal_Lama";
        private const string Elephant = "Animal_Elephant";

        public static string CutUnderscores(this string str) =>
            str.Replace("_", " ");

        public static string AnimalTypeToString(this AnimalType animalType)
        {
            switch (animalType)
            {
                case AnimalType.CAT:
                    return Cat;
                case AnimalType.PIG:
                    return Pig;
                case AnimalType.CHICKEN:
                    return Chicken;
                case AnimalType.LAMA:
                    return Lama;
                case AnimalType.ELEPHANT:
                    return Elephant;
            }

            return Cat;
        }
        
        public static AnimalType ToAnimalType(this string animalType)
        {
            switch (animalType)
            {
                case Cat:
                    return AnimalType.CAT;
                case Pig:
                    return AnimalType.PIG;
                case Chicken:
                    return AnimalType.CHICKEN;
                case Lama:
                    return AnimalType.LAMA;
                case Elephant:
                    return AnimalType.ELEPHANT;
            }

            return AnimalType.CAT;
        }
        
        public static string ToAnimalPath(this AnimalType animalType)
        {
            switch (animalType)
            {
                case AnimalType.CAT:
                    return AssetPath.Animal_CatPath;
                case AnimalType.PIG:
                    return AssetPath.Animal_PigPath;
                case AnimalType.CHICKEN:
                    return AssetPath.Animal_ChickenPath;
                case AnimalType.LAMA:
                    return AssetPath.Animal_LamaPath;
                case AnimalType.ELEPHANT:
                    return AssetPath.Animal_ElephantPath;
            }

            return AssetPath.Animal_CatPath;
        }
        
        public static string ToAnimalCoinPath(this AnimalType animalType)
        {
            switch (animalType)
            {
                case AnimalType.CHICKEN:
                    return AssetPath.ChickenCoinPath;
                case AnimalType.PIG:
                    return AssetPath.PigCoinPath;
                case AnimalType.LAMA:
                    return AssetPath.LamaCoinPath;
                case AnimalType.ELEPHANT:
                    return AssetPath.ElephantCoinPath;
            }

            return AssetPath.Animal_CatPath;
        }

        public static bool IsEqualStrings(string str1, string str2) => 
            string.Equals(str1, str2, StringComparison.CurrentCultureIgnoreCase);

        public static GameLevels ToLevel(this string levelName)
        {
            switch (levelName)
            {
                case SceneNames.FirstLevel:
                    return GameLevels.FIRST;
                case SceneNames.SecondLevel:
                    return GameLevels.SECOND;
                case SceneNames.ThirdLevel:
                    return GameLevels.THIRD;
                case SceneNames.DailyReward:
                    return GameLevels.REWARD;
            }

            return GameLevels.FIRST;
        }
    }
}