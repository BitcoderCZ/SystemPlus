using System;
using System.Collections.Generic;
using System.Linq;
using SystemPlus.ClassSupport;
using SystemPlus.Extensions;
using SystemPlus.Vectors;

namespace SystemPlus.GameEngines
{
    public class GameObject : ICloneSupport<GameObject>
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale { get { return scale; } set { scale = value; scaleUpdate = true; } }
        private Vector3 scale;
        private bool scaleUpdate;

        private Mesh mainMesh;
        public Mesh mesh;
        private Component[] components;

        public bool Active;

        private GameEngine3D engine;

        public GameObject(GameEngine3D _engine)
        {
            Position = new Vector3();
            Rotation = new Vector3();
            scale = new Vector3(1, 1, 1);
            scaleUpdate = false;
            Active = true;
            mainMesh = new Mesh(engine);
            mesh = new Mesh(engine);
            engine = _engine;
            components = new Component[999];
        }

        public GameObject(Mesh _mesh, GameEngine3D _engine)
        {
            Position = new Vector3();
            Rotation = new Vector3();
            scale = new Vector3(1, 1, 1);
            scaleUpdate = false;
            Active = true;
            mainMesh = _mesh.Clone();
            mesh = mainMesh.Clone();
            engine = _engine;
            components = new Component[999];
        }

        public void Render()
        {
            ScaleMesh();
            if (Active && mesh != null)
                mesh.Render(Position, Rotation);
            int i = 0;
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < components.Length; i++)
                if (components[i] != null)
                    components[i].Update(deltaTime);
        }

        public void LateUpdate()
        {
            for (int i = 0; i < components.Length; i++)
                if (components[i] != null)
                    components[i].LateUpdate();
        }

        public Mesh GetMesh()
        {
            ScaleMesh();
            return mesh.Clone();
        }

        public Mesh GetOriginalMesh() => mainMesh.Clone();

        private void ScaleMesh()
        {
            if (!scaleUpdate)
                return;

            scaleUpdate = false;

            Triangle[] _tris = mainMesh.tris.ToArray();
            Triangle[] newTris = new Triangle[_tris.Length];
            for (int i = 0; i < _tris.Length; i++)
            {
                Vector3[] _verts = _tris[i].verts;
                newTris[i] = new Triangle(_verts[0] * scale, _verts[1] * scale, _verts[2] * scale);
            }
            mesh.tris = newTris.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_component"></param>
        /// <param name="pos">0 >= pos < 9999</param>
        public void AddComponent<T>(T _component, int pos) where T : Component
        {
            _component.Initialize(this, engine);
            components[pos] = _component;
            components[pos].Start();
        }

        public GameObject Clone()
        {
            GameObject _gm = new GameObject(mainMesh, engine);
            _gm.Position = Position;
            _gm.Rotation = Rotation;
            _gm.Scale = scale;
            return _gm;
        }
    }

    public class Component
    {
        private GameObject GM;
        public GameObject gameObject { get { return GM; } }
        private GameEngine3D GE3D;
        public GameEngine3D Engine { get { return GE3D; } }
        private bool initialized = false;

        public virtual void Start() { }
        public virtual void Update(float deltaTime) { }
        public virtual void LateUpdate() { }

        public void Initialize(GameObject _gm, GameEngine3D _engine)
        {
            if (!initialized)
            {
                initialized = true;
                GM = _gm;
                GE3D = _engine;
            }
        }
    }

    public class Mesh : ICloneSupport<Mesh>
    {
        public List<Triangle> tris;

        private GameEngine3D engine;

        public Mesh(GameEngine3D _engine)
        {
            tris = new List<Triangle>();
            engine = _engine;
        }

        public Mesh(List<Triangle> _tris, GameEngine3D _engine)
        {
            tris = _tris;
            engine = _engine;
        }

        public Mesh(Triangle[] _tris, GameEngine3D _engine)
        {
            tris = _tris.ToList();
            engine = _engine;
        }

        public void Render(Vector3 Position, Vector3 Rotation)
        {
            List<Triangle> _tris = tris.Clone();
            for (int i = 0; i < _tris.Count; i++)
                _tris[i].Rotate(Rotation);

            for (int i = 0; i < _tris.Count; i++)
                _tris[i].Translate(Position);

            for (int i = 0; i < _tris.Count; i++)
                _tris[i].ApplyPerspective(engine.Z0);

            for (int i = 0; i < _tris.Count; i++)
                _tris[i].CenterScreen(new Vector2(engine.BufferSize.x / 3, engine.BufferSize.y / 1));
            /*for (int i = 0; i < _tris.Count; i++)
            {
                for (int y = 0; y < _tris[i].verts.Length; y++)
                {
                    _tris[i].verts[y] = new Vector3(_tris[i].verts[y].x + 20, _tris[i].verts[y].y + 20);
                }
            }*/

            for (int i = 0; i < _tris.Count; i++)
            {
                Triangle tri = _tris[i];
                engine.DrawTriangle((Vector2Short)tri.verts[0], (Vector2Short)tri.verts[1],
                    (Vector2Short)tri.verts[2], PIXEL.PIXEL_SOLID, CONSOLE_COLOR.Green);
            }
        }

        public static Mesh FromFile(string[] lines, FileType ft, GameEngine3D _engine)
        {
            switch (ft)
            {
                case FileType.Obj:
                    List<Triangle> _tris = new List<Triangle>();
                    Vector3[] verts = new Vector3[lines.Length];
                    int vertIndex = 0;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        switch (lines[i][0])
                        {
                            case 'v': // vertex pos
                                string[] vert = lines[i].Split(' ');
                                verts[vertIndex] = new Vector3(vert[1], vert[2], vert[3]);
                                vertIndex++;
                                break;
                            case 'f': // triangle, pos in vertex array, starts at 1 -   -1
                                string[] tri = lines[i].Split(' ');
                                if (lines[i].Contains('/'))
                                    _tris.Add(new Triangle(verts[tri[1].Split('/')[0].ToInt() - 1],
                                        verts[tri[2].Split('/')[0].ToInt() - 1],
                                        verts[tri[3].Split('/')[0].ToInt() - 1]));
                                else
                                    _tris.Add(new Triangle(verts[tri[1].ToInt() - 1], verts[tri[2].ToInt() - 1],
                                        verts[tri[3].ToInt() - 1]));
                                break;
                        }
                    }
                    return new Mesh(_tris, _engine);
                default:
                    return null;
            }
        }

        public Mesh Clone() => new Mesh(tris.Clone(), engine);

        public enum FileType
        {
            Obj = 1,
        }
    }

    public struct Triangle : ICloneSupport<Triangle>
    {
        public readonly Vector3[] verts;

        public Triangle(Vector3[] _verts)
        {
            if (_verts.Length != 3)
                throw new Exception("_tris.Lenght must be 3");

            verts = new Vector3[] { _verts[0], _verts[1], _verts[2] };
        }

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            verts = new Vector3[] { v1, v2, v3 };
        }

        public void Rotate(Vector3 Rotation)
        {
            for (int i = 0; i < verts.Length; i++)
                verts[i] = Vector3.Rotate(verts[i], Rotation);
        }

        public void Translate(Vector3 Translation)
        {
            for (int i = 0; i < verts.Length; i++)
                verts[i] = Vector3.Translate(verts[i], Translation);
        }

        public void ApplyPerspective(float Z0)
        {
            for (int i = 0; i < verts.Length; i++)
                verts[i] = Vector3.ApplyPerspective(verts[i], Z0);
        }

        public void CenterScreen(Vector3 Size)
        {
            for (int i = 0; i < verts.Length; i++)
                verts[i] = Vector3.CenterScreen(verts[i], Size);
        }

        public Triangle Clone()
        {
            return new Triangle(verts.Clone<Vector3>());
        }
    }
}
