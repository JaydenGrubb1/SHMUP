using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHMUP
{
    public class ObjectPool : MonoBehaviour
    {
        private static ObjectPool instance;

        [SerializeField]
        private PoolItem[] poolItems;

        private Dictionary<string, List<GameObject>> poolItemsDict = new Dictionary<string, List<GameObject>>();
		private int lastIndex;

		public void Awake()
		{
			if(instance == null)
			{
                instance = this;
			}
			else
			{
                Destroy(this);
			}
		}

        public void Start()
        {
			foreach (PoolItem item in poolItems)
			{
				if (!poolItemsDict.ContainsKey(item.id))
				{
                    poolItemsDict.Add(item.id, new List<GameObject>());

					for (int i = 0; i < item.maxSize; i++)
					{
						GameObject go = Instantiate(item.prefab, transform);
						go.SetActive(false);
						poolItemsDict[item.id].Add(go);
					}
				}
				else
				{
					Debug.LogWarning("pool with id: \"" + item.id + "\" already exists");
				}
			}
        }

		public static GameObject Get(string id)
		{
			if (!instance.poolItemsDict.ContainsKey(id))
			{
				Debug.LogError("pool with id: \"" + id + "\" does not exist");
				return null;
			}

			List<GameObject> list = instance.poolItemsDict[id];

			for (int i = instance.lastIndex; i < list.Count; i++)
			{
				if (!list[i].activeInHierarchy)
				{
					return list[i];
				}
			}
			for (int i = 0; i < instance.lastIndex; i++)
			{
				if (!list[i].activeInHierarchy)
				{
					return list[i];
				}
			}

			return null;
		}

        [Serializable]
        struct PoolItem
		{
            public string id;
            public GameObject prefab;
            public int maxSize;
		}
    }
}