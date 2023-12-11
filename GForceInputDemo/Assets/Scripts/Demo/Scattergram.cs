using UnityEngine;
using UnityEngine.UI;

public class Scattergram : Graphic
{
    [HideInInspector]
    public float[] point = new float[180];
    [Space]
    public float lineWidth = 50f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        float length = point.Length;
        Vector3 size = GetComponent<RectTransform>().sizeDelta;
        for (int i = 0; i < point.Length - 1; i++)
        {
            //line
            UIVertex[] verts_line = new UIVertex[4];

            verts_line[0].position = new Vector3(i / length * size.x, point[i] * size.y) - size * 0.5f;
            verts_line[0].uv0 = new Vector2(i / length, point[i]);
            verts_line[0].color = color;

            verts_line[1].position = new Vector3(i / length * size.x, point[i] * size.y + lineWidth) - size * 0.5f;
            verts_line[1].uv0 = new Vector2((i + 1) / length, point[i] + lineWidth);
            verts_line[1].color = color;

            verts_line[2].position = new Vector3((i + 1) / length * size.x, point[i + 1] * size.y + lineWidth) - size * 0.5f;
            verts_line[2].uv0 = new Vector2((i + 1) / length, point[i + 1] + lineWidth);
            verts_line[2].color = color;

            verts_line[3].position = new Vector3((i + 1) / length * size.x, point[i + 1] * size.y) - size * 0.5f;
            verts_line[3].uv0 = new Vector2(i / length, point[i + 1]);
            verts_line[3].color = color;

            vh.AddUIVertexQuad(verts_line);

            //face
            UIVertex[] verts_face = new UIVertex[4];

            Color color0 = new Color(1, 1, 1, 0);
            verts_face[0].position = new Vector3(i / length * size.x, 0 * size.y ) - size * 0.5f;
            verts_face[0].uv0 = new Vector2(i / length, point[i]);
            verts_face[0].color = color0;

            Color color1 = new Color(color.r, color.g, color.b, point[i] * 0.5f);
            verts_face[1].position = new Vector3(i / length * size.x, point[i] * size.y) - size * 0.5f;
            verts_face[1].uv0 = new Vector2((i + 1) / length, point[i]);
            verts_face[1].color = color1;

            Color color2 = new Color(color.r, color.g, color.b, point[i + 1] * 0.5f);
            verts_face[2].position = new Vector3((i + 1) / length * size.x, point[i + 1] * size.y) - size * 0.5f;
            verts_face[2].uv0 = new Vector2((i + 1) / length, point[i + 1]);
            verts_face[2].color = color2;

            Color color3 = new Color(1, 1, 1, 0);
            verts_face[3].position = new Vector3((i + 1) / length * size.x, 0 * size.y ) - size * 0.5f;
            verts_face[3].uv0 = new Vector2(i / length, point[i + 1]);
            verts_face[3].color = color3;

            vh.AddUIVertexQuad(verts_face);
        }
    }
}
