using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    [Tooltip("The prefab to instantiate.")] public GameObject prefab;
    [Range(0, 1000), Tooltip("The amount of gameObject to instantiate in the pool")] public int amount = 20;
    [Tooltip("This prefab will be spawned on the chunks.")] public bool isLevelElement = false;
    public List<Entity> entities = new List<Entity>();
    public Transform parent;
    public bool foldout = true;
    public float usage = 0f;
}

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;
    public static PoolManager Instance { get { if (!instance) instance = FindObjectOfType<PoolManager>(); return instance; } }
    
    public List<Pool> pools = new List<Pool>();

    private void Start()
    {
        //Projectile p = GetEntity<Projectile>();
    }

    public T GetEntity<T>() where T : class
    {
        for (int p = 0; p < pools.Count; p++)
        {
            Pool poolable = pools[p];
            if (poolable.entities.Count > 0)
            {
                if (poolable.entities[0] is T)
                {
                    for (int i = 0; i < poolable.entities.Count; i++)
                    {
                        Entity entity = poolable.entities[i];
                        if (!entity.gameObject.activeInHierarchy)
                        {
                            return entity as T;
                        }
                    }
                }
            }
        }

        Debug.LogError("Error : not enough entities of types " + typeof(T).ToString() + " in pool.");
        return null;
    }
    
    public Entity GetEntityOfType(System.Type type)
    {
        for (int p = 0; p < pools.Count; p++)
        {
            Pool pool = pools[p];
            if (pool.entities.Count > 0)
            {
                if (pool.entities[0].GetType() == type)
                {
                    for (int i = 0; i < pool.entities.Count; i++)
                    {
                        Entity entity = pool.entities[i];
                        if (!entity.gameObject.activeInHierarchy)
                        {
                            return entity;
                        }
                    }
                }
            }
        }

        Debug.LogError("Error : not enough entities of types " + type.ToString() + " in pool.");
        return null;
    }

    private void Update()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            UpdateCurrentlyActive(i);
        }
    }

    public int UpdateCurrentlyActive(int i)
    {
        int currentlyActive = 0;
        pools[i].usage = 0;
        if (pools[i].entities.Count > 0)
        {
            for (int e = 0; e < pools[i].entities.Count; e++)
            {
                Entity entity = pools[i].entities[e];
                if (entity)
                {
                    if (entity.gameObject.activeInHierarchy)
                    {
                        currentlyActive++;
                        pools[i].usage += (1f / (pools[i].entities.Count));
                    }
                }
            }
        }

        return currentlyActive;
    }

    public bool ShouldRefresh()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            Pool pool = pools[i];
            if (pool.prefab == null)
            {
                return false;
            }

            else if (!pool.prefab.GetComponentInChildren<Entity>())
            {
                return false;
            }

            if (pool.entities.Count != pool.amount)
            {
                return true;
            }

            else
            {
                if (pool.entities.Count > 0)
                {
                    for (int e = 0; e < pool.entities.Count; e++)
                    {
                        if (pool.entities[e] == null)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public Pool GetLevelElementPoolAtIndex(int index, int tab)
    {
        List<Pool> onlyTiles = new List<Pool>();
        List<Pool> onlyEntities = new List<Pool>();

        for (int i = 0; i < pools.Count; i++)
        {
            if(pools[i].prefab.GetComponent<Tile>())
            {
                onlyTiles.Add(pools[i]);
            }
            else
            {
                if(pools[i].isLevelElement)
                    onlyEntities.Add(pools[i]);
            }
        }

        List<Pool> finalPools = new List<Pool>();
        if (tab == 0) finalPools = onlyTiles;
        else finalPools = onlyEntities;

        if (index < finalPools.Count)
        {
            return finalPools[index];
        }

        else return null;
    }
    
    public int GetIndexLevelElementPool(LevelElement lv)
    {

        List<Pool> onlyLevelElems = new List<Pool>();
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].isLevelElement)
            {
                onlyLevelElems.Add(pools[i]);
            }
        }

        for (int i = 0; i < onlyLevelElems.Count; i++)
        {
            if (onlyLevelElems[i].prefab)
            {
                LevelElement elem = onlyLevelElems[i].prefab.GetComponentInChildren<LevelElement>();
                if(elem)
                {

                    if (elem.GetType() == lv.GetType())
                    {
                        return i;
                    }
                }
            }
        }

        return 0;
    }
}
