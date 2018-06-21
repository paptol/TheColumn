
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Labs.ACW
{
    public class Materials
    {
        // Light vectors 
        public Vector3 mAmbient;
        public Vector3 mDiffuse;
        public Vector3 mSpecular;
        public float mShininess;

        // ACW Materials required
        public static Materials mattOrange = new Materials(new Vector3(1f, 0.5f, 0.0f), new Vector3(0.50f, 0.1995f, 0.0f), new Vector3(0.05f, 0.1995f, 0.0f), 100);
        public static Materials DodgerBlue = new Materials(new Vector3(0.0f, 0.4f, 0.7f), new Vector3(0.32f, 0.35f, 0.42f), new Vector3(0.41f, 0.41f, 0.39f), 8f);
        public static Materials ForestGreen = new Materials(new Vector3(0, 0.05f, 0), new Vector3(0.0f, 0.3f, 0.0f), new Vector3(0.004f, 0.5f, 0.004f), 10f);
        public static Materials silver = new Materials(new Vector3(0.19225f, 0.19225f, 0.19225f), new Vector3(0.50754f, 0.50754f, 0.50754f), new Vector3(0, 0, 0), 100f);

        public Materials(Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess)
        {
            mAmbient = ambient;
            mDiffuse = diffuse;
            mSpecular = specular;
            mShininess = shininess;
        }

        public void Assign_Material()
        {
            int AReflectLocation = GL.GetUniformLocation(ACWWindow.mShader.ShaderProgramID, "uMaterial.AmbientReflectivity");
            GL.Uniform3(AReflectLocation, mAmbient);

            int DReflectLocation = GL.GetUniformLocation(ACWWindow.mShader.ShaderProgramID, "uMaterial.DiffuseReflectivity");
            GL.Uniform3(DReflectLocation, mDiffuse);

            int SReflectLocation = GL.GetUniformLocation(ACWWindow.mShader.ShaderProgramID, "uMaterial.SpecularReflectivity");
            GL.Uniform3(SReflectLocation, mSpecular);

            int ShininessLocation = GL.GetUniformLocation(ACWWindow.mShader.ShaderProgramID, "uMaterial.Shininess");
            GL.Uniform1(ShininessLocation, mShininess);
        }

    }

}
