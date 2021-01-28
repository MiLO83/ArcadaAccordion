using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
public class Accordingly : MonoBehaviour
{
    public RawImage ri;
    public Color32[] prioColors;
    public Texture2D srcTex;
    public Texture2D destTex;
    public Color32[] srcImg;
    public Color32[] destImg;
    public string[] allPNGs;
    public string[] thisPicPNGs;
    public bool picNotP56 = true;
    public List<int> foundViewNumbers = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ProcessBatch());
    }

    public IEnumerator ProcessBatch()
    {
        
        ri = gameObject.GetComponent<RawImage>();
        ri.texture = destTex;
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/../Input/");
        Directory.CreateDirectory(Application.dataPath + "/../Output/");
        Debug.Log("Looking for Images in : " + di.FullName);
        foreach (var fi in di.GetFiles("*.png"))
        {
            if (fi.Name.Split('.')[0] == "pic")
            {
                picNotP56 = true;
                int viewNum = int.Parse(fi.Name.Split('.')[1]);
                if (!foundViewNumbers.Contains(viewNum))
                {
                    foundViewNumbers.Add(viewNum);
                }
            }
            else
            {
                picNotP56 = false;
                int viewNum = int.Parse(fi.Name.Split('.')[0]);
                if (!foundViewNumbers.Contains(viewNum))
                {
                    foundViewNumbers.Add(viewNum);
                }
            }
            Debug.Log(fi.Name);
        }

        foreach (int vstr in foundViewNumbers.ToArray())
        {
            srcTex = new Texture2D(32, 32);
            
            if (picNotP56)
            {
                
                for (int n = 0; n < 16; n++)
                {     
                    if (File.Exists(di.FullName + "/pic." + vstr.ToString("000") + "." + n.ToString() + ".png"))
                    {
                        Debug.Log("FOUND : " + di.FullName + "/pic." + vstr.ToString("000") + "." + n.ToString() + ".png");
                        srcTex.LoadImage(File.ReadAllBytes(di.FullName + "/pic." + vstr.ToString("000") + "." + n.ToString() + ".png"));
                        srcTex.Apply();

                        if (n == 0)
                        {
                            destTex = new Texture2D(srcTex.width, srcTex.height);
                            destImg = new Color32[srcTex.width * srcTex.height];
                            for (int y = 0; y < srcTex.height; y++)
                            {
                                for (int x = 0; x < srcTex.width; x++)
                                {
                                    destImg[(y * srcTex.width) + x] = prioColors[0];
                                }
                            }
                        }
                        srcImg = srcTex.GetPixels32();
                        for (int y = 0; y < srcTex.height; y++)
                        {
                            for (int x = 0; x < srcTex.width; x++)
                            {
                                if (srcImg[(y * srcTex.width) + x].a != 0)
                                {
                                    destImg[(y * srcTex.width) + x] = prioColors[n];
                                }
                            }
                        }
                    }
                }
                if (destTex == null)
                    destTex = new Texture2D(srcTex.width, srcTex.height);
                destTex.SetPixels32(destImg);
                destTex.Apply();
                byte[] bytes = destTex.EncodeToPNG();
                System.IO.File.WriteAllBytes(Application.dataPath + "/../Output/" + "/pic." + vstr + "_p.png", bytes);
                Debug.Log("WROTE : " + Application.dataPath + "/../Output/" + "/pic." + vstr + "_p.png");
                yield return new WaitForEndOfFrame();
            }
            else
            {
                
                for (int n = 0; n < 16; n++)
                {
                    
                    if (File.Exists(di.FullName + "/" + vstr + ".p56." + n.ToString() + ".png"))
                    {
                        Debug.Log("FOUND : " + di.FullName + "/" + vstr + ".p56." + n.ToString() + ".png");
                        srcTex.LoadImage(File.ReadAllBytes(di.FullName + "/" + vstr + ".p56." + n.ToString() + ".png"));
                        srcTex.Apply();
                        if (n == 0)
                        {
                            destTex = new Texture2D(srcTex.width, srcTex.height);
                            destImg = new Color32[srcTex.width * srcTex.height];
                            for (int y = 0; y < srcTex.height; y++)
                            {
                                for (int x = 0; x < srcTex.width; x++)
                                {
                                    destImg[(y * srcTex.width) + x] = prioColors[0];
                                }
                            }
                        }
                        srcImg = srcTex.GetPixels32();
                        for (int y = 0; y < srcTex.height; y++)
                        {
                            for (int x = 0; x < srcTex.width; x++)
                            {
                                if (srcImg[(y * srcTex.width) + x].a != 0)
                                {
                                    destImg[(y * srcTex.width) + x] = prioColors[n];
                                }
                            }
                        }
                    }
                }

                destTex.SetPixels32(destImg);
                destTex.Apply();
                byte[] bytes = destTex.EncodeToPNG();
                System.IO.File.WriteAllBytes(Application.dataPath + "/../Output/" + "/pic." + vstr + "_p.png", bytes);
                Debug.Log("WROTE : " + Application.dataPath + "/../Output/" + "/pic." + vstr + "_p.png");
                yield return new WaitForEndOfFrame();
            }
        }
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        ri.texture = destTex;
    }
}
