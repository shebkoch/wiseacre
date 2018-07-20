﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {
	private bool isDrawing = false;
	private Vector3 drawingPos;
	public LineRenderer line;
	public Color lineStartColor;
	public Color lineEndColor;
	public Color lineAcceptColor;
	public float lineStep;
	public int disappearSpeed;
	public int manaCost;
	private float disapperTimer = -1;
	public int minLoopDetection;
	public float loopDistance;
	private bool isLoop = false;
	private List<Vector3> pointsList = new List<Vector3>();

	void Awake() {
	}

	public void SetActiveDraw(bool draw) {
		isDrawing = draw;
	}

	void Paint()
	{
		drawingPos -= transform.position;
		line.positionCount = pointsList.Count;
		if (pointsList.Count != 0)
			line.SetPositions(pointsList.ToArray());
			//line.SetPosition(pointsList.Count - 1, pointsList[pointsList.Count - 1]);
		if (pointsList.Count == 0 || (Vector2.Distance(pointsList[pointsList.Count - 1], drawingPos) > lineStep))
			pointsList.Add(drawingPos - Vector3.forward); //todo
		Debug.Log(drawingPos - Vector3.forward);
		if (pointsList.Count > minLoopDetection) {
			for (var i = 0; i < pointsList.Count - minLoopDetection; i++) {
				if (Vector2.Distance(pointsList[i], drawingPos) < loopDistance) {
					isLoop = true;
					Cast(i);
					break;
				}

			}
		}
		if (isLoop) {
			//line.startColor = Color.green;
			//line.endColor = Color.green;
			line.startColor = lineAcceptColor;
			line.endColor = lineAcceptColor;

			PlayerParametersController.Instance.Mana -= pointsList.Count * manaCost;
			isDrawing = false;
			isLoop = false;
			Invoke("ClearLine", 0.5f);
		}
		disapperTimer++;
		if (Math.Abs(disapperTimer % disappearSpeed) < Double.Epsilon) pointsList.RemoveAt(0);
	}
	void ClearLine() {
		line.positionCount = 0;
		line.startColor = lineStartColor;
		line.endColor = lineEndColor;
		pointsList.Clear();
		isDrawing = false;
		isLoop = false;
	}
	void Cast(int contactIndex) {
		float maxDist = -1;
		Vector3 diameterPoint = Vector3.zero;
		for (int i = contactIndex; i < pointsList.Count; i++) {
			var curDist = Vector2.Distance(pointsList[contactIndex], pointsList[i]);
			if (curDist > maxDist) {
				maxDist = curDist;
				diameterPoint = pointsList[i];
			}
		}

		Vector3 centerPoint = (pointsList[contactIndex] + diameterPoint) / 2;
		centerPoint.z = -9;
		float length = Vector2.Distance(centerPoint, diameterPoint);

		List<GameObject> enemies = new List<GameObject>();
		enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
		foreach (var enemy in enemies) {
			if (Vector2.Distance(enemy.transform.position, centerPoint) < length)
				enemy.GetComponent<EnemyLogic>().ForceKill();
		}
	}

	private void SetDrawing()
	{
		var touchPos = PlayerInput.GetCastTouchPos();
		if (touchPos.HasValue)
		{
			isDrawing = true;
			drawingPos = touchPos.Value;
			Paint();
		}
		else ClearLine();
	}
	void Update()
	{
		SetDrawing();
	}
}
