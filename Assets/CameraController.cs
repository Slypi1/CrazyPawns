using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom;
    [SerializeField] private Vector2 _cameraBounds;
    
    private Vector3 dragOrigin;
    private bool isDragging = false;
    private Camera cam;
    
    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    

    public void BeginCameraDrag()
    {
        dragOrigin = GetMouseWorldPosition();
        isDragging = true;
        HandleCameraDrag();
    }

    public void EndCameraDrag()
    {
        isDragging = false;
    }
    
    public void HandleCameraDrag()
    {
        if (isDragging)
        {
            Vector3 currentMousePos = GetMouseWorldPosition();
            Vector3 difference = dragOrigin - currentMousePos;
            
            Vector3 newPosition = transform.position + difference;
            newPosition.y = transform.position.y; 
            
            newPosition.x = Mathf.Clamp(newPosition.x, -_cameraBounds.x, _cameraBounds.x);
            newPosition.z = Mathf.Clamp(newPosition.z, -_cameraBounds.y, _cameraBounds.y);
            
            transform.position = newPosition;
        }
        
    }
    
    public void HandleCameraZoom(float scroll)
    {
        if (Mathf.Abs(scroll) > 0.01f)
        {
            ZoomTowardsCursor(scroll);
        }
    }
    
    private void ZoomTowardsCursor(float scrollDelta)
    {
        if (cam == null) return;
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        
        if (plane.Raycast(ray, out distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            
            
            Vector3 direction = (targetPoint - transform.position).normalized;
            
          
            float zoomAmount = scrollDelta * _zoomSpeed;
            Vector3 newPosition = transform.position + direction * zoomAmount;
            
            float currentHeight = newPosition.y;
            float clampedHeight = Mathf.Clamp(currentHeight, _minZoom, _maxZoom);
            
            if (currentHeight != clampedHeight)
            {
                newPosition = transform.position + direction * (clampedHeight - transform.position.y);
            }
            
            newPosition.y = clampedHeight;
            
            transform.position = newPosition;
        }
    }
    
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        
        return Vector3.zero;
    }
}
