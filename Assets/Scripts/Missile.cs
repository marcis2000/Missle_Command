using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

   //Missile which can be friendly or hostile.
   //Friendly missiles use markers to show where their explosion begins.

public class Missile : MonoBehaviour, iDestroyable
{
    public GameObject Marker;
    public CircleCollider2D CirceClollider;
    public SpriteRenderer SpriteRenderer;
    public Vector2 Destination;
    public float MissleSpeed;
    public bool IsFriendly;

    public event EventHandler OnAddMissilePoints;
    public event EventHandler OnAddToDestroyedEnemyMissilesCount;

    public enum State
    {
        Fly,
        Explode,
    }
    public State state;


    [SerializeField] private Sprite _missle;
    [SerializeField] private Sprite _circle;

    [SerializeField] private Vector2 _targetSize = new Vector2(8f, 8f);
    [SerializeField] private float _growSpeed = 6f;
    [SerializeField] private float _explosionTimer = 2f;

    private float _timer = 0;

    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CirceClollider = GetComponent<CircleCollider2D>();
        CirceClollider.enabled = false;
        SpriteRenderer.sprite = _missle;
    }

    void OnEnable()
    {
        transform.localScale = new Vector2(0.5f, 0.5f);
        state = State.Fly;
    }

    void FixedUpdate()
    {
       if(state == State.Fly)
       {
            transform.position = Vector2.MoveTowards(transform.position, Destination, MissleSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, Destination) < 0.001f)
            {
                SpriteRenderer.sprite = _circle;
                CirceClollider.enabled = true;
                transform.localScale = new Vector2(0.5f, 0.5f);
                state = State.Explode;
                if(Marker!= null)
                {
                    Marker.SetActive(false);
                }
            }
        }
       else if(state == State.Explode)
       {
            transform.localScale = Vector2.MoveTowards(transform.localScale, _targetSize, _growSpeed * Time.deltaTime);
            _timer += Time.deltaTime;
            if(_timer >= _explosionTimer)
            {
                _timer = 0;
                DisactivateMissle();
            }
       }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        iDestroyable destroyable = collider.GetComponent<iDestroyable>();
        if (destroyable != null)
        {

            // Avoids hitting allies.

            if (collider.tag == "Spaceship" && !IsFriendly)   
            {
                return;
            }
            else if (collider.tag == "Missile" && SpriteRenderer.sprite == _missle &&
                     collider.GetComponent<Missile>().SpriteRenderer.sprite == _missle)
            {
                return;
            }
            else
            {
                destroyable.Destroy();   // Destroys all objects inheriting from IDestroyable
            }
        }
    }

    // Disactivates missile after explosion so it can be pooled again.
    public void DisactivateMissle()
    {
        if (!IsFriendly)
        {
            OnAddToDestroyedEnemyMissilesCount?.Invoke(this, EventArgs.Empty);
        }
        transform.localScale = new Vector2(1f, 1f);
        CirceClollider.enabled = false;
        SpriteRenderer.sprite = _missle;
        state = State.Fly;
        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        SpriteRenderer.sprite = _circle;
        CirceClollider.enabled = true;
        state = State.Explode;
        if (!IsFriendly)
        {
            OnAddMissilePoints?.Invoke(this, EventArgs.Empty);
        }
    }

}
