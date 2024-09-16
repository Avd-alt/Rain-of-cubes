using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T Object;
    [SerializeField] protected Transform PointSpawner;

    protected int _defaultCapacity = 20;
    protected int _maxSize = 100;
    protected ObjectPool<T> _poolObjects;

    private int _quntitySpawn;

    public int ActiveObjects { get; private set; }

    public int GetQuntitySpawn() => _quntitySpawn;

    private void Awake()
    {
        _poolObjects = new ObjectPool<T>(
            createFunc: () => CreatePoolObjects(),
            actionOnGet: (T) => TakeObject(T),
            actionOnRelease: (T) => T.gameObject.SetActive(false),
            actionOnDestroy: (T) => Destroy(T),
            collectionCheck: true,
            defaultCapacity: _defaultCapacity,
            maxSize: _maxSize);
    }

    private void Update()
    { 
        ActiveObjects = _poolObjects.CountActive;
    }

    protected abstract T CreatePoolObjects();

    protected virtual void TakeObject(T figure)
    {
        _quntitySpawn++;
        figure.gameObject.SetActive(true);
    }

    protected virtual void ReturnObjectToPool(T figure)
    {
        _poolObjects.Release(figure);
    }
}