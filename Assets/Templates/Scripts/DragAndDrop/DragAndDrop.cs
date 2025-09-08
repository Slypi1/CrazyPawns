using CrazyPawn;
using Generator;
using UnityEngine;

public class DragAndDrop 
{
    private ChessboardGenerator _chessboardGenerator;
    private CrazyPawnSettings _settings;
    
    private Pawn _selectedPawn;
    private Vector3 _initialMousePosition;
    private Vector3 _initialPawnPosition;
    private bool _isDragging;
    
    private float _halfMultiplier = 2f;
    

    public DragAndDrop(ChessboardGenerator chessboardGenerator, CrazyPawnSettings settings)
    {
        _chessboardGenerator = chessboardGenerator;
        _settings = settings;
    }
    
    public void Update()
    {
        if (_isDragging)
        {
            PreciseMove();
            CheckBoardBoundaries();
        }
    }
    
    public void StartPreciseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            Pawn pawn= hit.collider.GetComponentInParent<Pawn>();
            
            if (pawn != null)
            {
                _selectedPawn = pawn;
                _isDragging = true;
                
                _initialMousePosition = GetMouseWorldPosition();
                _initialPawnPosition = pawn.transform.position;
            }
        }
    }
    
    private void PreciseMove()
    {
        if (_selectedPawn == null)
        {
            return;
        }
        
        Vector3 currentMousePos = GetMouseWorldPosition();
        Vector3 mouseDelta = currentMousePos - _initialMousePosition;
        
        Vector3 targetPosition = _initialPawnPosition + mouseDelta;
        targetPosition.y = 0f; 
        
        _selectedPawn.transform.position = targetPosition;
    }
    
    private void CheckBoardBoundaries()
    {
        if (_selectedPawn == null)
        {
            return;
        }
        
        bool isOutsideBoard = IsOutsideBoard(_selectedPawn.transform.position);
        
        if (isOutsideBoard && _selectedPawn.IsOnBoard())
        {
            _selectedPawn.SetDeleteMaterial(_settings.DeleteMaterial);
        }
        else if (!isOutsideBoard && !_selectedPawn.IsOnBoard())
        {
            _selectedPawn.RestoreOriginalMaterials();
        }
    }
    
    public void EndDrag()
    {
        if (!_isDragging) return;
        
        if (_selectedPawn != null && IsOutsideBoard(_selectedPawn.transform.position))
        {
            _selectedPawn.Delete();
        }
        
        _isDragging = false;
        _selectedPawn = null;
    }
    
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        
        return Vector3.zero;
    }
    
    private bool IsOutsideBoard(Vector3 position)
    {
        if (_chessboardGenerator != null)
        {
            return !_chessboardGenerator.IsPointOnBoard(position);
        }
        
        float boardHalfSize = (_settings.CheckerboardSize * _chessboardGenerator.CellSize) / _halfMultiplier;
        
        return Mathf.Abs(position.x) > boardHalfSize || Mathf.Abs(position.z) > boardHalfSize;
    }
}
