﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skylight;
public class DoorController : MonoBehaviour
{
	public float m_speed = 0.2f;
	public LayerMask m_layerMask;
	public int m_detectCount = 20;

	private Vector3 m_dir;
	private Collider2D m_collider2D;
	private float m_radius;

	private Transform m_sibline;



	bool isFlying = false;
	bool m_isStay = false;

	float interval;


	public void Disappear ()
	{
		if (m_isStay) {
			transform.position = new Vector3 (-100, -100, 0);

		}

		m_isStay = false;

	}

	public bool GetIsDoorStay ()
	{
		return m_isStay;
	}

	public bool Shoot (Vector3 pos, Vector3 dir)
	{
		if (m_isStay) return false;



		transform.position = pos;
		if (dir.x <= 0.001f && dir.x >= -0.001f) {
			if (dir.y >= 0f) {
				transform.localRotation = Quaternion.Euler (0, 0, 0);

			} else {
				transform.localRotation = Quaternion.Euler (0, 0, 180);

			}
		}
		float atan = dir.y / dir.x;
		atan = Mathf.Atan (atan) * 57.2957f;
		if (dir.y >= 0) {
			transform.localRotation = Quaternion.Euler (0, 0, atan + 90);

		} else {
			transform.localRotation = Quaternion.Euler (0, 0, atan - 90);
		}


		m_dir = dir;

		isFlying = true;




		return true;
	}


	// Use this for initialization
	void Start ()
	{

		interval = 360f / m_detectCount;
		m_collider2D = transform.GetComponent<Collider2D> ();
		m_radius = m_collider2D.bounds.extents.y + 0.1f;

		if (gameObject.name == "Door1") {
			m_sibline = GameObject.Find ("Door2").transform;
		} else {
			m_sibline = GameObject.Find ("Door1").transform;

		}
	}

	// Update is called once per frame
	void Update ()
	{

	}

	private void FixedUpdate ()
	{
		if (isFlying) {
			transform.position += m_dir * m_speed;

		}
	}


	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.gameObject.name == "Barrier") {
			m_isStay = true;
			Disappear ();
		}


		if (collision.gameObject.name == "Wall") {
			isFlying = false;
			List<int> touched = new List<int> ();
			for (int i = 0; i < m_detectCount; i++) {

				Vector3 rotateQuaternion = Quaternion.AngleAxis (interval * i, Vector3.forward) * Vector3.up;


				RaycastHit2D raycastHit2D = Physics2D.Raycast (transform.position, rotateQuaternion, m_radius, m_layerMask);
				if (raycastHit2D.collider != null) {
					Debug.DrawLine (transform.position, raycastHit2D.point, Color.green, 20f);

					touched.Add (i);
					//Debug.Log ("Angle:" + i);
				}
			}
			if (touched.Count == 0) {
				m_isStay = true;
				Disappear ();
				return;
			}
			float rotate = FindLeftestLine (touched) * interval - (touched.Count - 1) * interval / 2;

			if (rotate > 360) rotate -= 360;
			rotate -= 90;
			transform.localRotation = Quaternion.Euler (0, 0, rotate);

			//Debug.Log ("Rotate:" + rotate);

			Debug.DrawRay (transform.position, transform.TransformDirection (Vector2.right), Color.red, 20f);


			m_isStay = true;
			//StartCoroutine (Disappear (5));
		}

		if (m_isStay) {
			if (collision.gameObject.name == "Hero") {
				Transform hero = collision.gameObject.transform;
				//Debug.Log (hero.GetComponent<Rigidbody2D> ().velocity.sqrMagnitude);
				Vector2 heroVelocity = hero.GetComponent<Rigidbody2D> ().velocity;
				if (heroVelocity.sqrMagnitude > 1f) {

					//Vector3 heroVec = hero.TransformDirection (Vector3.up);
					Vector2 doorNormal = transform.TransformDirection (Vector2.right);

					float dotResult = Vector2.Dot (heroVelocity, doorNormal) / heroVelocity.magnitude * doorNormal.magnitude;


					if (dotResult <= -0.3f) {

						Vector3 siblingNormal = m_sibline.transform.TransformDirection (Vector3.right);

						bool isSiblingStay = m_sibline.GetComponent<DoorController> ().GetIsDoorStay ();

						if (isSiblingStay) {

							float angle;
							float normalDotCos = Vector2.Dot (new Vector2 (doorNormal.x, doorNormal.y), new Vector2 (siblingNormal.x, siblingNormal.y)) / doorNormal.magnitude * siblingNormal.magnitude;

							if (normalDotCos <= -0.9f) {
								angle = 180;

							} else if (normalDotCos >= 0.9) {
								angle = 0;
							} else {
								angle = Mathf.Acos (normalDotCos) * Mathf.Rad2Deg;
							}

							Debug.Log ("angle : " + angle);
							Vector3 axis;
							if (normalDotCos <= -0.9f) {
								axis = Vector3.forward;
							} else if (normalDotCos >= 0.9) {
								axis = Vector3.back;
							} else {
								axis = Vector3.Cross (doorNormal, siblingNormal);
							}

							Debug.Log ("Axis : " + axis);
							Rigidbody2D heroRigid = hero.GetComponent<Rigidbody2D> ();

							//get negative
							Vector3 velocity = heroRigid.velocity;
							velocity.x = -velocity.x;
							velocity.y = -velocity.y;
							velocity.z = -velocity.z;

							Debug.DrawRay (transform.position, velocity.normalized, Color.yellow, 20f);
							Debug.Log ("Before rotation : " + velocity);

							//TODO  Adding a drag coefficient make player slow down
							heroRigid.velocity = Quaternion.AngleAxis (angle, axis) * velocity;

							Debug.Log ("After rotation : " + heroRigid.velocity);
							Debug.DrawRay (m_sibline.position, heroRigid.velocity.normalized, Color.yellow, 20f);

							Vector2 padding = heroRigid.velocity.normalized * m_radius;

							hero.position = m_sibline.position + new Vector3 (padding.x, padding.y, 0);

						}
					}
				}


			}

			if (collision.gameObject.name == "Knife") {
				Transform knife = collision.gameObject.transform;

				KnifeController knifeController = knife.GetComponent<KnifeController> ();

				if (knifeController.GetIsFlying ()) {

					//Vector2 dir = knifeController.GetDir ();


					bool isSiblingStay = m_sibline.GetComponent<DoorController> ().GetIsDoorStay ();
					if (isSiblingStay) {
						//Start calculate
						Vector2 doorNormal = transform.TransformDirection (Vector2.right);
						Vector3 siblingNormal = m_sibline.transform.TransformDirection (Vector3.right);

						Vector3 dir = knifeController.GetDir ();
						knife.position = m_sibline.position + dir * m_radius;
					}


					//Vector2 heroVelocity = hero.GetComponent<Rigidbody2D> ().velocity;
					//if (heroVelocity.sqrMagnitude > 1f) {

					//	//Vector3 heroVec = hero.TransformDirection (Vector3.up);
					//	Vector2 doorNormal = transform.TransformDirection (Vector2.right);

					//	float dotResult = Vector2.Dot (heroVelocity, doorNormal) / heroVelocity.magnitude * doorNormal.magnitude;


					//	if (dotResult <= -0.3f) {

					//		Vector3 siblingNormal = m_sibline.transform.TransformDirection (Vector3.right);

					//		bool isSiblingStay = m_sibline.GetComponent<DoorController> ().GetIsDoorStay ();

					//		if (isSiblingStay) {

					//			float angle;
					//			float normalDotCos = Vector2.Dot (new Vector2 (doorNormal.x, doorNormal.y), new Vector2 (siblingNormal.x, siblingNormal.y)) / doorNormal.magnitude * siblingNormal.magnitude;

					//			if (normalDotCos <= -0.9f) {
					//				angle = 180;

					//			} else if (normalDotCos >= 0.9) {
					//				angle = 0;
					//			} else {
					//				angle = Mathf.Acos (normalDotCos) * Mathf.Rad2Deg;
					//			}

					//			Debug.Log ("angle : " + angle);
					//			Vector3 axis;
					//			if (normalDotCos <= -0.9f) {
					//				axis = Vector3.forward;
					//			} else if (normalDotCos >= 0.9) {
					//				axis = Vector3.back;
					//			} else {
					//				axis = Vector3.Cross (doorNormal, siblingNormal);
					//			}

					//			Debug.Log ("Axis : " + axis);
					//			Rigidbody2D heroRigid = hero.GetComponent<Rigidbody2D> ();

					//			//get negative
					//			Vector3 velocity = heroRigid.velocity;
					//			velocity.x = -velocity.x;
					//			velocity.y = -velocity.y;
					//			velocity.z = -velocity.z;

					//			Debug.DrawRay (transform.position, velocity.normalized, Color.yellow, 20f);
					//			Debug.Log ("Before rotation : " + velocity);

					//			heroRigid.velocity = Quaternion.AngleAxis (angle, axis) * velocity;

					//			Debug.Log ("After rotation : " + heroRigid.velocity);
					//			Debug.DrawRay (m_sibline.position, heroRigid.velocity.normalized, Color.yellow, 20f);

					//			Vector2 padding = heroRigid.velocity.normalized * m_radius;

					//			hero.position = m_sibline.position + new Vector3 (padding.x, padding.y, 0);

					//		}
					//	}
					//}
				}
			}

		}

	}
	private void OnDrawGizmos ()
	{

		//for (int i = 0; i < m_detectCount; i++) {
		//	Vector3 rotateQuaternion = Quaternion.AngleAxis (interval * i, Vector3.forward) * Vector3.up;
		//	Debug.DrawLine (transform.position, transform.position + (rotateQuaternion * m_radius), Color.green);
		//	//Debug.DrawRay (transform.position, rotateQuaternion, Color.green,);
		//}
	}

	int FindLeftestLine (List<int> lines)
	{
		bool isZero = false;
		foreach (int i in lines) {
			if (i == 0) {
				isZero = true;
			}
		}

		if (isZero) {
			if (lines.Count == 1) return lines [lines.Count - 1];
			int index = 0;
			for (int i = 0; i < lines.Count - 1; i++) {
				if (lines [i + 1] - lines [i] == 1) {
					index = i + 1;
				}
			}
			return lines [index];
		} else {
			return lines [lines.Count - 1];
		}
	}

}
