using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public sealed class Tower : MonoBehaviour
{
    public static Tower This;
    public static Block[,] Map;
    public static Surface[,] Surfaces;
    public static readonly Vector2Int MapSize = new Vector2Int(16, 48); // must be even
    public static List<Worker> WorkersFirst;
    public static List<Worker> WorkersSecond;

    [SerializeField] private Camera _placersCamera1;
    [SerializeField] private Image _placersImage1;
    [SerializeField] private Text _placersText1;
    [SerializeField] private Material _placersMaterial1;
    [SerializeField] private Color _cameraColor1;
    [SerializeField] private Text _resource1;
    [SerializeField] private Camera _placersCamera2;
    [SerializeField] private Image _placersImage2;
    [SerializeField] private Text _placersText2;
    [SerializeField] private Material _placersMaterial2;
    [SerializeField] private Color _cameraColor2;
    [SerializeField] private Text _resource2;

    private static bool _shouldFall = true;

    [NotNull] private Placer _placer1;
    [NotNull] private Placer _placer2;

    public static Placer GetFirstPlacer()
    {
        return This._placer1;
    }
    
    public static Placer GetSecondPlacer()
    {
        return This._placer2;
    }

    public static void SetShouldFall(bool shouldFall)
    {
        _shouldFall = shouldFall;
    }

    public Vector2Int GetSize()
    {
        return MapSize;
    }

    public static bool CanPlace(Vector2Int position, Vector2Int blockSize, uint workersNeeded, bool first)
    {
        if ((first ? WorkersFirst : WorkersSecond).Where(worker => !(worker.GetOccupied())).Count() < workersNeeded) return false;

        if ((first ? position.x - blockSize.x < -1 : position.x + blockSize.x > MapSize.x) ||
            position.y + blockSize.y > MapSize.y || position.y <= 0)
        {
            return false;
        }

        bool hasTether = false;
        for (int x = 0; x < blockSize.x; x++)
        {
            if (!(Map[position.x + ((first ? -1 : 1) * x), position.y - 1] is null))
            {
                if (Map[position.x + ((first ? -1 : 1) * x), position.y - 1].GetPlacer().First() == first)
                {
                    hasTether = true;
                    break;
                }
            }
        }
        if (!hasTether) return false;
        
        for (int x = 0; x < blockSize.x; x++)
        {
            for (int y = 0; y < blockSize.y; y++)
            {
                if (!(Map[position.x + (x * (first ? -1 : 1)), position.y + y] is null)) return false;
            }
        }

        return true;
    }
    
    public static void ProjectBlock(Vector2Int position, Block block, BlockInfo info, bool first)
    {
        block.Locate();
        SomethingChanged();

        for (int i = 0; i < info.workersNeeded; i++)
        {
            FindNearestNonOccupiedWorker(position + new Vector2Int(info.offsets[i].x * (first ? -1 : 1), info.offsets[i].y), first).SetOccupation(block,
                position + new Vector2Int(info.offsets[i].x * (first ? -1 : 1), info.offsets[i].y),
                Worker.JobType.Build);
        }
    }

    public static void SomethingChanged()
    {
        if (_shouldFall)
        {
            for (int x = 0; x < MapSize.x; x++)
            {
                for (int y = 0; y < MapSize.y; y++)
                {
                    if (!(Map[x, y] is null)) Map[x, y].felt = false;
                }
            }

            for (int y = 1; y < MapSize.y; y++)
            {
                for (int x = 0; x < MapSize.x; x++)
                {
                    if (!(Map[x, y] is null) && !(Map[x, y] is Grapper)) Map[x, y].Fall();
                }
            }

            CalculateWeight();
        }
    }
    
    private static void CalculateWeight()
    {
        for (int x = 0; x < MapSize.x; x++)
        {
            for (int y = 0; y < MapSize.y; y++)
            {
                if (!(Map[x, y] is null))
                {
                    Map[x, y].justLoaded = false;
                    Map[x, y].load = 0;
                }
            }
        }
        
        for (int y = MapSize.y - 1; y > 0 ; y--)
        {
            for (int x = 0; x < MapSize.x; x++)
            {
                if (!(Map[x, y] is null))
                {
                    List<Block> onWhatStanding = new List<Block>();

                    for (int xAdd = 0; xAdd < Map[x, y].GetSize().x; xAdd++)
                    {
                        if (!(Map[x + xAdd, y - 1] is null))
                        {
                            onWhatStanding.Add(Map[x + xAdd, y - 1]);
                            xAdd += Map[x + xAdd, y - 1].GetSize().x;
                        }
                    }

                    foreach (Block block in onWhatStanding)
                    {
                        if (block != Map[x, y] && !block.justLoaded)
                        {
                            block.load += (Map[x, y].load + Map[x, y].mass) / onWhatStanding.Count;
                            block.justLoaded = true;
                        }
                    }

                    x += Map[x, y].GetSize().x - 1;
                }
            }
        }
        
        for (int i = MapSize.y - 1; i >= 0; i--)
        {
            string test = "";
            for (int j = 0; j < MapSize.x; j++)
            {
                test += Map[j, i] is null ? "0 " : (Map[j, i].load + "/" + Map[j, i].mass + " ");
            }
            Debug.Log(test);
        }
    }
    
    public static Surface UpdateAssistPlatform(Vector2Int position, bool first)
    {
        if (Surfaces[position.x, position.y] is null)
        {
            Surfaces[position.x, position.y] = new AssistPlatform((first ? This._placer1 : This._placer2), position, GameManager.AssistPlatform());
        }

        return Surfaces[position.x, position.y];
    }

    public static void AddWorker(Vector2Int position, bool first)
    {
        (first ? WorkersFirst : WorkersSecond).Add(new Worker((first ? This._placer1 : This._placer2), position, GameManager.Worker()));
    }
    
    public static void RemoveWorker(Worker worker, bool first)
    {
        (first ? WorkersFirst : WorkersSecond).Remove(worker);
    }

    public void AddResources(int count)
    {
        // if (count < 0) throw new ArgumentOutOfRangeException();
        //
        // _resources += count;
        // Debug.Log(_resources);
    }

    public void LateStart(SeparatedTimer firstTimer, SeparatedTimer secondTimer)
    {
        This = this;
        Map = new Block[MapSize.x, MapSize.y];
        _placersCamera1.backgroundColor = _cameraColor1;
        _placersCamera2.backgroundColor = _cameraColor2;

        _placer1 = new Placer(null, new Vector2Int(MapSize.x / 2 - 1, 0), GameManager.Placer(),
            _placersCamera1, _placersImage1, _placersText1, true, firstTimer, _placersMaterial1, _resource1);
        _placer2 = new Placer(null, new Vector2Int(MapSize.x / 2, 0), GameManager.Placer(),
            _placersCamera2, _placersImage2, _placersText2, false, secondTimer, _placersMaterial2, _resource2);
        Surfaces = new Surface[MapSize.x, MapSize.y];
        WorkersFirst = new List<Worker>();
        WorkersSecond = new List<Worker>();

        for (int x = 0; x < MapSize.x; x++)
        {
            Surfaces[x, 0] = new Ground(((x < MapSize.x / 2) ? _placer1 : _placer2), new Vector2Int(x, 0), GameManager.Ground());
            Map[x, 0] = Surfaces[x, 0];
            new WinPoint(((x < MapSize.x / 2) ? _placer2 : _placer1), new Vector2Int(x, MapSize.y - 1), GameManager.WinPoint());
        }

        for (int i = 0; i < GameManager.START_WORKERS_AMOUNT; i++)
        {
            WorkersFirst.Add(new Worker(_placer1, new Vector2Int(0, 1), GameManager.Worker()));
            WorkersSecond.Add(new Worker(_placer2, new Vector2Int(MapSize.x - 1, 1), GameManager.Worker()));
        }
    }

    private static Worker FindNearestNonOccupiedWorker(Vector2Int position, bool first)
    {
        return (Worker) ExtendedLogic.FindNearestIn((first ? WorkersFirst : WorkersSecond).Where(worker => !(worker.GetOccupied())).ToArray(), position, MapSize);
    }
}
