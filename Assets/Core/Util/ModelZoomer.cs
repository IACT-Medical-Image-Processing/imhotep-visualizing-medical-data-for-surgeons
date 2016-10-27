﻿// Model Zoomer 
//

using System;
using UnityEngine;

public class ModelZoomer : MonoBehaviour
{


    public float zoomingSpeed = 1;
    public float maxZoom = 2f;
    public float minZoom = 0.2f;

	private Vector3 targetZoom;
	private Vector3 zoomVelocity;

	public float autoZoomSpeed = 0.5f;
	private float scaleTime = 0.3f;

    private void Start()
    {
		targetZoom = transform.localScale;
    }


    private void Update()
    {
		if (UI.Core.instance.pointerIsOverPlatformUIObject == false) {
			if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
				

				float inputScroll = Input.GetAxis ("Mouse ScrollWheel");

				float zoom = transform.localScale.x + inputScroll / (1 / zoomingSpeed);

				zoom = Mathf.Clamp (zoom, minZoom, maxZoom);

				transform.localScale = new Vector3 (zoom, zoom, zoom);
				targetZoom = transform.localScale;
				//setTargetZoom ( new Vector3(zoom, zoom, zoom), 0.02f );
			}
		}

		// Auto-Zoom to target, if given:
		transform.localScale = Vector3.SmoothDamp(transform.localScale, targetZoom, ref zoomVelocity, scaleTime);
    }

	public void setTargetZoom( Vector3 zoom, float timeForScaling = 0f )
	{
		targetZoom = zoom;
		zoomVelocity = new Vector3 (0, 0, 0);
		if (timeForScaling == 0) {
			scaleTime = 1f;
			transform.localScale = targetZoom;
		} else {
			scaleTime = timeForScaling;
		}
	}
}

