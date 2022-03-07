//
// fiddling around from a forum post:
// https://forum.unity.com/threads/csgo-bullet-spray-editor.606919/
//

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SimpleDrawing : EditorWindow
{
	// Select this actual script in your /Editor folder, then drag the
	// textures you want for these markers on the script in the inspector.
	//
	// I set the texture imports to no compression and "Editor GUI and Legacy GUI"
	//
	// The pixel size of this GridTexture is used to scale the values in the Points
	// list below, just for convenience... and we assumes a square texture so if you
	// don't provide this as a square texture, not sure what you'll see!
	public Texture2D GridTexture;
	public Texture2D MarkTexture;


	[MenuItem("Example/Simple Drawing")]
	static void Init()
	{
		instance = (SimpleDrawing)EditorWindow.GetWindow(typeof(SimpleDrawing));

		instance.wantsMouseMove = true;
	}

	static SimpleDrawing instance;

	List<Vector2> Points = new List<Vector2>( new Vector2[] {
		new Vector2( -255, -255),
		new Vector2(    0,    0),
	});

	// I want it square so I'm forcing that...
	float squareSize
	{
		get
		{
			float sz = Mathf.Min( instance.position.width, instance.position.height);

			sz -= Mathf.Max( LEFT, TOP);

			return sz;
		}
	}

	// moving our window down for the above elements; not gonna bother to get tricky
	float LEFT { get { return 20.0f; } }
	float TOP { get { return 100.0f; } }

	Vector2 MouseToPoint( Vector2 mousePos)
	{
		mousePos.x -= LEFT;
		mousePos.y -= TOP;

		mousePos.x = (GridTexture.width * mousePos.x) / squareSize;
		mousePos.y = (GridTexture.height * mousePos.y) / squareSize;

		mousePos.x -= GridTexture.width / 2;
		mousePos.y -= GridTexture.height / 2;

		return mousePos;
	}

	Rect PointToWindowRect( Vector2 pos)
	{
		// offset the coord according to half the grid
		pos.x += GridTexture.width / 2;
		pos.y += GridTexture.height / 2;

		// scale the coord
		pos.x = (squareSize * pos.x) / GridTexture.width;
		pos.y = (squareSize * pos.y) / GridTexture.height;

		// mark is proportional according to supplied texture dimensions
		float marksize = (squareSize * MarkTexture.width) / GridTexture.width;

		// scale the rect
		Rect r = new Rect( 0, 0, marksize, marksize);

		pos.x += LEFT;
		pos.y += TOP;

		// move the rect
		r.center = pos;

		return r;
	}

	void RemovePointClosestToThisOne( Vector2 PointPos)
	{
		if (Points.Count > 0)
		{
			int closesti = 0;
			float closestDistance = 0.0f;
			for (int i = 0; i < Points.Count; i++)
			{
				float distance = (Points[i] - PointPos).magnitude;

				if (i == 0 || distance < closestDistance)
				{
					closesti = i;
					closestDistance = distance;
				}
			}

			Points.RemoveAt(closesti);
		}
	}

	void AddPointIfWithinBounds( Vector2 PointPos)
	{
		if (PointPos.x > -GridTexture.width / 2)
		{
			if (PointPos.x < GridTexture.width / 2)
			{
				if (PointPos.y > -GridTexture.height / 2)
				{
					if (PointPos.y < GridTexture.height / 2)
					{
						Points.Add( PointPos);
					}
				}
			}
		}
	}

	void OnGUI()
	{
		if (!instance) return;

		// code to read and display mouse coords, and also generate point coordinate
		{
			Event e = Event.current;
			GUILayout.Label("Mouse pos: " + e.mousePosition);

			Vector2 PointPos = MouseToPoint( e.mousePosition);
			GUILayout.Label("Point pos: " + PointPos);

			if (e.type == EventType.MouseDown)
			{
				if (e.button == 0)
				{
					AddPointIfWithinBounds( PointPos);
				}

				if (e.button == 1)
				{
					RemovePointClosestToThisOne( PointPos);
				}
			}
		}


		// TODO: I'll let you display the point coordinates if you really care


		// TODO: I'll let you format your output, write it to a file, etc.
		if (GUILayout.Button( "Spit Out Point Coordinates"))
		{
			for (int i = 0; i < Points.Count; i++)
			{
				Debug.Log( Points[i]);
			}
		}


		Rect r = new Rect( LEFT, TOP, squareSize, squareSize);

		GUI.DrawTexture( r, GridTexture);

		for (int i = 0; i < Points.Count; i++)
		{
			r = PointToWindowRect( Points[i]);

			GUI.DrawTexture( r, MarkTexture);
		}
	}

	// This keeps us constantly getting OnGUI() calls whenever the mouse is over us
	void OnInspectorUpdate()
	{
		if (EditorWindow.focusedWindow == this &&
			EditorWindow.mouseOverWindow == this)
		{
			this.Repaint();
		}
	}
}
