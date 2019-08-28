using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavRandomPoint : MonoBehaviour
{
	public Vector3 randomPoint;

	void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(randomPoint, 1);
    }

    public Vector3 GetPoint()
    {
		float xScale = transform.localScale.x / 2f;
		float zScale = transform.localScale.z / 2f;
		Vector3 rayOrigin = new Vector3(transform.position.x + Random.Range(-xScale, xScale), transform.position.y + 10, transform.position.z + Random.Range(-zScale, zScale));
		
		bool clear = false;
		int tries = 100;

		while (!clear && tries > 0)
		{
			Ray ray = new Ray(rayOrigin, Vector3.down * 11);
			RaycastHit rayHit;
			NavMeshHit navHit;
			
			if (Physics.Raycast(ray, out rayHit) && NavMesh.SamplePosition(rayHit.point, out navHit, 0.1f, NavMesh.AllAreas))
			{
				randomPoint = navHit.position;
				return randomPoint;
			}

			tries--;
		}

		return Vector3.zero;
    }
}
