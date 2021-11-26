using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct RequestTextureMessage : NetworkMessage
{

}

public struct TextureMessage : NetworkMessage
{
    public GameObject _obj;
    public byte[] _byteTexture;
}

public class SendTextures : MonoBehaviour
{
    [SerializeField] private ObjInfo info;

    private void Awake()
    {
        NetworkServer.RegisterHandler<RequestTextureMessage>(OnServerReqTexGet);
        NetworkClient.RegisterHandler<TextureMessage>(OnClientTextureGet);
    }

    [Server]
    public void OnServerReqTexGet(NetworkConnection conn, RequestTextureMessage msg)
    {
        if (info.oobj == null) { return; }

        TextureMessage sendMsg = new TextureMessage()
        {
            _obj = info.oobj,
            _byteTexture = ((Texture2D)info.material.mainTexture).EncodeToPNG(),
        };

        conn.Send(sendMsg);
    }

    [Client]
    public void OnClientTextureGet(TextureMessage msg)
    {
        Texture2D tex = (Texture2D)GameObject.Find(msg._obj.name).GetComponent<Renderer>().sharedMaterial.mainTexture;
        //tex.LoadRawTextureData(msg._byteTexture);
        ImageConversion.LoadImage(tex, msg._byteTexture);
        tex.Apply();
    }


}
