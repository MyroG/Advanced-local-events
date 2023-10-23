#define DEBUG_ADVANDED_EVENTS

using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;
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
			ChechReferences();

			_advancedEventLateUpdateSeconds.enabled = false;
			_advancedEventUpdateSeconds.enabled = false;
			_advancedEventLateUpdateFrames.enabled = false;
			_advancedEventUpdateFrames.enabled = false;
		}

		private void OnEnable()
		{
			ChechReferences();

			_advancedEventLateUpdateSeconds.enabled = true;
			_advancedEventUpdateSeconds.enabled = true;
			_advancedEventLateUpdateFrames.enabled = true;
			_advancedEventUpdateFrames.enabled = true;
		}

		private void ChechReferences()
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
		public int AdvancedSendCustomEventDelayedSeconds(UdonSharpBehaviour behaviour, string eventName, float delaySeconds, EventTiming eventTiming = EventTiming.Update)
		{
			ChechReferences();

			if (eventTiming == EventTiming.Update)
				return _advancedEventUpdateSeconds.AdvancedSendCustomEventDelayed(behaviour, eventName, delaySeconds, false);
			else
				return _advancedEventLateUpdateSeconds.AdvancedSendCustomEventDelayed(behaviour, eventName, delaySeconds, false);
		}

		public void RemoveCustomEventDelayedSeconds(int id, EventTiming eventTiming = EventTiming.Update)
		{
			ChechReferences();

			if (eventTiming == EventTiming.Update)
				_advancedEventUpdateSeconds.RemoveCustomEvent(id);
			else
				_advancedEventLateUpdateSeconds.RemoveCustomEvent(id);
		}

		public void DelayCustomEventDelayedSeconds(int id, float delaySeconds, EventTiming eventTiming = EventTiming.Update)
		{
			ChechReferences();
		}
		#endregion

		#region Delay frames
		public int AdvancedSendCustomEventDelayedFrames(UdonSharpBehaviour behaviour, string eventName, int delayFrames, EventTiming eventTiming = EventTiming.Update)
		{
			ChechReferences();

			if (eventTiming == EventTiming.Update)
				return _advancedEventUpdateFrames.AdvancedSendCustomEventDelayed(behaviour, eventName, delayFrames, true);
			else
				return _advancedEventLateUpdateFrames.AdvancedSendCustomEventDelayed(behaviour, eventName, delayFrames, true);
		}

		public void RemoveCustomEventDelayedFrames(int id, EventTiming eventTiming = EventTiming.Update)
		{
			ChechReferences();

			if (eventTiming == EventTiming.Update)
				_advancedEventUpdateFrames.RemoveCustomEvent(id);
			else
				_advancedEventLateUpdateFrames.RemoveCustomEvent(id);
		}

		public void DelayCustomEventDelayedFrames(int id, int delayFrames, EventTiming eventTiming = EventTiming.Update)
		{
			ChechReferences();
		}
		#endregion

		protected int AdvancedSendCustomEventDelayedSeconds(string eventName, float delaySeconds, EventTiming eventTiming = EventTiming.Update)
		{
			return AdvancedSendCustomEventDelayedSeconds(this, eventName, delaySeconds, eventTiming);
		} 
	}
}
 