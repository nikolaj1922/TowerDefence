using UnityEngine;
using System.Collections.Generic;

namespace _Project.Scripts.Infrastructure.ObjectsPool
{
    public class ObjectsPool<T> where T : MonoBehaviour
    {
        private GameObject _parent;
        private readonly T _prefab;
        private readonly Stack<T> _pool = new ();
        
        public ObjectsPool(T prefab) => _prefab = prefab;

        public T Get()
        {
            var obj = _pool.Count > 0 ? _pool.Pop() : Create();

            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Release(T obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Push(obj);
        }

        private T Create()
        {
            if (_parent == null)
                _parent = new GameObject($"{_prefab.name}_Pool");
            
            T obj = Object.Instantiate(_prefab, _parent.transform);
            obj.gameObject.SetActive(false);
            return obj;
        }
    }
}
