using UnityEngine;

namespace Strungerhulder.Pool
{
	/// <summary>
	/// Implements a Pool for Component types.
	/// </summary>
	/// <typeparam name="T">Specifies the component to pool.</typeparam>
	public abstract class ComponentPoolSO<T> : PoolSO<T> where T : Component
	{
		private Transform m_PoolRoot;
		private Transform PoolRoot
		{
			get
			{
				if (m_PoolRoot == null)
				{
					m_PoolRoot = new GameObject(name).transform;
					m_PoolRoot.SetParent(m_Parent);
				}

				return m_PoolRoot;
			}
		}

		private Transform m_Parent;


		/// <summary>
		/// Parents the pool root transform to <paramref name="t"/>.
		/// </summary>
		/// <param name="t">The Transform to which this pool should become a child.</param>
		/// <remarks>NOTE: Setting the parent to an object marked DontDestroyOnLoad will effectively make this pool DontDestroyOnLoad.<br/>
		/// This can only be circumvented by manually destroying the object or its parent or by setting the parent to an object not marked DontDestroyOnLoad.</remarks>
		public void SetParent(Transform t)
		{
			m_Parent = t;
			PoolRoot.SetParent(m_Parent);
		}

		public override T Request()
		{
			T member = base.Request();
			member.gameObject.SetActive(true);
			return member;
		}

		public override void Return(T member)
		{
			member.transform.SetParent(PoolRoot.transform);
			member.gameObject.SetActive(false);
			base.Return(member);
		}

		public override void OnDisable()
		{
			base.OnDisable();
			if (m_PoolRoot != null)
			{
#if UNITY_EDITOR
				DestroyImmediate(m_PoolRoot.gameObject);
#else
				Destroy(m_PoolRoot.gameObject);
#endif
			}
		}


        protected override T Create()
        {
            T newMember = base.Create();
            newMember.transform.SetParent(PoolRoot.transform);
            newMember.gameObject.SetActive(false);
            return newMember;
        }
	}
}
