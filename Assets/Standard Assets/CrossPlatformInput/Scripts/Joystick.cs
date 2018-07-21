<<<<<<< HEAD
=======
/*
 * This script is from the Unity 5 Standard Assets with edits from Devin Curry
 * Search for changes tagged with the //DCURRY comment
 * Watch the tutorial here: www.Devination.com
 */
>>>>>>> ee8372107ccc4202b8a4c10e0919c8af43ee5e7c
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		public enum AxisOption
		{
			// Options for which axes to use
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}
<<<<<<< HEAD
=======
		public static bool isPress;

>>>>>>> ee8372107ccc4202b8a4c10e0919c8af43ee5e7c

		public int MovementRange = 100;
		public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
		public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

		Vector3 m_StartPos;
		bool m_UseX; // Toggle for using the x axis
		bool m_UseY; // Toggle for using the Y axis
		CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

<<<<<<< HEAD
		void OnEnable()
		{
			CreateVirtualAxes();
		}

        void Start()
        {
            m_StartPos = transform.position;
        }

=======
		void Start() //DCURRY: Changed to Start from OnEnable
		{
			m_StartPos = transform.position;
			CreateVirtualAxes();
		}

>>>>>>> ee8372107ccc4202b8a4c10e0919c8af43ee5e7c
		void UpdateVirtualAxes(Vector3 value)
		{
			var delta = m_StartPos - value;
			delta.y = -delta.y;
			delta /= MovementRange;
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Update(-delta.x);
			}

			if (m_UseY)
			{
				m_VerticalVirtualAxis.Update(delta.y);
			}
		}

		void CreateVirtualAxes()
		{
			// set axes to use
			m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
			m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

			// create new axes based on axes to use
			if (m_UseX)
			{
				m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
			}
		}


		public void OnDrag(PointerEventData data)
		{
			Vector3 newPos = Vector3.zero;

			if (m_UseX)
			{
				int delta = (int)(data.position.x - m_StartPos.x);
<<<<<<< HEAD
				delta = Mathf.Clamp(delta, - MovementRange, MovementRange);
=======
				//delta = Mathf.Clamp(delta, - MovementRange, MovementRange); //DCURRY: Dont want to clamp individual axis
>>>>>>> ee8372107ccc4202b8a4c10e0919c8af43ee5e7c
				newPos.x = delta;
			}

			if (m_UseY)
			{
				int delta = (int)(data.position.y - m_StartPos.y);
<<<<<<< HEAD
				delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
				newPos.y = delta;
			}
			transform.position = new Vector3(m_StartPos.x + newPos.x, m_StartPos.y + newPos.y, m_StartPos.z + newPos.z);
=======
				//delta = Mathf.Clamp(delta, -MovementRange, MovementRange); //DCURRY: Dont want to clamp individual axis
				newPos.y = delta;
			}
			//DCURRY: ClampMagnitude to clamp in a circle instead of a square
			transform.position = Vector3.ClampMagnitude(new Vector3(newPos.x, newPos.y, newPos.z), MovementRange) + m_StartPos;
>>>>>>> ee8372107ccc4202b8a4c10e0919c8af43ee5e7c
			UpdateVirtualAxes(transform.position);
		}


		public void OnPointerUp(PointerEventData data)
		{
<<<<<<< HEAD
=======
			isPress = false;
>>>>>>> ee8372107ccc4202b8a4c10e0919c8af43ee5e7c
			transform.position = m_StartPos;
			UpdateVirtualAxes(m_StartPos);
		}

<<<<<<< HEAD

		public void OnPointerDown(PointerEventData data) { }
=======
		public void OnPointerDown(PointerEventData data) {
			isPress = true;
		}
>>>>>>> ee8372107ccc4202b8a4c10e0919c8af43ee5e7c

		void OnDisable()
		{
			// remove the joysticks from the cross platform input
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Remove();
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis.Remove();
			}
		}
	}
}