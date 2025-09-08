using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _normalMaterial;
    [SerializeField] private Material _activeMaterial;
    [SerializeField] private LayerMask _connectorLayerMask;
    [SerializeField] private Collider _collider;
    
    private ConnectionManager _connectionManager;
    private Pawn _parentPawn;
    
    private bool _isActive;
    private bool _isDragging;
    
    private List<ConnectionLine> _connectionLines = new List<ConnectionLine>();
    public void Initialize(ConnectionManager connectionManager, Pawn pawn)
    {
        _connectionManager = connectionManager;
        _parentPawn = pawn;
    }

    public void SetActive(bool active)
    {
        _isActive = active;
        _meshRenderer.material = active ? _activeMaterial : _normalMaterial;
    }
    
    public void SetConnectionLine(ConnectionLine connectionLine)
    {
        _connectionLines.Add(connectionLine);
    }
    
    public void Remove()
    {
        foreach (var connectionLine in _connectionLines )
        {
            _connectionManager.RemoveConnection(connectionLine);
        }
    }


    public Pawn GetParentPawn()
    {
        return _parentPawn;
    }

    public bool IsActive()
    {
        return _isActive;
    }

    private void OnMouseDown()
    {
        _connectionManager.OnConnectorClicked(this);
    }

    private void OnMouseDrag()
    {
        _isDragging = true;
        _collider.isTrigger = true;
    }

    private void OnMouseUp()
    {
        _collider.isTrigger = false;
        _isDragging = false;
        _connectionManager.OnConnectorReleased(this);
    }
    
  private void OnTriggerEnter(Collider other)
    {
        if (_isDragging)
        {
            return;
        }
        
        if (((1 << other.gameObject.layer) & _connectorLayerMask) != 0)
        {
            _connectionManager.OnConnectorClicked(this);
            _connectionManager.OnConnectorReleased(other.GetComponent<Connector>());
        }
    }
}
