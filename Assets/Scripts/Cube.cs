using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    private const int MinLifeTime = 2;
    private const int MaxLifeTime = 5;

    private Color _defaultColor = Color.black;
    private MeshRenderer _meshRenderer;
    private Coroutine _destroyingCoroutine;
    private bool _isTouching;

    public event Action<Cube> CubeDestroyed;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        _meshRenderer.material.color = _defaultColor;
    }

    private void OnCollisionEnter(Collision platformCollision)
    {
        if (platformCollision.collider.TryGetComponent(out Platform platform))
        {
            if (!_isTouching)
            {
                _isTouching = true;

                _meshRenderer.material.color = Color.red;

                if (_destroyingCoroutine != null)
                {
                    StopCoroutine(_destroyingCoroutine);
                }

                _destroyingCoroutine = StartCoroutine(LaunchingDestroy());
            }
        }
    }

    private IEnumerator LaunchingDestroy()
    {
        int lifeTime = Random.Range(MinLifeTime, MaxLifeTime);
        int oneSecondTime = 1;
        var timeToDelay = new WaitForSeconds(oneSecondTime);

        while(lifeTime > 0)
        {
            yield return timeToDelay;

            lifeTime--;
        }

        _isTouching = false;
        _meshRenderer.material.color = _defaultColor;

        CubeDestroyed?.Invoke(this);
    }
}