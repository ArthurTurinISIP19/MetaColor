using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Paint : NetworkBehaviour
{
    [SerializeField] private TextureWrapMode _textureWrapMode;
    [SerializeField] private FilterMode _filterMode;

    [SerializeField] private int _brushSize = 32;

    public TestingMemoryFootprints tests;
    [SerializeField] private ObjInfo info;
    // public Colors color;

    private int _oldRayX, _oldRayY;

    private Color _color = Color.black;

    Color[] posColors = { Color.black, Color.red, Color.blue, Color.green };

    public void SetColor(int color)
    {
        _color = posColors[color];
    }

    override public void OnStartClient()
    {
        RequestTextureMessage msg = new RequestTextureMessage();
        //NetworkClient.Send(msg);
    }

    [Client]
    private void Update()
    {
        if (!isLocalPlayer) { return; }

        if (_brushSize + (int)Input.mouseScrollDelta.y > 0.0f)
        {
            _brushSize += (int)Input.mouseScrollDelta.y;
        }
        else
        {
            _brushSize = 0;
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.transform.TryGetComponent<NetworkIdentity>(out NetworkIdentity identity))
                {

                    CmdDraw(identity, hit.textureCoord, _brushSize, _color);
                }
            }
        }
    }

    [Command]
    void CmdDraw(NetworkIdentity obj, Vector2 textureCoord, int brushSize, Color color)
    {
        if (obj.tag == "Paintable")
        {
            DrawOnTexture(obj, textureCoord, brushSize, color);
            //info.oobj = obj;
            //info.material = obj.GetComponent<Renderer>().sharedMaterial;

            RpcDrawOnTexture(obj, textureCoord, brushSize, color);
        }
    }

    [ClientRpc]
    void RpcDrawOnTexture(NetworkIdentity obj, Vector2 textureCoord, int brushSize, Color color)
    {
        DrawOnTexture(obj, textureCoord, brushSize, color);
    }

    void DrawOnTexture(NetworkIdentity obj, Vector2 textureCoord, int brushSize, Color color)
    {
        //MaterialPropertyBlock block = tests.matBlocks[obj];
        //obj.GetComponent<Renderer>().GetPropertyBlock(block);
        //Debug.Log(obj.GetComponent<Renderer>());
        //Texture2D texture = (Texture2D)block.GetTexture("_MainTex");

        Texture2D texture = new Texture2D(256, 256);

        if (obj.GetComponent<Renderer>().sharedMaterial.mainTexture == null)
        {
            texture.Apply();
            // MaterialPropertyBlock block = new MaterialPropertyBlock();
            // block.SetTexture("_MainTex", texture);
            // obj.GetComponent<Renderer>().SetPropertyBlock(block);
            obj.GetComponent<Renderer>().material.mainTexture = texture;
        }
        else
        {
            texture = (Texture2D)obj.GetComponent<Renderer>().material.mainTexture;
        }

        int rayX = (int)(textureCoord.x * texture.width);
        int rayY = (int)(textureCoord.y * texture.height);

        if (_oldRayX != rayX || _oldRayY != rayY)
        {
            DrawCircle(rayX, rayY, texture, brushSize, color);
            texture.Apply();
            _oldRayX = rayX;
            _oldRayY = rayY;
        }
    }


    void DrawQuad(int rayX, int rayY)
    {
        for (int y = 0; y < _brushSize; y++)
        {
            for (int x = 0; x < _brushSize; x++)
            {
                // _texture.SetPixel(rayX + x - _brushSize / 2, rayY + y - _brushSize / 2, _color);
            }
        }
    }

    void DrawCircle(int rayX, int rayY, Texture2D texture, int brushSize, Color color)
    {
        for (int y = 0; y < brushSize; y++)
        {
            for (int x = 0; x < brushSize; x++)
            {

                float x2 = Mathf.Pow(x - brushSize / 2, 2);
                float y2 = Mathf.Pow(y - brushSize / 2, 2);
                float r2 = Mathf.Pow(brushSize / 2 - 0.5f, 2);

                if (x2 + y2 < r2)
                {
                    int pixelX = rayX + x - (int)(brushSize / 2);
                    int pixelY = rayY + y - (int)(brushSize / 2);

                    if (pixelX >= 0 && pixelX < texture.width && pixelY >= 0 && pixelY < texture.height)
                    {
                        Color oldColor = texture.GetPixel(pixelX, pixelY);
                        Color resultColor = Color.Lerp(oldColor, color, 0.5f);
                        texture.SetPixel(pixelX, pixelY, resultColor);
                    }
                }
            }
        }
    }
}