using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropUntilDead : MonoBehaviour
{
	public GameObject prefab;
	public Transform goal;

	public void Start()
	{
		GameObject go = Instantiate(prefab, transform.position, transform.rotation);
		go.transform.DOMove(goal.position, 1f);
	}
}
