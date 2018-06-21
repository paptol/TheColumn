using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace Labs.ACW
{
    public class Level
    {
        //List
        public static List<Cylinder> Box1_Cylinders = new List<Cylinder>();
        public static List<Cylinder> Box2_Cylinders = new List<Cylinder>();
        public static Sphere SphereOfDoom = new Sphere(new Vector3(0, 0, 0), new Vector3(0, 0, 0), 3, 0, Materials.ForestGreen);
        public static Matrix4 Box0 = Matrix4.CreateTranslation(new Vector3(0, 15f, 0)); //EmitterBox Position
        public static Matrix4 Box1 = Box0 * Matrix4.CreateTranslation(0, -10, 0); //Level 1
        public static Matrix4 Box2 = Box1 * Matrix4.CreateTranslation(0, -10, 0); //Level 2
        public static Matrix4 Box3 = Box2 * Matrix4.CreateTranslation(0, -10, 0); // Doom

        public static void Inside()
        {
            //Level 1
            Box1_Cylinders.Add(new Cylinder(new Vector3(-1, 0, 2.3f), (float)Math.PI / 2, (float)Math.PI / 2, 0, ConvertToCm(7.5f), 5, Materials.silver));
            Box1_Cylinders.Add(new Cylinder(new Vector3(-1, 0, -2.3f), (float)Math.PI / 2, (float)Math.PI / 2, 0, ConvertToCm(7.5f), 5, Materials.silver));
            Box1_Cylinders.Add(new Cylinder(new Vector3(1, 0, 0), (float)Math.PI / 2, 0, 0, ConvertToCm(7.5f), 5, Materials.silver));
            Box1_Cylinders.Add(new Cylinder(new Vector3(-3, 0, 0), (float)Math.PI / 2, 0, 0, ConvertToCm(15), 5, Materials.silver));
            //Level 2
            Box2_Cylinders.Add(new Cylinder(new Vector3(-1, 2.5f, 0f), (float)Math.PI / 2, 3.9f, 0, ConvertToCm(15), 6.4f, Materials.silver));
            Box2_Cylinders.Add(new Cylinder(new Vector3(-1, -2.5f, 0f), (float)Math.PI / 2, -3.9f, 0, ConvertToCm(15), 6.4f, Materials.silver));

        }
        public static void DrawCylinders(Cylinder c, Matrix4 Level)
        {
            int uModelLocation = GL.GetUniformLocation(ACWWindow.mShader.ShaderProgramID, "uModel");
            c.MaterialsValues();
            Matrix4 cylinderPos = c.TranslationMatrix * Level * ACWWindow.mGroundModel;
            GL.UniformMatrix4(uModelLocation, true, ref cylinderPos);
            GL.DrawElements(BeginMode.Triangles, ACWWindow.mCylinderModel.Indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        // Converting value to cm 
        public static float ConvertToCm(float value)
        {
            return value * 0.06f;
        }

    }

}
