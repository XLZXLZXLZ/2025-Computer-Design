using UnityEditor;
using UnityEngine;

public class BgmRecorder
{
    private const string bgmContainerPath = @"Assets/C#Script/Manager/AudioManager/BgmContainer.asset";
    private static BgmContainer _bgmContainer;

    private static BgmContainer bgmContainer
    {
        get
        {
            if (_bgmContainer == null)
            {
                _bgmContainer = AssetDatabase.LoadAssetAtPath<BgmContainer>(bgmContainerPath);
                if (_bgmContainer == null)
                {
                    _bgmContainer = ScriptableObject.CreateInstance<BgmContainer>();
                    AssetDatabase.CreateAsset(_bgmContainer, bgmContainerPath);
                    EditorUtility.SetDirty(_bgmContainer);
                    AssetDatabase.SaveAssets();
                }
            }

            return _bgmContainer;
        }
    }
    
    
    [MenuItem("Assets/SaveBgm")]
    static void Select()
    {
        string[] guids = Selection.assetGUIDs;
        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
            if (audioClip == null) return;
            
            bgmContainer.AddBgm(audioClip);
            EditorUtility.SetDirty(bgmContainer);
            AssetDatabase.SaveAssets();
            
        }
    }
    
    [MenuItem("Assets/SaveBgm", true)]
    static bool IsSelect()
    {
        return null != Selection.assetGUIDs && Selection.assetGUIDs.Length > 0;
    }
}