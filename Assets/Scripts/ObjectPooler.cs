using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPooler : MonoSingleton<ObjectPooler>
{
    [SerializeField] private List<ObjectPooledItem> itemsToPool;
    [SerializeField] private GameObject pooledObjectHolder;

    private List<GameObject> pooledObjects;

    /* //GAMEOBJECT
    private ObjectPooler objPooler;

    private void Start()
    {
        objPooler = FindObjectOfType<ObjectPooler>();
    }

    public void InstanPipe()
    {
        var poolObj = objPooler.GetPooledObject("testTag");
        poolObj.SetActive(true);
    }*/

    /* 
    // PARTICLE POOLER 
    public ParticleColor colorType;

    public static GemCollection gemCollection;

    public enum ParticleColor
    {
        Blue, 
        Green,
        Yellow,
        Pink,
        Purple,
        Orange
    } 
    
    //usage
     gameObject.SetActive(false);

     var particle = objPooler.GetPooledObject(colorType.ToString());
     particle.transform.position = transform.position;
     particle.transform.rotation = Quaternion.Euler(235, Random.Range(-15f, 16f), 0);
     particle.SetActive(true);
     particle.GetComponent<ParticleSystem>().Play();
     
     */

    private void Awake()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPooledItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.transform.SetParent(pooledObjectHolder.transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }
    public GameObject GetPooledObject(string tag)
    {
        for (int i = pooledObjects.Count - 1; i > -1; i--)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }
        //pooledObjects.First(o => o.activeInHierarchy && o.tag == tag);
        foreach (ObjectPooledItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    obj.transform.SetParent(pooledObjectHolder.transform);
                    return obj;
                }
            }
        }
        return null;
    }

}