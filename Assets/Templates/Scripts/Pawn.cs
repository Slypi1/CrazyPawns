using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private List<Connector> _connectors = new List<Connector>();
    private ConnectionManager _connectionManager;
    private Renderer[] renderers;
    private Material[] originalMaterials;
    private bool isOnBoard = true;
    
    public void Initialize(ConnectionManager connectionManager)
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length];

        _connectionManager = connectionManager;
        
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }
        
        foreach (var connector in _connectors)
        {
            connector.Initialize(connectionManager, this);
            connectionManager.RegisterConnector(connector);
        }
    }

    public void SetDeleteMaterial(Material deleteMaterial)
    {
        if (deleteMaterial != null)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.material = deleteMaterial;
            }
            isOnBoard = false;
        }
    }
    
    public void RestoreOriginalMaterials()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (originalMaterials[i] != null)
            {
                renderers[i].material = originalMaterials[i];
            }
        }
        isOnBoard = true;
    }
    
    public bool IsOnBoard()
    {
        return isOnBoard;
    }

    public void Delete()
    {
        foreach (var connector in _connectors)
        {
            connector.Remove();
        }
        
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        foreach (var connector in _connectors)
        {
            _connectionManager.UnregisterConnector(connector);
        }
    }
}
