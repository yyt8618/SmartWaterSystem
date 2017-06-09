namespace SmartWaterSystem
{
    public class JScriptMath
    {
        public static object EvalExpress(string sExpression)
        {
            Microsoft.JScript.Vsa.VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
            return Microsoft.JScript.Eval.JScriptEvaluate(sExpression, ve);
        }
    }
}
