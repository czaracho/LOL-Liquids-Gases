using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaballParticleClass : MonoBehaviour {

	public GameObject MObject;
	public float LifeTime;
	public bool Active{
		get{ return _active;}
		set{ _active = value;
			if (MObject) {
				MObject.SetActive (value);

				if (tr)
					tr.Clear ();

			}
		}
	}
	public bool witinTarget; // si esta dentro de la zona de vaso de vidrio en la meta


	bool _active;
	float delta;
	Rigidbody2D rb;
	TrailRenderer tr;

	void Start () {
		//MObject = gameObject;
		rb = GetComponent<Rigidbody2D> ();
		tr = GetComponent<TrailRenderer> ();
	}

	void Update () {

		if (Active == true) {

			VelocityLimiter ();

			if (LifeTime < 0)
				return;

			if (delta > LifeTime) {
				delta *= 0;
				Active = false;
			} else {
				delta += Time.deltaTime;
			}


		}

	}

	private void ImpulseWater() { 
		
	}



	void VelocityLimiter()
	{
		
		
		Vector2 _vel = rb.velocity;
		if (_vel.y < -8f) {
			_vel.y = -8f;
		}
		rb.velocity = _vel;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag == "Destroyer")
		{
			this.Active = false;


		}
		else if (collision.tag == "impulser") {
			transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y, 0);
			rb.AddForce(new Vector2(1, 1) * 2.5f, ForceMode2D.Impulse);
		}
	}

}
