using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ColorBlocks.Utilities
{
    public class Pooler
    {
        private readonly Transform _parent;
        private readonly Dictionary<Component, ObjectPool<Component>> _pools = new Dictionary<Component, ObjectPool<Component>>();
        private readonly Dictionary<Component, Component> _prefabByItem = new Dictionary<Component, Component>();

        public Pooler(Transform parent)
        {
            _parent = parent;
        }

        public T Get<T>(T prefab) where T : Component
        {
            if (!_pools.ContainsKey(prefab))
                _pools[prefab] = CreatePool(prefab);

            T item = (T)_pools[prefab].Get();
            _prefabByItem[item] = prefab;

            return item;
        }

        public void Release(Component item)
        {
            Component prefab = _prefabByItem[item];
            _pools[prefab].Release(item);
        }

        private ObjectPool<Component> CreatePool(Component prefab)
        {
            return new ObjectPool<Component>(
                createFunc: () => Object.Instantiate(prefab, _parent),
                actionOnGet: item => item.gameObject.SetActive(true),
                actionOnRelease: item => item.gameObject.SetActive(false));
        }
    }
}