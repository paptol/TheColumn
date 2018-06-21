using OpenTK;
using System;
using System.Collections.Generic;

namespace Labs.ACW
{
    class EmitterBox
    {
        public static List<EmitterBox> mEmitterList = new List<EmitterBox>();
        private Timer mSpawnTimer = new Timer();

        private Vector3 mPosition;
        private float mRespawnT;
        private Sphere mRespawnS;

        private bool mRandom_Velocity = true;
        private Vector3 mStaticVelocity;

        public EmitterBox(Vector3 pos, float repawnT, Sphere sphere, bool RVel)
        {
            mPosition = pos;
            mRespawnT = repawnT;
            mRespawnS = sphere;
            timeStep = 10;
            mRandom_Velocity = RVel;
        }
        public EmitterBox(Vector3 pos, float repawnT, Sphere sphere, Vector3 Vel)
        {
            mPosition = pos;
            mRespawnT = repawnT;
            mRespawnS = sphere;
            mRandom_Velocity = false;
            mStaticVelocity = Vel;
        }
        //Used to create and initalise the emitters
        public static void Inside()
        {
            mEmitterList.Add(new EmitterBox(new Vector3(1, 18, 0), 3f, Sphere.mOrangeSphere, true));
            mEmitterList.Add(new EmitterBox(new Vector3(-2, 18, 0), 2f, Sphere.dBlueSphere, true));
        }
        //Updates emitter
        public static void Update()
        {
            
            foreach (EmitterBox Em in mEmitterList)
            {
                Em.CheckEM();
            }
        }

        float timeStep;
        bool Emitted = false;
        public void CheckEM()
        {
            timeStep += mSpawnTimer.GetElapsedSeconds();

            //If spawn time is off
            if (mRespawnT == 0 && Emitted == false)
            {
                //Using for gravity test values
                mRespawnS.mVelocity = Vector3.Zero;
                Sphere.DrawList.Add(new Sphere(mRespawnS));
                Emitted = true;
            }
            else if (mRespawnT != 0)
            {
                {
                    if (timeStep > mRespawnT && Sphere.DrawList.Count < ACWWindow.sphereLimit)
                    {
                        Vector3 Velocity = mStaticVelocity;
                        if (mRandom_Velocity == true) //If random values are used
                        {
                            Velocity = RandomV();
                        }
                        Sphere.DrawList.Add(new Sphere(mPosition, Velocity, mRespawnS.mRadius, mRespawnS.mDensity, mRespawnS.material)); //Creates a new sphere to draw
                        timeStep = 0;
                    }
                }
            }
        }
        //Randomizes the x and z value
        Random rnd = new Random();
        private Vector3 RandomV()
        {
            int maxSpeed = 1;
            return new Vector3(rnd.Next(1, maxSpeed), rnd.Next(-maxSpeed, maxSpeed), rnd.Next(-maxSpeed, maxSpeed)); 
        }
    }
}
