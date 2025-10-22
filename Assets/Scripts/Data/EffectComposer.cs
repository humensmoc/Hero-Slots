using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 效果组合器 - 用于组合多个协程效果
/// </summary>
public static class EffectComposer
{
    /// <summary>
    /// 顺序执行多个效果
    /// </summary>
    public static IEnumerator Sequential(params IEnumerator[] effects)
    {
        foreach (var effect in effects)
        {
            yield return effect;
        }
    }

    /// <summary>
    /// 并行执行多个效果（同时开始，等待全部完成）
    /// </summary>
    public static IEnumerator Parallel(MonoBehaviour runner, params IEnumerator[] effects)
    {
        var coroutines = new List<Coroutine>();
        
        // 启动所有协程
        foreach (var effect in effects)
        {
            coroutines.Add(runner.StartCoroutine(effect));
        }
        
        // 等待所有协程完成
        foreach (var coroutine in coroutines)
        {
            yield return coroutine;
        }
    }

    /// <summary>
    /// 延迟执行效果
    /// </summary>
    public static IEnumerator Delayed(float delay, IEnumerator effect)
    {
        yield return new WaitForSeconds(delay);
        yield return effect;
    }

    /// <summary>
    /// 重复执行效果
    /// </summary>
    public static IEnumerator Repeat(int count, IEnumerator effect)
    {
        for (int i = 0; i < count; i++)
        {
            yield return effect;
        }
    }

    /// <summary>
    /// 条件执行效果
    /// </summary>
    public static IEnumerator Conditional(Func<bool> condition, IEnumerator effect)
    {
        if (condition())
        {
            yield return effect;
        }
    }
}
