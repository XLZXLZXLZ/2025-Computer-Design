using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WorldType
{
    Outer,
    Inner  
}

/// <summary>
/// 可在双世界对调的物体实现该接口
/// </summary>
public interface IClippable
{
    /// <summary>
    /// 对于某些机关,它们在特定点被切割进/出世界时需要被相应地激活/终止，从该接口进行实现
    /// </summary>
    public void OnCilp();
}

public interface ISwappable
{
    /// <summary>
    /// 对于某些物体，在发出指令的情况下对调它们的表里世界逻辑关系，但不对调视觉关系
    /// </summary>
    public void OnSwap()
    {

    }
}


public interface IEditorSwappale
{
    /// <summary>
    /// 在编辑器模式下，可以将物体从一个世界对调到另一世界，它们自行实现对调时需要调整的部分
    /// </summary>
    public void EditorSwapAction(WorldType type);
}

