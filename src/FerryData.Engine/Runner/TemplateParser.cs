using NLog;
using System.Collections.Generic;

namespace FerryData.Engine.Runner
{
    public class TemplateParser
    {
        public static bool TemplateContainsExpressions(string template)
        {
            return template.Contains("{{");
        }

        public static List<string> ExtractExpressions(string template)
        {
            var expressions = new List<string>();

            if (string.IsNullOrEmpty(template))
            {
                return expressions;
            }

            var tmp = template;
            bool flagContinue = false;
            do
            {
                var formulaStartInd = tmp.IndexOf("{{");
                var formulaEndInd = tmp.IndexOf("}}");

                if (formulaEndInd == -1)
                {
                    flagContinue = false;
                    break;
                }

                var formulaLength = formulaEndInd - formulaStartInd - 2;
                var currentFormula = tmp.Substring(formulaStartInd + 2, formulaLength);

                if (!string.IsNullOrEmpty(currentFormula))
                {
                    expressions.Add(currentFormula.Trim());
                }

                tmp = tmp.Substring(formulaEndInd + 2);

                formulaStartInd = tmp.IndexOf("{{");
                flagContinue = formulaStartInd > -1;

            } while (flagContinue);

            return expressions;
        }

        public static string PrepareTemplate(string template, Dictionary<string, object> stepsData, Logger logger)
        {
            var resultString = template;

            var evaluator = new ExpressionEvaluator(stepsData, logger);
            var expressions = TemplateParser.ExtractExpressions(template);

            foreach (var expression in expressions)
            {
                var expressionResult = evaluator.Eval(expression);
                var expressionResultStr = expressionResult?.ToString();

                resultString = resultString.Replace("{{" + expression + "}}", expressionResultStr);
            }

            logger.Info($"Template prepared {template} -> {resultString}");

            return resultString;
        }

    }
}
