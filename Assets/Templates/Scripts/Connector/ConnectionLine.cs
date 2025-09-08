using UnityEngine;

public class ConnectionLine : MonoBehaviour
{
    [SerializeField] private float _width;
    [SerializeField] private LineRenderer _lineRenderer;
    
    private Connector _startConnector;
    private Connector _endConnector;
    
    public void Initialize(Connector start, Connector end)
    {
        _lineRenderer.startWidth = _width;
        _lineRenderer.endWidth = _width;
        
        _startConnector = start;
        _endConnector = end;
        
        UpdatePositions();
    }

    public void UpdatePositions()
    {
        if (_startConnector != null && _endConnector != null)
        {
            _lineRenderer.SetPosition(0, _startConnector.transform.position);
            _lineRenderer.SetPosition(1, _endConnector.transform.position);
        }
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
