using System.Collections.Generic;
using Environment;
using Heroes;
using Heroes.Bot;
using Misc;
using Misc.Validators;
using Models;
using NaughtyAttributes;
using Unity.AI.Navigation;
using UnityEngine;
using Random = System.Random;

namespace Battlefield
{
    public class BattleFieldBuilder : MonoBehaviour
    {
        [SerializeField] [BoxGroup(Group.Prefabs)] [Required] [ShowAssetPreview]
        private GameObject _wall;

        [SerializeField] [BoxGroup(Group.Prefabs)] [Required] [ShowAssetPreview]
        private GameObject _border;

        [SerializeField] [BoxGroup(Group.Prefabs)] [Required] [ShowAssetPreview]
        private GameObject _grass;

        [SerializeField] [BoxGroup(Group.Prefabs)] [Required] [ShowAssetPreview]
        private Crate _crate;

        [SerializeField] [BoxGroup(Group.Prefabs)] [Required] [ShowAssetPreview]
        [ValidateInput(nameof(Has.NavMeshSurface), "Ground must have NavMeshSurface component")]
        private GameObject _ground;

        [SerializeField] [BoxGroup(Group.Prefabs)]
        private List<Bot> _bots;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(15, 60)]
        private int _fieldLength = 30;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(15, 60)]
        private int _fieldWidth = 30;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 0.2f)]
        private float _wallDensity = 0.05f;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 0.3f)]
        private float _bushesDensity = 0.03f;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 6)]
        private int _maxBushSize = 3;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 20)]
        private int _cratesCount = 9;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(1, 30)]
        private int _heroesCount = 15;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(3, 8)]
        private int _spawnDistance = 8;

        private const int TileSize = 2;

        private List<Hero> Heroes { get; } = new();
        private List<Crate> Crates { get; } = new();

        private BattleField _map;
        private BattleFieldGenerator _generator;
        private Random _random;
        private Transform _environmentTransform;
        private NavMeshSurface _navMeshSurface;

        private float _xCorrection;
        private float _zCorrection;

        private void OnValidate()
        {
            this.CheckIfNull(_wall, _border, _grass, _ground);
            this.CheckIfNull(_crate);
        }

        private void Awake()
        {
            _xCorrection = -_fieldLength * TileSize;
            _zCorrection = -_fieldWidth * TileSize;
            _random = new Random();
            _environmentTransform = new GameObject("Environment").transform;
        }

        private void Start()
        {
            BuildTerrain();
            BakeNavMesh();
            PlaceBots();
            ObjectsPool.Instance.SetPlayerSpawnPosition(GetPlayerSpotCoordinates());
            ObjectsPool.Instance.SetHeroes(Heroes);
            ObjectsPool.Instance.SetCrates(Crates);
            ObjectsPool.Instance.InstantiatePlayer();
        }

        private void BuildTerrain()
        {
            GameObject field = Instantiate(_ground, new Vector3(-1, 0, -1), Quaternion.identity, _environmentTransform);
            _navMeshSurface = field.GetComponent<NavMeshSurface>();

            do
            {
                _generator = new BattleFieldGenerator(new BattleField(_fieldLength, _fieldWidth), _wallDensity,
                        _bushesDensity, _maxBushSize, _cratesCount, _heroesCount, _spawnDistance)
                    .GenerateExternalWalls()
                    .GenerateWalls()
                    .DeleteSingleWalls()
                    .FillEmpties()
                    .AddBushes()
                    .AddPots()
                    .MakeSymmetric();
            } while (!_generator.HasGround());

            _map = _generator.AddHeroesSpots().BuildMap();

            Transform wallsTransform = new GameObject("Walls").transform;
            wallsTransform.parent = _environmentTransform;
            Transform bushesTransform = new GameObject("Bushes").transform;
            bushesTransform.parent = _environmentTransform;
            Transform cratesTransform = new GameObject("Crates").transform;
            cratesTransform.parent = _environmentTransform;

            for (int i = 0; i < _map.Rows; i++)
            {
                for (int j = 0; j < _map.Cols; j++)
                {
                    switch (_map[i, j])
                    {
                        case 1:
                            Instantiate(
                                _wall,
                                new Vector3(_xCorrection + i * TileSize, 0, _zCorrection + j * TileSize),
                                Quaternion.identity,
                                wallsTransform);
                            break;
                        case 2:
                            Instantiate(
                                _wall,
                                new Vector3(_xCorrection + i * TileSize, 0, _zCorrection + j * TileSize),
                                Quaternion.identity,
                                wallsTransform);
                            Instantiate(
                                _border,
                                new Vector3(_xCorrection + i * TileSize, 0, _zCorrection + j * TileSize),
                                Quaternion.identity,
                                wallsTransform);
                            break;
                        case 3:
                            Instantiate(
                                _grass,
                                new Vector3(_xCorrection + i * TileSize, 0, _zCorrection + j * TileSize),
                                Quaternion.identity,
                                bushesTransform);
                            break;
                        case 5:
                            Crates.Add(Instantiate(
                                _crate,
                                new Vector3(_xCorrection + i * TileSize, 1, _zCorrection + j * TileSize),
                                Quaternion.identity,
                                cratesTransform));
                            break;
                    }
                }
            }
        }

        private Bot GetRandomBot()
        {
            return _bots[_random.Next(_bots.Count)];
        }

        private Vector3 GetPlayerSpotCoordinates()
        {
            for (int i = 0; i < _map.Rows; i++)
            {
                for (int j = 0; j < _map.Cols; j++)
                {
                    if (_map[i, j] == 6)
                    {
                        return new Vector3(_xCorrection + i * TileSize, 0, _zCorrection + j * TileSize);
                    }
                }
            }

            return new Vector3(0, 0, 0);
        }

        private void PlaceBots()
        {
            Transform botsTransform = new GameObject("Bots").transform;

            for (int i = 0; i < _map.Rows; i++)
            {
                for (int j = 0; j < _map.Cols; j++)
                {
                    if (_map[i, j] == 4)
                    {
                        Heroes.Add(Instantiate(
                            GetRandomBot(),
                            new Vector3(_xCorrection + i * TileSize, 0, _zCorrection + j * TileSize),
                            Quaternion.identity,
                            botsTransform));
                    }
                }
            }
        }

        private void BakeNavMesh()
        {
            _navMeshSurface.BuildNavMesh();
        }
    }
}