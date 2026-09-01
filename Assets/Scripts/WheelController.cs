using UnityEngine;

public class WheelController : MonoBehaviour
{
    [Header("Settings")]
    public bool isDriveWheel = false;
    public bool isSteerWheel = false;
    public float lateralFrictionStrength = 10f;
    public float slidingLateralFrictionStrength = 1f;
    public float maxLateralStrength = 10f;
    public float slidingMaxLateralStrength = 1f;
    public float rollingResistance = 0.1f;
    public float maxDriveForce = 10f;
    public float maxBrakeForce = 3f;

    private Rigidbody2D carRb;
    private TrailRenderer skidTrail = null;

    public void Init(Rigidbody2D rb)
    {
        carRb = rb;
        skidTrail = GetComponent<TrailRenderer>();
    }

    public void Tick(float driveInput, float brakeInput, float steerAngle, bool sliding)
    {
        SetRotation(steerAngle);
        ApplyDriveForce(driveInput);
        ApplyBrakeForce(brakeInput, sliding);
        ApplyLateralFriction(sliding);
        ApplyRollingResistance(sliding);
    }

    float GetForwardSpeed()
    {
        Vector2 wheelVelocity = carRb.GetPointVelocity(transform.position);
        // Extracts the forward component of the wheel's velocity by projecting it onto the forward axis
        return Vector2.Dot(wheelVelocity, transform.up);
    }

    float GetLateralSpeed()
    {
        Vector2 wheelVelocity = carRb.GetPointVelocity(transform.position);
        // Extracts the sideways component of the wheel's velocity by projecting it onto the sideways axis   
        return Vector2.Dot(wheelVelocity, transform.right);
    }

    void SetRotation(float steerAngle)
    {
        if (!isSteerWheel) return;

        transform.localEulerAngles = new Vector3(0, 0, steerAngle);
    }
    void ApplyDriveForce(float input)
    {
        if (!isDriveWheel) return;

        float force = input * maxDriveForce;
        carRb.AddForceAtPosition(transform.up * force, transform.position);
    }

    void ApplyBrakeForce(float input, bool sliding)
    {
        if (sliding) return;

        carRb.AddForceAtPosition(-transform.up * input * maxBrakeForce *  GetForwardSpeed(), transform.position);
    }

    void ApplyRollingResistance(bool sliding)
    {
        if (sliding) return;

        carRb.AddForceAtPosition(-transform.up * rollingResistance * GetForwardSpeed(), transform.position);
    }
    void ApplyLateralFriction(bool sliding)
    {
        if (skidTrail != null)
            skidTrail.emitting = sliding;
        float strength = sliding ? slidingLateralFrictionStrength : lateralFrictionStrength;
        float maxStrength = sliding ? slidingMaxLateralStrength : maxLateralStrength;

        float correction = Mathf.Clamp(-GetLateralSpeed() * strength, -maxStrength, maxStrength);
        carRb.AddForceAtPosition(transform.right * correction, transform.position);
    }
}