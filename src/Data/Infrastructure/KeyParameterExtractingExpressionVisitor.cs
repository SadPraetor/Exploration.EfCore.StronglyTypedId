using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using StronglyTypedId.Models;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace StronglyTypedId.Data.Infrastructure
{
	internal class KeyParameterExtractingExpressionVisitor : ParameterExtractingExpressionVisitor
	{
		public KeyParameterExtractingExpressionVisitor(IEvaluatableExpressionFilter evaluatableExpressionFilter, IParameterValues parameterValues, Type contextType, IModel model, IDiagnosticsLogger<DbLoggerCategory.Query> logger, bool parameterize, bool generateContextAccessors)
			: base(evaluatableExpressionFilter, parameterValues, contextType, model, logger, parameterize, generateContextAccessors)
		{
		}

		[return: NotNullIfNotNull("expression")]
		public override Expression? Visit(Expression? expression)
		{
			var result = base.Visit(expression);

			if (expression is null)
			{
				return result;
			}

			if (expression.NodeType == ExpressionType.MemberAccess && expression.Type == typeof(ContractKey))
			{
				var member = MemberExpression.Property(expression, nameof(ContractKey.ContractId));
				ExtractParameters(member);
				return result;
				Console.WriteLine("breakpoint");
			}

			return result;
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			var result = base.VisitMember(node);

			if (node is MemberExpression member)
			{

			}

			return result;
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			return base.VisitConstant(node);
		}
	}
}
