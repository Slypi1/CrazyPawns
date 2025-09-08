using CrazyPawn;
using Generator;
using Spawner;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private CrazyPawnSettings _settings;
    [Header("PawnSettings")]
    [SerializeField] private Pawn _pawnPrefab;
    [SerializeField] private Transform _parentPawn;
    [Header("Controllers")]
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private ConnectionManager _connectionManager;
    
    
    private ChessboardGenerator _chessboardGenerator;
    private PawnSpawner _pawnSpawner;
    private InputManager _inputManager;
    private DragAndDrop _dragAndDrop;
  

    private void Start()
    {
        _chessboardGenerator = new ChessboardGenerator(_settings);
        _pawnSpawner = new PawnSpawner(_settings, _pawnPrefab, _parentPawn, _connectionManager);
        _inputManager = new InputManager();
        _dragAndDrop = new DragAndDrop(_chessboardGenerator, _settings);
      

        _chessboardGenerator.GenerateChessboard();
        _pawnSpawner.SpawnPawns();
    }

    private void Update()
    {
        if (_inputManager.MouseLeftDown)
        {
            _dragAndDrop.StartPreciseDrag();
        }

        if (_inputManager.MouseLeftUp)
        {
            _dragAndDrop.EndDrag();
        }

        if (_inputManager.MouseRightDown)
        {
            _cameraController.BeginCameraDrag();
        }

        if (_inputManager.MouseRightUp)
        {
            _cameraController.EndCameraDrag();
        }
        
        _dragAndDrop.Update();
        _cameraController.HandleCameraDrag();
        _connectionManager.UpdateConnectionLines();
        _cameraController.HandleCameraZoom(_inputManager.MouseScrollWheel);
    }
}
