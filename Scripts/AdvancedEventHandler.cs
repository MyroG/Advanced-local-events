#define DEBUG_ADVANDED_EVENTS

using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace myro
{
	public class AdvancedEventHandler : UdonSharpBehaviour
	{
		private DataDictionary _events;
		private DataDictionary _IdToEvent;
		private DataList _sortedTime;
		private int _lastID;
		private float _elapsedTime;

		public int AdvancedSendCustomEventDelayedSeconds(UdonSharpBehaviour behaviour, string eventName, float delaySeconds)
		{
			enabled = true;
			if (_events == null)
			{
				_events = new DataDictionary();
				_IdToEvent = new DataDictionary();
			}

			_lastID++;
			float timeMS = _elapsedTime + delaySeconds;

			while (_events.ContainsKey(timeMS))
			{
				//Two events get executed at the same time, which would cause issues with the DataDictionary indexing, sine each index need to be unique, so we will just slightly delay the event
				timeMS += 0.001f;
			}
			 
			DataList packedData = new DataList();
			packedData.Add(behaviour);
			packedData.Add(eventName);
			packedData.Add(_lastID);

			_events[timeMS] = packedData;
			_IdToEvent[_lastID] = timeMS;
			_sortedTime = _events.GetKeys();
			_sortedTime.Sort();

#if DEBUG_ADVANDED_EVENTS
			Debug.Log($"Preparing to send {eventName} in {delaySeconds}s ({timeMS}), number of events {_events.Count}");
#endif
			return _lastID;
		}

		public void RemoveCustomEvent(int id)
		{
			if (_IdToEvent == null || !_IdToEvent.ContainsKey(id))
			{
#if DEBUG_ADVANDED_EVENTS
				Debug.Log($"Failed to remove event {id}");
#endif
				return;
			}

#if DEBUG_ADVANDED_EVENTS
			Debug.Log($"Removing event {id}");
#endif

			_sortedTime.Remove(_IdToEvent[id]);
			_events.Remove(_IdToEvent[id]);
			_IdToEvent.Remove(id);
		}

		private void Update()
		{
			if (_events != null && _events.Count != 0)
			{
				while (_events.Count != 0)
				{
					float frontTime = _sortedTime[0].Float;

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
				if (_events.Count == 0)
				{
					enabled = false;
				}
			}

			_elapsedTime += Time.deltaTime;
		}
	}
}
