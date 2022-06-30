using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyMissile : MonoBehaviour, iDestroyable
{
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D circeClollider;

    public float missleSpeed;
    public Vector2 destination;
    public enum State
    {
        Fly,
        Explode,
    }
    public State state;

    [SerializeField] private Sprite missle;
    [SerializeField] private Sprite circle;

    private Vector2 targetSize = new Vector2(6f, 6f);
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

    public void DisactivateMissle()
    {
        transform.localScale = new Vector2(1f, 1f);
        state = State.Fly;
        circeClollider.enabled = false;
        spriteRenderer.sprite = missle;
        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        spriteRenderer.sprite = circle;
        circeClollider.enabled = true;
        state = State.Explode;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        iDestroyable destroyable = collider.GetComponent<iDestroyable>();
        if(destroyable != null)
        {
            destroyable.Destroy();
        }
    }

}
