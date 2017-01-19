using UnityEngine;
using System.Collections;

public class Vitality : MonoBehaviour {
    private Animator anim;

    public int maxLife;
    public int currentLife;

    private float deathTimestamp;
    private bool dying;
    public float deathTime;

    private float hitTimestamp;
    public float flinchTime;

    public bool deathOnTerrainTouch;

    public GameObject deathObject;

    void Awake () {
        anim = GetComponent<Animator>();
        hitTimestamp = -1 * flinchTime;
    }

    // Use this for initialization
    void Start () {
    
    }
    
    void FixedUpdate () {
        if (dying == true && Time.time >= deathTimestamp + deathTime) {
            Die();
        }
    }

    void OnTriggerEnter (Collider other) {
        if (deathOnTerrainTouch == true && other.gameObject.tag == "Terrain") {
            StartDeath();
        }
    }

    public void TakeDamage (int amount) {
        if (Time.time >= hitTimestamp + flinchTime && dying == false) {

            hitTimestamp = Time.time;
            currentLife = Mathf.Max(currentLife - amount, 0);

            if (currentLife <= 0) {
                StartDeath();
            } else {
                Flinch();
            }
        }
    }

    void StartDeath () {
        dying = true;
        anim.SetTrigger("die");
        deathTimestamp = Time.time;
        foreach (Transform child in transform) {
            if (child.gameObject.name.Contains("Hitbox")) {
                Destroy(child.gameObject);
            }
        }

        if (deathObject != null) {
            Instantiate(deathObject, transform.position, Quaternion.identity);
        }
    }

    void Flinch () {
        
    }

    void Die () {     
        Destroy(gameObject);
    }
}
