using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Vector2 Napravlenie;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sprite;

    [SerializeField]
    private bool keyboardActive;
    float playerSpeed;
    int HP;

    bool up, down, left, right;

    void Start()
    {
        Napravlenie.x = 0;
        Napravlenie.y = 0;
        playerSpeed = 40;
        //keyboardActive = false;

        animator = this.GetComponent<Animator>();
        sprite = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent< Rigidbody2D >();
    }

    void Update()
    {
        //if(Input.GetKey(KeyCode.W)) { 
        //    rb.velocity = new Vector2(rb.velocity.x, playerSpeed); 
        //}
        //if (Input.GetKey(KeyCode.A)) { 
        //    rb.velocity = new Vector2(-playerSpeed, rb.velocity.y); 
        //}
        //if (Input.GetKey(KeyCode.S)) { 
        //    rb.velocity = new Vector2(rb.velocity.x, -playerSpeed); 
        //}
        //if (Input.GetKey(KeyCode.D)) { 
        //    rb.velocity = new Vector2(playerSpeed, rb.velocity.y); 
        //}


        if (keyboardActive)
        {
            if (Input.GetKeyDown(InputController.getInput("up")) && (up == false)) 
            { 
                Napravlenie.y = Napravlenie.y + 1; 
                up = true; 
            }
            if (Input.GetKeyUp(InputController.getInput("up")) && (up == true)) 
            { 
                Napravlenie.y = Napravlenie.y - 1; 
                up = false; 
            }

            if (Input.GetKeyDown(InputController.getInput("left")) && (left == false)) 
            { 
                Napravlenie.x = Napravlenie.x - 1; 
                sprite.flipX = false; 
                left = true; 
            }
            if (Input.GetKeyUp(InputController.getInput("left")) && (left == true)) 
            { 
                Napravlenie.x = Napravlenie.x + 1; 
                left = false; 
            }

            if (Input.GetKeyDown(InputController.getInput("down")) && (down == false)) 
            { 
                Napravlenie.y = Napravlenie.y - 1; 
                down = true; 
            }
            if (Input.GetKeyUp(InputController.getInput("down")) && (down == true)) 
            { 
                Napravlenie.y = Napravlenie.y + 1; 
                down = false; 
            }

            if (Input.GetKeyDown(InputController.getInput("right")) && right == false) { Napravlenie.x = Napravlenie.x + 1; sprite.flipX = true; right = true; }
            if (Input.GetKeyUp(InputController.getInput("right")) && right == true) { Napravlenie.x = Napravlenie.x - 1; right = false; }

            if ((Input.GetKey(InputController.getInput("up")) ^ Input.GetKey(InputController.getInput("down"))) || 
                (Input.GetKey(InputController.getInput("left")) ^ Input.GetKey(InputController.getInput("right"))))
            { 
                animator.Play("Utka_go"); 
            } 
            else { animator.Play("Utka_idle"); }
        }
    }

    void FixedUpdate()
    {
        // Скорость перемещения
        rb.AddForce(Napravlenie.normalized * playerSpeed);
    }

    public void SetKeyboardActive(bool Active)
    {
        keyboardActive = Active;

        Napravlenie.x = 0;
        Napravlenie.y = 0;
        
        up = false;
        left = false;
        right = false;
        down = false;
    }
}
