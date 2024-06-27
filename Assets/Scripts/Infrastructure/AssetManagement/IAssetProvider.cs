using UnityEngine;

namespace Infrastructure.AssetManagement
{
    public interface IAssetProvider
    {
        // TInstance Instantiate<TInstance>(string path, Vector3 at) where TInstance : MonoBehaviour;
        // TInstance Instantiate<TInstance>(string path) where TInstance : Object;
        TResource LoadResource<TResource>(string path) where TResource : Object;
    }
}