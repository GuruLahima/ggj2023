using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// this class spawns assigned prefab on random places with uniform distribution within a given rectangle
/// </summary>
public class Spawner : MonoBehaviour
{
  [SerializeField] private GameObject prefabToSpawn;
  [SerializeField] private int numberOfObjectsToSpawn;
  [SerializeField] BoxCollider2D spawnArea;


  private Bounds limits;
  private List<GameObject> spawnedObjects = new List<GameObject>();


  private void Start()
  {
    ClearObjects();
    SpawnObjects();
  }

  [Button("Spawn Objects")]
  private void SpawnObjects()
  {
    limits = spawnArea.bounds;
    for (int i = 0; i < numberOfObjectsToSpawn; i++)
    {
      var x = UnityEngine.Random.Range(limits.center.x - limits.size.x / 2, limits.center.x + limits.size.x / 2);
      var y = UnityEngine.Random.Range(limits.center.y - limits.size.y / 2, limits.center.y + limits.size.y / 2);
      var position = new Vector3(x, y, 0);
      spawnedObjects.Add(Instantiate(prefabToSpawn, position, Quaternion.identity));
    }
  }

  [Button("Clear Objects")]
  private void ClearObjects()
  {
    foreach (var obj in spawnedObjects)
    {
      DestroyImmediate(obj);
    }
    spawnedObjects.Clear();
  }
}
