﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BDFramework.Helper;
using BDFramework.Editor;
using UnityEditor;
using UnityEngine;

public class EditorWindow_OnkeyBuildAsset : EditorWindow
{
    private EditorWindow_Table editorTable;

    private EditorWindow_ScriptBuildDll editorScript;
    private EditorWindow_GenAssetBundle editorAsset;
   public void Show()
   {
      this.editorTable  = new EditorWindow_Table();
      this.editorAsset  = new EditorWindow_GenAssetBundle();
      this.editorScript = new EditorWindow_ScriptBuildDll();
       
       this.minSize = this.maxSize = new Vector2(1050,600);
       base.Show();
   }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        {
            if (editorScript != null)
            {            
                GUILayout.BeginVertical(GUILayout.Width(350), GUILayout.Height(220));
                
                editorScript.OnGUI();
                GUILayout.EndVertical();
                Layout_DrawLineV(Color.white);
            }
            if (editorAsset != null)
            {
                GUILayout.BeginVertical(GUILayout.Width(350), GUILayout.Height(220));
                editorAsset.OnGUI();
                GUILayout.EndVertical();
                Layout_DrawLineV(Color.white);
            }
            if (editorTable != null)
            {
                GUILayout.BeginVertical(GUILayout.Width(350), GUILayout.Height(220));
                editorTable.OnGUI();
                GUILayout.EndVertical();
                Layout_DrawLineV(Color.white);
            }
        }
        GUILayout.EndHorizontal();

        Layout_DrawLineH(Color.white);
        OnGUI_OneKeyExprot();
    }


    public string exportPath;
    public void OnGUI_OneKeyExprot()
    {
        GUILayout.BeginVertical();
        {
            GUILayout.Label("注:上面按钮操作,会默认生成到StreamingAssets", GUILayout.Width(500), GUILayout.Height(30));

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("导出地址:" + exportPath, GUILayout.Width(500));
            }
            GUILayout.EndHorizontal();
            //
            if (GUILayout.Button("一键导出", GUILayout.Width(350), GUILayout.Height(30)))
            {
                //选择目录
                exportPath = EditorUtility.OpenFolderPanel("选择导出目录", Application.dataPath.Replace("Assets",""), "");

                var files = Directory.GetFiles(exportPath, "*.*", SearchOption.AllDirectories);
                //
                if (string.IsNullOrEmpty(exportPath) || Directory.Exists(exportPath) == false || files.Length>0)
                {
                    EditorUtility.DisplayDialog("错误!", "文件夹不存在,或者文件夹不为空", "爱咋咋地!");
                }
                else
                {                 
                    var outPath = exportPath+"/"+Utils.ResourcePlatformPath;
                    //1.编译脚本
                    ScriptBiuldTools.GenDllByMono(Application.dataPath,outPath);
                    //2.打包资源
                    AssetBundleEditorTools.GenAssetBundle("Resource/Runtime/",outPath, BuildTarget.StandaloneWindows);
                    //3.打包表格
                    Excel2SQLiteTools.GenSQLite(outPath);
                }
                   
            }

            //
            if (GUILayout.Button("上传到CDN", GUILayout.Width(350), GUILayout.Height(30)))
            {

            }
        }
        GUILayout.EndVertical();
        
    }
    
    public static void Layout_DrawLineH(Color color, float height = 4f)
    {

        Rect rect = GUILayoutUtility.GetLastRect();
        GUI.color = color;
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMax, rect.width, height), EditorGUIUtility.whiteTexture);
        GUI.color = Color.white;
        GUILayout.Space(height);
    }

    public static void Layout_DrawLineV(Color color, float width = 4f)
    {
        Rect rect = GUILayoutUtility.GetLastRect();
        GUI.color = color;
        GUI.DrawTexture(new Rect(rect.xMax, rect.yMin, width, rect.height), EditorGUIUtility.whiteTexture);
        GUI.color = Color.white;
        GUILayout.Space(width);
    }
}