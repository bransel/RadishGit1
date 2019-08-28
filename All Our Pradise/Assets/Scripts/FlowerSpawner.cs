using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
	public Flower flowerPrefab;
	public Transform blobDataParent;
	public LayerMask flowerLayers;
	public LayerMask groundLayers;
	
	public ImageLoader imageLoader { get; set; }
	public List<Flower> flowerPool { get; set; }

    // Use this for initialization
    void Start()
    {
		flowerPool = new List<Flower>();
		
		if (!imageLoader)
			imageLoader = FindObjectOfType<ImageLoader>();
    }

	public void PoolFlower(Flower flower)
	{
		flower.gameObject.SetActive(false);
		flowerPool.Add(flower);
	}

    // Update is called once per frame
    void Update()
    {
		for (int i = 0; i < imageLoader.blobsPos.Length; i++)
		{
			RaycastHit hit;
			Vector3 camPos = Camera.main.transform.position;

			if (Physics.Raycast(camPos, imageLoader.blobsPos[i] - camPos, out hit, 100, groundLayers))
			{
				Collider[] flowers = new Collider[0];
				flowers = Physics.OverlapSphere(hit.point, 0.33f, flowerLayers);

				if (flowers.Length > 0)
				{
					/* foreach (var flower in flowers)
					{
						if (flower.name.Contains("Flower"))
							flower.GetComponent<Flower>().Refresh();
					} */

					if (flowers[0].name.Contains("Flower"))
						flowers[0].GetComponent<Flower>().Refresh();
				}
				else
				{
					if (flowerPool.Count == 0)
					{
						Flower newFlower = Instantiate(flowerPrefab);
						newFlower.transform.position = hit.point;
						newFlower.onFinishEvent.AddListener(delegate { PoolFlower(newFlower); });
					}
					else
					{
						Flower newFlower = flowerPool[0];
						newFlower.transform.position = hit.point;
						newFlower.gameObject.SetActive(true);
						flowerPool.RemoveAt(0);
					}
				}
			}
		}
    }
}
