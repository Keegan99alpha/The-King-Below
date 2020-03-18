using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //map requirments, tile prefabs and the map array.
    [SerializeField]
    private BasePieceClass[] terrainPrefabs;
    private int randPrefab;
    [SerializeField]
    private BasePieceClass defaultPrefab;
    [SerializeField]
    private BasePieceClass centerPrefab;
    [SerializeField]
    private int mapSize = 1;
    private BasePieceClass[,] mapSpace;

    //generator coords to and sizes of tiles to neatly and tightly make the map
    private Vector3 origin;
    private Vector3 shiftMap;
    private Vector3 prefabSize;
    private MeshRenderer mesh;

    void Start()
    {
        //Ensures at least one prefab type is loaded
        mapSpace = new BasePieceClass[mapSize, mapSize];
        if (defaultPrefab == null)
        {
            defaultPrefab = (DefaultClass)Resources.Load("Prefabs/Terrain/DefaultTerrainPrefab", typeof(DefaultClass));
        }
        if (terrainPrefabs.Length == 0)
        {
            terrainPrefabs = new BasePieceClass[1];
        }
        for (int i = 0; i < terrainPrefabs.Length; i++)
        {
            if (terrainPrefabs[i] == null)
            {
                //default to a basic cube if no custom one is allocated
                terrainPrefabs[i] = (DefaultClass)Resources.Load("Prefabs/Terrain/DefaultTerrainPrefab", typeof(DefaultClass));
            }
        }
        //use the mesh size and generator scale to find the actual size of tiles
        mesh = defaultPrefab.GetComponent<MeshRenderer>();
        prefabSize = Vector3.Scale(transform.localScale, mesh.bounds.size);
        //use map size and tile size to modify the origin so that the puzzle is centered around the generator
        origin = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        if ((mapSize % 2).Equals(0))
        {
            origin.x -= ((mapSize / 2) * prefabSize.x) - prefabSize.x / 2;
            origin.z -= ((mapSize / 2) * prefabSize.z) - prefabSize.z / 2;
        }
        else
        {
            origin.x -= (mapSize / 2) * prefabSize.x;
            origin.z -= (mapSize / 2) * prefabSize.z;
        }
        shiftMap = origin;
       
        //generate tiles from the shift away from the origin
        for (int i = 0; i < mapSpace.GetLength(0); i++)
        {
            for (int j = 0; j < mapSpace.GetLength(1); j++)
            {
                randPrefab = Random.Range(0, terrainPrefabs.Length);
                BasePieceClass clone;
                //create a cube at the generators position as a child of the generator and then set the next column
                //create a default tile at center
                if (i == mapSpace.GetLength(0) / 2 && j == mapSpace.GetLength(1) / 2)
                {
                    //can only assign tile type, can't modify it's bevahiour as the map is of BaseClass and will disregard any modifications while cast
                    //modify the center tile/any tile in said tiles script
                    clone = (CenterPiece)Instantiate(centerPrefab, shiftMap, Quaternion.identity, gameObject.transform);
                }
                else
                {
                    clone = Instantiate(terrainPrefabs[randPrefab], shiftMap, Quaternion.identity, gameObject.transform);
                }
                mapSpace[i, j] = clone;
                shiftMap.x += prefabSize.x;
            }
            //reset columns and move to the next row
            shiftMap.x = origin.x;
            shiftMap.z += prefabSize.z;
        }

        //establish neighbours and whether the tile is a corner/edge
        for (int i = 0; i < mapSpace.GetLength(0); i++)
        {
            for (int j = 0; j < mapSpace.GetLength(1); j++)
            {
                int emptyNeighbours = 4;
                if (j - 1 >= 0 && mapSpace[i, j - 1] != null)
                {
                    mapSpace[i, j].above = mapSpace[i, j - 1];
                    emptyNeighbours -= 1;
                }
                if (j + 1 < mapSpace.GetLength(1) && mapSpace[i, j + 1] != null)
                {
                    mapSpace[i, j].above = mapSpace[i, j + 1];
                    emptyNeighbours -= 1;
                }
                if (i - 1 >= 0 && mapSpace[i - 1, j] != null)
                {
                    mapSpace[i, j].above = mapSpace[i - 1, j];
                    emptyNeighbours -= 1;
                }
                if (i + 1 < mapSpace.GetLength(0) && mapSpace[i + 1, j] != null)
                {
                    mapSpace[i, j].above = mapSpace[i + 1, j];
                    emptyNeighbours -= 1;
                }

                if (emptyNeighbours > 0)
                {
                    mapSpace[i, j].border = true;
                }
                if (emptyNeighbours > 1)
                {
                    mapSpace[i, j].corner = true;
                }
            }
        }
    }
}
