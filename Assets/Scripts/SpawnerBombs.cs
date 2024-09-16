using System;
using UnityEngine;

public class SpawnerBombs : Spawner<Bomb>
{
    private int _spawnCount = 0;

    public event Action<int> Spawned;

    public void GetBomb(Vector3 position)
    {
        Bomb bomb = _poolObjects.Get();
        bomb.transform.position = position;
    }

    protected override Bomb CreatePoolObjects()
    {
        Bomb bomb = Instantiate(Object);
        bomb.BobmDestroyed += ReturnObjectToPool;
        bomb.gameObject.SetActive(false);
        _spawnCount++;
        Spawned?.Invoke(_spawnCount);

        return bomb;
    }
}