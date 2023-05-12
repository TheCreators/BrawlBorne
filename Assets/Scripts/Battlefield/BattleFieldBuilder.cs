using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.AI;

namespace Battlefield
{
    public class BattleFieldBuilder : MonoBehaviour
    {
        [Header("Objects")] [SerializeField] private GameObject _wall;
        [SerializeField] private GameObject _grass;
        [SerializeField] private GameObject _bottle;
        [SerializeField] private GameObject _ground;
        [SerializeField] private Hero _bot;

        [Header("Settings")] [SerializeField] private int _fieldLength = 30;
        [SerializeField] private int _fieldWidth = 30;
        [SerializeField] private int _tileSize = 2;

        private readonly List<NavMeshSurface> _navMeshSurfaces = new List<NavMeshSurface>();
        public List<Hero> Heroes { get; private set; } = new();
        private BattleField map;

        private void Start()
        {
            BuildTerrain();
            BakeNavMesh();
            PlaceBots();
            HeroesPool.Instance.SetHeroes(Heroes);
        }

        private void BuildTerrain()
        {
            GameObject field = Instantiate(_ground, new Vector3(0, 0, 0), Quaternion.identity);
            _navMeshSurfaces.Add(field.GetComponentInChildren<NavMeshSurface>());

            map = new BattleFieldGenerator(new BattleField(_fieldLength, _fieldWidth))
                .GenerateExternalWalls()
                .GenerateWalls()
                .DeleteSingleWalls()
                .AddBushes()
                .AddPots()
                .MakeSymmetric()
                .AddHeroesSpots()
                .BuildMap();

            float xCorrection = -_fieldLength * _tileSize;
            float zCorrection = -_fieldWidth * _tileSize;

            for (int i = 0; i < map.Cols; i++)
            {
                for (int j = 0; j < map.Rows; j++)
                {
                    GameObject createdObject;
                    switch (map[i, j])
                    {
                        case 1:
                            createdObject = Instantiate(_wall,
                                new Vector3(xCorrection + i * _tileSize, 0, zCorrection + j * _tileSize),
                                Quaternion.identity);
                            _navMeshSurfaces.Add(createdObject.GetComponent<NavMeshSurface>());
                            break;
                        case 2:
                            createdObject = Instantiate(_wall,
                                new Vector3(xCorrection + i * _tileSize, 0, zCorrection + j * _tileSize),
                                Quaternion.identity);
                            _navMeshSurfaces.Add(createdObject.GetComponent<NavMeshSurface>());
                            break;
                        case 3:
                            Instantiate(_grass,
                                new Vector3(xCorrection + i * _tileSize, 0, zCorrection + j * _tileSize),
                                Quaternion.identity);
                            break;
                        case 5:
                            Instantiate(_bottle,
                                new Vector3(xCorrection + i * _tileSize, 0, zCorrection + j * _tileSize),
                                Quaternion.identity);
                            break;
                    }
                }
            }
        }
        
        private void PlaceBots()
        {
            float xCorrection = -_fieldLength * _tileSize;
            float zCorrection = -_fieldWidth * _tileSize;
            for (int i = 0; i < map.Cols; i++)
            {
                for (int j = 0; j < map.Rows; j++)
                {
                    if (map[i, j] == 4)
                    {
                        Heroes.Add(Instantiate(_bot,
                            new Vector3(xCorrection + i * _tileSize, 0, zCorrection + j * _tileSize),
                            Quaternion.identity));
                        break;
                    }
                    
                }
            }
        }

        private void BakeNavMesh()
        {
            foreach (var navMeshSurface in _navMeshSurfaces)
            {
                navMeshSurface.BuildNavMesh();
            }
        }
    }
}
