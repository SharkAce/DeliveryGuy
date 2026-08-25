using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    [SerializeField] private GameObject walkingIntersections;
    [SerializeField] private List<GameObject> WalkerTypes;
    [SerializeField] private int walkerSpawnCount = 100;
    void Start()
    {
        for (int i = 0; i < walkerSpawnCount; i++)
        {
            GameObject WalkerType = WalkerTypes[Random.Range(0, WalkerTypes.Count)];
            WaypointController startWp = walkingIntersections.transform.GetChild(Random.Range(0, walkingIntersections.transform.childCount))
                .GetComponent<WaypointController>();
            
            WaypointController previousWp = startWp.nextWaypoints[Random.Range(0, startWp.nextWaypoints.Count)];

            GameObject newNpc = Instantiate(WalkerType, transform);
            newNpc.GetComponent<NpcWalkerController>().Init(startWp, previousWp);
        }
    }
}
