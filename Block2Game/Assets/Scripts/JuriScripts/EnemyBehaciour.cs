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
    private float lerpTime = 0;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 midPoint;
    public PlayerScript pScript;
    [SerializeField] float lerpSpeed = 1f;
    public Vector3 lerpValue;
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
        //rb.MovePosition((Vector3)transform.position + (direction * moveSpeed * Time.deltaTime));
        if (pScript._positions.Count > 3)
        {
            while (lerpTime < 1)
            {
                startPoint = pScript._positions[0]; //grab all the points to move with
                endPoint = pScript._positions[2];
                midPoint = pScript._positions[1];
                transform.position = Vector3.Lerp(              //nested lerps to make curve
                Vector3.Lerp(startPoint, midPoint, lerpTime),
                Vector3.Lerp(midPoint, endPoint, lerpTime),
                lerpTime);
                lerpTime += Time.deltaTime * lerpSpeed;
            }
        }
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
