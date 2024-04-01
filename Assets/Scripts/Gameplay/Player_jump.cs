﻿using UnityEngine;

public class Player_jump : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Material trailMaterial;

    private void OnBecameInvisible()
    {
        if(gameObject!=null)
        {
         // GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
         // spriteRenderer.enabled = false;
//          GetComponent<TrailRenderer>().enabled = false;
        }
        

        if (GameManager_jump.Instance.uIManager.gameState == GameState_jump.PLAYING)
            GameManager_jump.Instance.GameOver();
    }

    //when player hit obstacle object
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Rigidbody2D tempRigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();

            //dont play hit sound on first obstacle
            if (collision.gameObject.GetComponent<Obstacle>().index == 0)
            {
                trailMaterial.color = collision.gameObject.GetComponent<SpriteRenderer>().color;
                spriteRenderer.color = collision.gameObject.GetComponent<SpriteRenderer>().color;
                return;
            }

            AudioManager_jump.Instance.PlayEffects(AudioManager_jump.Instance.hit);

            //Debug.Log(Mathf.Abs(collision.transform.position.x - transform.position.x));

            if (Mathf.Abs(collision.transform.position.x - transform.position.x) > .45f) //more than half player si over the obstacle so dont enable gravity for it
            {
                GameManager_jump.Instance.GameOver();
                GetComponent<Rigidbody2D>().angularVelocity = 0;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else
            {
                //perfect hit
                if (Mathf.Abs(collision.transform.position.x - transform.position.x) < .05f)
                {
                    AudioManager_jump.Instance.PlayEffects(AudioManager_jump.Instance.perfect);
                    transform.position = new Vector2(collision.transform.position.x, transform.position.y);
                    GameManager_jump.Instance.scoreManager.UpdateScore(2);
                    GameManager_jump.Instance.PlayPerfect();
                }
                else
                {
                    GameManager_jump.Instance.scoreManager.UpdateScore(1);
                }

                transform.rotation = new Quaternion(0, 0, 0, 0);
                GetComponent<Rigidbody2D>().angularVelocity = 0;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GameManager_jump.Instance.inAir = false;
                tempRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                tempRigidbody2D.gravityScale = 3f;
                tempRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                spriteRenderer.color = collision.gameObject.GetComponent<SpriteRenderer>().color;
                trailMaterial.color = collision.gameObject.GetComponent<SpriteRenderer>().color;
            }
        }
    }

}
