using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [SerializeField] private int initCreateCount = 5;

    [SerializeField] private List<PoolableMono> poolingList = new();
    private Dictionary<string, Stack<PoolableMono>> pool = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        CreatePool();
    }

    public PoolableMono Pop(string name, Vector3 popPos)
    {
        PoolableMono obj = null;

        if (!pool.ContainsKey(name))
            Debug.Log("풀 오브젝트 없음");

        if (pool[name].Count > 0)
        {
            obj = pool[name].Pop();
        }
        else
        {
            obj = Instantiate(poolingList.Find(x => x.name == name), transform);
            obj.name = obj.name.Replace("(Clone)", "");
            obj.transform.position = popPos;    
        }

        obj.gameObject.SetActive(true);
        obj.transform.position = popPos;
        obj.Init();

        return obj;
    }

    public void Push(PoolableMono obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        pool[obj.name].Push(obj);
    }

    private void CreatePool()
    {
        foreach(PoolableMono poolObj in  poolingList)
        {
            pool.Add(poolObj.name, new());

            for(int i = 0; i < initCreateCount; i++)
            {
                PoolableMono obj = Instantiate(poolObj, transform);
                obj.name = obj.name.Replace("(Clone)", "");
                obj.gameObject.SetActive(false);
                pool[poolObj.name].Push(obj);
            }
        }
    }
}
