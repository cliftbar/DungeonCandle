using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {
	private PlayerController pc;

	// Lifebar variables:
	private RectTransform lifebarTransform;
	private RectTransform lifeTransform;
	public float lifebarScale;
	public float lifebarX;
	public float lifebarY;

	void Awake () {
		lifebarTransform = transform.Find("Lifebar").GetComponent<RectTransform>();
		lifeTransform = transform.Find("Lifebar").Find("Life").GetComponent<RectTransform>();
	}

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void UpdateLife(int currentLife, int maxLife) {
		lifebarTransform.sizeDelta = new Vector2((maxLife + 1) * lifebarScale * 2, lifebarScale * 4);
		lifebarTransform.anchoredPosition = new Vector2(lifebarX + maxLife * lifebarScale, lifebarY);

		lifeTransform.sizeDelta = new Vector2(currentLife * lifebarScale * 2, lifebarScale * 2);
		lifeTransform.anchoredPosition = new Vector2(currentLife * lifebarScale + lifebarScale, 0f);
	}
}
