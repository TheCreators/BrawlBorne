using System;
using UnityEngine;

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

    void Start()
    {
        Build();
    }

    void Build()
    {
        float wallSize = 2;

        BattleField map = new BattleFieldGenerator(new BattleField(fieldLength, fieldWidth)).GenerateExternalWalls().GenerateWalls()
            .DeleteSingleWalls().AddBushes().AddPots().BuildMap();
        
        float xCorrection = -fieldLength * wallSize;
        float zCorrection = -fieldWidth * wallSize;
        Instantiate(ground, new Vector3(0, 0, 0),Quaternion.identity);
        for (int i = 0; i < map.Cols; i++)
        {
            for (int j = 0; j < map.Rows; j++)
            {
                switch (map[i, j])
                {
                    case 1:
                        Instantiate(wall, new Vector3(xCorrection + i * wallSize, 0, zCorrection + j * wallSize),
                            Quaternion.identity);
                        break;
                    case 2:
                        Instantiate(wall, new Vector3(xCorrection + i * wallSize, 0, zCorrection + j * wallSize),
                            Quaternion.identity);
                        break;
                    case 3:
                        Instantiate(grass, new Vector3(xCorrection + i * wallSize, 0, zCorrection + j * wallSize),
                            Quaternion.identity);
                        break;
                    case 5:
                        Instantiate(bottle, new Vector3(xCorrection + i * wallSize, 0, zCorrection + j * wallSize),
                            Quaternion.identity);
                        break;
                    case 6:
                        Instantiate(obstacle, new Vector3(xCorrection + i * wallSize, 0, zCorrection + j * wallSize),
                            Quaternion.identity);
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}