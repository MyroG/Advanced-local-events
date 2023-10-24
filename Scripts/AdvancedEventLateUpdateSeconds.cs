
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace myro
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class AdvancedEventLateUpdateSeconds : IAdvancedEvent
	{
		void LateUpdate()
		{
			Loop();
		}

		protected override void IncrementTimer()
		{
			_elapsedTime += Time.deltaTime;
		}
	}
}
