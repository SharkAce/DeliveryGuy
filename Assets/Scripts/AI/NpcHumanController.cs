using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Left,
    Right,
    Down
}

public class NpcHumanController : MonoBehaviour
{

    [SerializeField] private WaypointController currentWaypoint = null;
    private WaypointController previousWaypoint = null;
    [SerializeField] private Direction currentDirection = Direction.Up;
    [SerializeField] private float animationFrameDuration = 0.3f;
    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private List<Sprite> upWalk;
    [SerializeField] private List<Sprite> leftWalk;
    [SerializeField] private List<Sprite> rightWalk;
    [SerializeField] private List<Sprite> downWalk;

    public void Init(WaypointController startWp, WaypointController previousWp)
    {
        if (startWp == null || previousWp == null) return;
        currentWaypoint = startWp;
        previousWaypoint = previousWp;

        // Select a random point between the two waypoints
        transform.position = Vector3.Lerp(startWp.transform.position, previousWp.transform.position, Random.value);
        SetDirection();
    }

    void Update()
    {
        if (currentWaypoint == null) return;

        Animate();

        if (currentWaypoint.transform.position == transform.position) SelectNextWaypoint();

        transform.position = Vector2.MoveTowards(transform.position, currentWaypoint.transform.position, moveSpeed * Time.deltaTime);
    }

    void SelectNextWaypoint()
    {
        foreach (WaypointController wp in currentWaypoint.nextWaypoints)
        {
            if (wp != currentWaypoint && wp != previousWaypoint)
            {
                previousWaypoint = currentWaypoint;
                currentWaypoint = wp;
                break;
            }

        }

        SetDirection();
    }

    void SetDirection()
    {
        
        if (previousWaypoint.transform.position.y == currentWaypoint.transform.position.y)
        {
            currentDirection = previousWaypoint.transform.position.x > currentWaypoint.transform.position.x ? Direction.Left : Direction.Right;
        }
        else
        {
            currentDirection = previousWaypoint.transform.position.y > currentWaypoint.transform.position.y ? Direction.Down : Direction.Up;
        }
    }

    void Animate()
    {
        List<Sprite> currentAnimation = null;
        switch (currentDirection)
        {
            case Direction.Up:
                currentAnimation = upWalk;
                break;
            case Direction.Left:
                currentAnimation = leftWalk;
                break;
            case Direction.Right:
                currentAnimation = rightWalk;
                break;
            case Direction.Down:
                currentAnimation = downWalk;
                break;
        }

        float cycleProgress = (Time.time / animationFrameDuration) % currentAnimation.Count;
        int animationFrame = (int)Mathf.Floor(cycleProgress);

        GetComponent<SpriteRenderer>().sprite = currentAnimation[animationFrame];
    }
}
