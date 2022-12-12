using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaciour : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Vector3 movement;
    public float dashPower = 25f;
    // Start is called before the first frame update
    void Start()
    {
        //grab the rigidbody
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        //Debug.Log(direction);
        direction.Normalize();
        movement = direction;
    }

    private void FixedUpdate()
    {
        moveCharacter(movement);
    }

    void moveCharacter(Vector3 direction)
    {
        rb.MovePosition((Vector3)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    //trigger enemy to drive at opponent quickly when the two are in close proximity
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rb.AddForce(player.position * dashPower);
        }
    }
}
