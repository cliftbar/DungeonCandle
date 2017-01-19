using UnityEngine;
using System.Collections;

public class EnemyHitbox : MonoBehaviour {
    public int damage;
    public Vector2 knockback;

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    void OnTriggerStay (Collider other) {
        if (other.gameObject.name == "Player") {
            Vector2 directedKnockback = knockback;
            if (other.transform.position.x < transform.position.x) {
                directedKnockback = new Vector2(directedKnockback.x * -1, directedKnockback.y);
            }
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage, directedKnockback);
        }
    }
}
