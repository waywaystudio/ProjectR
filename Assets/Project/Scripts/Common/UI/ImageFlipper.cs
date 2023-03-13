using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
	[RequireComponent(typeof(RectTransform)), RequireComponent(typeof(Graphic)), DisallowMultipleComponent]
	public class ImageFlipper : MonoBehaviour, IMeshModifier
	{

		[SerializeField] private bool horizontal;
		[SerializeField] private bool vertical;
	
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="UnityEngine.UI.UIFlippable"/> should be flipped horizontally.
		/// </summary>
		/// <value><c>true</c> if horizontal; otherwise, <c>false</c>.</value>
		public bool Horizontal
		{
			get => horizontal;
			set { horizontal = value; GetComponent<Graphic>().SetVerticesDirty(); }
		}
	
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="UnityEngine.UI.UIFlippable"/> should be flipped vertically.
		/// </summary>
		/// <value><c>true</c> if vertical; otherwise, <c>false</c>.</value>
		public bool Vertical
		{
			get => vertical;
			set { vertical = value; GetComponent<Graphic>().SetVerticesDirty(); }
		}
		
		public void ModifyMesh(VertexHelper vertexHelper)
		{
			if (!enabled)
				return;
		
			var list = new List<UIVertex>();
			vertexHelper.GetUIVertexStream(list);
		
			ModifyVertices(list); // calls the old ModifyVertices which was used on pre 5.2
		
			vertexHelper.Clear();
			vertexHelper.AddUIVertexTriangleStream(list);
		}

		public void ModifyMesh(Mesh mesh)
		{
			if (!enabled)
				return;

			var list = new List<UIVertex>();
			using (var vertexHelper = new VertexHelper(mesh))
			{
				vertexHelper.GetUIVertexStream(list);
			}

			ModifyVertices(list); // calls the old ModifyVertices which was used on pre 5.2

			using (var vertexHelper2 = new VertexHelper())
			{
				vertexHelper2.AddUIVertexTriangleStream(list);
				vertexHelper2.FillMesh(mesh);
			}
		}

		public void ModifyVertices(List<UIVertex> verts)
		{
			if (!enabled)
				return;

			var rt = transform as RectTransform;
		
			for (var i = 0; i < verts.Count; ++i)
			{
				var v = verts[i];
			
				// Modify positions
				v.position = new Vector3(
					(horizontal ? (v.position.x + (rt.rect.center.x - v.position.x) * 2) : v.position.x),
					(vertical ?  (v.position.y + (rt.rect.center.y - v.position.y) * 2) : v.position.y),
					v.position.z
				);
			
				// Apply
				verts[i] = v;
			}
		}
		

#if UNITY_EDITOR
		protected void OnValidate()
		{
			GetComponent<Graphic>().SetVerticesDirty();
		}
#endif
	}
}
