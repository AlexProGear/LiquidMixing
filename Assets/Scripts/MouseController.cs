using UnityEngine;

public class MouseController : MonoBehaviour
{
    public static bool CursorLocked { get; private set; }
    
    void Start()
    {
        CursorLocked = true;
        UpdateCursorState();
    }
    
    void Update()
    {
        UpdateCursorLock();
    }

    private void UpdateCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CursorLocked = false;
            UpdateCursorState();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CursorLocked = true;
            UpdateCursorState();
        }
    }

    private void UpdateCursorState()
    {
        Cursor.lockState = CursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !CursorLocked;
    }
}
