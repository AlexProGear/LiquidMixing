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
    private Vector3 lastContactPosition = Vector3.zero;
    
    private Collider lastCollider;
    private ValveRotation cachedValveComponent;

    ValveRotation GetCachedOrNewValve(Collider newCollider)
    {
        if (lastCollider != newCollider)
        {
            lastCollider = newCollider;
            cachedValveComponent = newCollider.GetComponentInParent<ValveRotation>();
        }
        return cachedValveComponent;
    }

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
            lastContactPosition = Vector3.zero;
            return;
        }

        canvasHand.enabled = true;
        bool mouseButtonPressed = Input.GetMouseButton(0);
        int spriteIndex = mouseButtonPressed ? 1 : 0;
        canvasHand.sprite = canvasHandSprites[spriteIndex];
        if (mouseButtonPressed && lastContactPosition != hitInfo.point)
        {
            Vector3 boundsCenter = hitInfo.collider.bounds.center;
            
            Vector3 startVector = lastContactPosition - boundsCenter;
            Vector3 endVector = hitInfo.point - boundsCenter;
            
            float rotationAngle = Vector3.SignedAngle(startVector, endVector, Vector3.up);
            GetCachedOrNewValve(hitInfo.collider).Rotate(rotationAngle);
        }
        lastContactPosition = hitInfo.point;
    }
}
