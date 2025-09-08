using System.Collections.Generic;
using CrazyPawn;
using UnityEngine;

namespace Spawner
{
    public class PawnSpawner
    {
        private CrazyPawnSettings _settings;
        private ConnectionManager _connectionManager;
        private Pawn _pawnPrefab;
        private Transform _parent;

        private float _maxAngle = 360;

        private const float FULL_CIRCLE_RADIANS = 2f * Mathf.PI;
        private const float MAX_RANDOM_VALUE = 1f;
        
        public PawnSpawner(CrazyPawnSettings settings, Pawn pawnPrefab, Transform parent, ConnectionManager connectionManager)
        {
            _settings = settings;
            _pawnPrefab = pawnPrefab;
            _parent = parent;
            _connectionManager = connectionManager;
        }

        public void SpawnPawns()
        {
            for (int i = 0; i < _settings.InitialPawnCount; i++)
            {
                SpawnPawnInCircle();
            }
        }

        private void SpawnPawnInCircle()
        {
            Vector3 spawnPosition = GetRandomCirclePosition();
            Quaternion spawnRotation = GetRandomRotation();

            Pawn newPawn = Object.Instantiate(_pawnPrefab, spawnPosition, spawnRotation, _parent);
            
            newPawn.Initialize(_connectionManager);
            
        }

        private Vector3 GetRandomCirclePosition()
        {
            float randomRadius = Mathf.Sqrt(Random.Range(0f, MAX_RANDOM_VALUE)) * _settings.InitialZoneRadius;
            float randomAngle = Random.Range(0f, FULL_CIRCLE_RADIANS);

            Vector3 spawnPosition = new Vector3(
                Mathf.Cos(randomAngle) * randomRadius,
                0f,
                Mathf.Sin(randomAngle) * randomRadius);

            return spawnPosition;
        }
        
        private Quaternion GetRandomRotation()
        {
            float randomYRotation = Random.Range(0f, _maxAngle);
            return Quaternion.Euler(0f, randomYRotation, 0f);
        }
    }
}
