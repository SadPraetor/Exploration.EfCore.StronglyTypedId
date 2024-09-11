using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace StronglyTypedId.Data.Infrastructure
{
	public class InterceptingQueryCompiler : QueryCompiler
	{

		private IModel _model;
		private Lazy<Dictionary<Type, IEntityType>> _ownedTypes;

		public InterceptingQueryCompiler(IQueryContextFactory queryContextFactory, ICompiledQueryCache compiledQueryCache, ICompiledQueryCacheKeyGenerator compiledQueryCacheKeyGenerator, IDatabase database, IDiagnosticsLogger<DbLoggerCategory.Query> logger, ICurrentDbContext currentContext, IEvaluatableExpressionFilter evaluatableExpressionFilter, IModel model)
			: base(queryContextFactory, compiledQueryCache, compiledQueryCacheKeyGenerator, database, logger, currentContext, evaluatableExpressionFilter, model)
		{
			_model = model;
			_ownedTypes = new Lazy<Dictionary<Type, IEntityType>>(() =>
				_model.GetEntityTypes()
					.Where(e => e.IsOwned())
				.ToDictionary(e => e.ClrType, e => e));
		}

		public override TResult Execute<TResult>(Expression query)
		{
			var replacement = HandleOwnedKeys(query);
			return base.Execute<TResult>(replacement);
		}

		public override TResult ExecuteAsync<TResult>(Expression query, CancellationToken cancellationToken = default)
		{
			var replacement = HandleOwnedKeys(query);
			return base.ExecuteAsync<TResult>(replacement, cancellationToken);
		}

		private Expression HandleOwnedKeys(Expression expression)
		{
			var visitor = new KeyExpressionVisitor(_ownedTypes);

			var replacement = visitor.Visit(expression);

			return replacement;
		}
	}
}
