using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TestingMemoryFootprints : NetworkBehaviour
{
    public readonly SyncDictionary<NetworkIdentity, MaterialPropertyBlock> matBlocks = new SyncDictionary<NetworkIdentity, MaterialPropertyBlock>();

    [SerializeField] private GameObject _cube;

    public static int num = 0;

    public void AddCube(int textureSize)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f, 0.0f));
        Physics.Raycast(ray, out RaycastHit hit, 100f);

        CmdAddCube(textureSize, hit.point);
    }

    [Command]
    public void CmdAddCube(int textureSize, Vector3 origin)
    {
        GameObject instCube = Instantiate(_cube, origin, Quaternion.identity);
        NetworkServer.Spawn(instCube);
        num++;
        Debug.Log(num);

        RpcSetPropertyblock(textureSize, instCube);
    }

    [ClientRpc]
    public void RpcSetPropertyblock(int textureSize, GameObject go)
    {
        uint cubeNetId = go.GetComponent<NetworkIdentity>().netId;
        go.name += "[" + cubeNetId + "]";

        // Texture2D texture = new Texture2D(256, 256);
        // texture.Apply();
        // go.GetComponent<Renderer>().material.mainTexture = texture;

        // MaterialPropertyBlock block = new MaterialPropertyBlock();
        // Texture2D tex = new Texture2D(textureSize, textureSize);
        // block.SetTexture("_MainTex", tex);
        // go.GetComponent<Renderer>().SetPropertyBlock(block);
        // matBlocks.Add(go.GetComponent<NetworkIdentity>(), block);
    }
}
