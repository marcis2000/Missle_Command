using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Building : MonoBehaviour, iDestroyable
{
    public event EventHandler OnBuildingDestroyed;

    private Animator _animator;
    private int _animIDExplosion;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }
    void Start()
    {
        AssignAnimationIDs();
    }

    private void AssignAnimationIDs()
    {
        _animIDExplosion = Animator.StringToHash("Explosion");
    }

    public void Destroy()
    {
        _animator.SetBool(_animIDExplosion, true);
        StartCoroutine(WaitForAnimationToFinish());

    }

    private IEnumerator WaitForAnimationToFinish()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
        OnBuildingDestroyed?.Invoke(this, EventArgs.Empty);
    }
}
