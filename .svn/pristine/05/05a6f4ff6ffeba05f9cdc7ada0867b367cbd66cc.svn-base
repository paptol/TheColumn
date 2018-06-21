using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.ACW
{
    enum CameraType
    {
        StaticCam,
        RotationCam,
        FreeCam,
        FollowCam,
    }
    /// <summary>
    /// 
    /// Main Camera class inside this class are creted 3 modes of Camera 
    ///  
    /// </summary>
    class Camera
    {
        public static CameraType Type = new CameraType();

        public static void Update(Camera camera)
        {
            if (Camera.Type == CameraType.StaticCam)
            {
                camera.Reset();
            }
            else
            {

                if (Camera.Type == CameraType.RotationCam)
                {
                    camera.Rotate_World_Y(0.005f);
                    int uView = GL.GetUniformLocation(ACWWindow.mShader.ShaderProgramID, "uView");
                    GL.UniformMatrix4(uView, true, ref ACWWindow.mView);
                }
                else if (Camera.Type == CameraType.FollowCam)
                {
                    if (Sphere.DrawList.Count != 0)
                    {
                        ACWWindow.mView = Matrix4.CreateTranslation(0, 0, -10) * Matrix4.CreateTranslation(-Sphere.DrawList[0].mPosition);
                        int uView = GL.GetUniformLocation(ACWWindow.mShader.ShaderProgramID, "uView");
                        GL.UniformMatrix4(uView, true, ref ACWWindow.mView);

                        ACWWindow.mGroundModel = Matrix4.CreateTranslation(1, -0.5f, 0f);
                    }
                }
                else if (Camera.Type != CameraType.FreeCam)
                {
                    Camera.Type = CameraType.StaticCam;
                }
            }
        }
        // World rotation camera function by Y
        public void Rotate_World_Y(float angle)
        {
            Vector3 t = ACWWindow.mView.ExtractTranslation();
            Matrix4 translation = Matrix4.CreateTranslation(t);
            Matrix4 inverseTranslation = Matrix4.CreateTranslation(-t);
            ACWWindow.mView = ACWWindow.mView * inverseTranslation * Matrix4.CreateRotationY(angle) * translation;
        }

        // World rotation camera function by X
        public void Rotate_World_X(float angle)
        {
            Vector3 t = ACWWindow.mGroundModel.ExtractTranslation();
            Matrix4 translation = Matrix4.CreateTranslation(t);
            Matrix4 inverseTranslation = Matrix4.CreateTranslation(-t);
            ACWWindow.mGroundModel = ACWWindow.mGroundModel * inverseTranslation * Matrix4.CreateRotationX(angle) * translation;

        }
        public void MoveCamera(Matrix4 transformation)
        {
            ACWWindow.mView = ACWWindow.mView * transformation;
            int uView = GL.GetUniformLocation(ACWWindow.mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref ACWWindow.mView);

            UpdateLightPositions();

            int uEyePositionLocation = GL.GetUniformLocation(ACWWindow.mShader.ShaderProgramID, "uEyePosition");
            Vector4 eyePosition = Vector4.Transform(new Vector4(1, 1, 1, 1), ACWWindow.mView);
            GL.Uniform4(uEyePositionLocation, eyePosition);

        }
        //Update positions of light
        public void UpdateLightPositions()
        {
            
            for (int i = 0; i < Light.mLights.Count; i++)
            {
                int uLightDirectionLocation = GL.GetUniformLocation(ACWWindow.mShader.ShaderProgramID, string.Format("uLight[{0}].Position", i)); 
                Vector4 position = Vector4.Transform(new Vector4(Light.mLights[i].mPosition, 1), ACWWindow.mView);
                GL.Uniform4(uLightDirectionLocation, position);
            }
        }
        public void Reset()
        {
            ACWWindow.mView = Matrix4.CreateTranslation(-0.8f, -0.5f, -50f);
            int uView = GL.GetUniformLocation(ACWWindow.mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref ACWWindow.mView);

            ACWWindow.mGroundModel = Matrix4.CreateTranslation(1, -0.5f, 0f);
        }
        // Free camera buttons functions 
        public void FreeCamera(KeyPressEventArgs e)
        {
            float speed = 1f;

            // X Cam
            if (e.KeyChar == 'a')
            {
                MoveCamera(Matrix4.CreateTranslation(speed, 0, 0));

            }
            if (e.KeyChar == 'd')
            {
                MoveCamera(Matrix4.CreateTranslation(-speed, 0, 0));

            }

            // Y Cam
            if (e.KeyChar == ' ') 
            {
                MoveCamera(Matrix4.CreateTranslation(0, -speed, 0));
            }
            if (e.KeyChar == 'c') 
            {
                MoveCamera(Matrix4.CreateTranslation(0, speed, 0));
            }

            // Z Cam
            if (e.KeyChar == 'w') 
            {
                MoveCamera(Matrix4.CreateTranslation(0, 0, speed));
            }
            if (e.KeyChar == 's') 
            {
                MoveCamera(Matrix4.CreateTranslation(0, 0, -speed));
            }

            //Rotate Y-Axis Cam
            if (e.KeyChar == 'e')
            {
                MoveCamera(Matrix4.CreateRotationX(0.05f));
            }
            if (e.KeyChar == 'q')
            {
                MoveCamera(Matrix4.CreateRotationX(-0.05f));
            }

            //Rotate World Cam
            if (e.KeyChar == 'z')
            {
                Rotate_World_Y(-0.05f);
            }
            if (e.KeyChar == 'x')
            {
                Rotate_World_Y(0.05f);
            }
        }
    }
}
