using UnityEngine;

namespace Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        // public TInstance Instantiate<TInstance>(string path, Vector3 at) where TInstance : MonoBehaviour
        // {
        //     TInstance prefab = LoadResource<TInstance>(path);
        //     return Object.Instantiate(prefab, at, Quaternion.identity);
        // }
        //
        // public TInstance Instantiate<TInstance>(string path) where TInstance : Object
        // {
        //     TInstance prefab = LoadResource<TInstance>(path);
        //     return Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        // }
        
        public TResource LoadResource<TResource>(string path) where TResource : Object => 
            Resources.Load<TResource>(path);
    }
}