using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace City.AddressableServices
{
    public class AddressablesService
    {
        public async UniTask<T> LoadAsset<T>(string assetId)
        {
            var asset = await Addressables.InstantiateAsync(assetId);
            asset.TryGetComponent(out T result);
            return result;
        }
    }
}
