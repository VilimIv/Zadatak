using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class Grenade : NetworkBehaviour
{
    public AudioClip ExoplosionSound;
    public ParticleSystem ExplosionAffect;

    public float Range = 6f;
    public float Delay = 5f;
    public float Damage = 100f;

    public float Timer;

    public NetworkVariable<int> OwnerID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    void Start()
    {

    }

    private void Update()
    {
        Timer += Time.deltaTime;

        if(Timer > Delay)
        {
            Explode();
            Timer = 0;
        }
    }
    void Explode()
    {
        Collider[] Colliders = Physics.OverlapSphere(transform.position, Range);
        foreach (Collider collider in Colliders)
        {
            if(collider.transform.gameObject.GetComponent<Health>() != null)
            {
                if (IsOwner)
                {
                    collider.transform.gameObject.GetComponent<Health>().TakeDamageServerRpc(Damage);
                }

                if (collider.gameObject.GetComponent<Health>().CurrentHealth.Value <= 0 && !collider.gameObject.GetComponent<Health>().isDeadCounted)
                {
                    print("Eliminated!!");
                    FPSCharacterManager[] Objs = FindObjectsOfType<FPSCharacterManager>();
                    if (Objs.Length > 0)
                    {
                        for (int i = 0; i < Objs.Length; i++)
                        {
                            if (Objs[i].OwnerClientId.ToString() == OwnerID.Value.ToString())
                            {
                                Objs[i].gameObject.GetComponent<FPSCharacterManager>().AddEliminations(1);
                            }
                        }
                    }
                    collider.gameObject.GetComponent<Health>().isDeadCounted = true;
                }
            }
        }

        if(ExplosionAffect != null)
        {
            Instantiate(ExplosionAffect, transform.position, Quaternion.identity);
        }

        if(ExoplosionSound != null)
        {
            AudioSource.PlayClipAtPoint(ExoplosionSound, transform.position);
        }

       gameObject.SetActive(false);
    }

    [ClientRpc]
    private void DestroyBulletClientRpc()
    {
        Destroy(this.gameObject);
    }
}