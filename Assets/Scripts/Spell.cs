using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {
	private bool isDrawing = false;
	public LineRenderer line;
	public Color lineStartColor;
	public Color lineEndColor;
	public float lineStep;
	public int disappearSpeed;
	private float disapperTimer = -1;
	public int minLoopDetection;
	public float loopDistance;
	private bool isLoop = false;
	private List<Vector3> pointsList = new List<Vector3>();

	void Awake() {
	}

	Vector3 GetMouseWorldPos() {
		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		return mousePos;
	}
	public void SetActiveDraw(bool draw) {
		isDrawing = draw;
	}
	void Paint() {
		line.positionCount = pointsList.Count;
		if (pointsList.Count != 0)
			line.SetPositions(pointsList.ToArray());
			//line.SetPosition(pointsList.Count - 1, pointsList[pointsList.Count - 1]);
		var mousePos = GetMouseWorldPos();
		if (pointsList.Count == 0 || (Vector2.Distance(pointsList[pointsList.Count - 1], mousePos) > lineStep))
			pointsList.Add(mousePos + Vector3.forward*transform.position.z);

		if (pointsList.Count > minLoopDetection) {
			for (int i = 0; i < pointsList.Count - minLoopDetection; i++) {
				if (Vector2.Distance(pointsList[i], mousePos) < loopDistance) isLoop = true;
			}
		}
		if (isLoop) {
			line.startColor = Color.green;
			line.endColor = Color.green;
			Cast();
			isDrawing = false;
			isLoop = false;
			Invoke("ClearLine", 1f);
		}
		disapperTimer++;
		if (disapperTimer % disappearSpeed == 0) pointsList.RemoveAt(0);
	}
	void ClearLine() {
		line.positionCount = 0;
		line.startColor = lineStartColor;
		line.endColor = lineEndColor;
		pointsList.Clear();
		isDrawing = false;
		isLoop = false;
	}
	void Cast() {
		//todo
	}
	void Update() {
		if(Input.GetMouseButton(0) && isDrawing) {
			Paint();
		}
		if (Input.GetMouseButtonUp(0)) {
			ClearLine();
		}
	}
}
