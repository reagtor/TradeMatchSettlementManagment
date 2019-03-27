using System;
using System.CodeDom;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Text;
using System.Reflection;
using MSScriptControl;


namespace GTA.VTS.Common.CommonUtility
{
	/// <summary>
	/// 公式计算器
    /// 作者：朱亮
    /// 日期：2008-11-25
	/// </summary>
	public class Evaluator
	{
        #region Construction

        private Evaluator()
        {

        }
		private Evaluator(EvaluatorItem[] items)
		{
            ConstructEvaluator(items);
		}

        private Evaluator(Type returnType, string expression, string name)
        {
            EvaluatorItem[] items = { new EvaluatorItem(returnType, expression, name) };
            ConstructEvaluator(items);
        }

        private Evaluator(EvaluatorItem item)
        {
            EvaluatorItem[] items = { item };
            ConstructEvaluator(items);
        }

        private void ConstructEvaluator(EvaluatorItem[] items)
        {
              //方法已过时 update by 董鹏 2010-02-25
              //ICodeCompiler comp = (new CSharpCodeProvider().CreateCompiler());
              CompilerParameters cp = new CompilerParameters();
              cp.ReferencedAssemblies.Add("system.dll");
              cp.ReferencedAssemblies.Add("system.data.dll");
              cp.ReferencedAssemblies.Add("system.xml.dll");
              cp.GenerateExecutable = false;
              cp.GenerateInMemory = true;

              StringBuilder code = new StringBuilder();
              code.Append("using System; \n");
              code.Append("using System.Data; \n");
              code.Append("using System.Data.SqlClient; \n");
              code.Append("using System.Data.OleDb; \n");
              code.Append("using System.Xml; \n");
              code.Append("namespace CommonUtility { \n");
              code.Append("  public class _Evaluator { \n");
              foreach(EvaluatorItem item in items)
              {
                code.AppendFormat("    public {0} {1}() ", 
                                  item.ReturnType.Name, 
                                  item.Name);
                code.Append("{ ");
                code.AppendFormat("      return ({0})({1}); ", item.ReturnType.Name, item.Expression);
                code.Append("}\n");
              }
              code.Append("} }");
              
              //方法已过时 update by 董鹏 2010-02-25
              //CompilerResults cr = comp.CompileAssemblyFromSource(cp, code.ToString());
              CompilerResults cr = CodeDomProvider.CreateProvider("CSharp").CompileAssemblyFromSource(cp, code.ToString());
              if (cr.Errors.HasErrors)
              {
                StringBuilder error = new StringBuilder();
                error.Append("Error Compiling Expression: ");
                foreach (CompilerError err in cr.Errors)
                {
                  error.AppendFormat("{0}\n", err.ErrorText);
                }
                throw new Exception("Error Compiling Expression: " + error.ToString());
              }
              Assembly a = cr.CompiledAssembly;
              _Compiled = a.CreateInstance("CommonUtility._Evaluator");
           }

        #endregion

        #region Public Members

        public static
            double EvaluateDouble(EvaluatorItem item)
        {
            Evaluator el = GetEvaluator(item);
            return (double)el.Evaluate(item.Name);
        }
        public static float EvaluateFloat(EvaluatorItem item)
        {
            Evaluator el = GetEvaluator(item);
            return (float)el.Evaluate(item.Name);
        }
        public static int EvaluateInt(EvaluatorItem item)
        {
            Evaluator el = GetEvaluator(item);
            return (int)el.Evaluate(item.Name);
        }

        public static string EvaluateString(EvaluatorItem item)
        {
            Evaluator el = GetEvaluator(item);
            return (string)el.Evaluate(item.Name);
        }

        public static bool EvaluateBool(EvaluatorItem item)
        {
            Evaluator el = GetEvaluator(item);
            return (bool) el.Evaluate(item.Name);
        }

        private object Evaluate(string name)
        {
          MethodInfo mi = _Compiled.GetType().GetMethod(name);
          return mi.Invoke(_Compiled, null);
        }
        #endregion

        #region Private
        const string staticMethodName = "__foo";
        //Type _CompiledType = null;
        object _Compiled = null;
        static Dictionary<string, Evaluator> _evaluatorList = new Dictionary<string, Evaluator>();
        static Evaluator GetEvaluator(EvaluatorItem item)
        {
            Evaluator result = null;
            string strKey = item.Expression+item.Expression + item.ReturnType;
            if(　!_evaluatorList.ContainsKey(strKey))
                result = _evaluatorList[strKey] = new Evaluator(item);
            
            return result;

        }
        #endregion
	}

    public class EvaluatorItem
  {
    public EvaluatorItem(Type returnType, string expression, string name)
    {
      ReturnType = returnType;
      Expression = expression;
      Name = name;
    }

    public Type ReturnType;
    public string Name;
    public string Expression;
  }



    /// <summary>
    /// Microsoft   Script   Control   1.0
    /// 提供的公式计算功能包装类
    /// </summary>
    public class ScriptEvaluatorWrapper
    {

        /// <summary>
        /// 计算表达式的值
        /// </summary>
        /// <param name="strExpression">表达式</param>
        /// <returns>值的字符串形式</returns>
        public static string Eval(string strExpression )
        {

            string strResult = string.Empty;
            ScriptControl vScriptControl = new ScriptControl();
            try
            {
                strResult = vScriptControl.Eval(strExpression).ToString();
            }
            catch (Exception ex)
            {
                LogHelper.WriteDebug(string.Format("RC10001 : {0}",ex.Message));
            }
            return strResult;
        }
    }

}
