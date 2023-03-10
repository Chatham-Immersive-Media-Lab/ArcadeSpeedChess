using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Chess
{

    public class GridManager : MonoBehaviour
    {
        [SerializeField] private int width, height;
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private Transform cam;
        private Dictionary<Vector2Int, Tile> _tiles;
        [SerializeField] private Transform background;

        //generates grid layout (adjustable in the editor)
        public void GenerateGrid()
        {
            _tiles = new Dictionary<Vector2Int, Tile>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var spawnedTile = Instantiate(tilePrefab, GridToWorld(x,y), quaternion.identity);
                    spawnedTile.name = $"Tile {x} {y}";


                    spawnedTile.Init(new Vector2Int(x, y), this);

                    _tiles[new Vector2Int(x, y)] = spawnedTile;
                }
            }

            //transforms the camera position to center around the created grid
            cam.transform.position = new Vector3((float) width / 2 - 0.5f, (float) height / 2 - 0.5f, -10f);
            background.transform.position = new Vector3((float) width / 2 - 0.5f, (float) height / 2 - 0.5f, 0f);
        }

        public static Vector3 GridToWorld(Vector2Int pos)
        {
            return GridToWorld(pos.x, pos.y);
        }
        public static Vector3 GridToWorld(int x, int y)
        {
            return new Vector3(x, y, 0);
        }

        public Tile GetTileAtPosition(Vector2Int pos)
        {
            //if tile is available return the tile
            if (_tiles.TryGetValue(pos, out var tile))
            {
                return tile;
            }

            return null;
        }
    }
}