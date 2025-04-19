using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEngine.Scripting;

[Preserve]
public class NavigationBaker : MonoBehaviour
{
    public NavMeshSurface surface; // Single NavMeshSurface instead of an array
    public GameObject[] Characters;
    public Transform[] objectsToRotate;
    public GameObject pet;

    private void Awake() {
        surface = GetComponent<NavMeshSurface>();
        //pet = GetComponent<Pet>();
    }
    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3f);
        BuildNavMesh();

       //yield return new WaitForSeconds(1f);
       // SpawnPet();
    }
    void BuildNavMesh()
    {
        if (surface != null)
        {
            surface.layerMask = LayerMask.GetMask("Default", "Ground", "Environment");
            surface.BuildNavMesh();
        }
    }
    void SpawnPet()
    {
        pet.SetActive(true);
    }
}