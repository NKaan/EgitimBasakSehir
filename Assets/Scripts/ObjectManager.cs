using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public List<GameObject> objects;

    public static ObjectManager sing;

    private void Awake()
    {
        sing = this;
    }

    public GameObject CreateObject(string objectName,Vector3 position)
    {
        GameObject gameObject = objects.FirstOrDefault(x => x.gameObject.name == objectName);
        return Instantiate(gameObject, position,Quaternion.identity);
    }

    public T CreateObject<T>(Vector3 position) where T : MonoBehaviour
    {
        T gameObject = objects.FirstOrDefault(x => x.GetComponent<T>() != null).GetComponent<T>();
        return Instantiate(gameObject, position, Quaternion.identity);
    }

}
