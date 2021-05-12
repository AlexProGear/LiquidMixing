using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Renderer))]
public class FluidLevelController : MonoBehaviour
{
    [SerializeField] private float totalTankVolume;
    [SerializeField] private float maxPouringVolumePerSecond;
    [SerializeField] private float minScale = 0.049f;
    [SerializeField] private float maxScale = 0.8f;
    [SerializeField] private Color firstLiquidColor = Color.blue;
    [SerializeField] private Color secondLiquidColor = Color.red;
    
    public float FirstLiquidVolume { get; private set; }
    public float SecondLiquidVolume { get; private set; }
    public float TotalLiquidVolume => FirstLiquidVolume + SecondLiquidVolume;
    public float TotalTankVolume => totalTankVolume;
    public float FreeVolume => TotalTankVolume - TotalLiquidVolume;
    public float TankFillPercentage => TotalLiquidVolume / TotalTankVolume;

    public UnityEvent tankOverflowEvent;

    private float[] liquidFlowStrengths = new float[2];
    private bool overflow;

    private Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        ClearTank();
    }

    private void Update()
    {
        PourLiquidToTank(liquidFlowStrengths[0], x => FirstLiquidVolume += x);
        PourLiquidToTank(liquidFlowStrengths[1], x => SecondLiquidVolume += x);
        UpdateLiquidMesh();
    }

    public void ClearTank()
    {
        overflow = false;
        FirstLiquidVolume = 0;
        SecondLiquidVolume = 0;
    }

    private void PourLiquidToTank(float flowStrength, Action<float> volumeAdder)
    {
        float pouredVolume = flowStrength * maxPouringVolumePerSecond * Time.deltaTime;
        if (IsEnoughSpace(pouredVolume))
        {
            volumeAdder.Invoke(pouredVolume);
        }
        else if (!overflow)
        {
            tankOverflowEvent.Invoke();
            overflow = true;
        }
    }

    private bool IsEnoughSpace(float requiredVolume)
    {
        return FreeVolume >= requiredVolume;
    }

    private void UpdateLiquidMesh()
    {
        transform.localScale = new Vector3(1, Mathf.Lerp(minScale, maxScale, TankFillPercentage), 1);
        float liquidPercentage = SecondLiquidVolume / TotalLiquidVolume;
        renderer.material.color = Color.Lerp(firstLiquidColor, secondLiquidColor, liquidPercentage);
    }

    public void OnFirstLiquidFlowChanged(float newStrength)
    {
        liquidFlowStrengths[0] = newStrength;
    }

    public void OnSecondLiquidFlowChanged(float newStrength)
    {
        liquidFlowStrengths[1] = newStrength;
    }
}
