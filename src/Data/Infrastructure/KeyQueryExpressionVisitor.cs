using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace StronglyTypedId.Data.Infrastructure
{
	internal class KeyQueryExpressionVisitor : ExpressionVisitor, IQueryExpressionInterceptor
	{
		private Lazy<Dictionary<Type, IEntityType>> _ownedTypes;
		private IModel _model;

		public KeyQueryExpressionVisitor()
		{
			_ownedTypes = new Lazy<Dictionary<Type, IEntityType>>(() => _model.GetEntityTypes().Where(e => e.IsOwned()).ToDictionary(e => e.ClrType, e => e));
		}

		public Expression QueryCompilationStarting(Expression expression, QueryExpressionEventData eventData)
		{
			_model = eventData.Context.Model;

			return Visit(expression);
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{

			var left = Visit(node.Left);
			var right = Visit(node.Right);

			var result = (left.NodeType, right.NodeType) switch
			{
				(ExpressionType.MemberAccess, ExpressionType.Parameter) => Test((left as MemberExpression)!, (right as ParameterExpression)!, node),
				(ExpressionType.Parameter, ExpressionType.MemberAccess) => Test((right as MemberExpression)!, (left as ParameterExpression)!, node),
				_ => base.VisitBinary(node)
			};


			return result;
		}



		private Expression Test(MemberExpression memberExpression, ParameterExpression parameterExpression, BinaryExpression node)
		{
			if (memberExpression.Type.Equals(parameterExpression.Type))
			{
				return base.VisitBinary(node);
			}

			if (!BothAreOwned(memberExpression, parameterExpression))
			{
				return base.VisitBinary(node);
			}

			if (memberExpression.Type.BaseType is not null && memberExpression.Type.BaseType.Equals(parameterExpression.Type))
			{
				var properties = _ownedTypes.Value[parameterExpression.Type].GetProperties().ToList();


				//### default
				var left = Expression.Property(memberExpression, properties[0].Name);
				var property = MemberExpression.Property(parameterExpression, properties[0].Name);
				var right = Expression.Lambda(property, parameterExpression);



				var attempt = Expression.MakeBinary(node.NodeType, left, right.Body);
				var visited = Visit(attempt);
				return visited;

				//return attempt;
				//return base.VisitBinary(node);
			}

			var result = base.VisitBinary(node);
			return result;



			bool BothAreOwned(MemberExpression memberExpression, ParameterExpression parameterExpression)
			{
				return _ownedTypes.Value.ContainsKey(memberExpression.Type) && _ownedTypes.Value.ContainsKey(parameterExpression.Type);
			}

			Expression BuildMemberSidePropertyAccess(MemberExpression expression)
			{
				var stack = new Stack<MemberExpression>();
				stack.Push(expression);
				while (expression.Expression is MemberExpression innerExpression)
				{
					expression = innerExpression;
					stack.Push(expression);
				}

				if (expression.Expression is ParameterExpression parameterExpression)
				{
					Expression propertyExpression = parameterExpression;
					while (stack.TryPop(out var memberExpression))
					{
						propertyExpression = Expression.Property(propertyExpression, memberExpression.Member.Name);

					}

					return propertyExpression;
				}


				throw new NotImplementedException();
			}
		}
	}
}
