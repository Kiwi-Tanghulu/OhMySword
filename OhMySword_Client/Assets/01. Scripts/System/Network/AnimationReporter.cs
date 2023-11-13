using Packets;
using UnityEngine;

public class AnimationReporter : MonoBehaviour
{
	public void PublishAnimation(int animationType)
    {
        C_AnimationPacket packet = new C_AnimationPacket(RoomManager.Instance.PlayerID, (ushort)animationType);
        NetworkManager.Instance.Send(packet);
    }
}
