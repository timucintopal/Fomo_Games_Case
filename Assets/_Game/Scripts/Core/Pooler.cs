using UnityEngine;
using UnityEngine.Pool;

namespace ColorBlocks.View
{
    public class Pooler<T> where T : Component
    {
        private readonly ObjectPool<T> _pool;

        public Pooler(T prefab, Transform parent)
        {
            _pool = new ObjectPool<T>(
                createFunc: () => Object.Instantiate(prefab, parent),
                actionOnGet: item => item.gameObject.SetActive(true),
                actionOnRelease: item => item.gameObject.SetActive(false));
        }

        public T Get()
        {
            return _pool.Get();
        }

        public void Release(T item)
        {
            _pool.Release(item);
        }
    }
}