using System.Linq.Expressions;

namespace Foxminded.Library.DAL.Repositories.IQueryableSupport
{
    internal class RootReplacingVisitor : ExpressionVisitor
    {
        private readonly IQueryable _newRoot;

        public RootReplacingVisitor(IQueryable newRoot)
        {
            _newRoot = newRoot ?? throw new ArgumentNullException();
        }

        protected override Expression VisitConstant(ConstantExpression node) =>
                   node.Type.BaseType != null && 
                   node.Type.BaseType.IsGenericType &&
                   node.Type.BaseType.GetGenericTypeDefinition() == typeof(RepositoryBase<>) ?
                        Expression.Constant(_newRoot) : 
                        node;
    }
}
