
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace myro
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class AdvancedEventUpdateSeconds : IAdvancedEvent
	{
		private void Update()
		{
			_elapsedTime += Time.deltaTime;
			Loop();
		}
	}
}
