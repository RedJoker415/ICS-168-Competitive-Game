using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class bullet : NetworkBehaviour
{

    Vector3 velocity;
    Vector3 bestGuessPosition;

    public GameObject owner;

    public float speed;
    public float deathTime;
    void Start()
    {
        velocity = new Vector3 (0, 0, speed);
        if (hasAuthority)
            CmdUpdateVelocity(velocity, transform.position);
        StartCoroutine(Example());
    }

    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if (hasAuthority) {
            if (other.gameObject != owner) {
                PlayerUnit opponent = other.gameObject.GetComponent<PlayerUnit>();
                if (opponent) {
                    opponent.CmdDamage(10);
                    opponent.CmdKnockback(transform.forward * 5);
                }
            }
        }
    }

    IEnumerator Example()
    {
        //print(Time.time);
        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
        //print(Time.time);
    }

    [Command]
    void CmdUpdateVelocity(Vector3 v, Vector3 p)
    {
        transform.position = p;
        velocity = v;
        RpcUpdateVelocity(velocity, transform.position);
    }

    [ClientRpc]
    void RpcUpdateVelocity(Vector3 v, Vector3 p)
    {
        if (hasAuthority)
        { 
            return;
        }
        transform.position = p;
        velocity = v;
    }

}