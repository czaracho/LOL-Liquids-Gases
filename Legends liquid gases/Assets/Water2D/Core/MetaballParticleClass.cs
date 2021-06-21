using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Water2D;

public class MetaballParticleClass : MonoBehaviour {

	public GameObject MObject;
	public float LifeTime;
	private SpriteRenderer sprite;
	private Burner burner;
	private Color steamColor;
	private Color steamStrokeColor;

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
		//steamColor = Water2D_Spawner.instance.SmokeColor;
		//steamStrokeColor = Water2D_Spawner.instance.SmokeStrokeColor;

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
                redirectWater(new Vector2(1, 1));
                break;
            case "left":
                redirectWater(new Vector2(-1, 1));
                break;
            case "up":
                redirectWater(new Vector2(0, 2));
                break;
            case "down":
                redirectWater(new Vector2(0, -2));
                break;
            case "WaterPumpToContainer":
				StartCoroutine(ActivateTrailSprite());
				break;
			case "Burner":
				Debug.Log("Tocamos al barn burner");
				transformToSteam(new Vector2(0,0));
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
	

	void redirectWater(Vector2 waterDirection) {
		rb.velocity = Vector2.zero;
		sprite.enabled = false;
		tr.enabled = false;
	}

	void transformToSteam(Vector2 steamDirection)
	{
		
		rb.velocity = Vector2.zero;
		rb.AddForce(transform.right * 5f, ForceMode2D.Impulse);
		rb.gravityScale = -5;
	}

	IEnumerator ActivateTrailSprite() {
		yield return new WaitForSeconds(0.25f);
		sprite.enabled = true;
		tr.enabled = true;
	}
}
