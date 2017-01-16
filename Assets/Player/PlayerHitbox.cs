using UnityEngine;
using System.Collections;

public class PlayerHitbox : MonoBehaviour {
    public int damage;

    // Use this for initialization
    void Start () {

    }
    
    // Update is called once per frame
    void Update () {

    }

    void OnTriggerStay (Collider other) {
        if (other.gameObject.GetComponent<Vitality>() != null) {
            other.gameObject.GetComponent<Vitality>().TakeDamage(damage);
        }
    }
}
