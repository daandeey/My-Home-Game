﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpVelocity;
    public Vector2 velocity;

    public float bounceVelocity;
    public float gravity;
    public LayerMask wallMask;
    public LayerMask floorMask;
    private bool walk, walk_left, walk_right, jump;

    public enum PlayerState {
        jumping,
        idle,
        walking,
        bouncing
    }

    private PlayerState playerstate = PlayerState.idle;

    private bool grounded = false;

    private bool bounce = false;

    private int score;

    public Text scoreText;

    // Start is called before the first frame update
    void Start() {
        //Fall();
        score = 0;
        SetScoreText();
    }

    // Update is called once per frame
    void Update() {
        CheckPlayerInput();
        UpdatePlayerPosition();
        UpdateAnimationStates();
    }

    void UpdatePlayerPosition() {
        Vector3 pos = transform.localPosition;
        Vector3 scale = transform.localScale;

        if (walk) {
            if (walk_left) {
                pos.x -= velocity.x * Time.deltaTime;
                scale.x = -1;
            }
            if (walk_right) {
                pos.x += velocity.x * Time.deltaTime;
                scale.x = 1;
            }

            pos = CheckWallRays(pos, scale.x);
        }

        if (jump && playerstate != PlayerState.jumping) {

            playerstate = PlayerState.jumping;

            velocity = new Vector2(velocity.x, jumpVelocity);
        }

        if (playerstate == PlayerState.jumping) {

            pos.y += velocity.y * Time.deltaTime;

            velocity.y -= gravity * Time.deltaTime;
        }

        if (bounce && playerstate != PlayerState.bouncing) {
            playerstate = PlayerState.bouncing;

            velocity = new Vector2 (velocity.x, bounceVelocity);
        }

        if (playerstate == PlayerState.bouncing) {

            pos.y += velocity.y * Time.deltaTime;

            velocity.y -= gravity * Time.deltaTime;
        }

        if (velocity.y <= 0) pos = CheckFloorRays(pos);

        if (velocity.y >= 0) pos = CheckCeilingRays(pos);

        transform.localPosition = pos;
        transform.localScale = scale;
    }

    void UpdateAnimationStates() {
        if (grounded && !walk && !bounce) {
            GetComponent<Animator>().SetBool("isJumping", false);
            GetComponent<Animator>().SetBool("isRunning", false);
        }

        if (grounded && walk) {
            GetComponent<Animator>().SetBool("isJumping", false);
            GetComponent<Animator>().SetBool("isRunning", true);
        }

        if (playerstate == PlayerState.jumping) {
            GetComponent<Animator>().SetBool("isJumping", true);
            GetComponent<Animator>().SetBool("isRunning", false);
        }
    }

    void CheckPlayerInput() {
        bool input_left = Input.GetKey(KeyCode.LeftArrow);
        bool input_right = Input.GetKey(KeyCode.RightArrow);
        bool input_space = Input.GetKey(KeyCode.Space);
        
        walk = input_left || input_right;
        walk_left = input_left && !input_right;
        walk_right = input_right && !input_left;
        jump = input_space;
    }

    Vector3 CheckWallRays (Vector3 pos, float direction) {
        Vector2 originTop = new Vector2(pos.x + direction * .4f, pos.y + 1f - 0.2f);
        Vector2 originMiddle = new Vector2 (pos.x + direction * .4f, pos.y);
        Vector2 originBottom = new Vector2 (pos.x + direction * .4f, pos.y - 1f + 0.2f);

        RaycastHit2D wallTop = Physics2D.Raycast (originTop, new Vector2 (direction, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallMiddle = Physics2D.Raycast (originMiddle, new Vector2 (direction, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallBottom = Physics2D.Raycast (originBottom, new Vector2 (direction, 0), velocity.x * Time.deltaTime, wallMask);
    
        if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null) {

            pos.x -= velocity.x * Time.deltaTime * direction;
        }

        return pos;
    }

    Vector3 CheckFloorRays (Vector3 pos) {

        Vector2 originLeft = new Vector2 (pos.x - 0.5f + 0.2f, pos.y - 1f);
        Vector2 originMiddle = new Vector2 (pos.x, pos.y - 1f);
        Vector2 originRight = new Vector2 (pos.x + 0.5f - 0.2f, pos.y - 1f);

        RaycastHit2D floorLeft = Physics2D.Raycast (originLeft, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorMiddle = Physics2D.Raycast (originMiddle, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorRight = Physics2D.Raycast (originRight, Vector2.down, velocity.y * Time.deltaTime, floorMask);

        if (floorLeft.collider != null || floorMiddle.collider != null || floorRight.collider != null) {

            RaycastHit2D hitRay = floorRight;

            if (floorLeft) {
                hitRay = floorLeft;
            } else if (floorMiddle) {
                hitRay = floorMiddle;
            } else if (floorRight) {
                hitRay = floorRight;
            }

            if (hitRay.collider.tag == "Enemy") {

                bounce = true;
                hitRay.collider.GetComponent<EnemyAI>().Crush();
            }

            playerstate = PlayerState.idle;

            grounded = true;

            velocity.y = 0;

            pos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + 1;
        } else {

            if (playerstate != PlayerState.jumping) {

                Fall();
            }
        }

        return pos;
    }

    Vector3 CheckCeilingRays (Vector3 pos) {
        
        Vector2 originLeft = new Vector2 (pos.x - 0.5f + 0.2f, pos.y + 1f);
        Vector2 originMiddle = new Vector2 (pos.x, pos.y + 1f);
        Vector2 originRight = new Vector2 (pos.x + 0.5f - 0.2f, pos.y + 1f);

        RaycastHit2D ceilLeft = Physics2D.Raycast (originLeft, Vector2.up, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D ceilMiddle = Physics2D.Raycast (originMiddle, Vector2.up, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D ceilRight = Physics2D.Raycast (originRight, Vector2.up, velocity.y * Time.deltaTime, floorMask);

        if (ceilLeft.collider != null || ceilMiddle .collider != null || ceilRight.collider != null) {

            RaycastHit2D hitRay = ceilLeft;

            if (ceilLeft) {
                hitRay = ceilLeft;
            } else if (ceilMiddle) {
                hitRay = ceilMiddle;
            } else if (ceilRight) {
                hitRay = ceilRight;
            }

            if (hitRay.collider.tag == "QuestionBlock") {

                hitRay.collider.GetComponent<QuestionBlock>().QuestionBlockBounce();
            }

            if (hitRay.collider.tag == "QuestionBlock") {
                score = score + 100;
                SetScoreText();
            }

            pos.y = hitRay.collider.bounds.center.y - hitRay.collider.bounds.size.y / 2 - 1;
            Fall();
        }

        return pos;
    }

    void Fall() {
        velocity.y = 0;

        playerstate = PlayerState.jumping;

        bounce = false;
        grounded = false;
    }

    void SetScoreText () {
        scoreText.text = "Score: " + score.ToString();
        PlayerPrefs.SetInt("Player Score", score);
    }

    public int GetScore () {
        return score;
    }
}
