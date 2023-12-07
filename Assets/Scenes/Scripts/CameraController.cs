using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float scrollSpeed = 5f; // The speed at which the camera scrolls
    public float followSpeed = 5f; // The speed at which the camera follows the mouse

    [SerializeField] private float minX; //Set the min x position of the camera in the inspector
    [SerializeField] private float maxX; //Set the max x position of the camera in the inspector
    [SerializeField] private float minY; //Set the min y position of the camera in the inspector
    [SerializeField] private float maxY; //Set the max y position of the camera in the inspector

    private bool isMouseAtScreenEdge = false; // A flag to determine if the mouse is at the screen edge

    void Update()
    {
        HandleMouseMovementInput();
        HandleMouseScrollInput();
    }

    void HandleMouseMovementInput()
    {
        Vector3 mousePosition = Input.mousePosition;
        CheckMouseAtScreenEdge(mousePosition);

        //Check if the mouse is at the screen edge
        if (isMouseAtScreenEdge)
        {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            //Clamp the mouse position to the min and max x and y values
            worldMousePosition.x = Mathf.Clamp(worldMousePosition.x, minX, maxX);
            worldMousePosition.y = Mathf.Clamp(worldMousePosition.y, minY, maxY);

            //move the camera towards the mouse position
            transform.position = Vector3.Lerp(transform.position, worldMousePosition, followSpeed * Time.deltaTime);
        }
    }

    void HandleMouseScrollInput()
    {
        // Get the mouse scroll input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Calculate the movement vector based on mouse scroll input
        Vector3 scrollDirection = new Vector3(0f, 0f, scrollInput);
        transform.Translate(scrollDirection * scrollSpeed * Time.deltaTime, Space.World);
    }

    void CheckMouseAtScreenEdge(Vector3 mousePosition)
    {
        float edgeThreshold = 20f; // A threshold to determine if the mouse is at the screen edge. A lowe number is higher sensitivity
        // Check if the mouse is at the screen edge
        bool nearLeftEdge = mousePosition.x <= edgeThreshold;
        bool nearRightEdge = mousePosition.x >= Screen.width - edgeThreshold;
        bool nearBottomEdge = mousePosition.y <= edgeThreshold;
        bool nearTopEdge = mousePosition.y >= Screen.height - edgeThreshold;

        //set the flag based on the mouse position
        isMouseAtScreenEdge = nearLeftEdge || nearRightEdge || nearBottomEdge || nearTopEdge;

    }
}
