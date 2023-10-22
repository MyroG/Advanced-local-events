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
		private AdvancedEventLateUpdateSeconds _advancedEventLateUpdateSeconds;
		private AdvancedEventUpdateSeconds _advancedEventUpdateSeconds;
		private AdvancedEventLateUpdateFrames _advancedEventLateUpdateFrames;
		private AdvancedEventUpdateFrames _advancedEventUpdateFrames;

#region Delay seconds
		public int AdvancedSendCustomEventDelayedSeconds(UdonSharpBehaviour behaviour, string eventName, float delaySeconds, EventTiming eventTiming = EventTiming.Update)
		{
			if (eventTiming == EventTiming.Update)
				return _advancedEventUpdateSeconds.AdvancedSendCustomEventDelayed(behaviour, eventName, delaySeconds);
			else
				return _advancedEventLateUpdateSeconds.AdvancedSendCustomEventDelayed(behaviour, eventName, delaySeconds);
		}

		public void RemoveCustomEventDelayedSeconds(int id, EventTiming eventTiming = EventTiming.Update)
		{
			if (eventTiming == EventTiming.Update)
				_advancedEventUpdateSeconds.RemoveCustomEvent(id);
			else
				_advancedEventLateUpdateSeconds.RemoveCustomEvent(id);
		}

		public void DelayCustomEventDelayedSeconds(int id, float delaySeconds, EventTiming eventTiming = EventTiming.Update)
		{

		}
		#endregion

		#region Delay frames
		public int AdvancedSendCustomEventDelayedFrames(UdonSharpBehaviour behaviour, string eventName, int delayFrames, EventTiming eventTiming = EventTiming.Update)
		{
			if (eventTiming == EventTiming.Update)
				return _advancedEventUpdateFrames.AdvancedSendCustomEventDelayed(behaviour, eventName, delayFrames);
			else
				return _advancedEventLateUpdateFrames.AdvancedSendCustomEventDelayed(behaviour, eventName, delayFrames);
		}

		public void RemoveCustomEventDelayedFrames(int id, EventTiming eventTiming = EventTiming.Update)
		{
			if (eventTiming == EventTiming.Update)
				_advancedEventUpdateFrames.RemoveCustomEvent(id);
			else
				_advancedEventLateUpdateFrames.RemoveCustomEvent(id);
		}

		public void DelayCustomEventDelayedFrames(int id, int delayFrames, EventTiming eventTiming = EventTiming.Update)
		{

		}
		#endregion

		protected int AdvancedSendCustomEventDelayedSeconds(string eventName, float delaySeconds, EventTiming eventTiming = EventTiming.Update)
		{
			return AdvancedSendCustomEventDelayedSeconds(this, eventName, delaySeconds, eventTiming);
		} 
	}
}
 