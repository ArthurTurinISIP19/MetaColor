using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public struct CharacterCreatedMessage : NetworkMessage
{
    public NetworkIdentity identity;
}

public class PreparePlayer : NetworkBehaviour
{

    override public void OnStartClient()
    {
        NetworkClient.RegisterHandler<CharacterCreatedMessage>(OnCharacterCreated);
    }

    void OnCharacterCreated(CharacterCreatedMessage message)
    {
        GameObject playerobject = message.identity.gameObject;
        playerobject.name += "[" + playerobject.GetComponent<NetworkIdentity>().netId + "]";
        playerobject.transform.GetChild(2).gameObject.SetActive(true);

        var cam = GameObject.FindGameObjectWithTag("CinemachineTarget").GetComponent<CinemachineVirtualCamera>();
        cam.Follow = playerobject.transform.GetChild(1);
    }
}
