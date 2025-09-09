using UnityEngine;
using Zenject;

namespace Core.PrefabFactory
{
    //Можно в будущенм подменить реализацию и использовать асинхронную загрузку через ассет бандлы или адрессблс
    
    public class BasicPrefabFactory : IPrefabFactory
    {
        private readonly DiContainer _container;

        public BasicPrefabFactory(DiContainer container)
        {
            _container = container;
        }

        public GameObject SpawnPrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return _container.InstantiatePrefab(prefab, position, rotation, parent);
        }

        public GameObject SpawnPrefab(GameObject prefab, Transform parent)
        {
            return _container.InstantiatePrefab(prefab, parent);
        }
    }
}