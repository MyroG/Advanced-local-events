using UdonSharp;
using UnityEngine;
using VRC.Udon.Common.Enums;

namespace myro
{
	[RequireComponent(typeof(AdvancedEventUpdateSeconds))]
	[RequireComponent(typeof(AdvancedEventLateUpdateSeconds))]
	[RequireComponent(typeof(AdvancedEventUpdateFrames))]
	[RequireComponent(typeof(AdvancedEventLateUpdateFrames))]
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class AdvancedEventHandler : UdonSharpBehaviour
	{
		private AdvancedEventLateUpdateSeconds	_advancedEventLateUpdateSeconds;
		private AdvancedEventUpdateSeconds		_advancedEventUpdateSeconds;
		private AdvancedEventLateUpdateFrames	_advancedEventLateUpdateFrames;
		private AdvancedEventUpdateFrames		_advancedEventUpdateFrames;

		private void OnDisable()
		{
			CheckReferences();

			_advancedEventLateUpdateSeconds.enabled = false;
			_advancedEventUpdateSeconds.enabled = false;
			_advancedEventLateUpdateFrames.enabled = false;
			_advancedEventUpdateFrames.enabled = false;
		}

		private void OnEnable()
		{
			CheckReferences();

			_advancedEventLateUpdateSeconds.enabled = true;
			_advancedEventUpdateSeconds.enabled = true;
			_advancedEventLateUpdateFrames.enabled = true;
			_advancedEventUpdateFrames.enabled = true;
		}

		private void CheckReferences()
		{
			if (_advancedEventLateUpdateSeconds == null)
			{
				_advancedEventLateUpdateSeconds = GetComponent<AdvancedEventLateUpdateSeconds>();
				_advancedEventUpdateSeconds = GetComponent<AdvancedEventUpdateSeconds>();
				_advancedEventLateUpdateFrames = GetComponent<AdvancedEventLateUpdateFrames>();
				_advancedEventUpdateFrames = GetComponent<AdvancedEventUpdateFrames>();
			}
		}
#region Delay seconds

		/// <summary>
		/// Executes an event 
		/// </summary>
		/// <param name="behaviour">The UdonBehaviour that has the event</param>
		/// <param name="eventName">The name of the event</param>
		/// <param name="delaySeconds">The delay in seconds</param>
		/// <param name="eventTiming">If the event should be executed in an Update or a LateUpdate event</param>
		/// <returns>ID of the event that got created, use it to access the event later if needed</returns>
		public int AdvancedSendCustomEventDelayedSeconds(UdonSharpBehaviour behaviour, string eventName, float delaySeconds, EventTiming eventTiming = EventTiming.Update)
		{
			CheckReferences();

			if (eventTiming == EventTiming.Update)
				return _advancedEventUpdateSeconds.AdvancedSendCustomEventDelayed(behaviour, eventName, delaySeconds, false);
			else
				return _advancedEventLateUpdateSeconds.AdvancedSendCustomEventDelayed(behaviour, eventName, delaySeconds, false);
		}

		/// <summary>
		/// Removes an event to prevent it from being executed
		/// </summary>
		/// <param name="id">The ID of the event, note that the event needs to match the eventTiming</param>
		/// <param name="eventTiming">Event timing used by the event</param>
		public void RemoveCustomEventDelayedSeconds(int id, EventTiming eventTiming = EventTiming.Update)
		{
			CheckReferences();

			if (eventTiming == EventTiming.Update)
				_advancedEventUpdateSeconds.RemoveCustomEvent(id);
			else
				_advancedEventLateUpdateSeconds.RemoveCustomEvent(id);
		}

		/// <summary>
		/// Delays an event
		/// </summary>
		/// <param name="id">ID of the event that needs to be delayed</param>
		/// <param name="delaySeconds">Time it needs to be delayed</param>
		/// <param name="eventTiming">Event timing used by the event</param>
		public void DelayCustomEventSeconds(int id, float delaySeconds, EventTiming eventTiming = EventTiming.Update)
		{
			CheckReferences();
			if (eventTiming == EventTiming.Update)
				_advancedEventUpdateSeconds.DelayEvent(id, delaySeconds);
			else
				_advancedEventLateUpdateSeconds.DelayEvent(id, delaySeconds);
		}
		#endregion

		#region Delay frames

		/// <summary>
		/// Executes an event 
		/// </summary>
		/// <param name="behaviour">The UdonBehaviour that has the event</param>
		/// <param name="eventName">The name of the event</param>
		/// <param name="delayFrames">The number of frames the event needs to be delayed</param>
		/// <param name="eventTiming">If the event should be executed in an Update or a LateUpdate event</param>
		/// <returns>ID of the event that got created, use it to access the event later if needed</returns>
		public int AdvancedSendCustomEventDelayedFrames(UdonSharpBehaviour behaviour, string eventName, int delayFrames, EventTiming eventTiming = EventTiming.Update)
		{
			CheckReferences();

			if (eventTiming == EventTiming.Update)
				return _advancedEventUpdateFrames.AdvancedSendCustomEventDelayed(behaviour, eventName, delayFrames, true);
			else
				return _advancedEventLateUpdateFrames.AdvancedSendCustomEventDelayed(behaviour, eventName, delayFrames, true);
		}

		/// <summary>
		/// Removes an event to prevent it from being executed
		/// </summary>
		/// <param name="id">The ID of the event, note that the event needs to match the eventTiming</param>
		/// <param name="eventTiming">Event timing used by the event</param>
		public void RemoveCustomEventDelayedFrames(int id, EventTiming eventTiming = EventTiming.Update)
		{
			CheckReferences();

			if (eventTiming == EventTiming.Update)
				_advancedEventUpdateFrames.RemoveCustomEvent(id);
			else
				_advancedEventLateUpdateFrames.RemoveCustomEvent(id);
		}

		/// <summary>
		/// Delays an event
		/// </summary>
		/// <param name="id">ID of the event that needs to be delayed</param>
		/// <param name="delayFrames">Number of frames it needs to be delayed</param>
		/// <param name="eventTiming">Event timing used by the event</param>
		public void DelayCustomEventFrames(int id, int delayFrames, EventTiming eventTiming = EventTiming.Update)
		{
			CheckReferences();
			if (eventTiming == EventTiming.Update)
				_advancedEventUpdateFrames.DelayEvent(id, delayFrames);
			else
				_advancedEventLateUpdateFrames.DelayEvent(id, delayFrames);
		}
		#endregion


		/// <summary>
		/// Executes an event with a delay in seconds
		/// </summary>
		/// <param name="eventName">The name of the event</param>
		/// <param name="delaySeconds">The delay in seconds</param>
		/// <param name="eventTiming">If the event should be executed in an Update or a LateUpdate event</param>
		/// <returns>ID of the event that got created, use it to access the event later if needed</returns>
		protected int AdvancedSendCustomEventDelayedSeconds(string eventName, float delaySeconds, EventTiming eventTiming = EventTiming.Update)
		{
			return AdvancedSendCustomEventDelayedSeconds(this, eventName, delaySeconds, eventTiming);
		}

		/// <summary>
		/// Executes an event with a delay in frames
		/// </summary>
		/// <param name="eventName">The name of the event</param>
		/// <param name="delayFrames">The delay in frames</param>
		/// <param name="eventTiming">If the event should be executed in an Update or a LateUpdate event</param>
		/// <returns>ID of the event that got created, use it to access the event later if needed</returns>
		protected int AdvancedSendCustomEventDelayedFrames(string eventName, int delayFrames, EventTiming eventTiming = EventTiming.Update)
		{
			return AdvancedSendCustomEventDelayedFrames(this, eventName, delayFrames, eventTiming);
		}
	}
}
 