//adapted from:
//https://forum.unity3d.com/threads/read-image-from-disk.117866/
//

using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class ImageLoader : MonoBehaviour
{
    public float testWidth = 706;
    public string path = @"C:\DF\screen_grabber\";
    public Renderer r;
    public GameObject blobPrefab;
    public Transform blobRoot;
    public Vector2 blobScaleOffset;
    public Vector2 blobScale;
    public string[] lines;
    public Vector2[] blobData;
    public Vector3[] blobsPos;
    public Vector3[] blobsPosPlane; // intersecting the plane
    public float sampleInterval = 0.005f;
    public LayerMask groundLayers;

    int fileIndex = 0;
    public int maxFileIndex = 20;

    string imageFile = "hello";
    string blobDataFile = "blobs";

    // Use this for initialization
    void Start()
    {
        //r.GetComponent<Renderer>();
        StartCoroutine("load_image", sampleInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator load_image()
    {
        //string[] filePaths = Directory.GetFiles(path, "*.jpg"); // get every file in chosen directory with the extension.png
        WWW www = new WWW("file:///" + path + imageFile + "_" + fileIndex + "_.jpg");    // "download" the first file from disk
        yield return www;                                                               // Wait unill its loaded

        //Debug.Log(filePaths[0]);
        
        Texture2D new_texture = new Texture2D(706, 506);               // create a new Texture2D (you could use a gloabaly defined array of Texture2D )
        www.LoadImageIntoTexture(new_texture);                           // put the downloaded image file into the new Texture2D
        if(new_texture.width == testWidth)
            r.material.mainTexture = new_texture;           // put the new image into the current material as defuse material for testing.

        UpdateBlobData();
        UpdateBlobsPos();
        //SpawnBlobs();


        fileIndex++;

        if (fileIndex > maxFileIndex)
            fileIndex = 0;

        if (fileIndex < 0)
            fileIndex = maxFileIndex;


        Resources.UnloadUnusedAssets();
        StartCoroutine("load_image", sampleInterval);
    }

    void UpdateBlobsPos()
    {
        blobsPos = new Vector3[blobData.Length];

        blobsPosPlane = new Vector3[blobsPos.Length];
        RaycastHit hit;
        Vector3 camPos = Camera.main.transform.position;

      
            for (int i = 0; i < blobData.Length; i++)
        {
            Vector2 blobPos = new Vector2(blobData[i].x * blobScale.x, blobData[i].y * blobScale.y);
            blobPos += blobScaleOffset;

            blobsPos[i] = blobRoot.TransformPoint(blobPos);

            if (Physics.Raycast(camPos, blobsPos[i] - camPos, out hit, 100, groundLayers)) // tryna get the plane blobs
            {
                blobsPosPlane[i] = hit.point;
                    }
        }
    }

    void SpawnBlobs()
    {
        foreach(Vector2 v in blobData)
        {
            Vector2 blobPos = new Vector2(v.x * blobScale.x, v.y * blobScale.y);
            blobPos += blobScaleOffset;

            //GameObject b = Instantiate(blobPrefab, blobPos, Quaternion.identity, blobRoot);
            GameObject b = Instantiate(blobPrefab, blobRoot, false);
            b.transform.localPosition = blobPos;
        }

    }

    void UpdateBlobData()
    {
        try
        { 
            //read in the lines
            lines = File.ReadAllLines(path + blobDataFile + "_" + fileIndex + "_.txt");
            blobData = new Vector2[lines.Length];

            int i = 0;
            foreach (string l in lines)
            {
                string[] components = lines[i].Split(',');
                //Debug.Log("HELLO : " + components[0] + ": " + components[1]);
                blobData[i] = new Vector2(Int32.Parse(components[0]), Int32.Parse(components[1]));

                i++;
            }
        }
        catch(System.Exception e)
        {
            Debug.Log("File Contention: FileIndex: "+ fileIndex);
            Debug.LogError(e.Message);
            fileIndex--;
        }

    }
}