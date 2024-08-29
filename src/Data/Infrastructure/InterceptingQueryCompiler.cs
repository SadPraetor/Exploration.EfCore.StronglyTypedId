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
		private readonly IEvaluatableExpressionFilter _evaluatableExpressionFilter;
		private IModel _model;
		private Type _contextType;
		public InterceptingQueryCompiler(IQueryContextFactory queryContextFactory, ICompiledQueryCache compiledQueryCache, ICompiledQueryCacheKeyGenerator compiledQueryCacheKeyGenerator, IDatabase database, IDiagnosticsLogger<DbLoggerCategory.Query> logger, ICurrentDbContext currentContext, IEvaluatableExpressionFilter evaluatableExpressionFilter, IModel model)
			: base(queryContextFactory, compiledQueryCache, compiledQueryCacheKeyGenerator, database, logger, currentContext, evaluatableExpressionFilter, model)
		{
			_evaluatableExpressionFilter = evaluatableExpressionFilter;
			_model = model;
			_contextType = currentContext.Context.GetType();
		}

		public override Expression ExtractParameters(
		Expression query,
		IParameterValues parameterValues,
		IDiagnosticsLogger<DbLoggerCategory.Query> logger,
		bool parameterize = true,
		bool generateContextAccessors = false)
		{
			var visitor = new KeyParameterExtractingExpressionVisitor(
				_evaluatableExpressionFilter,
				parameterValues,
				_contextType,
				_model,
				logger,
				parameterize,
				generateContextAccessors);

			return visitor.ExtractParameters(query);
		}
	}
}
