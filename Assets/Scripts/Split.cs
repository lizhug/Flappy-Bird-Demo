using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEditor;
public class SpriteTailed : MonoBehaviour
{
    [MenuItem("Tools/��������")]
    static void SaveSprite()
    {
        //ÿһ����ͼ����Advanced�� Read/Write Enabled���Ϲ����ܽ����ļ���ȡ
        string resourcesPath = @"Assets/Images/";
        foreach (Object obj in Selection.objects)
        {

            string selectionPath = AssetDatabase.GetAssetPath(obj);

            // �������ϼ���"Assets/Resources/"
            if (selectionPath.StartsWith(resourcesPath))
            {
                //��ȡ�ļ�������.png
                string selectionExt = System.IO.Path.GetExtension(selectionPath);

                if (selectionExt.Length == 0) continue;

                // ��·��"Assets/Resources/UI/testUI.png"�õ�·��"UI/testUI"
                string loadPath = selectionPath.Remove(selectionPath.Length - selectionExt.Length);
                loadPath = loadPath.Substring(resourcesPath.Length);

                //���ش��ļ��µ�������Դ
                Sprite[] spriteList = Resources.LoadAll<Sprite>(loadPath);

                if (spriteList.Length > 0)
                {
                    //���������ļ���
                    string outPath = Application.dataPath + "/outSprite/" + loadPath;
                    System.IO.Directory.CreateDirectory(outPath);

                    foreach (var sprite in spriteList)
                    {
                        Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
                        tex.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin
                                                    , (int)sprite.rect.yMin
                                                    , (int)sprite.rect.width
                                                    , (int)sprite.rect.height));
                        tex.Apply();

                        //д����png�ļ�
                        System.IO.File.WriteAllBytes(outPath + "/" + sprite.name + ".png", tex.EncodeToPNG());
                        Debug.Log("SaveSprite to" + outPath);
                    }
                    Debug.Log("����ͼƬ���!" + outPath);
                }
            }
        }
    }
}