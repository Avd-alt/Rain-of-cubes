using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
public class Bomb : MonoBehaviour
{
    private const int MinLifeTime = 2;
    private const int MaxLifeTime = 5;

    private Color _color;
    private Renderer _renderer;
    private int _radiusExplousion = 2;
    private Coroutine _coroutine;

    public event Action<Bomb> BobmDestroyed;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        _color = _renderer.material.color;
        _color.a = 1;
        _renderer.material.color = _color;

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(LaunchingDestroy());
    }

    private IEnumerator LaunchingDestroy()
    {
        int lifeTime = Random.Range(MinLifeTime, MaxLifeTime);
        float oneSecondTime = 1f;
        float numberDecreaseAlpha = oneSecondTime / lifeTime;
        var timeToDelay = new WaitForSeconds(oneSecondTime);

        while (lifeTime > 0)
        {

            yield return timeToDelay;

            DecreaseAlpha(numberDecreaseAlpha);

            lifeTime--;
        }

        ExplodeObjects();
        BobmDestroyed?.Invoke(this);
    }

    private void ExplodeObjects()
    {
        int explosionForce = 15;
        int upwardsModifier = 1;

        foreach(Rigidbody explodableObject in GetObjectsForExplode())
        {
            explodableObject.AddExplosionForce(explosionForce, transform.position, _radiusExplousion, upwardsModifier, ForceMode.Impulse);
        }
    }

    private List<Rigidbody> GetObjectsForExplode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radiusExplousion);

        List<Rigidbody> objectsRigidbodyes = new();

        if (hitColliders.Length > 0)
        {
            foreach (Collider hitObject in hitColliders)
            {
                Rigidbody objectRigidbody = hitObject.attachedRigidbody;

                if (objectRigidbody != null)
                {
                    objectsRigidbodyes.Add(objectRigidbody);
                }
            }
        }

        return objectsRigidbodyes;
    }

    private void DecreaseAlpha(float numberDecreaseAlpha)
    {
        _color.a = Mathf.Max(_color.a - numberDecreaseAlpha, 0f);

        _renderer.material.color = _color;
    }
}