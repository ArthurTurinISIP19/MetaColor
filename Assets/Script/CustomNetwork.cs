using UnityEngine;
using Mirror;

public struct CreateCharacterMessage : NetworkMessage { }

public class CustomNetwork : NetworkManager
{

    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler<CreateCharacterMessage>(OnCreateCharacter);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        CreateCharacterMessage characterMessage = new CreateCharacterMessage();
        conn.Send(characterMessage);
    }

    // Дальше в PreparePlayer.cs
    void OnCreateCharacter(NetworkConnection conn, CreateCharacterMessage message)
    {
        GameObject playerobject = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, playerobject);

        CharacterCreatedMessage charCreatedMsg = new CharacterCreatedMessage()
        {
            identity = playerobject.GetComponent<NetworkIdentity>()
        };

        conn.Send(charCreatedMsg);
    }
}