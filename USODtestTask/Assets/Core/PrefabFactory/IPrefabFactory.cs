using UnityEngine;

namespace Core.PrefabFactory
{
    public interface IPrefabFactory
    {
        GameObject SpawnPrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null);
        GameObject SpawnPrefab(GameObject prefab, Transform parent);
    }
}