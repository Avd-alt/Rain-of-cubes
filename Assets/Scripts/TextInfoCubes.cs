using TMPro;
using UnityEngine;

public class TextInfoCubes : MonoBehaviour
{
    [SerializeField] private SpawnerCubes _spawnerCubes;
    [SerializeField] private TextMeshProUGUI _objectsCreated;
    [SerializeField] private TextMeshProUGUI _objectsActivated;
    [SerializeField] private TextMeshProUGUI _objectsSpawned;

    private void Update()
    {
        _objectsActivated.text = _spawnerCubes.ActiveObjects.ToString();
        _objectsSpawned.text = _spawnerCubes.GetQuntitySpawn().ToString();
    }

    private void OnEnable()
    {
        _spawnerCubes.Spawned += ChangeDisplaySpawn;
    }
    private void OnDisable()
    {
        _spawnerCubes.Spawned -= ChangeDisplaySpawn;
    }

    private void ChangeDisplaySpawn(int spawnCount)
    {
        _objectsCreated.text = spawnCount.ToString();
    }
}