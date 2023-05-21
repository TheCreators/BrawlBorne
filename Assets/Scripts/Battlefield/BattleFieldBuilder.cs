using System.Collections;
using System.Collections.Generic;
using Environment;
using Misc;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

namespace Battlefield
{
    public class BattleFieldBuilder : MonoBehaviour
    {
        [Header("Objects")] [SerializeField] private GameObject _wall;
        [SerializeField] private GameObject _border;
        [SerializeField] private GameObject _grass;
        [SerializeField] private Crate _crate;
        [SerializeField] private GameObject _ground;
        [SerializeField] private List<Hero> _bots;

        [Header("Settings")] [SerializeField] [Range(30, 30)]
        private int _fieldLength = 30;

        [SerializeField] [Range(30, 30)] private int _fieldWidth = 30;
        [SerializeField] [Range(2, 2)] private int _tileSize = 2;
        [SerializeField] [Range(0, 0.2f)] private float _wallDensity = 0.05f;
        [SerializeField] [Range(0, 0.3f)] private float _bushesDensity = 0.03f;
        [SerializeField] [Range(0, 6)] private int _maxBushSize = 3;
        [SerializeField] [Range(0, 20)] private int _potsCount = 9;
        [SerializeField] [Range(1, 30)] private int _heroesCount = 15;
        [SerializeField] [Range(3, 8)] private int _spawnDistance = 8;

        private readonly List<NavMeshSurface> _navMeshSurfaces = new();
        private List<Hero> Heroes { get; } = new();
        private List<Crate> Crates { get; } = new();
        private BattleField _map;
        private BattleFieldGenerator _generator;
        private readonly Random _rnd = new();
        private float _xCorrection;
        private float _zCorrection;
        
        private void OnValidate()
        {
            this.CheckIfNull(_wall, _border, _grass, _ground);
            this.CheckIfNull(_crate);
        }

        private void Start()
        {
            _xCorrection = -_fieldLength * _tileSize;
            _zCorrection = -_fieldWidth * _tileSize;
            BuildTerrain();
            BakeNavMesh();
            
            Invoke(nameof(Spawn), 0.1f);
        }

        private void Spawn()
        {
            PlaceBots();
            ObjectsPool.Instance.SetPlayerSpawnPosition(GetPlayerSpotCoordinates());
            ObjectsPool.Instance.SetHeroes(Heroes);
            ObjectsPool.Instance.SetCrates(Crates);
            ObjectsPool.Instance.InstantiatePlayer();
        }

        private void BuildTerrain()
        {
            GameObject field = Instantiate(_ground, new Vector3(0, 0, 0), Quaternion.identity);
            _navMeshSurfaces.Add(field.GetComponentInChildren<NavMeshSurface>());
            do
            {
                _generator = new BattleFieldGenerator(new BattleField(_fieldLength, _fieldWidth), _wallDensity,
                        _bushesDensity, _maxBushSize, _potsCount, _heroesCount, _spawnDistance)
                    .GenerateExternalWalls()
                    .GenerateWalls()
                    .DeleteSingleWalls()
                    .FillEmpties()
                    .AddBushes()
                    .AddPots()
                    .MakeSymmetric();
            } while (!_generator.HasGround());

            _map = _generator.AddHeroesSpots().BuildMap();
            
            for (int i = 0; i < _map.Rows; i++)
            {
                for (int j = 0; j < _map.Cols; j++)
                {
                    GameObject createdObject;
                    switch (_map[i, j])
                    {
                        case 1:
                            createdObject = Instantiate(_wall,
                                new Vector3(_xCorrection + i * _tileSize, 0, _zCorrection + j * _tileSize),
                                Quaternion.identity);
                            _navMeshSurfaces.Add(createdObject.GetComponent<NavMeshSurface>());
                            break;
                        case 2:
                            createdObject = Instantiate(_wall,
                                new Vector3(_xCorrection + i * _tileSize, 0, _zCorrection + j * _tileSize),
                                Quaternion.identity);
                            Instantiate(_border,
                                new Vector3(_xCorrection + i * _tileSize, 0, _zCorrection + j * _tileSize),
                                Quaternion.identity);
                            // _navMeshSurfaces.Add(createdObject.GetComponent<NavMeshSurface>());
                            break;
                        case 3:
                            Instantiate(_grass,
                                new Vector3(_xCorrection + i * _tileSize, 0, _zCorrection + j * _tileSize),
                                Quaternion.identity);
                            break;
                        case 5:
                            Crates.Add(Instantiate(_crate,
                                new Vector3(_xCorrection + i * _tileSize, 1, _zCorrection + j * _tileSize),
                                Quaternion.identity));
                            break;
                    }
                }
            }
        }

        private Hero GetRandomBot()
        {
            return _bots[_rnd.Next(_bots.Count)];
        }

        private Vector3 GetPlayerSpotCoordinates()
        {
            for (int i = 0; i < _map.Rows; i++)
            {
                for (int j = 0; j < _map.Cols; j++)
                {
                    if (_map[i, j] == 6)
                    {
                        return new Vector3(_xCorrection + i * _tileSize, 0, _zCorrection + j * _tileSize);
                    }
                }
            }

            return new Vector3(0, 0, 0);
        }

        private void PlaceBots()
        {
            for (int i = 0; i < _map.Rows; i++)
            {
                for (int j = 0; j < _map.Cols; j++)
                {
                    if (_map[i, j] == 4)
                    {
                        Heroes.Add(Instantiate(GetRandomBot(),
                            new Vector3(_xCorrection + i * _tileSize, 0, _zCorrection + j * _tileSize),
                            Quaternion.identity));
                    }
                }
            }
        }

        private void BakeNavMesh()
        {
            foreach (var navMeshSurface in _navMeshSurfaces)
            {
                navMeshSurface.BuildNavMeshAsync();
            }
        }
    }
}