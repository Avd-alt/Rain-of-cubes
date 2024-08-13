using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    [SerializeField] private Transform _pointSpawner;

    private ObjectPool<Cube> _poolCubes;
    private int _defaultCapacity = 20;
    private int _maxSize = 100;
    private Coroutine _spawningCoroutine;

    private void Awake()
    {
        _poolCubes = new ObjectPool<Cube>(
            createFunc: () => CreatePoolCubes(),
            actionOnGet: (cube) => TakeCube(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _defaultCapacity,
            maxSize: _maxSize);
    }

    private void Start()
    {
        if (_spawningCoroutine == null)
        {
            StartCoroutine(SpawningCubes());
        }
    }

    private Cube CreatePoolCubes()
    {
        Cube cube = Instantiate(_cube);
        cube.CubeDestroyed += ReturnCubeToPool;
        cube.gameObject.SetActive(false);

        return cube;
    }

    private void TakeCube(Cube cube)
    {
        int minValue = -9;
        int maxValue = 9;

        cube.transform.position = new Vector3(Random.Range(minValue, maxValue), _pointSpawner.transform.position.y, Random.Range(minValue, maxValue));
        cube.gameObject.SetActive(true);
    }

    private void ReturnCubeToPool(Cube cube)
    {
        _poolCubes.Release(cube);
    }

    private IEnumerator SpawningCubes()
    {
        float timeToDelay = 0.1f;

        var delay = new WaitForSeconds(timeToDelay);

        while (true)
        {
            yield return delay;
            _poolCubes.Get();
        }
    }
}