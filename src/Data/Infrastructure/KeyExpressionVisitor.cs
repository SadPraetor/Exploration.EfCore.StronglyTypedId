﻿using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace StronglyTypedId.Data.Infrastructure
{
	internal class KeyExpressionVisitor : ExpressionVisitor
	{

		private readonly Lazy<Dictionary<Type, IEntityType>> _ownedTypes;

		public KeyExpressionVisitor(Lazy<Dictionary<Type, IEntityType>> ownedTypes)
		{
			_ownedTypes = ownedTypes;
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			var left = Visit(node.Left);
			var right = Visit(node.Right);

			if (node.Left.NodeType == ExpressionType.MemberAccess &&
				node.Right.NodeType == ExpressionType.MemberAccess &&
				_ownedTypes.Value.ContainsKey(node.Left.Type) &&
				_ownedTypes.Value.ContainsKey(node.Right.Type) &&
				AreRelated(node.Left.Type, node.Right.Type, out var commonBase)
				)
			{
				var properties = _ownedTypes.Value[commonBase!]
					.GetProperties()
					.ToList();
			}


			var result = (left.NodeType, right.NodeType) switch
			{
				(ExpressionType.MemberAccess, ExpressionType.Parameter) => Test((left as MemberExpression)!, (right as ParameterExpression)!, node),
				(ExpressionType.Parameter, ExpressionType.MemberAccess) => Test((right as MemberExpression)!, (left as ParameterExpression)!, node),
				_ => base.VisitBinary(node)
			};


			return result;
		}


		private bool AreRelated(Type left, Type right, out Type? commonBase)
		{
			switch (left, right)
			{
				case ({ BaseType: not null }, { BaseType: null }) when left.BaseType.Equals(right):
					commonBase = right;
					return true;


				case ({ BaseType: null }, { BaseType: not null }) when right.BaseType.Equals(left):
					commonBase = left;
					return true;


				default:
					commonBase = null;
					return false;

			}
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
				var right = Expression.Parameter(typeof(int), "__contractKey_ContractId_keyProperty");



				var attempt = Expression.MakeBinary(node.NodeType, left, right);
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
		}
	}
}
