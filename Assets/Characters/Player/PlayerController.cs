using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    private SpriteRenderer sr;
    private SpriteRenderer srCandle;
    private TerrainDetector gd;
    private TerrainDetector cd;
    private Light li;
    private Animator anim;
    private UIController ui;
    private SceneController sc;

    public GameObject flameAttack;

    // Life/defense variables:
    public int maxLife;
    public int currentLife;
    private bool flinching;
    private float flinchTimestamp;
    public float flinchTime;
    private bool flinchInvuln;
    private float flinchInvulnTimestamp;
    public float flinchInvulnTime;
    private bool dying;
    public float deathTime;
    private float deathTimestamp;

    // Move variables:
    private float moveInput;
    public float maxSpeed;
    public float joystickDeadzone;
    public float accel;
    public float airAccel;
    public float friction;

    // Jump variables:
    private bool jumping;
    private float jumpTimestamp;
    public float jumpSpeed;
    public float jumpTime;

    // Candle/attack variables:
    private bool attacking;
    private bool flameTriggered;
    private bool attackEnabled;
    private float attackTimestamp;
    private bool candleLit;
    public float flameSpeed;
    public float flameDelay;
    public float attackTime;
    public float coolDown;

    // Lighting variables:
    private bool roomLit = false;
    public int illumCount;

    // Count of rooms player currently occupies:
    public int roomCount;

    void Awake () {
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        li = GetComponentInChildren<Light>();
        foreach (Transform child in transform) {
            if (child.GetComponent<SpriteRenderer>() != null) {
                srCandle = child.GetComponent<SpriteRenderer>();
            }
            if (child.gameObject.name == "Ground Detector") {
                gd = child.gameObject.GetComponent<TerrainDetector>();
            } else if (child.gameObject.name == "Ceiling Detector") {
                cd = child.gameObject.GetComponent<TerrainDetector>();
            }
        }

        // External components:
        ui = GameObject.Find("UI Canvas").GetComponent<UIController>();
    }

    // Use this for initialization
    void Start () {
        sc = GameObject.Find("Scene Controller").GetComponent<SceneController>();

        attackEnabled = true;
        candleLit = true;

        ui.UpdateLife(currentLife, maxLife);
    }
    
    // Update is called once per frame
    void Update () {
        // IF PAUSED == FALSE:
        if (flinching == false && attacking == false) {
            GetInput();
        } else {
            moveInput = 0f;
        }

        UpdateAnimation();
    }

    void FixedUpdate () {
        // Movement physics:
        if (Mathf.Abs(moveInput) >= 0.2) {
            Move();
        } else if (gd.Detected() == true) {
            ApplyFriction(friction);
        }

        // Jumping physics:
        if (jumping == true) {
            if (Time.time >= jumpTimestamp + jumpTime) {
                StopJump();
            } else {
                ContinueJump();
            }
        }

        //Attack processing:
        if (Time.time >= attackTimestamp + flameDelay && attacking == true && flameTriggered == true) {
            BreatheFlame();
        }

        if (Time.time >= attackTimestamp + attackTime && attacking == true) {
            StopAttack();
        }

        if (Time.time >= attackTimestamp + coolDown && attackEnabled == false) {
            EndCoolDown();
        }

        // Flinch processing:
        if (flinching == true) {
            if (Time.time > flinchTimestamp + flinchTime || (Time.time > flinchTimestamp + flinchTime / 2f && gd.Detected() == true)) {
                StopFlinch();
            }
        }

        if (flinchInvuln == true && Time.time > flinchInvulnTimestamp + flinchInvulnTime) {
            StopFlinchInvuln();
        }

        // Death processing:
        if (dying == true && Time.time >= deathTimestamp + deathTime) {
            FinishDeath();
        }
    }

    //-----------//
    // GET INPUT //
    //-----------//

    void GetInput () {
        if (attackEnabled == true && candleLit == true) {
            GetCandleInput();
        }

        if (gd.Detected() == true || jumping == true) {
            GetJumpInput();
        }

        GetMoveInput();
    }

    void GetCandleInput () {
        if (Input.GetButtonDown("Candle")) {
            StartAttack();
        }
    }

    void GetJumpInput () {
        if (Input.GetButtonDown("Jump") && jumping == false && gd.Detected() == true) {
            StartJump();
        }

        if (Input.GetButtonUp("Jump") && jumping == true) {
            StopJump();
        }
    }

    void GetMoveInput () {
        moveInput = Input.GetAxis("Horizontal");
    }

    //--------//
    // MOTION //
    //--------//

    void Move () {
        if ((-1f * maxSpeed < rb.velocity.x && moveInput < 0) || (rb.velocity.x < maxSpeed && moveInput > 0)) {
            anim.SetBool("moving", true);
            if (gd.Detected() == true) {
                rb.AddForce(moveInput * accel, 0f, 0f);
            } else {
                rb.AddForce(moveInput * airAccel, 0f, 0f);
            }
        }
    }

    public void TurnAround () {
        sr.flipX = !sr.flipX;
        srCandle.flipX = !srCandle.flipX;

        foreach (Transform child in transform) {
            child.localPosition = new Vector3(-1 * child.localPosition.x, child.localPosition.y, child.localPosition.z);
        }
    }

    void ApplyFriction (float amount) {
        anim.SetBool("moving", false);
        if (rb.velocity.magnitude >= 0.5f) {
            rb.velocity *= amount;
        } else {
            rb.velocity *= 0f;
        }
    }

    void StartJump () {
        jumping = true;
        jumpTimestamp = Time.time;
        rb.velocity = new Vector3 (rb.velocity.x, jumpSpeed, 0f);
    }

    void ContinueJump () {
        if (cd.Detected() == true) {
            jumping = false;
            rb.velocity = new Vector3 (rb.velocity.x, rb.velocity.y * -0.1f, 0f);
        } else {
            rb.velocity = new Vector3 (rb.velocity.x, jumpSpeed, 0f);   
        }
    }

    void StopJump () {
        rb.velocity = new Vector3 (rb.velocity.x, jumpSpeed / 2, 0f);
        jumping = false;
    }

    //---------------//
    // CANDLE/ATTACK //
    //---------------//

    void StartAttack () {
        attacking = true;
        flameTriggered = true;
        attackEnabled = false;
        anim.SetBool("attacking", true);
        anim.SetTrigger("attack");

        attackTimestamp = Time.time;
    }

    void BreatheFlame () {
        flameTriggered = false;
        anim.SetTrigger("candleOff");
        if (candleLit == true) {
            PutCandleOut();
        }

        // Create fire attack:
        GameObject newFlame = (GameObject)Instantiate(flameAttack, transform.position, Quaternion.identity);
        newFlame.name = "Player Flame Attack";

        float direction = 1f;
        if (sr.flipX == true) {
            direction *= -1f;
        }
        newFlame.GetComponent<Rigidbody>().velocity = new Vector3(flameSpeed * direction, 0f, 0f);

        PlayerFlameAttack newFlameController = newFlame.GetComponent<PlayerFlameAttack>();
        if (sr.flipX == true) {
            newFlameController.TurnAround();
        }
        newFlameController.Initialize(transform);
    }

    void StopAttack () {
        attacking = false;
        anim.SetBool("attacking", false);
    }

    void EndCoolDown () {
        attackEnabled = true;
    }

    public void PutCandleOut () {
        candleLit = false;
        srCandle.enabled = false;

        li.range = 5f;
        li.intensity = 1f;
    }

    public void LightCandle () {
        candleLit = true;
        srCandle.enabled = true;
        anim.SetTrigger("candleOn");

        li.range = 20f;
    }

    public bool CandleLit () {
        return candleLit;
    }

    //-----------//
    // ANIMATION //
    //-----------//

    void UpdateAnimation () {
        anim.SetBool("grounded", gd.Detected());

        if (sr.flipX == false) {
            anim.SetFloat("velocityX", rb.velocity.x);
        } else {
            anim.SetFloat("velocityX", -1 * rb.velocity.x);
        }
        
        anim.SetFloat("velocityY", rb.velocity.y);

        if (flinching == false && attacking == false) {
            if (rb.velocity.x < 0 && Input.GetAxis("Horizontal") < -1 * joystickDeadzone && sr.flipX == false) {
                TurnAround();
            } else if (rb.velocity.x > 0 && Input.GetAxis("Horizontal") > joystickDeadzone && sr.flipX == true) {
                TurnAround();
            }
        }
    }

    //-------------//
    // TAKE DAMAGE //
    //-------------//

    public void TakeDamage (int amount, Vector2 knockback) {
        if (flinchInvuln == false && dying == false) {
            currentLife = Mathf.Max(currentLife - amount, 0);
            ui.UpdateLife(currentLife, maxLife);

            rb.velocity = new Vector3(0f, 0f, 0f);

            if (currentLife == 0) {
                StartDeath();
            } else {
                StartFlinch(knockback);
            }
        }
    }

    void StartFlinch (Vector2 knockback) {
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.AddForce(knockback.x, knockback.y, 0f, ForceMode.Impulse);

        if (knockback.x < 0 && sr.flipX == true) {
            TurnAround();
        } else if (knockback.x > 0 && sr.flipX == false) {
            TurnAround();
        }

        jumping = false;
        flinching = true;
        flinchInvuln = true;
        anim.SetTrigger("flinch");
        anim.SetBool("flinching", true);
        flinchTimestamp = Time.time;
        flinchInvulnTimestamp = Time.time;
    }

    void StopFlinch () {
        flinching = false;
        anim.SetBool("flinching", false);
    }

    void StopFlinchInvuln () {
        flinchInvuln = false;
    }

    void StartDeath () {
        deathTimestamp = Time.time;
        dying = true;
    }

    void FinishDeath () {
        sc.Respawn();
    }

    //----------------//
    // CHECK LIGHTING //
    //----------------//

    public void SetRoomLighting (bool lit) {
        roomLit = lit;
    }

    public bool Illuminated () {
        return roomLit || candleLit || illumCount > 0;
    }
}
