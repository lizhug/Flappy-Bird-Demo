using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEditor;
public class SpriteTailed : MonoBehaviour
{
    [MenuItem("Tools/导出精灵")]
    static void SaveSprite()
    {
        //每一张贴图类型Advanced下 Read/Write Enabled打上勾才能进行文件读取
        string resourcesPath = @"Assets/Images/";
        foreach (Object obj in Selection.objects)
        {

            string selectionPath = AssetDatabase.GetAssetPath(obj);

            // 必须最上级是"Assets/Resources/"
            if (selectionPath.StartsWith(resourcesPath))
            {
                //获取文件后罪名.png
                string selectionExt = System.IO.Path.GetExtension(selectionPath);

                if (selectionExt.Length == 0) continue;

                // 从路径"Assets/Resources/UI/testUI.png"得到路径"UI/testUI"
                string loadPath = selectionPath.Remove(selectionPath.Length - selectionExt.Length);
                loadPath = loadPath.Substring(resourcesPath.Length);

                //加载此文件下的所有资源
                Sprite[] spriteList = Resources.LoadAll<Sprite>(loadPath);

                if (spriteList.Length > 0)
                {
                    //创建导出文件夹
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

                        //写出成png文件
                        System.IO.File.WriteAllBytes(outPath + "/" + sprite.name + ".png", tex.EncodeToPNG());
                        Debug.Log("SaveSprite to" + outPath);
                    }
                    Debug.Log("保存图片完毕!" + outPath);
                }
            }
        }
    }
}