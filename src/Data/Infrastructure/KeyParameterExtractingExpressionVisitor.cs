using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Linq.Expressions;

namespace StronglyTypedId.Data.Infrastructure
{
	internal class KeyParameterExtractingExpressionVisitor : ParameterExtractingExpressionVisitor
	{
		public KeyParameterExtractingExpressionVisitor(IEvaluatableExpressionFilter evaluatableExpressionFilter, IParameterValues parameterValues, Type contextType, IModel model, IDiagnosticsLogger<DbLoggerCategory.Query> logger, bool parameterize, bool generateContextAccessors)
			: base(evaluatableExpressionFilter, parameterValues, contextType, model, logger, parameterize, generateContextAccessors)
		{
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			return base.VisitMember(node);
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			return base.VisitConstant(node);
		}
	}
}
