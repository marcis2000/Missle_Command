using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyMissle : MonoBehaviour
{
    [SerializeField] Transform cannonTransform;
    [SerializeField] Sprite missle;
    [SerializeField] Sprite circle;


    private Vector2 destination;
    private Vector2 targetSize = new Vector2(6f, 6f);

    private float missleSpeed;
    private float growSpeed = 6f;
    private float explosionTimer = 2f;
    private float timer = 0;


    private SpriteRenderer spriteRenderer;
    private CircleCollider2D collider;

    private enum State
    {
        Fly,
        Explode,
    }
    private State state;

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        collider = this.GetComponent<CircleCollider2D>();
    }

    void FixedUpdate()
    {
       if(state == State.Fly)
       {
            transform.position = Vector3.MoveTowards(transform.position, destination, missleSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, destination) < 0.001f)
            {
                spriteRenderer.sprite = circle;
                collider.enabled = true;
                this.transform.localScale = new Vector2(0.5f, 0.5f);
                state = State.Explode;
            }
        }
       else if(state == State.Explode)
       {
            this.transform.localScale = Vector2.MoveTowards(transform.localScale, targetSize, growSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            if(timer >= explosionTimer)
            {
                timer = 0;
                DisactivateMissle();
            }
       }
    }

    public void SetMissleDestination(Vector3 missleDestination, float speed)
    {
        spriteRenderer.sprite = missle;
        transform.localScale = new Vector2(0.5f, 0.5f);
        collider.enabled = false;
        destination = missleDestination;
        missleSpeed = speed;
        transform.position = cannonTransform.position;
        state = State.Fly;
    }

    public void DisactivateMissle()
    {
        transform.localScale = new Vector2(1f, 1f);
        state = State.Fly;
        collider.enabled = false;
        this.gameObject.SetActive(false);
    }


}
