using UnityEditor;
using UnityEngine;

public class WaypointEditor : EditorWindow
{
    private float gridSize = 0.5f;
    private bool connectionMode = false;
    private bool bidirectionalMode = false;
    private WaypointController selectedWaypoint = null;

    
    [MenuItem("Tools/Waypoint Editor")]
    public static void ShowWindow()
    {
        GetWindow<WaypointEditor>("Waypoint Editor");
    }
    
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }
    
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        selectedWaypoint = null;
    }
    
    private void OnSceneGUI(SceneView sceneView)
    {
        if (!connectionMode) return;
        
        if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            HandleWaypointClick();
            Event.current.Use();        
        }
    }
    
    private void HandleWaypointClick()
    {
        
        if (Selection.activeGameObject == null) return;
        
        WaypointController waypoint = Selection.activeGameObject.GetComponent<WaypointController>();
        if (waypoint == null) return;
        
        if (selectedWaypoint == null)
        {
            selectedWaypoint = waypoint;
            Debug.Log($"Selected: {waypoint.name}");
        }
        else if (selectedWaypoint == waypoint)
        {
            selectedWaypoint = null;
            Debug.Log("Deselected");
        }
        else
        {
            ConnectWaypoints(selectedWaypoint, waypoint);
            if (bidirectionalMode) ConnectWaypoints(waypoint, selectedWaypoint);
            
            selectedWaypoint = null;
        }

        Selection.activeGameObject = null;
    }

    private void ConnectWaypoints(WaypointController first, WaypointController second)
    {
        if (!first.nextWaypoints.Contains(second))
        {
            first.nextWaypoints.Add(second);
            Debug.Log($"Connected {first.name} -> {second.name}");
        }
        else
        {
            first.nextWaypoints.Remove(second);
            Debug.Log($"Disconnected {first.name} -> {second.name}");
        }
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Waypoint Setup", EditorStyles.boldLabel);
        
        gridSize = EditorGUILayout.FloatField("Grid Size", gridSize);
        
        if (GUILayout.Button("Snap All Waypoints to Grid", GUILayout.Height(30)))
        {
            SnapAllToGrid();
        }

        GUILayout.Space(20);
        GUILayout.Label("Waypoint Connections", EditorStyles.boldLabel);
        
        bidirectionalMode = EditorGUILayout.Toggle("Bidirectional Connections", bidirectionalMode);
        
        if (GUILayout.Button(connectionMode ? "Exit Connection Mode" : "Enter Connection Mode", GUILayout.Height(30)))
        {
            connectionMode = !connectionMode;
            if (!connectionMode) selectedWaypoint = null;
        }        
    }
    
    private void SnapAllToGrid()
    {
        WaypointController[] waypoints = FindObjectsOfType<WaypointController>();
        foreach (var wp in waypoints)
        {
            Vector3 pos = wp.transform.position;
            pos.x = Mathf.Round(pos.x / gridSize) * gridSize;
            pos.y = Mathf.Round(pos.y / gridSize) * gridSize;
            wp.transform.position = pos;
        }
        Debug.Log($"Snapped {waypoints.Length} waypoints to grid");
    }
}