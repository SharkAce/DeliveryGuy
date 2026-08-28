using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarControls : MonoBehaviour
{
    private CarController.Controls controls = new CarController.Controls();
    private CarController car;

    void Start()
    {
        car = GetComponent<CarController>();
        controls.driveInput = 0f;
        controls.brakeInput = 0f;
        controls.steerInput = 0f;
        controls.slideInput = false;
    }

    void Update()
    {
        controls.driveInput = Input.GetKey(KeyCode.W) ? 1f : 0f;
        controls.brakeInput = Input.GetKey(KeyCode.S) ? 1f : 0f;
        
        if (Input.GetKey(KeyCode.A)) controls.steerInput = 1f;
        else if (Input.GetKey(KeyCode.D)) controls.steerInput = -1f;
        else controls.steerInput = 0f;

        controls.slideInput = Input.GetKey(KeyCode.Space);

        car.ApplyControls(controls);
    }
}
