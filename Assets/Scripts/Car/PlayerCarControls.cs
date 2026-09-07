using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarControls : MonoBehaviour
{
    [Range(0.1f, 1f)]
    public float reversePower = 0.65f;
    private CarController.Controls controls;
    private CarController car;
    [SerializeField] private HUDDisplay hudDisplay;

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
        DeliveryManager manager = FindObjectOfType<DeliveryManager>();

        if(manager != null && manager.IsDialogueActive)
        {
            controls.driveInput = 0f;
            controls.steerInput = 0f;
            controls.slideInput = false;
            car.ApplyControls(controls);

            if(hudDisplay != null)
            {
                hudDisplay.ShowDialogueHint();
            }
            return;
        }

        if(hudDisplay != null)
        {
            hudDisplay.HideDialogueHint();
        }

        if(Input.GetKey(KeyCode.W)) controls.driveInput = 1f;
        else if(Input.GetKey(KeyCode.S)) controls.driveInput = -reversePower;
        else controls.driveInput = 0f;

        if(Input.GetKey(KeyCode.A)) controls.steerInput = 1f;
        else if(Input.GetKey(KeyCode.D)) controls.steerInput = -1f;
        else controls.steerInput = 0f;

        controls.slideInput = Input.GetKey(KeyCode.Space);

        car.ApplyControls(controls);
    }
}
