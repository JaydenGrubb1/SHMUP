using System;
using UnityEngine;
using DG.Tweening.Core;

namespace DG.Tweening
{
	public static class DOTweenExt
	{
		//public static Tweener DOColor(this SpriteRenderer target, Color endValue, float duration)
		//{
		//	return DOTween.To(() => target.color, x => target.color = x, endValue, duration).SetTarget(target);
		//}

		//public static Tweener DOFade(this SpriteRenderer target, float endValue, float duration)
		//{
		//	return DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration).SetTarget(target);
		//}

        //public static Sequence DOGradientColor(this SpriteRenderer target, Gradient gradient, float duration)
        //{
        //    Sequence s = DOTween.Sequence();
        //    GradientColorKey[] colors = gradient.colorKeys;
        //    int len = colors.Length;
        //    for (int i = 0; i < len; ++i)
        //    {
        //        GradientColorKey c = colors[i];
        //        if (i == 0 && c.time <= 0)
        //        {
        //            target.color = c.color;
        //            continue;
        //        }
        //        float colorDuration = i == len - 1
        //            ? duration - s.Duration(false) // Verifies that total duration is correct
        //            : duration * (i == 0 ? c.time : c.time - colors[i - 1].time);
        //        s.Append(target.DOColor(c.color, colorDuration).SetEase(Ease.Linear));
        //    }
        //    return s;
        //}

        //public static Tweener DOBlendableColor(this SpriteRenderer target, Color endValue, float duration)
        //{
        //    endValue = endValue - target.color;
        //    Color to = new Color(0, 0, 0, 0);
        //    return DOTween.To(() => to, x =>
        //    {
        //        Color diff = x - to;
        //        to = x;
        //        target.color += diff;
        //    }, endValue, duration)
        //        .Blendable().SetTarget(target);
        //}
    }
}