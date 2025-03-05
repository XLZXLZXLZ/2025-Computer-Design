using UnityEditor;
using UnityEngine;

public class SoundEffectRecorder
{
    private const string soundEffectContainerPath = @"Assets/C#Script/Manager/AudioManager/SoundEffectContainer.asset";
    private static SoundEffectContainer _soundEffectContainer;

    private static SoundEffectContainer soundEffectContainer
    {
        get
        {
            if (_soundEffectContainer == null)
            {
                _soundEffectContainer = AssetDatabase.LoadAssetAtPath<SoundEffectContainer>(soundEffectContainerPath);
                if (_soundEffectContainer == null)
                {
                    _soundEffectContainer = ScriptableObject.CreateInstance<SoundEffectContainer>();
                    AssetDatabase.CreateAsset(_soundEffectContainer, soundEffectContainerPath);
                    EditorUtility.SetDirty(_soundEffectContainer);
                    AssetDatabase.SaveAssets();
                }
            }

            return _soundEffectContainer;
        }
    }
    
    
    [MenuItem("Assets/SaveSoundEffect")]
    static void Select()
    {
        string[] guids = Selection.assetGUIDs;
        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
            if (audioClip == null) return;
            
            soundEffectContainer.AddSoundEffect(audioClip);
            EditorUtility.SetDirty(soundEffectContainer);
            AssetDatabase.SaveAssets();
            
        }
    }
    
    [MenuItem("Assets/SaveSoundEffect", true)]
    static bool IsSelect()
    {
        return null != Selection.assetGUIDs && Selection.assetGUIDs.Length > 0;
    }
}