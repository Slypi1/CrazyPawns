using System.Collections.Generic;
using CrazyPawn;
using UnityEngine;

namespace Generator
{
    public class ChessboardGenerator
    {
        public float CellSize => _cellSize;
        
        private CrazyPawnSettings _settings;
        private float _boardWorldSize;

        private float _cellSize = 1.5f;
        private float _divide = 2f;
        private float _cellY = 0.01f;
        private int _cornersPerSquare = 4;
        private int _colorAlternationModule = 2;

        private Vector2[] _сellCoordinates = new Vector2[]
        {
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f)
        };

        private static readonly int[] _triangleIndices = new int[]
        {
            0, 1, 2,
            0, 2, 3
        };

        private const string MESH_NAME = "ChessboardMesh";
        private const string GAMEOBJECT_NAME = "Chessboard";

        public ChessboardGenerator(CrazyPawnSettings settings)
        {
            _settings = settings;
        }
        
        public void GenerateChessboard()
        {
            Mesh mesh = CreateMesh();

            CreateGameObject(mesh);
        }

        public bool IsPointOnBoard(Vector3 position)
        {
            float halfSize = _boardWorldSize / _divide;
            return Mathf.Abs(position.x) <= halfSize && Mathf.Abs(position.z) <= halfSize;
        }

        private Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.name = MESH_NAME;

            int boardSize = _settings.CheckerboardSize;
            float halfCell = _cellSize / _divide;
            float boardWidth = boardSize * _cellSize;
            float startX = -boardWidth / _divide;
            float startZ = -boardWidth / _divide;


            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            List<int> triangles = new List<int>();
            List<Color> colors = new List<Color>();

            int vertexIndex = 0;

            for (int x = 0; x < boardSize; x++)
            {
                for (int z = 0; z < boardSize; z++)
                {
                    Color cellColor = GetCellColor(x, z);

                    Vector3 cellCenter = new Vector3(startX + x * _cellSize + halfCell, _cellY,
                        startZ + z * _cellSize + halfCell);

                    vertices = AddCellVertices(vertices, cellCenter, halfCell);
                    uv = AddCellUV(uv);
                    colors = AddCellColors(colors, cellColor);
                    triangles = AddCellTriangles(triangles, vertexIndex);

                    vertexIndex += _cornersPerSquare;
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.uv = uv.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.colors = colors.ToArray();

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            _boardWorldSize = boardSize * _cellSize;

            return mesh;
        }

        private Color GetCellColor(int x, int z)
        {
            bool isBlack = (x + z) % _colorAlternationModule == 0;
            return isBlack ? _settings.BlackCellColor : _settings.WhiteCellColor;
        }

        private List<Vector3> AddCellVertices(List<Vector3> vertices, Vector3 center, float halfCell)
        {
            vertices.Add(center + new Vector3(-halfCell, 0, -halfCell));
            vertices.Add(center + new Vector3(-halfCell, 0, halfCell));
            vertices.Add(center + new Vector3(halfCell, 0, halfCell));
            vertices.Add(center + new Vector3(halfCell, 0, -halfCell));

            return vertices;
        }

        private List<Vector2> AddCellUV(List<Vector2> uv)
        {
            foreach (Vector2 uvCoord in _сellCoordinates)
            {
                uv.Add(uvCoord);
            }

            return uv;
        }

        private List<Color> AddCellColors(List<Color> colors, Color cellColor)
        {
            colors.Add(cellColor);
            colors.Add(cellColor);
            colors.Add(cellColor);
            colors.Add(cellColor);

            return colors;
        }

        private List<int> AddCellTriangles(List<int> triangles, int vertexIndex)
        {
            foreach (int index in _triangleIndices)
            {
                triangles.Add(vertexIndex + index);
            }

            return triangles;
        }

        private void CreateGameObject(Mesh mesh)
        {
            GameObject board = new GameObject(GAMEOBJECT_NAME);
            board.transform.position = Vector3.zero;

            MeshFilter meshFilter = board.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            MeshRenderer meshRenderer = board.AddComponent<MeshRenderer>();

            CreateMaterial(meshRenderer);

            MeshCollider meshCollider = board.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
        }

        private void CreateMaterial(MeshRenderer meshRenderer)
        {
            Material boardMaterial = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
            boardMaterial.enableInstancing = true;
            boardMaterial.color = Color.white;
            meshRenderer.material = boardMaterial;
        }
    }
}
