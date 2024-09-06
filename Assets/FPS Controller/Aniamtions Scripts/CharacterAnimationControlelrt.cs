using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterAnimationControlelrt : NetworkBehaviour
{
    FPSCharacterManager characterManager;

    private void Awake()
    {
        characterManager = GetComponentInParent<FPSCharacterManager>();
    }
    void OnAutoReload()
    {
        if (!GetComponentInParent<NetworkObject>().IsOwner)
            return;

        characterManager.Reload();
    }

    void RifleShoot()
    {
        if (!GetComponentInParent<NetworkObject>().IsOwner)
            return;

        characterManager.CanFire = true;
    }

    void Onthrow()
    {
        if (!GetComponentInParent<NetworkObject>().IsOwner)
            return;

        characterManager.CanFire = true;
        characterManager.CanSwitch = true;
        characterManager.Refrences.CharcaterAniamtor.SetBool("Shot_b", false);
        characterManager.GrabWeapon(characterManager.CurruntWeapon.Value);
    }

    void Throw()
    {
        if (!GetComponentInParent<NetworkObject>().IsOwner)
            return;

        characterManager.Throw();
    }
}