using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField] private ConnectionLine _connectionLinePrefab;

    private Material _activeConnectorMaterial;
    
    private Connector _firstSelectedConnector;
    private Connector _currentDraggedConnector;
    private bool _isInConnectionMode;
    
    private List<ConnectionLine> _connectionLines = new List<ConnectionLine>();
    private List<Connector> _allConnectors = new List<Connector>();
    
    public void RegisterConnector(Connector connector)
    {
        if (!_allConnectors.Contains(connector))
        {
            _allConnectors.Add(connector);
        }
    }

    public void UnregisterConnector(Connector connector)
    {
        if (_allConnectors.Contains(connector))
        {
            _allConnectors.Remove(connector);
        }
    }

    public void OnConnectorClicked(Connector connector)
    {
        if (!_isInConnectionMode)
        {
            StartConnectionMode(connector);
        }
        else
        {
            if (connector != _firstSelectedConnector && IsValidConnection(_firstSelectedConnector, connector))
            {
                CreateConnection(_firstSelectedConnector, connector);
            }
            EndConnectionMode();
        }
    }
    
    public void OnConnectorReleased(Connector connector)
    {
        if (_isInConnectionMode && _currentDraggedConnector != null)
        {
            if (connector != _firstSelectedConnector && IsValidConnection(_firstSelectedConnector, connector))
            {
                CreateConnection(_firstSelectedConnector, connector);
            }
            
            EndConnectionMode();
        }
        
        _currentDraggedConnector = null;
    }

    public void StartConnectionMode(Connector connector)
    {
        _isInConnectionMode = true;
        _firstSelectedConnector = connector;
        
        foreach (var conn in _allConnectors)
        {
            if (conn != _firstSelectedConnector && IsValidConnection(_firstSelectedConnector, conn))
            {
                conn.SetActive(true);
            }
        }
    }

    public void EndConnectionMode()
    {
        _isInConnectionMode = false;
        
        foreach (var connector in _allConnectors)
        {
            connector.SetActive(false);
        }
        
        _firstSelectedConnector = null;
    }

    private bool IsValidConnection(Connector connector1, Connector connector2)
    {
        return connector1.GetParentPawn() != connector2.GetParentPawn();
    }

    private void CreateConnection(Connector connectorBegin, Connector connectorEnd)
    {
        ConnectionLine line = Instantiate(_connectionLinePrefab);
        
        line.Initialize(connectorBegin, connectorEnd);
        
        _connectionLines.Add(line);
        
        connectorBegin.SetConnectionLine(line);
        connectorEnd.SetConnectionLine(line);
    }
    
    public void UpdateConnectionLines()
    {
        foreach (var line in _connectionLines)
        {
            ConnectionLine connectionLine = line.GetComponent<ConnectionLine>();
            if (connectionLine != null)
            {
                connectionLine.UpdatePositions();
            }
        }
    }

    public void RemoveConnection(ConnectionLine connectionLine)
    {
        if (_connectionLines.Contains(connectionLine))
        {
            _connectionLines.Remove(connectionLine);
            connectionLine.Delete();
        }
    }
}

