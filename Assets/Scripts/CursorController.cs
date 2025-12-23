using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController instance;

    public Texture2D defaultCursor;
    public Texture2D handCursor;
    public Texture2D hammerCursor;
    public Texture2D spongeCursor;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        ChangeCursor(defaultCursor);
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ChangeCursor(Texture2D cursorType)
    {
        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);
    }
}
