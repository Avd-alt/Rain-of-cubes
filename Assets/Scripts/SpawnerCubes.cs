using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerCubes : Spawner<Cube>
{
    [SerializeField] private SpawnerBombs _spawnerBombs;

    private Coroutine _spawningCoroutine;
    private int _spawnCount = 0;

    public event Action<int> Spawned;

    private void Start()
    {
        if (_spawningCoroutine != null)
        {
            StopCoroutine(_spawningCoroutine);
        }

       _spawningCoroutine = StartCoroutine(SpawningCubes());
    }

    protected override Cube CreatePoolObjects()
    {
        Cube cube = Instantiate(Object);
        cube.CubeDestroyed += ReturnObjectToPool;
        cube.gameObject.SetActive(false);
        _spawnCount++;
        Spawned?.Invoke(_spawnCount);

        return cube;
    }

    protected override void TakeObject(Cube cube)
    {
        base.TakeObject(cube);
        int minValue = -9;
        int maxValue = 9;

        cube.transform.position = new Vector3(Random.Range(minValue, maxValue), PointSpawner.transform.position.y, Random.Range(minValue, maxValue));
    }

    protected override void ReturnObjectToPool(Cube figure)
    {
        _poolObjects.Release(figure);
        _spawnerBombs.GetBomb(figure.transform.position);
    }

    private IEnumerator SpawningCubes()
    {
        float timeToDelay = 0.1f;

        var delay = new WaitForSeconds(timeToDelay);

        while (true)
        {
            yield return delay;

            _poolObjects.Get();

        }
    }
}