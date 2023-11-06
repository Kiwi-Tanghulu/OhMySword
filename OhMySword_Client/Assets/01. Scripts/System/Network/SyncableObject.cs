using UnityEngine;

namespace Base.Network
{
    public abstract class SyncableObject : MonoBehaviour
    {
        public ushort ObjectID { get; protected set; }

        private Vector3 targetPosition = Vector3.zero;

        public void SetID(ushort id) => ObjectID = id;

        public abstract void OnCreated();
        public abstract void OnDeleted();

        private void Awake()
        {
            targetPosition = transform.position;
        }

        private void FixedUpdate()
        {
            // 포지션 러핑
        }

        public void Init(ushort id, Vector3 position, Vector3 rotation)
        {
            SetID(id);
            SetRotation(rotation);
            SetPosition(position, true);
        }

        public virtual void SetPosition(Vector3 position, bool immediately = false)
        {
            if(immediately)
                transform.position = position;
            
            targetPosition = position;
        }

        public virtual void SetRotation(Vector3 rotation)
        {
            transform.rotation = Quaternion.Euler(rotation);
        }

    }
}
