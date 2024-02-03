using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DEV
{
	public class UserFollowUI : MonoBehaviour
	{
		private struct PointInSpace
		{
			public Vector3 Position ;
			public Vector3 Rotation;
			public float Time ;
		}
	
		[SerializeField]
		[Tooltip("The transform to follow")]
		private Transform target;
     
		[SerializeField]
		[Tooltip("The offset between the target and the camera")]
		private Vector3 offset;
	
		[Tooltip("The delay before the camera starts to follow the target")]
		[SerializeField]
		private float delay = 0.5f;
     
		[SerializeField]
		[Tooltip("The speed used in the lerp function when the camera follows the target")]
		private float speed = 5;
     
		///<summary>
		/// Contains the positions of the target for the last X seconds
		///</summary>
		private readonly Queue<PointInSpace> pointsInSpace = new Queue<PointInSpace>();

		private Transform t; // The transform of the UI
		private Canvas canvas; // The canvas component
		private TextMeshProUGUI textDisplay; // The text component

		private void Start()
		{
			t = transform;
			canvas = GetComponent<Canvas>();
			textDisplay = GetComponentsInChildren<TextMeshProUGUI>()[0];
		}

		private void LateUpdate ()
		{
			// Add the current target position to the list of positions
			var curRot = t.rotation.eulerAngles;
			var targetP = target.position;
			if (targetP.y < 0.3f)
			{
				targetP.y = 0.3f;
			}
			var targetR = new Vector3(curRot.x, target.rotation.eulerAngles.y, curRot.z); // Only set the y rotation for the UI
			pointsInSpace.Enqueue( new PointInSpace() { Position = targetP, Rotation = targetR, Time = Time.time } ) ;
		
			// Move the camera to the position of the target X seconds ago 
			while( pointsInSpace.Count > 0 && pointsInSpace.Peek().Time <= Time.time - delay + Mathf.Epsilon )
			{
				t.position = Vector3.Lerp( t.position, pointsInSpace.Dequeue().Position + offset, Time.deltaTime * speed);
				t.rotation = Quaternion.Euler(
					Vector3.Lerp(t.rotation.eulerAngles, pointsInSpace.Dequeue().Rotation, Time.deltaTime * speed)
				);
			}
		}

		/*
		 * Hide the floating UI beneath the players view
		 */
		public void Hide()
		{
			canvas.enabled = false;
		}

		/*
		 * Show the floating UI beneath the players view
		 */
		public void Show()
		{
			canvas.enabled = true;
		}

		/*
		 * Set the text in the floating UI beneath the players view
		 */
		public void SetText(string message)
		{
			textDisplay.SetText(message);
		}
	}
}
