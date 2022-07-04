using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Missile : MonoBehaviour, iDestroyable
{
    public GameObject marker;
    public CircleCollider2D circeClollider;
    public SpriteRenderer spriteRenderer;
    public Vector2 destination;
    public float missleSpeed;
    public bool isFriendly;

    public event EventHandler OnAddMissilePoints;
    public event EventHandler OnAddToDestroyedEnemyMissilesCount;

    public enum State
    {
        Fly,
        Explode,
    }
    public State state;


    [SerializeField] private Sprite missle;
    [SerializeField] private Sprite circle;

    private Vector2 targetSize = new Vector2(8f, 8f);
    private float growSpeed = 6f;
    private float explosionTimer = 2f;
    private float timer = 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circeClollider = GetComponent<CircleCollider2D>();
        circeClollider.enabled = false;
        spriteRenderer.sprite = missle;
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
            transform.position = Vector3.MoveTowards(transform.position, destination, missleSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, destination) < 0.001f)
            {
                spriteRenderer.sprite = circle;
                circeClollider.enabled = true;
                transform.localScale = new Vector2(0.5f, 0.5f);
                state = State.Explode;
                if(marker!= null)
                {
                    marker.SetActive(false);
                }
            }
        }
       else if(state == State.Explode)
       {
            transform.localScale = Vector2.MoveTowards(transform.localScale, targetSize, growSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            if(timer >= explosionTimer)
            {
                timer = 0;
                DisactivateMissle();
            }
       }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        iDestroyable destroyable = collider.GetComponent<iDestroyable>();
        if (destroyable != null)
        {
            if (collider.tag == "Spaceship" && !isFriendly)
            {
                return;
            }
            else if (collider.tag == "Missile" && spriteRenderer.sprite == missle &&
                     collider.GetComponent<Missile>().spriteRenderer.sprite == missle)
            {
                return;
            }
            else
            {
                destroyable.Destroy();
            }
        }
    }

    //public void SetMissileParameters(Vector3 start ,Vector2 target, float speed, bool isMissleHostile, Color color, GameObject missileMarker)
    //{
    //    transform.position = start;
    //    destination = target;
    //    missleSpeed = speed;
    //    spriteRenderer.color = color;
    //    isFriendly = isMissleHostile;
    //    circeClollider.enabled = isMissleHostile;
    //    marker = missileMarker;

    //    float targetDirectionX = target.x - transform.position.x;
    //    float targetDirectionY = target.y - transform.position.y;
    //    float angle = Mathf.Atan2(targetDirectionY, targetDirectionX) * Mathf.Rad2Deg;
    //    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 180));
    //}

    public void DisactivateMissle()
    {
        if (!isFriendly)
        {
            OnAddToDestroyedEnemyMissilesCount?.Invoke(this, EventArgs.Empty);
        }
        transform.localScale = new Vector2(1f, 1f);
        circeClollider.enabled = false;
        spriteRenderer.sprite = missle;
        state = State.Fly;
        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        spriteRenderer.sprite = circle;
        circeClollider.enabled = true;
        state = State.Explode;
        if (!isFriendly)
        {
            OnAddMissilePoints?.Invoke(this, EventArgs.Empty);
        }
    }

}
