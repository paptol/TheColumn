using Labs.Utility;
using OpenTK;
using OpenTK.Graphics;
using System;
using OpenTK.Graphics.OpenGL;

using System.Collections.Generic;




namespace Labs.ACW
{
    public class ACWWindow : GameWindow
    {
        Camera camera = new Camera();
        private int[] mVBO = new int[6];
        private int[] mVAO = new int[3];
        public static Vector3 gravityAcceleration = new Vector3(0, -9.81f, 0);
        public static bool gravityOn;
        public static float E = 0.8f;
        public static float timestep;
        public static float sphereLimit = 100;
        private Timer mTimer = new Timer();
        private float viewDistance = 100;
        public static ShaderUtility mShader;
        int uModelLocation;
        public static ModelUtility mSphereModel;
        public static ModelUtility mCubeModel;
        public static ModelUtility mCylinderModel;
        public static ModelUtility mParticleCube;
        private Texture Txt1;
        private Texture Txt2;

        //Matrices
        public static Matrix4 mGroundModel = Matrix4.CreateTranslation(1, -0.5f, 0f);
        public static Matrix4 mContainerMatrix = Matrix4.CreateScale(new Vector3(25, 100, 25)) * Matrix4.CreateRotationX((float)Math.PI);
        public static Matrix4 mBoxMatrix = Matrix4.CreateScale((new Vector3(25, (mContainerMatrix.ExtractScale().Y / 4), 25))) * Matrix4.CreateRotationX((float)Math.PI);
        public static Matrix4 mView = Matrix4.CreateTranslation(-0.8f, -0.5f, -50f);


        public ACWWindow()
            : base(
                1280, // Width
                720, // Height
                GraphicsMode.Default,
                "Assessed Coursework",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, // major
                3, // minor
                GraphicsContextFlags.ForwardCompatible
                )
        {
            #region User guide
            Console.WriteLine("Use the following keys to move:");
            Console.WriteLine("\nW = Forward");
            Console.WriteLine("S = Backward");
            Console.WriteLine("A = Left ");
            Console.WriteLine("D = Right ");
            Console.WriteLine("SPACE = Up");
            Console.WriteLine("C = Down");
            Console.WriteLine("Q = Axis Y UP");
            Console.WriteLine("E = Axis Y Down");
            Console.WriteLine("Z = WordR Left");
            Console.WriteLine("X = WordR Right");
            Console.WriteLine("\nEnter = FullScreen Mode");
            Console.WriteLine("\nESC = Close Program");
            Console.WriteLine("\nUse the following keys to change the camera:");
            Console.WriteLine("\n1 = Free Mode Camera");
            Console.WriteLine("2 = StaticCam Camera");
            Console.WriteLine("3 = Rotation Camera");
            Console.WriteLine("4 = Follow Camera");

            #endregion
        }

        //Main Methods

     
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            uModelLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");

            float viewDistance = 100;

            //Resets matrix
            int uProjectionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uProjection");
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(1, (float)ClientRectangle.Width / ClientRectangle.Height, 0.5f, viewDistance);
            GL.UniformMatrix4(uProjectionLocation, true, ref projection);

            //Renders the objects to the screen buffer
            //FrameBuffer.Unbind_Buffer();
            Render_Objects();

            GL.BindVertexArray(0);
            this.SwapBuffers();

            GL.UniformMatrix4(uProjectionLocation, true, ref projection);

        }  
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            timestep = mTimer.GetElapsedSeconds();

            Sphere.Update();
            Light.Update();
            EmitterBox.Update();
            SpotLight.Update();
            Camera.Update(camera);

        }
        protected override void OnLoad(EventArgs e)
        {
            // On  load methods to  reading shaders ,models , textures ...
            GL.ClearColor(Color4.Black);
            GL.Enable(EnableCap.DepthTest);
            Camera.Type = CameraType.FreeCam;

            mShader = new ShaderUtility(@"ACW/Shaders/vSdr.vert", @"ACW/Shaders/fLight.frag");

            mSphereModel = ModelUtility.LoadModel(@"Utility/Models/sphere.bin");
            mCubeModel = ModelUtility.LoadModel(@"Utility/Models/MainCube.sjg");
            mCylinderModel = ModelUtility.LoadModel(@"Utility/Models/cylinder.bin");


            int vPositionLocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vPosition");
            int vNormalLocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vNormal");
            int vTexCoords = GL.GetAttribLocation(mShader.ShaderProgramID, "vTexCoords");

            int size;
            GL.UseProgram(mShader.ShaderProgramID);

            GL.GenVertexArrays(mVAO.Length, mVAO);
            GL.GenBuffers(mVBO.Length, mVBO);

            //Textures
            Txt1 = new Texture(@"ACW/Shaders/tx2.jpg", TextureUnit.Texture0);
            Txt2 = new Texture(@"ACW/Shaders/tx.jpg", TextureUnit.Texture1);

            #region Spheres
            GL.BindVertexArray(mVAO[0]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mSphereModel.Vertices.Length * sizeof(float)), mSphereModel.Vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO[1]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mSphereModel.Indices.Length * sizeof(float)), mSphereModel.Indices, BufferUsageHint.StaticDraw);

            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mSphereModel.Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mSphereModel.Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            GL.EnableVertexAttribArray(vNormalLocation);
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
            #endregion // Binds Spheres

            #region Cubes
            GL.BindVertexArray(mVAO[1]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO[2]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mCubeModel.Vertices.Length * sizeof(float)), mCubeModel.Vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO[3]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mCubeModel.Indices.Length * sizeof(float)), mCubeModel.Indices, BufferUsageHint.StaticDraw);

            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mCubeModel.Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mCubeModel.Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            GL.EnableVertexAttribArray(vNormalLocation);
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 8 * sizeof(float), 3 * sizeof(float));

            GL.EnableVertexAttribArray(vTexCoords);
            GL.VertexAttribPointer(vTexCoords, 2, VertexAttribPointerType.Float, true, 8 * sizeof(float), 6 * sizeof(float));
            #endregion  // Bind Cubes

            #region Cylinders
            GL.BindVertexArray(mVAO[2]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO[4]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mCylinderModel.Vertices.Length * sizeof(float)), mCylinderModel.Vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO[5]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mCylinderModel.Indices.Length * sizeof(float)), mCylinderModel.Indices, BufferUsageHint.StaticDraw);

            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mCylinderModel.Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mCylinderModel.Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            GL.EnableVertexAttribArray(vNormalLocation);
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
            #endregion


            //Checking acctivity o gravity is on
            if (gravityAcceleration.Y == 0)
            {
                gravityOn = false;
            }
            else
            {
                gravityOn = true;
            }

            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);

            //Manages the projection for resizing the window
            int uProjectionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uProjection");
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(1, (float)ClientRectangle.Width / ClientRectangle.Height, 0.5f, viewDistance);
            GL.UniformMatrix4(uProjectionLocation, true, ref projection);


            Light.Inside();
            Level.Inside();
            EmitterBox.Inside();
            SpotLight.Inside();



            GL.BindVertexArray(0);

            base.OnLoad(e);
        }
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(ClientRectangle);

            // Resize the window , when its full screen
            if (mShader != null)
            {
                int uProjectionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uProjection");
                float windowHeight = ClientRectangle.Height;
                float windowWidth = ClientRectangle.Width;

                if (windowHeight > windowWidth)
                {
                    if (windowWidth < 1)
                    {
                        windowWidth = 1;
                    }


                    Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(1, (float)ClientRectangle.Width / ClientRectangle.Height, 0.5f, viewDistance);
                    GL.UniformMatrix4(uProjectionLocation, true, ref projection);
                }
                else
                {
                    if (windowHeight < 1)
                    {
                        windowHeight = 1;
                    }


                    Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(1, (float)ClientRectangle.Width / ClientRectangle.Height, 0.5f, viewDistance);
                    GL.UniformMatrix4(uProjectionLocation, true, ref projection);
                }
            }
        }
         
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (Camera.Type == CameraType.FreeCam)
            {
                camera.FreeCamera(e);
            }

            #region //Camera Keys Controls

            if (e.KeyChar == '1')
            {
                Camera.Type = CameraType.FreeCam;
            }
            if (e.KeyChar == '2')
            {
                Camera.Type = CameraType.StaticCam;
            }
            if (e.KeyChar == '3')
            {
                Camera.Type = CameraType.RotationCam;
            }
            if (e.KeyChar == '4')
            {
                Camera.Type = CameraType.FollowCam;
            }

            if (e.KeyChar == 27)
            {
                Exit();
            }

            if (e.KeyChar == 13)
            {
                if (WindowState == WindowState.Fullscreen)
                {
                    WindowState = WindowState.Normal;
                }
                else
                {
                    WindowState = WindowState.Fullscreen;
                }
            }
            #endregion

        } // Class onKeyPress for controls movements
        protected override void OnUnload(EventArgs e)
        {
            GL.DeleteBuffers(mVBO.Length, mVBO);
            GL.DeleteBuffers(mVAO.Length, mVAO);

            Txt1.Delete();
            Txt2.Delete();

            base.OnUnload(e);
        }

        //Renders the scene
        public void Render_Objects()
        {
            camera.UpdateLightPositions();
            GL.BindVertexArray(mVAO[1]);


            Texture.Unbind();
            GL.Disable(EnableCap.CullFace);

            GL.Enable(EnableCap.CullFace);


            //Draws the Spheres
            GL.BindVertexArray(mVAO[0]);
            foreach (Sphere s in Sphere.DrawList)
            {
                s.Draw();
            }

            //Draw Sphere of Doom 
            Texture.Unbind();
            Level.SphereOfDoom.MaterialsValues();
            Matrix4 DoomSphereLocation = Matrix4.CreateScale(Level.SphereOfDoom.mRadius) * Matrix4.CreateTranslation(Level.SphereOfDoom.mPosition) * Level.Box3 * mGroundModel;
            GL.UniformMatrix4(uModelLocation, true, ref DoomSphereLocation);
            GL.DrawElements(BeginMode.Triangles, mSphereModel.Indices.Length, DrawElementsType.UnsignedInt, 0);

            //Draws Cubes
            GL.BindVertexArray(mVAO[1]);

            Txt2.MakeActive();
            Materials.silver.Assign_Material();
            DrawTopBox(mBoxMatrix * mGroundModel * Level.Box0);
            DrawBox(mBoxMatrix * mGroundModel * Level.Box1);
            DrawBox(mBoxMatrix * mGroundModel * Level.Box2);
            DrawBottomBox(mBoxMatrix * mGroundModel * Level.Box3);

            //This stops the back of cylinders from not being drawn
            GL.Disable(EnableCap.CullFace);
            Texture.Unbind();

            //Draws Cylinders
            GL.BindVertexArray(mVAO[2]);

            foreach (Cylinder c in Level.Box1_Cylinders) //LEVEL 1
            {
                Level.DrawCylinders(c, Level.Box1);
            }
            foreach (Cylinder c in Level.Box2_Cylinders) //LEVEL 1
            {
                Level.DrawCylinders(c, Level.Box2);
            }
        }
        //Doesn't draw top and bottom section
        private void DrawBox(Matrix4 BoxPosition)
        {
            GL.UniformMatrix4(uModelLocation, true, ref BoxPosition);
            GL.DrawElements(BeginMode.Triangles, mCubeModel.Indices.Length - 12, DrawElementsType.UnsignedInt, 0);
        }
        //Drawing without the bottom section
        private void DrawTopBox(Matrix4 BoxPosition)
        {
            GL.UniformMatrix4(uModelLocation, true, ref BoxPosition);
            GL.DrawElements(BeginMode.Triangles, 12, DrawElementsType.UnsignedInt, 0); //Front and Back

            Materials.silver.Assign_Material();
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 12 * sizeof(float)); //RightSide

            Txt1.MakeActive();
            GL.DrawElements(BeginMode.Triangles, mCubeModel.Indices.Length - 24, DrawElementsType.UnsignedInt, 18 * sizeof(float)); //Rest


        }
        //Drawing without the top section
        private void DrawBottomBox(Matrix4 BoxPosition)
        {
            GL.UniformMatrix4(uModelLocation, true, ref BoxPosition);
            GL.DrawElements(BeginMode.Triangles, mCubeModel.Indices.Length - 12, DrawElementsType.UnsignedInt, 0); //Side sections

            Materials.silver.Assign_Material();

            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 30 * sizeof(float)); //Bottom

        } 






    }
}
