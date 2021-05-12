using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class LiquidFlowController : MonoBehaviour
{
    [SerializeField] private Color liquidColor = Color.cyan;
    [SerializeField] private float minFlowSpeed = 1;
    [SerializeField] private float maxFlowSpeed = 2;
    [SerializeField] private float minParticleSize = 0;
    [SerializeField] private float maxParticleSize = 1;
    
    private ParticleSystem particleSystem;
    private ParticleSystem.MainModule particleMainModule;

    private bool particlesEnabled = false;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleMainModule = particleSystem.main;
        particleMainModule.startColor = liquidColor;
        particleMainModule.startSpeed = minFlowSpeed;
        particleMainModule.startSize = minParticleSize;
    }

    /// <summary>
    /// Updates particle system flow strength
    /// </summary>
    /// <param name="flowStrength">Flow strength from 0 to 1</param>
    public void UpdateFlowRate(float flowStrength)
    {
        UpdateParticlesState(flowStrength);
        particleMainModule.startSpeed = Mathf.Lerp(minFlowSpeed, maxFlowSpeed, flowStrength);
        particleMainModule.startSize = Mathf.Lerp(minParticleSize, maxParticleSize, flowStrength);
    }

    private void UpdateParticlesState(float flowStrength)
    {
        if (particlesEnabled && Mathf.Approximately(flowStrength, 0))
        {
            particleSystem.Stop();
            particlesEnabled = false;
        }

        if (!particlesEnabled)
        {
            particleSystem.Play();
            particlesEnabled = true;
        }
    }
}
