﻿//powerful class, allows to detect intersection with mesh, without requiring any collider, etc
//Works in editor only
//
// Main Author https://gist.github.com/MattRix
// Igor Aherne improved it to include object picking as well   facebook.com/igor.aherne
//https://github.com/MattRix/UnityDecompiled/blob/master/UnityEditor/UnityEditor/HandleUtility.cs
using System; using Cirrus.Objects;
using System.Collections.Generic;
using System.Linq;
//using System.Numerics;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace Cirrus.UnityEditorExt
{

    [InitializeOnLoad]
    public static class EditorHandles
    {
        static Type type_HandleUtility;
        static MethodInfo meth_IntersectRayMesh;
        static MethodInfo meth_PickObjectMeth;

        static EditorHandles()
        {
            var editorTypes = typeof(Editor).Assembly.GetTypes();

            type_HandleUtility = editorTypes.FirstOrDefault(t => t.Name == "HandleUtility");
            meth_IntersectRayMesh = type_HandleUtility.GetMethod("IntersectRayMesh",
                                                                  BindingFlags.Static | BindingFlags.NonPublic);
            meth_PickObjectMeth = type_HandleUtility.GetMethod("PickGameObject",
                                                                BindingFlags.Static | BindingFlags.Public,
                                                                null,
                                                                new[] { typeof(Vector2), typeof(bool) },
                                                                null);
        }


        //get a point from interected with any meshes in scene, based on mouse position.
        //WE DON'T NOT NEED to have to have colliders ;)
        //usually used in conjunction with  PickGameObject()
        public static bool IntersectRayMesh(Ray ray, MeshFilter meshFilter, out RaycastHit hit)
        {
            return IntersectRayMesh(ray, meshFilter.sharedMesh, meshFilter.transform.localToWorldMatrix, out hit);
        }

        //get a point from interected with any meshes in scene, based on mouse position.
        //WE DON'T NOT NEED to have to have colliders ;)
        //usually used in conjunction with  PickGameObject()
        public static bool IntersectRayMesh(Ray ray, Mesh mesh, Matrix4x4 matrix, out RaycastHit hit)
        {
            var parameters = new object[] { ray, mesh, matrix, null };
            bool result = (bool)meth_IntersectRayMesh.Invoke(null, parameters);
            hit = (RaycastHit)parameters[3];
            return result;
        }



        //select a gameObject in scene, based on mouse position.
        //Object DOES NOT NEED to have to have colliders ;)
        //If you DON'T want object to be included into  Selection.activeGameObject,
        //(parameter works only in gui functions and scene view delegates) specify updateSelection = false
        public static GameObject PickGameObject(Vector2 position, bool updateSelection = true, bool selectPrefabRoot = false)
        {

            if (updateSelection == false && UnityEngine.Event.current != null)
            {
                int blocking_ix = GUIUtility.GetControlID(FocusType.Passive);
                HandleUtility.AddDefaultControl(blocking_ix);
                GUIUtility.hotControl = blocking_ix; //tell unity that your control is active now, so it won't do selections etc.
            }

            GameObject pickedGameObject = (GameObject)meth_PickObjectMeth.Invoke(null,
                                                           new object[] { position, selectPrefabRoot });

            return pickedGameObject;
        }


        public static bool Raycast(
            IEnumerable<MeshFilter> meshFilters, 
            Ray ray,
            out GameObject gameObjectHit)
        {
            var cam = SceneView.currentDrawingSceneView.camera;

            gameObjectHit = null;
            float min = float.MaxValue;
            foreach (var mesh in meshFilters)
            {
                if (IntersectRayMesh(ray, mesh, out RaycastHit hit))
                {
                    float dist = (mesh.transform.position - cam.transform.position).magnitude;

                    if (dist < min)
                    {
                        min = dist;
                        gameObjectHit = mesh.gameObject;
                    }
                }
            }

            return gameObjectHit != null;
        }

    }
}