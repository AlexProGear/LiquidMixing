using UnityEngine;
using UnityEngine.UI;

public class RotatorController : MonoBehaviour
{
    [SerializeField] private float maxDistance = 2f;
    [SerializeField] private LayerMask rotatorMask;
    [SerializeField] private Image canvasHand;
    [SerializeField] private Sprite[] canvasHandSprites;
    
    private Camera mainCamera;
    private Vector2 screenMiddle;

    private void Start()
    {
        mainCamera = Camera.main;
        screenMiddle = new Vector2(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2);
    }

    void Update()
    {
        CheckRotatorAvailible();
    }

    private void CheckRotatorAvailible()
    {
        Ray ray = mainCamera.ScreenPointToRay(screenMiddle);
        bool rayHit = Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, rotatorMask);
        UpdateHandIndicator(rayHit, hitInfo);
    }

    private void UpdateHandIndicator(bool rayHit, RaycastHit hitInfo)
    {
        if (!rayHit)
        {
            canvasHand.enabled = false;
            return;
        }

        canvasHand.enabled = true;
        int spriteIndex = Input.GetMouseButton(0) ? 1 : 0;
        canvasHand.sprite = canvasHandSprites[spriteIndex];
    }
}
