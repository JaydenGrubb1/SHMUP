using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SHMUP
{
	public class PlayerManager : MonoBehaviour
	{
		[SerializeField]
		private PlayerInputManager inputManager;
		[SerializeField]
		private GameObject uiSectionPrefab;
		[SerializeField]
		private Transform uiParent;
		[SerializeField]
		private Color[] colors;

		private List<PlayerInput> players = new List<PlayerInput>();
		private List<GameObject> uiSections = new List<GameObject>();

		public void OnPlayerJoined(PlayerInput input)
		{
			Vector2 size = (GameManager.screenUpper - GameManager.screenLower);
			input.transform.SetParent(transform, false);
			players.Add(input);
			uiSections.Add(Instantiate(uiSectionPrefab, uiParent));

			for (int i = 0; i < players.Count * 2; i++)
			{
				if (i % 2 == 1)
				{
					int localIndex = i / 2;
					Vector3 pos = new Vector3(size.x * (i / (float)(players.Count * 2)), size.y / 2, 0);
					players[localIndex].transform.position = pos + GameManager.screenLower;

					foreach (SpriteRenderer rend in players[localIndex].GetComponentsInChildren<SpriteRenderer>())
					{
						rend.color = colors[localIndex % colors.Length];
					}					

					RectTransform rect = uiSections[localIndex].GetComponent<RectTransform>();
					float width = 1f / players.Count;
					rect.anchorMin = new Vector2(localIndex * width, 0);
					rect.anchorMax = new Vector2((localIndex + 1) * width, 1);
				}
			}
		}

		public void OnPlayerLeft(PlayerInput input)
		{
			int index = players.IndexOf(input);
			players.RemoveAt(index);
			Destroy(uiSections[index]);
			uiSections.RemoveAt(index);
		}

		private void Start()
		{

		}
	}
}