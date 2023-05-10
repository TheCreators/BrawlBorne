using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleFieldBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject wall;
    public GameObject grass;
    public GameObject bottle;
    public GameObject obstacle;
    public GameObject ground;

    private int fieldLength = 30;
    private int fieldWidth = 30;
    private List<NavMeshSurface> navMeshSurfaces= new List <NavMeshSurface>();
    void Start()
    {
        Build();
        BakeNavMesh();
    }

    void Build()
    {
        GameObject fiel = Instantiate(ground, new Vector3(0, 0, 0), Quaternion.identity);
        navMeshSurfaces.Add(fiel.GetComponentInChildren<NavMeshSurface>());
        float wallSize = 2;

        BattleField map = new BattleFieldGenerator(new BattleField(fieldLength, fieldWidth)).GenerateExternalWalls()
            .GenerateWalls()
            .DeleteSingleWalls().AddBushes().AddPots().BuildMap();

        float xCorrection = -fieldLength * wallSize;
        float zCorrection = -fieldWidth * wallSize;


        GameObject createdObject;
        for (int i = 0; i < map.Cols; i++)
        {
            for (int j = 0; j < map.Rows; j++)
            {
                switch (map[i, j])
                {
                    case 1:
                        createdObject = Instantiate(wall,
                            new Vector3(xCorrection + i * wallSize, 0, zCorrection + j * wallSize),
                            Quaternion.identity);
                        navMeshSurfaces.Add((createdObject.GetComponentInChildren<NavMeshSurface>()));
                        break;
                    case 2:
                        createdObject = Instantiate(wall,
                            new Vector3(xCorrection + i * wallSize, 0, zCorrection + j * wallSize),
                            Quaternion.identity);
                        navMeshSurfaces.Add((createdObject.GetComponentInChildren<NavMeshSurface>()));
                        break;
                    case 3:
                        Instantiate(grass,
                            new Vector3(xCorrection + i * wallSize, 0, zCorrection + j * wallSize),
                            Quaternion.identity);
                        break;
                    case 5:
                        Instantiate(bottle, new Vector3(xCorrection + i * wallSize, 0, zCorrection + j * wallSize),
                            Quaternion.identity);
                        break;
                    case 6:
                        createdObject = Instantiate(obstacle,
                            new Vector3(xCorrection + i * wallSize, 0, zCorrection + j * wallSize),
                            Quaternion.identity);
                        navMeshSurfaces.Add((createdObject.GetComponentInChildren<NavMeshSurface>()));
                        break;
                }
            }
        }
    }

    void BakeNavMesh()
    {
        for (int i = 0; i < navMeshSurfaces.Count; i++)
        {
            navMeshSurfaces[i].BuildNavMesh();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}