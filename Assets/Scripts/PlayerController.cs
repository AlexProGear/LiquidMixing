using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3f;
    
    private Transform cameraTransform;
    private Rigidbody rb;
    
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateMovement();
        UpdateRotation();
    }

    private void UpdateMovement()
    {
        if (MouseController.CursorLocked)
        {
            float forwardMovement = Input.GetAxisRaw("Vertical");
            float sideMovement = Input.GetAxisRaw("Horizontal");
            Vector3 direction = transform.forward * forwardMovement + transform.right * sideMovement;
            rb.velocity = direction.normalized * movementSpeed;
        }
    }

    private void UpdateRotation()
    {
        if (MouseController.CursorLocked)
        {
            transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"));
            cameraTransform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y"));
        }
    }
}
