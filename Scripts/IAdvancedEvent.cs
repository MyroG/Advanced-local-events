//#define DEBUG_ADVANDED_EVENTS

using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;

namespace myro
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class IAdvancedEvent : UdonSharpBehaviour
	{
		private DataDictionary _events;
		private int _eventsCount; //This is basically _events.Count, stored as a separate variable to very slightly improve the performance
		private DataDictionary _IdToEventTime;
		private DataList _sortedTime;
		private int _lastID;
		protected double _elapsedTime;

		public int AdvancedSendCustomEventDelayed(UdonSharpBehaviour behaviour, string eventName, double delay)
		{
			if (_events == null)
			{
				_events = new DataDictionary();
				_IdToEventTime = new DataDictionary();
			}

			_lastID++;
			double time = _elapsedTime + delay;

			while (_events.ContainsKey(time))
			{
				//Two events get executed at the same time, which would cause issues with the DataDictionary indexing, sine each index need to be unique, so we will just slightly delay the event
				time -= 0.001f;
			}

			DataList packedData = new DataList();
			packedData.Add(behaviour);
			packedData.Add(eventName);
			packedData.Add(_lastID);

			_events[time] = packedData;
			_eventsCount++;

			_IdToEventTime[_lastID] = time;
			_sortedTime = _events.GetKeys();
			_sortedTime.Sort();

#if DEBUG_ADVANDED_EVENTS
			Debug.Log($"Preparing to send {eventName} with delay {delay} (current delay : {time}), number of events {_events.Count}, ID is {_lastID}");
#endif
			enabled = true;

			return _lastID;
		}

		public void RemoveCustomEvent(int id)
		{
			if (_IdToEventTime == null || !_IdToEventTime.ContainsKey(id))
			{
#if DEBUG_ADVANDED_EVENTS
				Debug.Log($"Failed to remove event {id}");
#endif
				return;
			}

#if DEBUG_ADVANDED_EVENTS
			Debug.Log($"Removing event {id}");
#endif

			_sortedTime.Remove(_IdToEventTime[id]);
			_events.Remove(_IdToEventTime[id]);
			_eventsCount--;
			_IdToEventTime.Remove(id);
		}

		public void DelayEvent(int id, double delay)
		{
			if (_IdToEventTime == null || !_IdToEventTime.ContainsKey(id))
			{
#if DEBUG_ADVANDED_EVENTS
				Debug.Log($"Failed to delay event {id}, it doesn't exist");
#endif
				return;
			}

			double oldTime = _IdToEventTime[id].Double;
			double newTime = oldTime + delay;

			while (_events.ContainsKey(newTime))
			{
				//Two events get executed at the same time, which would cause issues with the DataDictionary indexing
				newTime -= 0.001f;
			}

#if DEBUG_ADVANDED_EVENTS
			Debug.Log($"Delaying event {id} by {delay}, from {oldTime} to {newTime}");
#endif

			_events[newTime] = _events[oldTime].DataList;
			_events.Remove(oldTime);
			_IdToEventTime[id] = newTime;
			_sortedTime = _events.GetKeys();
			_sortedTime.Sort();
		}

		protected void Loop()
		{
			IncrementTimer();
			if (_events != null && _eventsCount != 0)
			{
				while (_eventsCount != 0)
				{
					double frontTime = _sortedTime[0].Double;

					if (frontTime > _elapsedTime)
					{
						break;
					}
					DataList packedData = _events[frontTime].DataList;
					UdonSharpBehaviour behaviour = (UdonSharpBehaviour)packedData[0].Reference;

					if (Utilities.IsValid(behaviour))
					{
						behaviour.SendCustomEvent(packedData[1].String);
					}
					RemoveCustomEvent(packedData[2].Int);
				}
				
			}
			if (_events == null || _eventsCount == 0)
			{
				enabled = false;
			}
		}

		protected virtual void IncrementTimer()
		{

		}
	}
}