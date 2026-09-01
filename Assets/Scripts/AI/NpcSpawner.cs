using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    [SerializeField] private GameObject walkingIntersections;
    [SerializeField] private GameObject drivingIntersections;
    [SerializeField] private List<GameObject> HumanTypes;
    [SerializeField] private int HumanSpawnCount = 100;
    [SerializeField] private List<GameObject> CarTypes;
    [SerializeField] private int CarSpawnCount = 30;
    void Start()
    {
        for (int i = 0; i < HumanSpawnCount; i++)
        {
            GameObject HumanType = HumanTypes[Random.Range(0, HumanTypes.Count)];
            WaypointController previousWp = walkingIntersections.transform.GetChild(Random.Range(0, walkingIntersections.transform.childCount))
                .GetComponent<WaypointController>();
            
            WaypointController startWp = previousWp.nextWaypoints[Random.Range(0, previousWp.nextWaypoints.Count)];

            GameObject newNpc = Instantiate(HumanType, transform);
            newNpc.GetComponent<NpcHumanController>().Init(startWp, previousWp);
        }

        for (int i = 0; i < CarSpawnCount; i++)
        {
            GameObject CarType = CarTypes[Random.Range(0, CarTypes.Count)];
            WaypointController previousWp = drivingIntersections.transform.GetChild(Random.Range(0, drivingIntersections.transform.childCount))
                .GetComponent<WaypointController>();
            
            WaypointController startWp = previousWp.nextWaypoints[Random.Range(0, previousWp.nextWaypoints.Count)];

            GameObject newNpc = Instantiate(CarType, transform);
            newNpc.GetComponent<NpcCarControls>().Init(startWp, previousWp);
        }
    }
}
