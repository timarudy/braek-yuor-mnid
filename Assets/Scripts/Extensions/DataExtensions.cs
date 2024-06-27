using Data;
using UnityEngine;

namespace Extensions
{
    public static class DataExtensions
    {
        public static Vector3Data AsVectorData(this Vector3 vector) => 
            new(vector.x, vector.y, vector.z);

        public static T ToDeserialized<T>(this string json) => 
            JsonUtility.FromJson<T>(json);

        public static string ToJson(this object obj) => 
            JsonUtility.ToJson(obj);
    }
}