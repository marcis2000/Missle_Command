using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, iDestroyable
{
    private Animator _animator;
    private int _animIDExplosion;

    // Start is called before the first frame update
    void Start()
    {
        AssignAnimationIDs();

    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool(_animIDExplosion, true);

    }

    private void AssignAnimationIDs()
    {
        _animIDExplosion = Animator.StringToHash("Explosion");
    }
}
