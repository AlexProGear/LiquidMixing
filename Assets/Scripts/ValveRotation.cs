using System;
using UnityEngine;
using UnityEngine.Events;

public class ValveRotation : MonoBehaviour
{
    [SerializeField] private Transform rotationTransform;
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    
    public float CurrentAngle { get; private set; }
    public float CurrentAnglePercent => Mathf.InverseLerp(minAngle, maxAngle, CurrentAngle);

    public UnityEvent<float> rotationUpdatedEvent;

    private void Start()
    {
        CurrentAngle = minAngle;
        UpdateRotation();
    }

    public void Rotate(float angle)
    {
        CurrentAngle = Mathf.Clamp(CurrentAngle + angle, minAngle, maxAngle);
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        rotationTransform.rotation = Quaternion.Euler(0, CurrentAngle, 0);
        rotationUpdatedEvent.Invoke(CurrentAnglePercent);
    }
}
