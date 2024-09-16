using TMPro;
using UnityEngine;

public class TextInfoBombs : MonoBehaviour
{
    [SerializeField] private SpawnerBombs _spawnerBombs;
    [SerializeField] private TextMeshProUGUI _objectsCreated;
    [SerializeField] private TextMeshProUGUI _objectsActivated;
    [SerializeField] private TextMeshProUGUI _objectsSpawned;

    private void Update()
    {
        _objectsActivated.text = _spawnerBombs.ActiveObjects.ToString();
        _objectsSpawned.text = _spawnerBombs.GetQuntitySpawn().ToString();
    }

    private void OnEnable()
    {
        _spawnerBombs.Spawned += ChangeDisplaySpawn;
    }
    private void OnDisable()
    {
        _spawnerBombs.Spawned -= ChangeDisplaySpawn;
    }

    private void ChangeDisplaySpawn(int spawnCount)
    {
        _objectsCreated.text = spawnCount.ToString();
    }
}