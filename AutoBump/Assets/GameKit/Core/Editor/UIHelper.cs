using UnityEngine;
using UnityEditor;

public static class UIHelper
{
    public static GUIStyle HeaderStyle;
    public static GUIStyle WarningStyle;

    public static GUIStyle MainStyle;
    public static GUIStyle SubStyle1;
    public static GUIStyle SubStyle2;
    public static GUIStyle ButtonStyle;
    public static GUIStyle RedButtonStyle;
    public static GUIStyle GreenButtonStyle;

    public static bool IsUIInitialized = false;

    private static Texture2D MakeTex (int width, int height, Color col)
    {
        Color32[] pix = new Color32[width * height];

        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new(width, height);
        result.SetPixels32(pix);
        result.Apply();

        return result;
    }

	public static void DrawArc (float angle, float range, Transform t)
	{
		Handles.color = Color.blue;
		Vector3 effectAngleA = Helper.DirFromAngle(-angle / 2, t);
		Vector3 effectAngleB = Helper.DirFromAngle(angle / 2, t);

		Vector3 tPosition = t.position;
		Handles.DrawWireArc(tPosition, Vector3.up, effectAngleA, angle, range);
		
		Handles.DrawLine(tPosition, tPosition + effectAngleA * range);
		Handles.DrawLine(tPosition, tPosition + effectAngleB * range);
	}

	public static void InitializeStyles ()
	{
		if (IsUIInitialized)
		{
			return;
		}

		HeaderStyle = new GUIStyle("box")
		{
			normal =
			{
				background = MakeTex(1, 1, new Color(0.3f, 0.3f, 0.4f, 1f)),
				textColor = Color.white
			},
			stretchWidth = true,
			fontStyle = FontStyle.Bold,
			hover =
			{
				background = MakeTex(1, 1, new Color(0.15f, 0.40f, 0.60f, 1f))
			},
			onHover =
			{
				background = MakeTex(1, 1, new Color(0.15f, 0.40f, 0.60f, 1f))
			}
		};

		ButtonStyle = new GUIStyle("box")
		{
			normal =
			{
				background = MakeTex(1, 1, new Color(0.15f, 0.3f, 0.4f, 1f)),
				textColor = Color.white
			},
			fontStyle = FontStyle.Bold,
			hover =
			{
				background = MakeTex(1, 1, new Color(0.15f, 0.40f, 0.50f, 1f))
			},
			onHover =
			{
				background = MakeTex(1, 1, new Color(0.15f, 0.40f, 0.50f, 1f))
			}
		};

		WarningStyle = new GUIStyle("box")
		{
			normal =
			{
				background = MakeTex(1, 1, new Color(0.75f, .0f, 0f, 1f)),
				textColor = Color.black
			}
		};

		MainStyle = new GUIStyle("box")
		{
			normal =
			{
				background = MakeTex(1, 1, new Color(0.2f, 0.2f, 0.2f, 1f)),
				textColor = Color.black
			}
		};

		SubStyle1 = new GUIStyle("box")
		{
			normal =
			{
				background = MakeTex(1, 1, new Color(0.25f, 0.25f, 0.25f, 1f)),
				textColor = Color.black
			}
		};

		SubStyle2 = new GUIStyle("box")
		{
			normal =
			{
				background = MakeTex(1, 1, new Color(0.3f, 0.3f, 0.3f, 1f)),
				textColor = Color.black
			}
		};

		RedButtonStyle = new GUIStyle("box")
		{
			normal =
			{
				background = MakeTex(1, 1, new Color(0.7f, 0.2f, 0.2f, 1f)),
				textColor = Color.white
			}
		};

		GreenButtonStyle = new GUIStyle("box")
		{
			normal =
			{
				background = MakeTex(1, 1, new Color(0.2f, 0.6f, 0.2f, 1f)),
				textColor = Color.white
			}
		};

		IsUIInitialized = true;
	}
}