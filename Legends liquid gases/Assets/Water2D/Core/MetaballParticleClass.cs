using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaballParticleClass : MonoBehaviour {

	public GameObject MObject;
	public float LifeTime;
	private SpriteRenderer sprite;
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
		sprite = GetComponent<SpriteRenderer>();

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


		switch (collision.name) {
            case "right":
                redirectWater(new Vector2(1, 1), collision);
                break;
            case "left":
                redirectWater(new Vector2(-1, 1), collision);
                break;
            case "up":
                redirectWater(new Vector2(0, 2), collision);
                break;
            case "down":
                redirectWater(new Vector2(0, -2), collision);
                break;
            case "WaterPumpToContainer":
				StartCoroutine(ActivateTrailSprite());
				break;
			default:
				break;
		}

		switch (collision.tag) {
			case "Destroyer":
				this.Active = false;
				break;
			default:
				break;
		}
	}
	

	void redirectWater(Vector2 waterDirection, Collider2D collision) {
		rb.velocity = Vector2.zero;
		sprite.enabled = false;
		tr.enabled = false;
	}

	IEnumerator ActivateTrailSprite() {
		yield return new WaitForSeconds(0.25f);
		sprite.enabled = true;
		tr.enabled = true;
	}
}
