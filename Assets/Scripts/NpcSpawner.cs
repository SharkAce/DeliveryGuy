using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    [SerializeField] private GameObject walkingIntersections;
    [SerializeField] private List<GameObject> HumanTypes;
    [SerializeField] private int HumanSpawnCount = 100;
    void Start()
    {
        for (int i = 0; i < HumanSpawnCount; i++)
        {
            GameObject HumanType = HumanTypes[Random.Range(0, HumanTypes.Count)];
            WaypointController startWp = walkingIntersections.transform.GetChild(Random.Range(0, walkingIntersections.transform.childCount))
                .GetComponent<WaypointController>();
            
            WaypointController previousWp = startWp.nextWaypoints[Random.Range(0, startWp.nextWaypoints.Count)];

            GameObject newNpc = Instantiate(HumanType, transform);
            newNpc.GetComponent<NpcHumanController>().Init(startWp, previousWp);
        }
    }
}
