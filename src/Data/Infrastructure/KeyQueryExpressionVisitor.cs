using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;

namespace StronglyTypedId.Data.Infrastructure
{
	internal class KeyQueryExpressionVisitor : ExpressionVisitor, IQueryExpressionInterceptor
	{
		public Expression QueryCompilationStarting(Expression expression, QueryExpressionEventData eventData)
		{
			return Visit(expression);
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			return base.VisitBinary(node);
		}
	}
}
