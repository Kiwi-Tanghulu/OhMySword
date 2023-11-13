using Packets;
using UnityEngine;

public class PositionReporter : MonoBehaviour
{
	[SerializeField] float distanceThreshold = 0.1f;
	[SerializeField] float rotateThreshold = 0.1f;
	[SerializeField] float cooldown = 0.1f;

	private Vector3 lastPosition = Vector3.zero;
	private Vector3 recentPosition = Vector3.zero;

	private Vector3 lastEuler = Vector3.zero;
	private Vector3 recentEuler = Vector3.zero;

	private float sendTime = 0f;

	public void OnMoveHandle(Vector3 position)
	{
		if(MovePredicator(position))
		{
			recentPosition = position;
			TryReport();
		}
	}

	public void OnRotateHandle(Vector3 euler)
	{
		if(RotatePredicator(euler))
		{
			recentEuler = euler;
			TryReport();
		}
	}

	private void TryReport()
	{
		if(sendTime + cooldown < Time.time)
		{
			VectorPacket pos = new VectorPacket(recentPosition.x, recentPosition.y, recentPosition.z);
			VectorPacket euler = new VectorPacket(recentEuler.x, recentEuler.y, recentEuler.z);

			C_PlayerPacket playerPacket = new C_PlayerPacket();
			playerPacket.objectPacket = new ObjectPacket(RoomManager.Instance.PlayerID, (ushort)ObjectType.Player, pos, euler);

			NetworkManager.Instance.Send(playerPacket);

			lastPosition = recentPosition;
			lastEuler = recentEuler;
			sendTime = Time.time;
		}
	}

	private bool MovePredicator(Vector3 position) => ((lastPosition - position).sqrMagnitude > Mathf.Pow(distanceThreshold, 2));
	private bool RotatePredicator(Vector3 euler) => ((lastEuler - euler).sqrMagnitude > Mathf.Pow(rotateThreshold, 2));
}
