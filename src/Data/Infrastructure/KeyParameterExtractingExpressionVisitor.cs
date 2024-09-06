using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using StronglyTypedId.Models;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace StronglyTypedId.Data.Infrastructure
{
	internal class KeyParameterExtractingExpressionVisitor : ParameterExtractingExpressionVisitor
	{
		private readonly IParameterValues _parameterValues;
		private readonly IDiagnosticsLogger<DbLoggerCategory.Query> _logger;

		public KeyParameterExtractingExpressionVisitor(IEvaluatableExpressionFilter evaluatableExpressionFilter, IParameterValues parameterValues, Type contextType, IModel model, IDiagnosticsLogger<DbLoggerCategory.Query> logger, bool parameterize, bool generateContextAccessors)
			: base(evaluatableExpressionFilter, parameterValues, contextType, model, logger, parameterize, generateContextAccessors)
		{
			_parameterValues = parameterValues;
			_logger = logger;
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

				var parameterValue = GetValue(member, out var parameterName);


				parameterName = QueryCompilationContext.QueryParameterPrefix
						+ parameterName
						+ "_"
						+ "keyProperty";

				_parameterValues.AddParameter(parameterName, parameterValue);

				return result!;
			}

			return result!;
		}

		private object? GetValue(Expression? expression, out string? parameterName)
		{
			parameterName = null;

			if (expression == null)
			{
				return null;
			}

			//if (_generateContextAccessors)
			//{
			//	var newExpression = _contextParameterReplacingExpressionVisitor.Visit(expression);

			//	if (newExpression != expression)
			//	{
			//		if (newExpression.Type is IQueryable)
			//		{
			//			return newExpression;
			//		}

			//		parameterName = QueryFilterPrefix
			//			+ (RemoveConvert(expression) is MemberExpression memberExpression
			//				? ("__" + memberExpression.Member.Name)
			//				: "");

			//		return Expression.Lambda(
			//			newExpression,
			//			_contextParameterReplacingExpressionVisitor.ContextParameterExpression);
			//	}
			//}

			switch (expression)
			{
				case MemberExpression memberExpression:
					var instanceValue = GetValue(memberExpression.Expression, out parameterName);
					try
					{
						switch (memberExpression.Member)
						{
							case FieldInfo fieldInfo:
								parameterName = (parameterName != null ? parameterName + "_" : "") + fieldInfo.Name;
								return fieldInfo.GetValue(instanceValue);

							case PropertyInfo propertyInfo:
								parameterName = (parameterName != null ? parameterName + "_" : "") + propertyInfo.Name;
								return propertyInfo.GetValue(instanceValue);
						}
					}
					catch
					{
						// Try again when we compile the delegate
					}

					break;

				case ConstantExpression constantExpression:
					return constantExpression.Value;

				case MethodCallExpression methodCallExpression:
					parameterName = methodCallExpression.Method.Name;
					break;

					//case UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unaryExpression
					//	when (unaryExpression.Type.UnwrapNullableType() == unaryExpression.Operand.Type):
					//	return GetValue(unaryExpression.Operand, out parameterName);
			}

			try
			{
				return Expression.Lambda<Func<object>>(
						Expression.Convert(expression, typeof(object)))
					.Compile(preferInterpretation: true)
					.Invoke();
			}
			catch (Exception exception)
			{
				throw new InvalidOperationException(
					_logger.ShouldLogSensitiveData()
						? CoreStrings.ExpressionParameterizationExceptionSensitive(expression)
						: CoreStrings.ExpressionParameterizationException,
					exception);
			}
		}

		private string? GetParameterName(Expression? expression)
		{
			string parameterName = null;
			if (expression is MemberExpression memberExpression)
			{
				parameterName = memberExpression.Member.Name;

				try
				{
					switch (memberExpression.Member)
					{
						case FieldInfo fieldInfo:
							parameterName = (parameterName != null ? parameterName + "_" : "") + fieldInfo.Name;
							break;

						case PropertyInfo propertyInfo:
							parameterName = (parameterName != null ? parameterName + "_" : "") + propertyInfo.Name;
							break;
					}

					parameterName = QueryCompilationContext.QueryParameterPrefix
						+ parameterName
						+ "_"
						+ _parameterValues.ParameterValues.Count;
					return parameterName;
				}
				catch
				{

				}
			}
			return null;
		}

		private static Expression RemoveConvert(Expression expression)
		{
			if (expression is UnaryExpression unaryExpression
				&& expression.NodeType is ExpressionType.Convert or ExpressionType.ConvertChecked)
			{
				return RemoveConvert(unaryExpression.Operand);
			}

			return expression;
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
