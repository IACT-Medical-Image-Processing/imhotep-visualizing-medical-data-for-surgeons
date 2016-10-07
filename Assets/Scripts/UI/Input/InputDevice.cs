﻿using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;
using UI;

public interface InputDevice {

    void activateVisualization();
    void deactivateVisualization();
    bool isVisualizerActive();

	Vector2 getScrollDelta();

    //Creates a ray (e.g. a mouse device creates a ray from the main camera to the courser on the screen. A vive controller creates a ray from the controller in forward direction)
    Ray createRay();

	ButtonInfo updateButtonInfo ();
}