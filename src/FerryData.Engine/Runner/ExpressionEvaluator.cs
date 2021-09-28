using DynamicExpresso;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Runner
{
    public class ExpressionEvaluator
    {
        private Interpreter _interpreter;
        private Logger _logger;

        public ExpressionEvaluator(Dictionary<string, object> context, Logger logger)
        {
            _interpreter = new Interpreter(InterpreterOptions.DefaultCaseInsensitive);
            _logger = logger;

            foreach (var kvp in context)
            {
                var varName = kvp.Key;
                var val = kvp.Value;
                _interpreter.SetVariable(varName, val);

            }
        }

        public object Eval(string expression)
        {
            object result = null;

            try
            {
                result = _interpreter.Eval(expression);
            }
            catch (Exception e)
            {
                _logger.Error("Cannot eval Formula: {0}. Message: {1}",
                    expression,
                    e.Message);

            }

            return result;
        }

        public T Eval<T>(string expression)
        {
            T result = default(T);

            try
            {
                result = _interpreter.Eval<T>(expression);
            }
            catch (Exception e)
            {
                _logger.Error("Cannot eval Formula: {0}. Message: {1}",
                    expression,
                    e.Message);

            }

            return result;
        }
    }
}
