using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class AsteroidSpawner : MonoBehaviour
{
    public List<GameObject> toSpawn;
    public Vector3 spawnRange;
    public int spawnCount = 10;
    public bool respawn = false;


    void Update()
    {
        if (respawn)
        {
            DespawnObjects();
            SpawnObjects();
            respawn = false;
        }
        DrawSpawnBoxDebug();
    }


    void DespawnObjects()
    {
        int nChilds = transform.childCount;
        var childsToDestroy = new List<GameObject>();

        for (int i = 0; i < nChilds; i++)
            childsToDestroy.Add(transform.GetChild(i).gameObject);

        foreach (var child in childsToDestroy)
            DestroyImmediate(child);
    }


    void SpawnObjects()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition;
            spawnPosition.x = Random.Range(-spawnRange.x, spawnRange.x);
            spawnPosition.y = Random.Range(-spawnRange.y, spawnRange.y);
            spawnPosition.z = Random.Range(-spawnRange.z, spawnRange.z);

            int k = Random.Range(0, toSpawn.Count);
            GameObject spawned = Instantiate(toSpawn[k], transform);
            spawned.transform.localPosition = spawnPosition;
        }
    }


    void DrawSpawnBoxDebug()
    {
        var center = transform.position;

        var p1 = new Vector3(-spawnRange.x, -spawnRange.y, -spawnRange.z) + center;
        var p2 = new Vector3(-spawnRange.x, -spawnRange.y, spawnRange.z) + center;
        var p3 = new Vector3(-spawnRange.x, spawnRange.y, -spawnRange.z) + center;
        var p4 = new Vector3(-spawnRange.x, spawnRange.y, spawnRange.z) + center;
        var p5 = new Vector3(spawnRange.x, -spawnRange.y, -spawnRange.z) + center;
        var p6 = new Vector3(spawnRange.x, -spawnRange.y, spawnRange.z) + center;
        var p7 = new Vector3(spawnRange.x, spawnRange.y, -spawnRange.z) + center;
        var p8 = new Vector3(spawnRange.x, spawnRange.y, spawnRange.z) + center;

        Debug.DrawLine(p1, p2, Color.green);
        Debug.DrawLine(p3, p4, Color.green);
        Debug.DrawLine(p1, p3, Color.green);
        Debug.DrawLine(p2, p4, Color.green);
                   
        Debug.DrawLine(p3, p7, Color.green);
        Debug.DrawLine(p4, p8, Color.green);
        Debug.DrawLine(p1, p5, Color.green);
        Debug.DrawLine(p2, p6, Color.green);
                        
        Debug.DrawLine(p7, p8, Color.green);
        Debug.DrawLine(p5, p6, Color.green);
        Debug.DrawLine(p5, p7, Color.green);
        Debug.DrawLine(p6, p8, Color.green);
    }


}
